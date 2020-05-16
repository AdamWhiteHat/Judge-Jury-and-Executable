using System;
using System.Linq;
using System.Collections.Generic;
using libyaraNET;
using System.Threading;

namespace FilePropertiesDataObject
{
	public static class YaraRules
	{
		public static string ScanBytes(byte[] fileBytes, string rulesPath)
		{
			string result = null;
			try
			{
				List<ScanResult> scanResults = QuickScan.Memory(fileBytes, rulesPath);
				result = FormatResults(scanResults);
			}
			catch
			{
			
			}

			return result;
		}

		public static string ScanFile(string filePath, string rulesPath)
		{
			string result = null;
			try
			{
				List<ScanResult> scanResults = QuickScan.File(filePath, rulesPath);
				result = FormatResults(scanResults);
			}
			catch
			{
			
			}

			return result;
		}

		private static string FormatResults(List<ScanResult> scanResults)
		{
			string result = null;

			CancellationHelper.ThrowIfCancelled();
			if (scanResults.Any())
			{
				IEnumerable<string> matchingRules = scanResults.Select(res => res.MatchingRule.Identifier);
				CancellationHelper.ThrowIfCancelled();
				if (matchingRules.Any())
				{
					result = string.Join("|", matchingRules);
				}
			}

			return result;
		}
	}
}
