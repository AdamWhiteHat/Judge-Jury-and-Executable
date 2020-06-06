using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Logging
{
	public static class Log
	{
		public static string ExceptionLogFilename = "Exceptions.log";
		public static string DebugLogFilename = "Debug.log";
		public static string RuntimeLogFilename;

		public static Action<string> LogOutputAction;

		static Log()
		{
			RuntimeLogFilename = BuildConfiguration.IsDebug() ? DebugLogFilename : ExceptionLogFilename;
		}

		public static void ExceptionMessage(string location, string commandText, Exception exception)
		{
			string cmdTextLine = string.Empty;

			if (!string.IsNullOrWhiteSpace(commandText))
			{
				cmdTextLine = $"Exception.SQL.CommandText: \"{commandText}\"";
			}

			string stackTrace = "";
			string exMessage = "";
			string exTypeName = "";

			if (exception != null)
			{
				if (!string.IsNullOrWhiteSpace(exception.Message))
				{
					exMessage = exception.Message;
				}

				if (exception.StackTrace != null)
				{
					stackTrace = $"    Exception.StackTrace = {Environment.NewLine}    {{{Environment.NewLine}        {exception.StackTrace.Replace("\r\n", "\r\n     ")}    }}{Environment.NewLine}";
				}

				exTypeName = exception?.GetType()?.FullName ?? "";
			}

			string loc = "";

			if (!string.IsNullOrWhiteSpace(location))
			{
				loc = location;
			}

			string[] lines =
			{
				"Exception.Information = ",
				"[",
				$"    Exception.Location (Name of function exception was thrown in): \"{loc}\"",
				$"    Exception.Type: \"{exTypeName}\"",
				$"    Exception.Message: \"{exMessage}\"",
				$"{stackTrace}",
				 cmdTextLine,
				"]" +
				" ",
				"---",
				" "
			};

			string toLog = string.Join(Environment.NewLine, lines);
			ToFile(toLog);
			ToUI($"Exception logged to: {RuntimeLogFilename}");
		}

		public static void ToAll(string message = "")
		{
			ToFile(message);
			ToUI(message);
		}

		public static void ToFile(string message)
		{
			File.AppendAllText(RuntimeLogFilename, message + Environment.NewLine);
		}

		public static void ToUI(string message = "")
		{
			if (LogOutputAction != null)
			{
				LogOutputAction.Invoke(message);
			}
		}
	}
}
