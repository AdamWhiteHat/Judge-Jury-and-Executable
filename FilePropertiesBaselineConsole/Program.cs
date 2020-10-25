using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Logging;
using DataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using Newtonsoft.Json;

namespace FilePropertiesBaselineConsole
{
	sealed class Program
	{
		static Program()
		{
			Log.LogOutputAction = Console.WriteLine;
			SQLHelper.LogExceptionAction = Log.ExceptionMessage;
		}

		private static void DisplayUsageSyntax()
		{
			ReportOutput();
			ReportOutput("-p:C:\\Windows           -  Search [p]ath");
			ReportOutput("-m:*.exe                 -  Search [m]ask");
			ReportOutput("-e                       -  Enable calulate [e]ntropy");
			ReportOutput("-y:C:\\YaraConfig.json   -  [Y]ara configuration file");
		}

		private static void Main(string[] args)
		{
			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				ReportOutput("ERROR: Connection string not set! Please set the SQL connection string in .config file.");
				ReportOutput("Aborting...");
				return;
			}
			else
			{
				FilePropertiesAccessLayer.SetConnectionString(connectionString);
			}

			if (args.Length == 0)
			{
				DisplayUsageSyntax();
				return;
			}

			// Will hold flag & parameter to flag, such as: "-p", "C:\Windows\"
			List<Tuple<string, string>> flags = GetFlags(args);
			if (!flags.Any())
			{
				DisplayUsageSyntax();
				return;
			}

			string searchPath = "";
			string searchMask = "*.*";
			bool calcEntropy = false;
			bool yaraScan = false;
			string yaraConfigFile = "";
			YaraScanConfiguration yaraConfiguration = new YaraScanConfiguration();

			foreach (Tuple<string, string> flagTuple in flags)
			{
				string flag = flagTuple.Item1;
				string parameter = flagTuple.Item2;

				switch (flag)
				{
					case "e":
						calcEntropy = true;
						break;
					case "p":
						searchPath = parameter;
						break;
					case "m":
						searchMask = parameter;
						break;
					case "y":
						yaraScan = true;
						yaraConfigFile = parameter;
						break;
				}
			}

			ReportOutput();
			ReportOutput("Running with these parameters:");
			ReportOutput($"   Search [P]ath:       \"{searchPath}\"");
			ReportOutput($"   Search [M]ask:       {searchMask}");
			ReportOutput($"   Calulate [E]ntropy:  {calcEntropy}");
			ReportOutput($"   [Y]ara configuration file: \"{yaraConfigFile}\"");
			ReportOutput();

			if (yaraScan)
			{
				if (!File.Exists(yaraConfigFile))
				{
					ReportOutput($"The yara configuration file path suppled does not exist: \"{yaraConfigFile}\".");
					return;
				}
				try
				{
					string loadJson = File.ReadAllText(yaraConfigFile);
					yaraConfiguration = JsonConvert.DeserializeObject<YaraScanConfiguration>(loadJson);
				}
				catch
				{
					ReportOutput("The yara configuration file must be a JSON file.");
					return;
				}
			}

			FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(
						CancellationToken.None,
						true, // Do not change this. If set to false, it will run on a thread, return immediately and exit, killing the thread.
						searchPath,
						searchMask,
						calcEntropy,
						yaraConfiguration,
						ReportOutput,
						Log.ToAll,
						ReportResults,
						Log.ExceptionMessage
					);

			ReportOutput("Beginning scan...");
			FileEnumerator.LaunchFileEnumerator(parameters);
		}

		private static List<Tuple<string, string>> GetFlags(string[] args)
		{
			List<Tuple<string, string>> results = new List<Tuple<string, string>>();

			foreach (string arg in args)
			{
				string argument = arg;

				string flag = "";
				string parameter = "";

				string currentCharacter = PopCharacter(ref argument);

				if (currentCharacter != "-")
				{
					return new List<Tuple<string, string>>();
				}

				flag = PopCharacter(ref argument).ToLower();

				currentCharacter = PopCharacter(ref argument);

				if (currentCharacter == ":")
				{
					parameter = argument;
				}

				results.Add(new Tuple<string, string>(flag, parameter));
			}

			return results;
		}

		// Removes the first character from the string and returns it. The source string is modified to exclude the returned character.
		private static string PopCharacter(ref string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return string.Empty;
			}

			string result = source[0].ToString();
			source = source.Substring(1);

			return result;
		}

		private static void ReportOutput(string message = "")
		{
			Log.ToUI(message);
		}

		private static void ReportResults(List<FailSuccessCount> counts)
		{
			foreach (FailSuccessCount count in counts)
			{
				count.ToStrings().ForEach(s => ReportOutput(s));
			}

			ReportOutput();
			ReportOutput("Scan completed!");
			ReportOutput("Exiting...");
		}
	}
}
