using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesEnumerator
{
	using NtfsNodeAttributes = System.IO.Filesystem.Ntfs.Attributes;

	public class FileEnumerator
	{
		private static string[] IgnoreTheseRootFiles = Properties.Settings.Default.IgnoreTheseRootFiles.Cast<string>().ToArray();

		private static string[] IgnoreTheseRootDirectories = Properties.Settings.Default.IgnoreTheseRootDirectories.Cast<string>().ToArray();

		private static char[] DirectorySeperatorChars = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};

		public static void LaunchFileEnumerator(FileEnumeratorParameters parameters)
		{
			var fileEnumerationDelegate
				= new Func<FileEnumeratorParameters, FileEnumeratorReport>((args) => Worker(args));

			if (!parameters.DisableWorkerThread)
			{
				Task<FileEnumeratorReport> enumerationTask = Task.Run(() =>
				{
					FileEnumeratorReport results = fileEnumerationDelegate.Invoke(parameters);
					return results;
				},
				parameters.CancelToken);

				Task reportResultsTask = enumerationTask.ContinueWith((antecedent) => parameters.ReportResultsFunction(antecedent.Result));
			}
			else
			{
				FileEnumeratorReport results = fileEnumerationDelegate.Invoke(parameters);
				parameters.ReportResultsFunction(results);
			}
		}

		private static FileEnumeratorReport Worker(FileEnumeratorParameters parameters)
		{
			TimingMetrics timingMetrics = new TimingMetrics();
			FailSuccessCount fileEnumCount = new FailSuccessCount("OS files enumerated");
			FailSuccessCount databaseInsertCount = new FailSuccessCount("OS database rows updated");

			try
			{
				parameters.CancelToken.ThrowIfCancellationRequested();

				StringBuilder currentPath = new StringBuilder(parameters.SelectedFolder);
				string lastParent = currentPath.ToString();

				string temp = currentPath.ToString();
				if (temp.Contains(':') && (temp.Length == 2 || temp.Length == 3)) // Is a root directory, i.e. "C:" or "C:\"
				{
					lastParent = ".";
				}

				string drive = parameters.SelectedFolder[0].ToString().ToUpper();

				List<DriveInfo> ntfsDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveFormat == "NTFS").ToList();

				DriveInfo driveToAnalyze = ntfsDrives.Where(dr => dr.Name.ToUpper().Contains(drive)).Single();

				NtfsReader ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.All);

				IEnumerable<INode> mftNodes =
					ntfsReader.GetNodes(driveToAnalyze.Name)
						.Where(node => (node.Attributes &
									 (NtfsNodeAttributes.Device
									  | NtfsNodeAttributes.Directory
									  | NtfsNodeAttributes.ReparsePoint
									  | NtfsNodeAttributes.SparseFile
									 )) == 0) // This means that we DONT want any matches of the above NtfsNodeAttributes type
						.Where(node => FileMatchesPattern(node.FullName, parameters.SearchPatterns));
				//.OrderByDescending(n => n.Size);

				if (parameters.SelectedFolder.ToCharArray().Length > 3)
				{
					string selectedFolderUppercase = parameters.SelectedFolder.ToUpperInvariant().TrimEnd(new char[] { '\\' });
					mftNodes = mftNodes.Where(node => node.FullName.ToUpperInvariant().Contains(selectedFolderUppercase));
				}

				IDataPersistenceLayer dataPersistenceLayer = parameters.DataPersistenceLayer;

				foreach (INode node in mftNodes)
				{
					string message = $"MFT#: {node.MFTRecordNumber.ToString().PadRight(7)} Seq.#: {node.SequenceNumber.ToString().PadRight(4)} Path: {node.FullName}";

					if (parameters.LogOutputFunction != null) parameters.LogOutputFunction.Invoke(message);
					if (parameters.ReportOutputFunction != null) parameters.ReportOutputFunction.Invoke(message);

					fileEnumCount.IncrementSucceededCount();

					FileProperties prop = new FileProperties();
					prop.PopulateFileProperties(parameters, parameters.SelectedFolder[0], node, timingMetrics);

					// INSERT file properties into _DATABASE_
					bool insertResult = dataPersistenceLayer.PersistFileProperties(prop);
					if (insertResult)
					{
						databaseInsertCount.IncrementSucceededCount();
					}
					else
					{
						databaseInsertCount.IncrementFailedCount();
					}

					parameters.CancelToken.ThrowIfCancellationRequested();
				}

				dataPersistenceLayer.Dispose();
				FileProperties.CleanUp();
			}
			catch (OperationCanceledException)
			{ }

			return new FileEnumeratorReport(new List<FailSuccessCount> { fileEnumCount, databaseInsertCount }, timingMetrics);
		}

		private static bool FileMatchesPattern(string fullName, string[] searchPatterns)
		{
			string filename = Path.GetFileName(fullName);
			string rootDirectory = Path.GetDirectoryName(fullName).Substring(3);

			int seperatorIndex = rootDirectory.IndexOfAny(DirectorySeperatorChars);
			if (seperatorIndex != -1)
			{
				rootDirectory = rootDirectory.Substring(0, seperatorIndex);
			}

			/* if (filename.FirstOrDefault() == '$') { return false; } */

			if (string.IsNullOrWhiteSpace(rootDirectory))
			{
				if (IgnoreTheseRootFiles.Contains(filename))
				{
					return false;
				}
			}
			else
			{
				if (IgnoreTheseRootDirectories.Any(dir => rootDirectory.Equals(dir)))
				{
					return false;
				}
			}

			if (searchPatterns.Contains("*"))
			{
				return true;
			}

			string extension = Path.GetExtension(filename);
			if (extension.Length == 0)
			{
				return false;
			}

			foreach (string pattern in searchPatterns)
			{
				if (pattern.Contains("."))
				{
					if (extension.Contains(pattern))
					{
						return true;
					}
				}
				else
				{
					if (filename.Contains(pattern))
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
