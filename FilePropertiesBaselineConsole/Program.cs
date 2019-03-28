using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using DataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;

namespace FilePropertiesBaselineConsole
{
	sealed class Program
	{
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
				return;
			}

			string searchPath = "";
			string searchMask = "*.*";
			bool calcEntropy = false;
			bool onlineValidation = false;
			string yaraRulesFile = "";

			foreach (Tuple<string, string> flagTuple in flags)
			{
				string flag = flagTuple.Item1;
				string parameter = flagTuple.Item2;

				switch (flag)
				{
					case "e":
						calcEntropy = true;
						break;
					case "v":
						onlineValidation = true;
						break;
					case "p":
						searchPath = parameter;
						break;
					case "m":
						searchMask = parameter;
						break;
					case "y":
						yaraRulesFile = parameter;
						break;
				}
			}

			ReportOutput($"Search [P]ath: \"{searchPath}\"");
			ReportOutput($"Search [M]ask: {searchMask}");
			ReportOutput($"Calulate [E]ntropy: {calcEntropy}");
			ReportOutput($"Online [V]alidation: {onlineValidation}");
			ReportOutput($"[Y]ara Rules File: \"{yaraRulesFile}\"");
			ReportOutput("");

			FileEnumeratorParameters parameters = new FileEnumeratorParameters(CancellationToken.None, Settings.FileEnumeration_DisableWorkerThread, searchPath, searchMask, calcEntropy, onlineValidation, yaraRulesFile, ReportOutput, LogOutput, ReportResults, ReportException);

			ReportOutput("Beginning enumeration...");
			FileEnumerator.LaunchFileEnumerator(parameters);
		}

		private static List<Tuple<string, string>> GetFlags(string[] args)
		{
			List<Tuple<string, string>> result = new List<Tuple<string, string>>();

			foreach (string arg in args)
			{
				string argument = arg;

				string flag = "";
				string parameter = "";

				string currentCharacter = PopCharacter(ref argument);

				if (currentCharacter != "-")
				{
					DisplayUsageSyntax();
					return new List<Tuple<string, string>>();
				}

				flag = PopCharacter(ref argument).ToLower();

				currentCharacter = PopCharacter(ref argument);

				if (currentCharacter == ":")
				{
					parameter = argument;
				}

				result.Add(new Tuple<string, string>(flag, parameter));
			}

			return result;
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

		private static void DisplayUsageSyntax()
		{
			ReportOutput();
			ReportOutput("-p:C:\\Windows        -   Search [p]ath");
			ReportOutput("-m:*.exe             -   Search [m]ask");
			ReportOutput("-e                   -   Enable calulate [e]ntropy");
			ReportOutput("-v                   -   Enable online [v]alidation");
			ReportOutput("-y:C:\\YaraRules.yar  -   [Y]ara rules file");
		}

		private static void ReportOutput(string message = "")
		{
			Console.WriteLine(message);
		}

		private static void ReportResults(List<FailSuccessCount> counts)
		{
			foreach (FailSuccessCount count in counts)
			{
				count.ToStrings().ForEach(s => ReportOutput(s));
			}

			ReportOutput();
			ReportOutput("Enumeration finished!");
		}

		private static void ReportException(string location, string commandText, Exception exception)
		{
			string cmdTextLine = string.Empty;

			if (!string.IsNullOrWhiteSpace(commandText))
			{
				cmdTextLine = $"Exception.SQL.CommandText: \"{commandText}\"";
			}

			string stackTrace = "";

			if (exception.StackTrace != null)
			{
				stackTrace = $"    Exception.StackTrace = {Environment.NewLine}    {{{Environment.NewLine}        {exception.StackTrace.Replace("\r\n", "\r\n     ")}    }}{Environment.NewLine}";
			}

			string[] lines =
			{
				"Exception.Information = ",
				"[",
				$"    Exception.Location (Name of function exception was thrown in): \"{location}\"",
				$"    Exception.Type: \"{exception.GetType().FullName}\"",
				$"    Exception.Message: \"{exception.Message}\"",
				$"{stackTrace}",
				 cmdTextLine,
				"]" +
				" ",
				"---",
				" "
			};

			string toLog = string.Join(Environment.NewLine, lines);
			LogOutput(toLog);
			ReportOutput("Exception logged to Exceptions.log");
		}

		private static void LogOutput(string message)
		{
			File.AppendAllText("Exceptions.log", message + Environment.NewLine);
		}
	}
}
