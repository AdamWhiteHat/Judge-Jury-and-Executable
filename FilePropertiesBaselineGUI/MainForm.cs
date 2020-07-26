using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using DataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using System.Diagnostics;
using Logging;

namespace FilePropertiesBaselineGUI
{
	public partial class MainForm : Form
	{
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private CancellationToken cancelToken;
		private Toggle ProcessingToggle = null;
		private DateTime enumerationStart;

		public MainForm()
		{
			InitializeComponent();

			OutputTextBox = tbOutput;
			Log.LogOutputAction = LogOutput;
			SQLHelper.LogExceptionAction = Log.ExceptionMessage;
			ProcessingToggle = new Toggle(ActivationBehavior, DeactivationBehavior);

			panelYara.Enabled = checkBoxYaraRules.Checked;
			radioButtonYara_AlwaysRun.Checked = true;
			//listBoxYaraFilters.DisplayMember = "";
			//listBoxYaraFilters.ValueMember = "";

			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				Log.ToAll("ERROR: Connection string not set! Please set the SQL connection string in .config file. Browse button disabled.");
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

		private static TextBox OutputTextBox;

		public static void LogOutput(string message)
		{
			if (OutputTextBox != null)
			{
				if (OutputTextBox.InvokeRequired)
				{
					OutputTextBox.Invoke(new MethodInvoker(() => LogOutput(message)));
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(message))
					{
						if (OutputTextBox.Lines.Length > 200)
						{
							string[] lines = OutputTextBox.Lines.Skip(OutputTextBox.Lines.Length - 50).ToArray();
							OutputTextBox.Lines = lines;
						}
						OutputTextBox.AppendText($"[{DateTime.Now.TimeOfDay.ToString()}] - " + message);
					}
					OutputTextBox.AppendText(Environment.NewLine);
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (ProcessingToggle.IsActive)
			{
				ProcessingToggle.SetState(false);
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

		private void textbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (!ProcessingToggle.IsActive)
				{
					BeginScanning();
				}
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

		private void btnSearch_Click(object sender, EventArgs e)
		{
			BeginScanning();
		}

		private void BeginScanning()
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
				string selectedFolder = tbPath.Text;
				string searchPatterns = tbSearchPatterns.Text;

				List<YaraFilter> yaraParameters = new List<YaraFilter>();

				if (checkBoxYaraRules.Checked)
				{
					yaraParameters = currentYaraFilters.ToList();
				}

				FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(cancelToken, Settings.FileEnumeration_DisableWorkerThread, selectedFolder, searchPatterns, calculateEntropy, yaraParameters,
													Log.ToUI, Log.ToFile, ReportNumbers, Log.ExceptionMessage);

				tbOutput.AppendText(Environment.NewLine);
				Log.ToAll($"Beginning Enumeration of folder: \"{selectedFolder}\"");

				enumerationStart = DateTime.Now;

				bool didThrow = false;
				try
				{
					ThrowIfParametersInvalid(parameters);
				}
				catch (Exception ex)
				{
					didThrow = true;
					MessageBox.Show(ex.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if (!didThrow)
				{
					FileEnumerator.LaunchFileEnumerator(parameters);
				}
				else
				{
					BeginScanning();
				}
			}
		}

		private static void ThrowIfParametersInvalid(FileEnumeratorParameters parameters)
		{
			if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
			if (parameters.SearchPatterns == null) { throw new ArgumentNullException(nameof(parameters.SearchPatterns)); }
			if (parameters.ReportExceptionFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportExceptionFunction)); }
			if (parameters.ReportOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportOutputFunction)); }
			if (parameters.LogOutputFunction == null) { throw new ArgumentNullException(nameof(parameters.LogOutputFunction)); }
			if (parameters.ReportResultsFunction == null) { throw new ArgumentNullException(nameof(parameters.ReportResultsFunction)); }
			if (!Directory.Exists(parameters.SelectedFolder)) { throw new DirectoryNotFoundException(parameters.SelectedFolder); }
			if (parameters.CancelToken == null) { throw new ArgumentNullException(nameof(parameters.CancelToken), "If you do not want to pass a CancellationToken, then pass 'CancellationToken.None'"); }
			parameters.CancelToken.ThrowIfCancellationRequested();
		}

