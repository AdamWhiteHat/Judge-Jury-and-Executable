using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using DataAccessLayer;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesEnumerator
{
	using NtfsNodeAttributes = System.IO.Filesystem.Ntfs.Attributes;

	public class FileEnumerator
	{
		private static string[] IgnoreTheseFiles = new string[] { "swapfile.sys", "pagefile.sys", "hiberfil.sys" };

		private static FailSuccessCount fileEnumCount = null;
		private static FailSuccessCount databaseInsertCount = null;

		public static void LaunchFileEnumerator(FileEnumeratorParameters parameters)
		{
			var fileEnumerationDelegate
				= new Func<FileEnumeratorParameters, List<FailSuccessCount>>((args) => Worker(args));

			if (!parameters.DisableWorkerThread)
			{
				Task<List<FailSuccessCount>> enumerationTask = Task.Run(() =>
				{
					List<FailSuccessCount> results = new List<FailSuccessCount>();
					results.AddRange(fileEnumerationDelegate.Invoke(parameters));
					return results;
				},
				parameters.CancelToken);

				Task reportResultsTask = enumerationTask.ContinueWith((antecedent) => parameters.ReportResultsFunction(antecedent.Result));
			}
			else
			{
				List<FailSuccessCount> results = fileEnumerationDelegate.Invoke(parameters);
				parameters.ReportResultsFunction(results);
			}
		}

		private static List<FailSuccessCount> Worker(FileEnumeratorParameters parameters)
		{
			fileEnumCount = new FailSuccessCount("OS files enumerated");
			databaseInsertCount = new FailSuccessCount("OS database rows updated");

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

				string drive = parameters.SelectedFolder[0].ToString();

				List<DriveInfo> ntfsDrives = DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS").ToList();

				DriveInfo driveToAnalyze = ntfsDrives.Where(dr => dr.Name.ToUpper().Contains(drive.ToUpper())).Single();

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

				foreach (INode node in mftNodes)
				{
					string message = $"MFT#: {node.MFTRecordNumber.ToString().PadRight(7)} Seq.#: {node.SequenceNumber.ToString().PadRight(4)} Path: {node.FullName}";

					if (parameters.LogOutputFunction != null) parameters.LogOutputFunction.Invoke(message);
					if (parameters.ReportOutputFunction != null) parameters.ReportOutputFunction.Invoke(message);

					fileEnumCount.IncrementSucceededCount();

					FileProperties prop = new FileProperties();
					prop.PopulateFileProperties(parameters, parameters.SelectedFolder[0], node);

					// INSERT file properties into _DATABASE_
					bool insertResult = FilePropertiesAccessLayer.InsertFileProperties(prop);
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
			}
			catch (OperationCanceledException)
			{ }

			return new List<FailSuccessCount> { fileEnumCount, databaseInsertCount };
		}

		private static bool FileMatchesPattern(string fullName, string[] searchPatterns)
		{
			string filename = Path.GetFileName(fullName);
			if (filename.FirstOrDefault() == '$')
			{
				return false;
			}

			if (IgnoreTheseFiles.Contains(filename))
			{
				return false;
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
