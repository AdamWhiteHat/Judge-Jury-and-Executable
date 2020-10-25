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
		public YaraScanConfiguration YaraParameters { get; set; }

		public Action<string> ReportOutputFunction { get; set; }
		public Action<string> LogOutputFunction { get; set; }
		public Action<List<FailSuccessCount>> ReportResultsFunction { get; set; }
		public Action<string, string, Exception> ReportExceptionFunction { get; set; }

		public FileEnumeratorParameters(CancellationToken cancelToken, bool disableWorkerThread,
										string selectedFolder, string searchPatterns,
										bool calculateEntropy, YaraScanConfiguration yaraParameters,
										Action<string> reportOutputFunction,
										Action<string> logOutputFunction,
										Action<List<FailSuccessCount>> reportResultsFunction,
										Action<string, string, Exception> reportExceptionFunction)
		{
			this.CancelToken = cancelToken;
			this.DisableWorkerThread = disableWorkerThread;
			this.SelectedFolder = selectedFolder;
			this.CalculateEntropy = calculateEntropy;
			this.YaraParameters = yaraParameters;
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
	}
}
