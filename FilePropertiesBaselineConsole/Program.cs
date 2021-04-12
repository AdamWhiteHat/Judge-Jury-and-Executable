﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

using Newtonsoft.Json;

using Logging;
using SqlDataAccessLayer;
using CsvDataAccessLayer;
using SqliteDataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;

namespace FilePropertiesBaselineConsole
{
	sealed class Program
	{
		private static string _thisExecutableFilename;

		static Program()
		{
			Log.LogOutputAction = Console.WriteLine;
			SQLHelper.LogExceptionAction = Log.ExceptionMessage;

			Assembly thisAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			_thisExecutableFilename = Path.GetFileName(thisAssembly.Location);
		}

		private static void DisplayUsageSyntax()
		{
			ReportOutput("Usage:");
			ReportOutput();
			ReportOutput("-p:C:\\Windows             -  Search [p]ath");
			ReportOutput("-m:*.exe                  -  Search [m]ask");
			ReportOutput("-e                        -  Enable calculate [e]ntropy");
			ReportOutput("-y:\"C:\\Yara Filters.json\" -  [Y]ara filters file");
			ReportOutput("-l:C:\\scan001.db          -  Output a Sq[l]ite database");
			ReportOutput("-c:C:\\scan001.csv         -  Output a [C]SV file");
			ReportOutput($"-s                        -  Output to [S]QL server (supply connection string in file: {_thisExecutableFilename}.config");
			ReportOutput();
			ReportOutput("RULES:");
			ReportOutput(" - All arguments must start with a dash.");
			ReportOutput(" - For flags (the part before the ':'), letter casing is ignored.");
			ReportOutput(" - For paths and other arguments after the ':', casing is retained.");
			ReportOutput(" - Do not uses spaces between the dash, the flag and the colon.");
			ReportOutput(" - If your path (or other arguments following the ':') contain a space, you must surround it in quotes (see the yara filters example above).");
			ReportOutput(" - If no output parameter is suppled, it will default to a SQL server connection. In that case, a connection string MUST be suppled in this executable's config file.");
			ReportOutput();
			ReportOutput("Press any key to continue . . .");
			Console.ReadKey(true);
		}

		private static void Main(string[] args)
		{
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
			bool isEntropyEnabled = false;
			bool isYaraEnabled = false;
			bool isSqlServerEnabled = false;
			bool isSqliteEnabled = false;
			bool isCsvEnabled = false;
			string sqliteDbFile = "";
			string csvFile = "";
			string sqlConnectionString = (Settings.Database_ConnectionString == "SetMe") ? "" : Settings.Database_ConnectionString;
			string yaraFiltersFile = "";
			List<YaraFilter> yaraFilters = new List<YaraFilter>();

			foreach (Tuple<string, string> flagTuple in flags)
			{
				string flag = flagTuple.Item1;
				string parameter = flagTuple.Item2;

				switch (flag)
				{
					case "e":
						isEntropyEnabled = true;
						break;
					case "p":
						searchPath = parameter;
						break;
					case "m":
						searchMask = parameter;
						break;
					case "y":
						isYaraEnabled = true;
						yaraFiltersFile = parameter;
						break;
					case "s":
						isSqlServerEnabled = true;
						break;
					case "l":
						isSqliteEnabled = true;
						sqliteDbFile = string.IsNullOrWhiteSpace(parameter) ? sqlConnectionString : parameter;
						break;
					case "c":
						isCsvEnabled = true;
						csvFile = parameter;
						break;
				}
			}

			ReportOutput();
			ReportOutput("Running with these parameters:");
			ReportOutput($"   Search [P]ath:       \"{searchPath}\"");
			ReportOutput($"   Search [M]ask:        {searchMask}");
			ReportOutput($"   Calulate [E]ntropy:   {isEntropyEnabled}");
			if (isYaraEnabled)
			{
				ReportOutput($"   [Y]ara filters file: \"{yaraFiltersFile}\"");
			}

			if (isSqlServerEnabled)
			{
				ReportOutput($"   [S]QL connection: \"{sqlConnectionString}\"");
			}
			else if (isSqliteEnabled)
			{
				ReportOutput($"   Sq[l]ite DB: \"{sqliteDbFile}\"");
			}
			else if (isCsvEnabled)
			{
				ReportOutput($"   [C]SV file: \"{csvFile}\"");
			}

			ReportOutput();

			if (string.IsNullOrWhiteSpace(searchPath))
			{
				ReportOutput("No search path provided!");
				ReportOutput("You must supply the -p flag with a path, e.g.:");
				ReportOutput("-p:\"C:\\Program Files\\BanzaiBuddy\"");
				ReportOutput();
				ReportOutput("Aborting...");
				return;
			}

			if (isYaraEnabled)
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

			if (isSqliteEnabled)
			{
				dataPersistenceLayer = new SqliteDataPersistenceLayer(sqliteDbFile);
			}
			else if (isCsvEnabled)
			{
				dataPersistenceLayer = new CsvDataPersistenceLayer(csvFile);
			}
			else if (isSqlServerEnabled)
			{
				dataPersistenceLayer = new SqlDataPersistenceLayer(sqlConnectionString);
			}
			else
			{
				ReportOutput("No output parameter provided!");

				if (!string.IsNullOrWhiteSpace(sqlConnectionString))
				{
					ReportOutput("(SQL server connection string supplied in config file, asuming SQL server output...)");
					dataPersistenceLayer = new SqlDataPersistenceLayer(sqlConnectionString);
				}
				else
				{
					ReportOutput("You must supply an output parameter, e.g.:");
					ReportOutput("-c:C:\\out.csv");
					ReportOutput($"OR provide a SQL server connection string in the config file: {_thisExecutableFilename}");
					ReportOutput("(Because it defaults to a SQL server connection. However, the connection string was missing.)");
					ReportOutput();
					ReportOutput("Aborting...");
					return;
				}
			}

			FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(
						CancellationToken.None,
						true, // Do not change this. If set to false, it will run on a thread, return immediately and exit, killing the thread.
						searchPath,
						searchMask,
						isEntropyEnabled,
						yaraFilters,
						dataPersistenceLayer,
						ReportOutput,
						Log.LogOutputAction,
						ReportResults,
						Log.ExceptionMessage
					);

			parameters.ThrowIfAnyParametersInvalid();

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

		private static void ReportResults(FileEnumeratorReport report)
		{
			foreach (FailSuccessCount count in report.Counts)
			{
				count.ToStrings().ForEach(s => Log.ToAll(s));
			}
			Log.ToAll();
			foreach (string line in report.Timings.GetReport())
			{
				Log.ToAll(line);
			}

			ReportOutput();
			ReportOutput("Scan completed!");
			ReportOutput("Exiting...");
		}
	}
}
