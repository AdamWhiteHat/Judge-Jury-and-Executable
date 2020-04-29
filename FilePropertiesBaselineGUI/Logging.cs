using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace FilePropertiesBaselineGUI
{
    public static class Logging
    {
        public static string ExceptionLogFilename = "Exceptions.log";
        public static string DebugLogFilename = "Debug.log";
        public static TextBox FormOutputControl = null;

        public static void LogExceptionMessage(string location, string commandText, Exception exception)
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
            LogOutput(toLog);
            ReportOutput("Exception logged to Exceptions.log");
        }

        public static void LogOutput(string message)
        {
            bool useDebugLog = BuildConfiguration.IsDebug();
            File.AppendAllText(useDebugLog ? DebugLogFilename : ExceptionLogFilename, message + Environment.NewLine);
        }

        public static void ReportOutput(string message = "")
        {
            if (FormOutputControl != null)
            {
                if (FormOutputControl.InvokeRequired)
                {
                    FormOutputControl.Invoke(new MethodInvoker(() => ReportOutput(message)));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        FormOutputControl.AppendText($"[{DateTime.Now.TimeOfDay.ToString()}] - " + message);
                    }
                    FormOutputControl.AppendText(Environment.NewLine);
                }
            }
        }

        public static class BuildConfiguration
        {
            private static bool _isDebug;

            static BuildConfiguration()
            {
                _isDebug = false;
                SetDebugFlag();
            }

            [Conditional("DEBUG")]
            private static void SetDebugFlag()
            {
                _isDebug = true;
            }

            public static bool IsRelease()
            {
                return !(_isDebug);
            }

            public static bool IsDebug()
            {
                return _isDebug;
            }
        }
    }
}