		private void ReportNumbers(List<FailSuccessCount> counts)
		{
			TimeSpan enumerationTimeSpan = DateTime.Now.Subtract(enumerationStart);

			foreach (FailSuccessCount count in counts)
			{
				Log.ToAll($"Succeeded: {count.SucceededCount} {count.Description}.");
			}
			foreach (FailSuccessCount count in counts)
			{
				Log.ToAll($"Failed: {count.FailedCount} {count.Description}.");
			}

			Log.ToAll($"Enumeration time: {enumerationTimeSpan.ToString()}");
			Log.ToAll();
			Log.ToAll("Enumeration finished!");

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

		#endregion

		#region Yara Shit

		private List<string> yaraMatchFiles = new List<string>();
		private List<string> yaraNoMatchFiles = new List<string>();
		private List<YaraFilter> currentYaraFilters = new List<YaraFilter>();

		private static string AddYaraRuleErrorCaption = "Error adding yara filter";

		private void checkBoxYaraRules_CheckedChanged(object sender, EventArgs e)
		{
			panelYara.Enabled = checkBoxYaraRules.Checked;

			if (!panelYara.Enabled)
			{
				currentYaraFilters.Clear();
				listBoxYaraFilters.Items.Clear();
			}
		}

		private void btnBrowseYaraMatch_Click(object sender, EventArgs e)
		{
			string[] selectedFiles = DialogHelper.BrowseForFilesDialog();

			if (selectedFiles.Any())
			{
				yaraMatchFiles = selectedFiles.ToList();
				tbYaraRuleMatchFiles.Text = string.Join(", ", yaraMatchFiles.Select(s => Path.GetFileName(s)));
			}
			else
			{
				yaraMatchFiles = new List<string>();
				tbYaraRuleMatchFiles.Text = "";
			}
		}

		private void btnBrowseYaraNoMatch_Click(object sender, EventArgs e)
		{
			string[] selectedFiles = DialogHelper.BrowseForFilesDialog();

			if (selectedFiles.Any())
			{
				yaraNoMatchFiles = selectedFiles.ToList();
				tbYaraRuleNoMatchFiles.Text = string.Join(", ", yaraNoMatchFiles.Select(s => Path.GetFileName(s)));
			}
			else
			{
				yaraNoMatchFiles = new List<string>();
				tbYaraRuleNoMatchFiles.Text = "";
			}
		}

		private void btnAddYaraFilter_Click(object sender, EventArgs e)
		{
			if (!yaraMatchFiles.Any())
			{
				MessageBox.Show("Must have at least one 'IF rule match files' selected.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			YaraFilterType filterType = YaraFilterType.AlwaysRun;
			string filterValue = string.Empty;

			if (radioButtonYara_AlwaysRun.Checked)
			{
				filterType = YaraFilterType.AlwaysRun;
			}
			else if (radioButtonYara_IsPeFile.Checked)
			{
				filterType = YaraFilterType.IsPeFile;
			}
			else if (radioButtonYara_Extention.Checked)
			{
				filterType = YaraFilterType.FileExtension;
				if (string.IsNullOrWhiteSpace(tbYaraFilterValue.Text))
				{
					MessageBox.Show("'Yara filter value' cannot be empty when 'By Extention' is selected.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				filterValue = tbYaraFilterValue.Text;
			}
			else if (radioButtonYara_MimeType.Checked)
			{
				filterType = YaraFilterType.MimeType;
				if (string.IsNullOrWhiteSpace(tbYaraFilterValue.Text))
				{
					MessageBox.Show("'Yara filter value' cannot be empty when 'By MIME Type' is selected.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				filterValue = tbYaraFilterValue.Text;
			}

			YaraFilter yaraFilter = new YaraFilter(filterType, filterValue, yaraMatchFiles, yaraNoMatchFiles);

			if (currentYaraFilters.Contains(yaraFilter))
			{
				MessageBox.Show("Yara filter already exists. Duplicate filter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			currentYaraFilters.Add(yaraFilter);

			UpdateYaraFilterListbox();
		}

		private void btnRemoveYaraFilter_Click(object sender, EventArgs e)
		{
			listBoxYaraFilters_RemoveSelected();
		}

		private void listBoxYaraFilters_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				listBoxYaraFilters_RemoveSelected();
			}
		}

		private void listBoxYaraFilters_RemoveSelected()
		{
			if (listBoxYaraFilters.SelectedIndex != -1)
			{
				var filter = currentYaraFilters.Where(yf => yf.ToString() == listBoxYaraFilters.SelectedItem.ToString()).Single();
				currentYaraFilters.Remove(filter);
				UpdateYaraFilterListbox();
			}
		}

		private void UpdateYaraFilterListbox()
		{
			listBoxYaraFilters.SuspendLayout();

			listBoxYaraFilters.Items.Clear();

			foreach (YaraFilter filter in currentYaraFilters)
			{
				listBoxYaraFilters.Items.Add(filter.ToString());
			}

			listBoxYaraFilters.ResumeLayout();
		}

		private void btnYaraSave_Click(object sender, EventArgs e)
		{
			string selectedFile = DialogHelper.SaveFileDialog();

			if (!string.IsNullOrWhiteSpace(selectedFile))
			{
				JsonSerialization.Save.Object(currentYaraFilters, selectedFile);
			}
		}

		private void btnYaraLoad_Click(object sender, EventArgs e)
		{
			string selectedFile = DialogHelper.BrowseForFileDialog();

			if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
			{
				currentYaraFilters = JsonSerialization.Load.Generic<List<YaraFilter>>(selectedFile);
				UpdateYaraFilterListbox();
			}
		}

		#endregion

	}
}
