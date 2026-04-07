using System;
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
using FilePropertiesDataObject.Helpers;
using FilePropertiesDataObject.Parameters;

namespace JudgeJuryAndExecutableConsole
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
            ReportOutput("[REQUIRED]:");
            ReportOutput("-fp:C:\\Windows             -  [F]ile [p]ath");
            ReportOutput("-fm:*.exe                  -  [F]ile [m]ask");
            ReportOutput();
            ReportOutput("[REQUIRED (Pick one)]:");
            ReportOutput($"-os                        -  [O]utput to [S]QL server (supply connection string in file: {_thisExecutableFilename}.config");
            ReportOutput("-ol:C:\\scan001.db          -  [O]utput to SQ[L]ite database");
            ReportOutput("-oc:C:\\scan001.csv         -  [O]utput to [C]SV file");
            ReportOutput();
            ReportOutput("[OPTIONAL]:");
            ReportOutput("-lq                        -  [L]ogging [Q]uiet. Logs only when there is an important event, like an error.");
            ReportOutput("-ls                        -  [L]ogging [S]tandard. Print every file scanned to console output (stdout); one line per file.");
            ReportOutput("-lv                        -  [L]ogging [V]erbose. Reports when entering and exiting each section (timing metric category); 14-19 lines per file.");
            ReportOutput();
            ReportOutput("[OPTIONAL]:");
            ReportOutput("-y:\"C:\\Yara Filters.json\" -  [Y]ARA filters file");
            ReportOutput("-e                        -  Enable calculating [e]ntropy");
            ReportOutput();
            ReportOutput("RULES:");
            ReportOutput(" - All arguments must start with a dash.");
            ReportOutput(" - For flags (the part before the ':'), letter casing is ignored.");
            ReportOutput(" - For paths and other arguments after the ':', casing is retained.");
            ReportOutput(" - Do not uses spaces between the dash, the flag and the colon.");
            ReportOutput(" - If your path (or other arguments following the ':') contain a space, you must surround it in quotes (see the YARA filters example above).");
            ReportOutput(" - You must supply a file path. You may also supply a file mask as well. Or not.");
            ReportOutput(" - If no output parameter is supplied, it will default to a SQL server connection. In that case, a connection string MUST be supplied in this executable's config file.");
            ReportOutput();
            ReportOutput("Press any key to exit . . .");
            Console.ReadKey(true);
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsageSyntax();
                return;
            }

            // Will hold flag & parameter to flag, such as: "-fp", "C:\Windows\"
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
            LoggingVerbosity loggingVerbosity = LoggingVerbosity.Standard;
            string sqliteDbFile = "";
            string csvFile = "";
            string sqlConnectionString = (Settings.Database_ConnectionString == "SetMe") ? "" : Settings.Database_ConnectionString;
            string yaraFiltersFile = "";
            List<YaraFilter> yaraFilters = new List<YaraFilter>();
            Action<string> reportOutputFunction = new Action<string>((s) => { return; });

            foreach (Tuple<string, string> flagTuple in flags)
            {
                string flag = flagTuple.Item1;
                string parameter = flagTuple.Item2;

                switch (flag)
                {
                    case "fp":
                        searchPath = parameter;
                        break;
                    case "fm":
                        searchMask = parameter;
                        break;

                    case "os":
                        isSqlServerEnabled = true;
                        break;
                    case "ol":
                        isSqliteEnabled = true;
                        sqliteDbFile = string.IsNullOrWhiteSpace(parameter) ? sqlConnectionString : parameter;
                        break;
                    case "oc":
                        isCsvEnabled = true;
                        csvFile = parameter;
                        break;

                    case "lq":
                        loggingVerbosity = LoggingVerbosity.Quiet;
                        reportOutputFunction = ReportOutput;
                        break;
                    case "ls":
                        loggingVerbosity = LoggingVerbosity.Standard;
                        reportOutputFunction = ReportOutput;
                        break;
                    case "lv":
                        loggingVerbosity = LoggingVerbosity.Verbose;
                        reportOutputFunction = ReportOutput;
                        break;

                    case "e":
                        isEntropyEnabled = true;
                        break;
                    case "y":
                        isYaraEnabled = true;
                        yaraFiltersFile = parameter;
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(searchPath))
            {
                ReportOutput("No [F]ile [p]ath provided!");
                ReportOutput("You must supply the -fp flag with a path, e.g.:");
                ReportOutput("-fp:\"C:\\Program Files\\BanzaiBuddy\"");
                ReportOutput();
                ReportOutput("Aborting...");
                return;
            }
            if (!Directory.Exists(searchPath))
            {
                ReportOutput("Path does not exist!");
                ReportOutput($"[F]ile [p]ath provided: {searchPath}");
                ReportOutput();
                ReportOutput("Aborting...");
                return;
            }

            if (isYaraEnabled)
            {
                if (!File.Exists(yaraFiltersFile))
                {
                    ReportOutput($"The [Y]ARA filters file path supplied does not exist: \"{yaraFiltersFile}\".");
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
                    ReportOutput("The [Y]ARA filters file must be a JSON file.");
                    ReportOutput();
                    ReportOutput("Aborting...");
                    return;
                }
            }

            IDataPersistenceLayer dataPersistenceLayer;

            if (isSqliteEnabled)
            {
                string saveFilename = sqliteDbFile;
                if (File.Exists(saveFilename))
                {
                    saveFilename = SequentialFilename.GetNextAvailable(saveFilename);
                    ReportOutput($"A file already exists at the supplied output location. [O]utput SQ[L]ite filename renamed to: {Path.GetFileName(saveFilename)}");
                }

                dataPersistenceLayer = new SqliteDataPersistenceLayer(saveFilename);
            }
            else if (isCsvEnabled)
            {
                string saveFilename = csvFile;
                if (File.Exists(saveFilename))
                {
                    saveFilename = SequentialFilename.GetNextAvailable(saveFilename);
                    ReportOutput($"A file already exists at the supplied output location. [O]utput [C]SV filename renamed to: {Path.GetFileName(saveFilename)}");
                }

                dataPersistenceLayer = new CsvDataPersistenceLayer(saveFilename);
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
                    ReportOutput("(SQL server connection string supplied in config file, assuming SQL server output...)");
                    dataPersistenceLayer = new SqlDataPersistenceLayer(sqlConnectionString);
                }
                else
                {
                    ReportOutput("You must supply an output parameter, e.g.:");
                    ReportOutput("-oc:C:\\out.csv");
                    ReportOutput($"OR provide a SQL server connection string in the config file: {_thisExecutableFilename}");
                    ReportOutput("(Because it defaults to a SQL server connection. However, the connection string was missing.)");
                    ReportOutput();
                    ReportOutput("Aborting...");
                    return;
                }
            }

            ReportOutput();
            ReportOutput("Running with these parameters:");

            if (!string.IsNullOrWhiteSpace(searchPath))
            {
                ReportOutput($"   [F]ile [P]ath:       \"{searchPath}\"");
            }
            if (!string.IsNullOrWhiteSpace(searchMask))
            {
                ReportOutput($"   [F]ile [M]ask:        {searchMask}");
            }

            if (isSqlServerEnabled)
            {
                ReportOutput($"   [O]utput [S]QL connection: \"{sqlConnectionString}\"");
            }
            else if (isSqliteEnabled)
            {
                ReportOutput($"   [O]utput SQ[L]ite DB: \"{sqliteDbFile}\"");
            }
            else if (isCsvEnabled)
            {
                ReportOutput($"   [O]utput [C]SV file: \"{csvFile}\"");
            }

            switch (loggingVerbosity)
            {
                case LoggingVerbosity.Quiet:
                    ReportOutput("   [L]ogging [Q]uiet");
                    break;
                case LoggingVerbosity.Standard:
                    ReportOutput("   [L]ogging [S]tandard");
                    break;
                case LoggingVerbosity.Verbose:
                    ReportOutput("   [L]ogging [V]erbose");
                    break;
            }

            if (isYaraEnabled)
            {
                ReportOutput($"   [Y]ARA filters file: \"{yaraFiltersFile}\"");
            }
            if (isEntropyEnabled)
            {
                ReportOutput($"   Calculate [E]ntropy:   Enabled");
            }

            ReportOutput();

            FileEnumeratorParameters parameters =
                    new FileEnumeratorParameters(
                        CancellationToken.None,
                        true, // Do not change this. If set to false, it will run on a thread, return immediately and exit, killing the thread.
                        searchPath,
                        searchMask,
                        isEntropyEnabled,
                        yaraFilters,
                        dataPersistenceLayer,
                        reportOutputFunction, // reportOutputFunction
                        Log.ToFile, // logOutputFunction
                        ReportResults, // reportResultsFunction
                        Log.ExceptionMessage, // reportExceptionFunction
                        loggingVerbosity
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

                if (string.IsNullOrWhiteSpace(argument))
                {
                    return new List<Tuple<string, string>>();
                }

                int colonIndex = argument.IndexOf(':');
                if (colonIndex == -1)
                {
                    results.Add(new Tuple<string, string>(argument, string.Empty));
                    continue;
                }

                flag = argument.Substring(0, colonIndex).ToLowerInvariant();
                parameter = argument.Substring(colonIndex + 1);

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
            foreach (string line in report.Timings)
            {
                Log.ToAll(line);
            }

            ReportOutput();
            ReportOutput("Scan completed!");
            ReportOutput("Exiting...");
        }
    }
}
