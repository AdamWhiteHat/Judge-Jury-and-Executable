using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Logging;
using SqlDataAccessLayer;
using SqliteDataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;

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
			ReportOutput("Usage:");
			ReportOutput();
			ReportOutput("-p:C:\\Windows             -  Search [p]ath");
			ReportOutput("-m:*.exe                  -  Search [m]ask");
			ReportOutput("-e                        -  Enable calculate [e]ntropy");
			ReportOutput("-y:\"C:\\Yara Filters.json\" -  [Y]ara filters file");
			ReportOutput("-l                        -  Use Sq[l]ite database instead");
			ReportOutput("                              (supply path to db file in");
			ReportOutput("                               place of connection string)");
			ReportOutput();
			ReportOutput("RULES:");
			ReportOutput(" - All arguments must start with a dash");
			ReportOutput(" - For flags (the part before the ':'), letter casing is ignored");
			ReportOutput(" - For paths and other arguments after the ':', casing is retained");
			ReportOutput(" - Do not uses spaces between the dash, the flag and the colon");
			ReportOutput(" - If your path (or other arguments following the ':') contain a space, you must surround it in quotes (see the yara filters example above))");
			ReportOutput();
			ReportOutput("Press any key to continue . . .");
			Console.ReadKey(true);
		}

		private static void Main(string[] args)
		{
			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				ReportOutput("ERROR: Connection string not set! Please set the SQL connection string in .config file.");
				ReportOutput();
				ReportOutput("Aborting...");
				return;
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
			bool sqliteDb = false;
			string yaraFiltersFile = "";
			List<YaraFilter> yaraFilters = new List<YaraFilter>();

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
						yaraFiltersFile = parameter;
						break;
					case "l":
						sqliteDb = true;
						break;
				}
			}

			ReportOutput();
			ReportOutput("Running with these parameters:");
			ReportOutput($"   Search [P]ath:       \"{searchPath}\"");
			ReportOutput($"   Search [M]ask:        {searchMask}");
			ReportOutput($"   Calulate [E]ntropy:   {calcEntropy}");
			if (yaraScan)
			{
				ReportOutput($"   [Y]ara filters file: \"{yaraFiltersFile}\"");
			}
			if (sqliteDb)
			{
				ReportOutput($"   Sq[l]ite DB          \"{connectionString}\"");
			}
			ReportOutput();

			if (string.IsNullOrWhiteSpace(searchPath))
			{
				ReportOutput("No search path provided!");
				ReportOutput("At a minimum, you must supply the -p flag with a path, e.g.:");
				ReportOutput("-p:\"C:\\Program Files\\BanzaiBuddy\"");
				ReportOutput();
				ReportOutput("Aborting...");
				return;
			}

			if (yaraScan)
			{
				if (!File.Exists(yaraFiltersFile))
				{
					ReportOutput($"The yara filters file path suppled does not exist: \"{yaraFiltersFile}\".");
					ReportOutput();
					ReportOutput("Aborting...");
					return;
				}
				try
				{
					string loadJson = File.ReadAllText(yaraFiltersFile);
					yaraFilters = JsonConvert.DeserializeObject<List<YaraFilter>>(loadJson);
				}
				catch
				{
					ReportOutput("The yara filters file must be a JSON file.");
					ReportOutput();
					ReportOutput("Aborting...");
					return;
				}
			}

			IDataPersistenceLayer dataPersistenceLayer;

			if (sqliteDb)
			{
				dataPersistenceLayer = new SqliteDataPersistenceLayer(connectionString);
			}
			else
			{
				dataPersistenceLayer = new SqlDataPersistenceLayer(connectionString);
			}

			FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(
						CancellationToken.None,
						true, // Do not change this. If set to false, it will run on a thread, return immediately and exit, killing the thread.
						searchPath,
						searchMask,
						calcEntropy,
						yaraFilters,
						dataPersistenceLayer,
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
