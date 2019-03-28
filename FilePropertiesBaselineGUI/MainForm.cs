using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using DataAccessLayer;
using FilePropertiesDataObject;
using FilePropertiesEnumerator;
using System.Diagnostics;

namespace FilePropertiesBaselineGUI
{
	public partial class MainForm : Form
	{
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private CancellationToken cancelToken;
		private static TextBox outputControl = null;
		private Toggle ProcessingToggle = null;
		private DateTime enumerationStart;

		public MainForm()
		{
			InitializeComponent();

			outputControl = tbOutput;
			SQLHelper.LogExceptionAction = LogExceptionMessage;
			ProcessingToggle = new Toggle(ActivationBehavior, DeactivationBehavior);

			panelYara.Enabled = checkBoxYaraRules.Checked;

			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				ReportOutput("ERROR: Connection string not set! Please set the SQL connection string in .config file. Browse button disabled.");
				btnBrowse.Enabled = false;
			}
			else
			{
				FilePropertiesAccessLayer.SetConnectionString(connectionString);
			}

			if (!string.IsNullOrWhiteSpace(Settings.GUI_DefaultFolder))
			{
				tbPath.Text = Settings.GUI_DefaultFolder;
			}

			if (!string.IsNullOrWhiteSpace(Settings.GUI_SearchPattern))
			{
				tbSearchPatterns.Text = Settings.GUI_SearchPattern;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (ProcessingToggle.IsActive)
			{
				ProcessingToggle.SetState(false);
			}
		}

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

		private static string ExceptionLogFilename = "Exceptions.log";
		private static string DebugLogFilename = "Debug.log";
		private static void LogOutput(string message)
		{
			bool useDebugLog = message.Contains("MFT File:");

			File.AppendAllText(useDebugLog ? DebugLogFilename : ExceptionLogFilename, message + Environment.NewLine);
		}

		public static void ReportOutput(string message = "")
		{
			if (outputControl != null)
			{
				if (outputControl.InvokeRequired)
				{
					outputControl.Invoke(new MethodInvoker(() => ReportOutput(message)));
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(message))
					{
						outputControl.AppendText($"[{DateTime.Now.TimeOfDay.ToString()}] - " + message);
					}
					outputControl.AppendText(Environment.NewLine);
				}
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			string selectedFolder = DialogHelper.BrowseForFolderDialog(tbPath.Text);

			if (Directory.Exists(selectedFolder))
			{
				tbPath.Text = selectedFolder;
			}
		}

		private void tbOutput_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.A)
				{
					tbOutput.SelectAll();
				}
			}
		}

		private void checkBoxYaraRules_CheckedChanged(object sender, EventArgs e)
		{
			panelYara.Enabled = checkBoxYaraRules.Checked;
		}

		private void btnBrowseYara_Click(object sender, EventArgs e)
		{
			string selectedFile = DialogHelper.BrowseForFileDialog(tbYaraRuleFile.Text);

			if (File.Exists(selectedFile))
			{
				tbYaraRuleFile.Text = selectedFile;
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (ProcessingToggle.IsActive)
			{
				btnSearch.Enabled = false;
				ProcessingToggle.SetState(false);
			}
			else
			{
				btnSearch.Text = "Cancel";
				ProcessingToggle.SetState(true);

				bool calculateEntropy = checkboxCalculateEntropy.Checked;
				bool onlineCertValidation = checkboxOnlineCertValidation.Checked;
				string selectedFolder = tbPath.Text;
				string searchPatterns = tbSearchPatterns.Text;

				string yaraRulesPath = "";

				if (checkBoxYaraRules.Checked)
				{
					yaraRulesPath = tbYaraRuleFile.Text;
				}

				FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(cancelToken, Settings.FileEnumeration_DisableWorkerThread, selectedFolder, searchPatterns, calculateEntropy, onlineCertValidation, yaraRulesPath,
													ReportOutput, LogOutput, ReportNumbers, LogExceptionMessage);



				tbOutput.AppendText(Environment.NewLine);
				ReportOutput($"Beginning Enumeration of folder: \"{selectedFolder}\"");

				enumerationStart = DateTime.Now;

				FileEnumerator.LaunchFileEnumerator(parameters);
			}
		}

		private void ReportNumbers(List<FailSuccessCount> counts)
		{
			TimeSpan enumerationTimeSpan = DateTime.Now.Subtract(enumerationStart);

			foreach (FailSuccessCount count in counts)
			{
				count.ToStrings().ForEach(s => ReportOutput(s));
			}
			ReportOutput($"Enumeration time: {enumerationTimeSpan.ToString()}");
			ReportOutput();
			ReportOutput("Enumeration finished!");

			ProcessingToggle.SetState(false);
			EnableControls();
		}

		#region Is Processing Toggle Members

		private void ActivationBehavior()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(() => ActivationBehavior()));
			}
			else
			{
				panel1.Enabled = false;
				btnSearch.Text = "Cancel";

				cancelTokenSource = new CancellationTokenSource();
				cancelToken = cancelTokenSource.Token;
			}
		}

		private void DeactivationBehavior()
		{
			cancelTokenSource.Cancel();
			EnableControls();
		}

		private void EnableControls()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(() => EnableControls()));
			}
			else
			{
				panel1.Enabled = true;
				btnSearch.Text = "Search";
				btnSearch.Enabled = true;
			}
		}

		private class Toggle
		{
			public bool IsActive { get; private set; }

			private Action ActivationBehavior = null;
			private Action DeactivationBehavior = null;

			public Toggle(Action activationBehavior, Action deactivationBehavior)
			{
				if (activationBehavior == null) throw new ArgumentNullException(nameof(activationBehavior));
				if (deactivationBehavior == null) throw new ArgumentNullException(nameof(deactivationBehavior));

				IsActive = false;
				ActivationBehavior = activationBehavior;
				DeactivationBehavior = deactivationBehavior;
			}

			public void ToggleState()
			{
				IsActive = !IsActive;
				OnToggle();
			}

			public void SetState(bool value)
			{
				bool previousState = IsActive;
				IsActive = value;

				if (previousState != value)
				{
					OnToggle();
				}
			}

			private void OnToggle()
			{
				if (IsActive)
				{
					ActivationBehavior.Invoke();
				}
				else
				{
					DeactivationBehavior.Invoke();
				}
			}
		}

		#endregion


	}
}
