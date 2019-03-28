using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NTFSLib.IO;
using DataAccessLayer;
using FilePropertiesDataObject;

namespace FilePropertiesEnumerator
{
	public class FileEnumerator
	{
		private static string[] ignoreFiles = new string[] { "swapfile.sys", "pagefile.sys", "hiberfil.sys" };

		private static FailSuccessCount fileEnumCount = null;
		private static FailSuccessCount databaseInsertCount = null;
		private static FailSuccessCount directoryEnumCount = null;

		public static void LaunchFileEnumerator(FileEnumeratorParameters parameters)
		{
			ThrowIfParametersInvalid(parameters);

			var fileEnumerationDelegate
				= new Func<FileEnumeratorParameters, List<FailSuccessCount>>((args) => ThreadLauncher(args));

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

		private static void ThrowIfParametersInvalid(FileEnumeratorParameters parameters)
		{
			if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
			if (parameters.SearchPatterns == null) { throw new ArgumentNullException(nameof(parameters.SearchPatterns)); }
			if (parameters.ReportExceptionFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportExceptionFunction)); }
			if (parameters.ReportOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportOutputFunction)); }
			if (parameters.LogOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.LogOutputFunction)); }
			if (parameters.ReportResultsFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportResultsFunction)); }
			if (!Directory.Exists(parameters.SelectedFolder)) { throw new DirectoryNotFoundException(parameters.SelectedFolder); }
			if (parameters.CancelToken == null) { throw new ArgumentNullException(nameof(parameters.CancelToken), "If you do not want to pass a CancellationToken, then pass 'CancellationToken.None'"); }
			parameters.CancelToken.ThrowIfCancellationRequested();
		}

		private static List<FailSuccessCount> ThreadLauncher(FileEnumeratorParameters parameters)
		{
			fileEnumCount = new FailSuccessCount("OS files enumerated");
			databaseInsertCount = new FailSuccessCount("OS database rows updated");
			directoryEnumCount = new FailSuccessCount("directories enumerated");

			try
			{
				char driveLetter = parameters.SelectedFolder[0];
				Queue<string> folders = new Queue<string>(new string[] { parameters.SelectedFolder });

				while (folders.Count > 0)
				{
					parameters.CancelToken.ThrowIfCancellationRequested();

					string currentDirectory = folders.Dequeue();

					// Get all _FILES_ inside folder
					IEnumerable<FileProperties> properties = EnumerateFileProperties(parameters, driveLetter, currentDirectory);
					foreach (FileProperties prop in properties)
					{
						parameters.CancelToken.ThrowIfCancellationRequested();

						// INSERT file properties into _DATABASE_
						bool insertResult = FilePropertiesAccessLayer.InsertFileProperties(prop);
						if (insertResult)
						{
							databaseInsertCount.IncrementSucceededCount();
						}
						else
						{
							databaseInsertCount.IncrementFailedCount();
							parameters.CancelToken.ThrowIfCancellationRequested();
							continue;
						}

						parameters.CancelToken.ThrowIfCancellationRequested();
					}

					// Get all _FOLDERS_ at this depth inside this folder
					IEnumerable<NtfsDirectory> nestedDirectories = MftHelper.GetDirectories(driveLetter, currentDirectory);
					foreach (NtfsDirectory directory in nestedDirectories)
					{
						parameters.CancelToken.ThrowIfCancellationRequested();
						string dirPath = Path.Combine(currentDirectory, directory.Name);
						folders.Enqueue(dirPath);
						directoryEnumCount.IncrementSucceededCount();
						parameters.CancelToken.ThrowIfCancellationRequested();
					}
				}
			}
			catch (OperationCanceledException)
			{ }

			return new List<FailSuccessCount> { fileEnumCount, databaseInsertCount, directoryEnumCount };
		}

		public static IEnumerable<FileProperties> EnumerateFileProperties(FileEnumeratorParameters parameters, char driveLetter, string currentDirectory)
		{
			foreach (NtfsFile file in MftHelper.EnumerateFiles(driveLetter, currentDirectory))
			{
				parameters.CancelToken.ThrowIfCancellationRequested();

				// File _PATTERN MATCHING_
				if (FileMatchesPattern(file, parameters.SearchPatterns))
				{
					string message = $"MFT File: {Path.Combine(currentDirectory, file.Name)}";
					if (parameters.LogOutputFunction != null) parameters.LogOutputFunction.Invoke(message);
					if (parameters.ReportOutputFunction != null) parameters.ReportOutputFunction.Invoke(message);

					fileEnumCount.IncrementSucceededCount();
					parameters.CancelToken.ThrowIfCancellationRequested();

					FileProperties prop = new FileProperties();
					prop.PopulateFileProperties(parameters, driveLetter, file);

					parameters.CancelToken.ThrowIfCancellationRequested();

					yield return prop;
				}
				else
				{
					fileEnumCount.IncrementFailedCount();
					parameters.CancelToken.ThrowIfCancellationRequested();
				}
			}
			yield break;
		}

		private static bool FileMatchesPattern(NtfsFile file, string[] searchPatterns)
		{
			string filename = file.Name;
			if (filename.FirstOrDefault() == '$')
			{
				return false;
			}

			if (ignoreFiles.Contains(filename))
			{
				return false;
			}

			if (searchPatterns.Contains("*"))
			{
				return true;
			}

			string extension = Path.GetExtension(filename);
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
