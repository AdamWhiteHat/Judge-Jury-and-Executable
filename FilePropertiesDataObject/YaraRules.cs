using System;
using System.Linq;
using System.Collections.Generic;
using libyaraNET;
using System.Threading;

namespace FilePropertiesDataObject
{
	public static class YaraRules
	{
		public static string Scan(byte[] fileBytes, string rulesPath)
		{
			string result = null;
			try
			{
				List<ScanResult> scanResults = QuickScan.Memory(fileBytes, rulesPath);
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
			}
			catch
			{ }

			return result;
		}
	}
}
