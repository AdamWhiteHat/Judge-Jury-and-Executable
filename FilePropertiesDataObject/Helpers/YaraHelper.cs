using System;
using System.Linq;
using System.Collections.Generic;
using libyaraNET;
using System.Threading;

namespace FilePropertiesDataObject.Helpers
{
	public static class YaraHelper
	{
		public static List<string> ScanBytes(byte[] fileBytes, string rulesPath)
		{
			List<string> result = null;
			try
			{
				List<ScanResult> scanResults = QuickScan.Memory(fileBytes, rulesPath);
				if (scanResults.Any())
				{
					result = scanResults.Select(res => res.MatchingRule.Identifier).ToList();
				}
			}
			catch
			{

			}

			return result;
		}

		public static List<string> ScanFile(string filePath, string rulesPath)
		{
			List<string> result = null;
			try
			{
				List<ScanResult> scanResults = QuickScan.File(filePath, rulesPath);
				if (scanResults.Any())
				{
					result = scanResults.Select(res => res.MatchingRule.Identifier).ToList();
				}
			}
			catch
			{

			}

			return result;
		}

		public static string MakeYaraIndexFile(List<string> yaraRuleFiles)
		{
			return string.Join(Environment.NewLine, yaraRuleFiles.Select(s => $"include \"{s}\""));
		}

		public static string FormatDelimitedRulesString(List<string> rules)
		{
			string result = null;
			if (rules.Any())
			{
				List<string> orderedRules = rules.Distinct().OrderBy(s => s).ToList();

				result = string.Join("|", orderedRules);
			}
			return result;
		}

		public static string[] ParseDelimitedRulesString(string delimitedRulesString)
		{
			return delimitedRulesString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
