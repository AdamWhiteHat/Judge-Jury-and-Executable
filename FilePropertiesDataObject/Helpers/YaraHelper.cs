using System;
using System.Linq;
using System.Collections.Generic;
using libyaraNET;
using System.Threading;

namespace FilePropertiesDataObject.Helpers
{
	public static class YaraHelper
	{
		private static Scanner _scanner;

		static YaraHelper()
		{
			_scanner = new Scanner();
		}

		public static List<string> ScanBytes(byte[] fileBytes, Rules compiledRules)
		{
			List<string> result = new List<string>();
			try
			{
				List<ScanResult> scanResults = _scanner.ScanMemory(fileBytes, compiledRules);
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

		public static List<string> ScanFile(string filePath, Rules compiledRules)
		{
			List<string> result = new List<string>();
			try
			{
				List<ScanResult> scanResults = _scanner.ScanFile(filePath, compiledRules);
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

		public static string FormatDelimitedRulesString(List<string> rules)
		{
			string result = "";
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
