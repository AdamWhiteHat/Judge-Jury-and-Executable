using System;
using System.Linq;
using System.Collections.Generic;
using YaraSharp;
using System.Threading;

namespace FilePropertiesDataObject.Helpers
{
	public static class YaraHelper
	{
		private static YSInstance _yaraInstance;
		private static YSContext _yaraContext;

		static YaraHelper()
		{
			_yaraInstance = new YSInstance();
			_yaraContext = new YSContext();
		}

		public static void CleanUp()
		{
			if (_yaraContext != null)
			{
				_yaraContext.Dispose();
				_yaraContext = null;
			}
			_yaraInstance = null;
		}

		public static YSRules CompileRules(List<string> yaraRuleFiles, Action<string> loggingFunction)
		{
			YSRules result = null;

			using (YSCompiler compiler = _yaraInstance.CompileFromFiles(yaraRuleFiles, null))
			{
				string yaraWarnings = string.Empty;
				string yaraErrors = string.Empty;

				YSReport warnings = compiler.GetWarnings();
				if (!warnings.IsEmpty())
				{
					yaraWarnings = string.Join(Environment.NewLine, warnings.Dump().Select(kvp => $"{kvp.Key}: {kvp.Value}"));
				}

				YSReport errors = compiler.GetErrors();
				if (!errors.IsEmpty())
				{
					yaraErrors = string.Join(Environment.NewLine, errors.Dump().Select(kvp => $"{kvp.Key}: {kvp.Value}"));
				}

				if (!string.IsNullOrWhiteSpace(yaraWarnings))
				{
					loggingFunction.Invoke("YARA reported warnings.");
					loggingFunction.Invoke(yaraWarnings);
				}

				if (!string.IsNullOrWhiteSpace(yaraErrors))
				{
					throw new Exception("YARA reported errors compiling rules:" + Environment.NewLine + yaraErrors);
				}

				result = compiler.GetRules();
			}

			return result;
		}

		public static List<string> ScanBytes(byte[] fileBytes, YSRules compiledRules)
		{
			List<string> result = new List<string>();
			List<YSMatches> scanResults = _yaraInstance.ScanMemory(fileBytes, compiledRules, null, 0);
			if (scanResults.Any())
			{
				result = scanResults.SelectMany(res => res.Matches.Keys).ToList();
			}

			return result;
		}

		public static List<string> ScanFile(string filePath, YSRules compiledRules)
		{
			List<string> result = new List<string>();
			List<YSMatches> scanResults = _yaraInstance.ScanFile(filePath, compiledRules, null, 0);
			if (scanResults.Any())
			{
				result = scanResults.SelectMany(res => res.Matches.Keys).ToList();
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
