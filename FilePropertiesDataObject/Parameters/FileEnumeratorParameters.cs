using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace FilePropertiesDataObject.Parameters
{
	public class FileEnumeratorParameters
	{
		public CancellationToken CancelToken { get; set; }

		public bool DisableWorkerThread { get; set; }
		public string SelectedFolder { get; set; }
		public string[] SearchPatterns { get; set; }

		public bool CalculateEntropy { get; set; }
		public List<YaraFilter> YaraParameters { get; set; }

		public Action<string> ReportOutputFunction { get; set; }
		public Action<string> LogOutputFunction { get; set; }
		public Action<List<FailSuccessCount>> ReportResultsFunction { get; set; }
		public Action<string, string, Exception> ReportExceptionFunction { get; set; }

		public IDataPersistenceLayer DataPersistenceLayer { get; set; }

		public FileEnumeratorParameters(CancellationToken cancelToken,
										bool disableWorkerThread,
										string selectedFolder,
										string searchPatterns,
										bool calculateEntropy,
										List<YaraFilter> yaraParameters,
										IDataPersistenceLayer dataPersistenceLayerClass,
										Action<string> reportOutputFunction,
										Action<string> logOutputFunction,
										Action<List<FailSuccessCount>> reportResultsFunction,
										Action<string, string, Exception> reportExceptionFunction
										)
		{
			this.CancelToken = cancelToken;
			this.DisableWorkerThread = disableWorkerThread;
			this.SelectedFolder = selectedFolder;
			this.CalculateEntropy = calculateEntropy;
			this.YaraParameters = yaraParameters;
			this.DataPersistenceLayer = dataPersistenceLayerClass;
			this.ReportOutputFunction = reportOutputFunction;
			this.LogOutputFunction = logOutputFunction;
			this.ReportResultsFunction = reportResultsFunction;
			this.ReportExceptionFunction = reportExceptionFunction;
			this.SearchPatterns = ParseSearchPatterns(searchPatterns);
		}

		private string[] ParseSearchPatterns(string searchPattern)
		{
			string[] patterns = new string[] { ".exe", ".dll", ".sys", ".drv", ".ocx", ".com", ".scr" };

			if (!string.IsNullOrWhiteSpace(searchPattern))
			{
				patterns = searchPattern.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				patterns = patterns.Select(s => s.Contains(".") ? s.Replace("*", "") : s).ToArray();
			}

			return patterns;

		}

		public void ThrowIfAnyParametersInvalid()
		{
			ThrowIfAnyParametersInvalid(this);
		}

		public static void ThrowIfAnyParametersInvalid(FileEnumeratorParameters parameters)
		{
			if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
			if (parameters.DataPersistenceLayer == null) { throw new ArgumentException(nameof(DataPersistenceLayer)); }
			if (parameters.SearchPatterns == null) { throw new ArgumentNullException(nameof(parameters.SearchPatterns)); }
			if (parameters.ReportExceptionFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportExceptionFunction)); }
			if (parameters.ReportOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportOutputFunction)); }
			if (parameters.LogOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.LogOutputFunction)); }
			if (parameters.ReportResultsFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportResultsFunction)); }
			if (string.IsNullOrWhiteSpace(parameters.SelectedFolder)) { throw new ArgumentNullException(nameof(parameters.SelectedFolder)); }
			if (!Directory.Exists(parameters.SelectedFolder)) { throw new DirectoryNotFoundException(parameters.SelectedFolder); }

			char suppliedDriveLetter = char.ToUpper(parameters.SelectedFolder[0]);

			List<char> foundDriveLetters = DriveInfo.GetDrives()
													.Where(d => d.IsReady && d.DriveFormat == "NTFS")
													.Select(di => di.Name.ToUpper()[0])
													.ToList();

			if (!foundDriveLetters.Any() || !foundDriveLetters.Contains(suppliedDriveLetter))
			{
				throw new DriveNotFoundException($"The drive {suppliedDriveLetter}:\\ was not found, the drive was not mounted or ready, or the drive had a file system other than NTFS.");
			}

			if (parameters.CancelToken == null) { throw new ArgumentNullException(nameof(parameters.CancelToken), "If you do not want to pass a CancellationToken, then pass 'CancellationToken.None'"); }
			parameters.CancelToken.ThrowIfCancellationRequested();
		}
	}
}
