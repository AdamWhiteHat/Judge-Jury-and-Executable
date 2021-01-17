using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Logging;
using CsvDataAccessLayer;
using SqlDataAccessLayer;
using SqliteDataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using Microsoft.Data.ConnectionUI;

namespace FilePropertiesBaselineGUI
{
	public partial class MainForm : Form
	{
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private CancellationToken cancelToken;
		private Toggle ProcessingToggle = null;
		private DateTime enumerationStart;

		private int mainformWidth_Previous = 0;
		private int mainformMinimumHeight_Collapsed = 0;
		private int mainformMinimumHeight_Expanded = 0;
		private int mainformHeight_Collapsed = 0;
		private int mainformHeight_Expanded = 0;
		private int yaraPanelHeight_Expanded = 0;
		private int yaraPanelHeightDifference = 0;

		private static int yaraPanelHeight_Collapsed = 10;
		private static TextBox OutputTextBox;

		public MainForm()
		{
			InitializeComponent();
			this.Shown += new EventHandler((s, e) => { checkBoxYaraRules.CheckState = CheckState.Unchecked; });

			#region Static members & methods initialization

			OutputTextBox = tbOutput;
			Log.LogOutputAction = LogOutput;
			SQLHelper.LogExceptionAction = Log.ExceptionMessage;
			ProcessingToggle = new Toggle(ActivationBehavior, DeactivationBehavior, ResetBehavior);

			#endregion

			#region Settings initialization

			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				//Log.ToAll("ERROR: Connection string not set! Please set the SQL connection //string in .config file. Browse button disabled.");
				//btnBrowse.Enabled = false;
				//btnSearch.Enabled = false;
				radioPersistenceSqlite.Checked = true;
			}
			else
			{
				radioPersistenceSqlServer.Checked = true;
				tbPersistenceParameter.Text = connectionString;
			}

			if (!string.IsNullOrWhiteSpace(Settings.GUI_DefaultFolder))
			{
				tbPath.Text = Settings.GUI_DefaultFolder;
			}

			if (!string.IsNullOrWhiteSpace(Settings.GUI_SearchPattern))
			{
				tbSearchPatterns.Text = Settings.GUI_SearchPattern;
			}

			#endregion

			#region Yara panel members initialization

			yaraPanelHeight_Expanded = panelYaraParameters.Height;
			yaraPanelHeightDifference = yaraPanelHeight_Expanded - yaraPanelHeight_Collapsed;

			mainformHeight_Expanded = this.Height;
			mainformHeight_Collapsed = this.MinimumSize.Height;

			mainformMinimumHeight_Collapsed = this.MinimumSize.Height;
			mainformMinimumHeight_Expanded = this.MinimumSize.Height + yaraPanelHeightDifference;

			mainformWidth_Previous = this.Width;

			radioButtonYara_AlwaysRun.Checked = true;

			#endregion

		}

		#region Search/Cancel/Reset Behavior

		private void btnSearch_Click(object sender, EventArgs e)
		{
			BeginScanning();
		}

		private void textbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (ProcessingToggle.CurrentState == ToggleState.Ready)
				{
					BeginScanning();
				}
			}
		}

		private void BeginScanning()
		{
			if (ProcessingToggle.CurrentState == ToggleState.Active)
			{
				btnSearch.Enabled = false;
				ProcessingToggle.SetState(ToggleState.Inactive);
			}
			else if (ProcessingToggle.CurrentState == ToggleState.Ready)
			{
				btnSearch.Enabled = false;
				ProcessingToggle.SetState(ToggleState.Active);

				bool calculateEntropy = checkboxCalculateEntropy.Checked;
				string selectedFolder = tbPath.Text;
				string searchPatterns = tbSearchPatterns.Text;

				List<YaraFilter> yaraParameters = new List<YaraFilter>();
				if (checkBoxYaraRules.Checked)
				{
					yaraParameters = currentYaraFilters.ToList();
				}

				IDataPersistenceLayer dataPersistenceLayer = null;
				if (radioPersistenceCSV.Checked)
				{
					dataPersistenceLayer = new CsvDataPersistenceLayer(tbPersistenceParameter.Text);
				}
				else if (radioPersistenceSqlite.Checked)
				{
					dataPersistenceLayer = new SqliteDataPersistenceLayer(tbPersistenceParameter.Text);
				}
				else if (radioPersistenceSqlServer.Checked)
				{
					dataPersistenceLayer = new SqlDataPersistenceLayer(tbPersistenceParameter.Text);
				}

				FileEnumeratorParameters parameters =
					new FileEnumeratorParameters(cancelToken,
												Settings.FileEnumeration_DisableWorkerThread,
												selectedFolder,
												searchPatterns,
												calculateEntropy,
												yaraParameters,
												dataPersistenceLayer,
												Log.ToUI, Log.ToFile, ReportNumbers, Log.ExceptionMessage);

				enumerationStart = DateTime.Now;

				bool didThrow = false;
				try
				{
					parameters.ThrowIfAnyParametersInvalid();
				}
				catch (Exception ex)
				{
					didThrow = true;
					MessageBox.Show(ex.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if (didThrow)
				{
					ProcessingToggle.SetState(ToggleState.Ready);
				}
				else
				{
					Log.ToUI(Environment.NewLine);
					Log.ToAll($"Beginning Enumeration of folder: \"{parameters.SelectedFolder}\"");
					Log.ToAll("Parsing MFT. (This may take a few minutes)");
					FileEnumerator.LaunchFileEnumerator(parameters);
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (ProcessingToggle.CurrentState == ToggleState.Active || ProcessingToggle.CurrentState == ToggleState.Inactive)
			{
				if (e.CloseReason == CloseReason.UserClosing)
				{
					DialogResult choice =
							MessageBox.Show(
								"There appears that there is still an active scan in progress.\n\n" +
								"Are you sure you wish to exit the application?",
								"Thread still in use",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Warning);

					if (choice == DialogResult.No)
					{
						e.Cancel = true;
					}
					else
					{
						ProcessingToggle.SetState(ToggleState.Inactive);
					}
				}
			}
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

			ProcessingToggle.SetState(ToggleState.Ready);
		}

		#region Is Processing Toggle Members

		private void ActivationBehavior()
		{
			cancelTokenSource = new CancellationTokenSource();
			cancelToken = cancelTokenSource.Token;
			DisableControls();
		}

		private void DeactivationBehavior()
		{
			cancelTokenSource.Cancel();
		}

		private void ResetBehavior()
		{
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
				if (checkBoxYaraRules.Checked)
				{
					panelYaraParameters.Enabled = true;
				}
				panelParameters.Enabled = true;
				btnSearch.Text = "Search";
				btnSearch.Enabled = true;
			}
		}

		private void DisableControls()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(() => DisableControls()));
			}
			else
			{
				if (checkBoxYaraRules.Checked)
				{
					panelYaraParameters.Enabled = false;
				}
				panelParameters.Enabled = false;
				btnSearch.Text = "Cancel";
				btnSearch.Enabled = true;
			}
		}

		#endregion

		#endregion

		#region Misc Controls & Event Handlers

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

		private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(linkGitHub.Text);
		}

		#endregion

		#region Yara Rules Controls

		private List<string> yaraMatchFiles = new List<string>();
		private List<YaraFilter> currentYaraFilters = new List<YaraFilter>();
		private static string AddYaraRuleErrorCaption = "Error adding YARA filter";

		#region Show/Hide/Resize

		private void MainForm_Resize(object sender, EventArgs e)
		{
			ResizePanelWidths();
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			ResizePanelWidths();
		}

		private void ResizePanelWidths()
		{
			int formWidth = this.Width;
			int newPanelWidths = this.ClientRectangle.Width;
			if (formWidth != mainformWidth_Previous)
			{
				mainformWidth_Previous = formWidth;
				tableLayoutPanel1.Width = newPanelWidths;
				panelTop.Width = newPanelWidths;
			}

			if (flowLayoutPanelTop.Width != newPanelWidths)
			{
				flowLayoutPanelTop.Width = newPanelWidths;
				panelParameters.Width = newPanelWidths - 2;
				panelYaraParameters.Width = newPanelWidths - 2;
				panelSearchButton.Width = newPanelWidths - 2;
			}
		}

		private void checkBoxYaraRules_CheckedChanged(object sender, EventArgs e)
		{
			if (!checkBoxYaraRules.Checked)
			{
				// Collapse               
				this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, mainformMinimumHeight_Collapsed);
				panelYaraParameters.Height = yaraPanelHeight_Collapsed;
				this.Height = mainformHeight_Collapsed;
				ClearAllYaraSettings();
			}
			else
			{
				// Expand                 
				this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, mainformMinimumHeight_Expanded);
				panelYaraParameters.Height = yaraPanelHeight_Expanded;
				this.Height = mainformHeight_Expanded;
			}

			panelYaraParameters.Visible = checkBoxYaraRules.Checked;
		}

		private void radioButtonYara_HideFilterValue_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton control = (RadioButton)sender;
			if (control.Checked)
			{
				panelYaraFilterValue.Visible = false;
			}
		}

		private void radioButtonYara_ShowFilterValue_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton control = (RadioButton)sender;
			if (control.Checked)
			{
				panelYaraFilterValue.Visible = true;
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

		private void ClearAllYaraSettings()
		{
			tbYaraRuleMatchFiles.Text = "";
			tbYaraFilterValue.Text = "";

			currentYaraFilters.Clear();

			listBoxYaraFilters.SuspendLayout();
			listBoxYaraFilters.Items.Clear();
			listBoxYaraFilters.ResumeLayout();
		}

		#endregion

		#region Add/Remove Filters

		private void btnBrowseYaraMatch_Click(object sender, EventArgs e)
		{
			string[] selectedFiles = DialogHelper.BrowseForFilesDialog(DialogHelper.Filters.YaraFiles);

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

		private void btnAddYaraFilter_Click(object sender, EventArgs e)
		{
			if (!yaraMatchFiles.Any())
			{
				MessageBox.Show($"Must have at least one file selected under \"{labelYaraRulesToRun.Text}\" selected.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					MessageBox.Show($"\"{labelYaraFilterValue.Text.Replace(":", "")}\" cannot be empty when \"{radioButtonYara_Extention.Text.Replace(":", "")}\" is selected.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				filterValue = tbYaraFilterValue.Text;

				if (filterValue.Any(c => char.IsWhiteSpace(c)))
				{
					MessageBox.Show("No whitespace is allowed in a file extension.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (!filterValue.Contains('.'))
				{
					if (filterValue.Contains('/'))
					{
						if (MessageBox.Show("You are attempting to add a file extension filter, yet the YARA filter value looks like a MIME type.\n\nDo you wish to add this as a MIME type filter instead?", AddYaraRuleErrorCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							return;
						}

						filterType = YaraFilterType.MimeType;
					}
					else
					{
						MessageBox.Show($"You are attempting to add a FILE EXTENSION filter, yet the YARA filter value does not contain the required character '.'.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			else if (radioButtonYara_MimeType.Checked)
			{
				filterType = YaraFilterType.MimeType;
				if (string.IsNullOrWhiteSpace(tbYaraFilterValue.Text))
				{
					MessageBox.Show($"\"{labelYaraFilterValue.Text.Replace(":", "")}\" cannot be empty when \"{radioButtonYara_MimeType.Text.Replace(":", "")}\" is selected.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				filterValue = tbYaraFilterValue.Text;

				if (filterValue.Any(c => char.IsWhiteSpace(c)))
				{
					MessageBox.Show("No whitespace is allowed in a MIME type.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (!filterValue.Contains('/'))
				{
					if (filterValue.Contains('.'))
					{
						if (MessageBox.Show("You are attempting to add a MIME type filter, yet the YARA filter value looks like a file extension.\n\nDo you wish to add this as a file extension filter type instead?", AddYaraRuleErrorCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							return;
						}

						filterType = YaraFilterType.FileExtension;
					}
					else
					{
						MessageBox.Show($"You are attempting to add a MIME type filter, yet the YARA filter value does not contain the required character '/'.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			else if (radioButtonYara_ElseNoMatch.Checked)
			{
				filterType = YaraFilterType.ElseNoMatch;
			}

			YaraFilter yaraFilter = new YaraFilter(filterType, filterValue, yaraMatchFiles);

			if (currentYaraFilters.Contains(yaraFilter))
			{
				MessageBox.Show("YARA filter already exists.\n\nDuplicate filter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			var selectedIndices = listBoxYaraFilters.SelectedIndices.Cast<int>().ToList();

			List<YaraFilter> toRemove = new List<YaraFilter>();
			foreach (int index in selectedIndices)
			{
				toRemove.Add(currentYaraFilters[index]);
			}

			foreach (YaraFilter filter in toRemove)
			{
				currentYaraFilters.Remove(filter);
			}

			UpdateYaraFilterListbox();
		}

		#endregion

		#region Load/Save Yara Rules Filter Configuration

		private void btnYaraSave_Click(object sender, EventArgs e)
		{
			string selectedFile = DialogHelper.SaveFileDialog(DialogHelper.Filters.JsonFiles);

			if (!string.IsNullOrWhiteSpace(selectedFile))
			{
				JsonSerialization.Save.Object(currentYaraFilters, selectedFile);
			}
		}

		private void btnYaraLoad_Click(object sender, EventArgs e)
		{
			string selectedFile = DialogHelper.BrowseForFileDialog(DialogHelper.Filters.JsonFiles);

			if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile))
			{
				currentYaraFilters = JsonSerialization.Load.Generic<List<YaraFilter>>(selectedFile);
				UpdateYaraFilterListbox();
			}
		}

		#endregion

		#endregion

		#region Data persistence settings

		private void radioPersistenceCSV_CheckedChanged(object sender, EventArgs e)
		{
			labelTextBoxDescription.Text = "Path to CSV file:";
			btnPersistenceBrowse.Text = "Browse...";
			tbPersistenceParameter.Multiline = false;
		}

		private void radioPersistenceSqlite_CheckedChanged(object sender, EventArgs e)
		{
			labelTextBoxDescription.Text = "Path to DB file:";
			btnPersistenceBrowse.Text = "Browse...";
			tbPersistenceParameter.Multiline = false;
		}

		private void radioPersistenceSqlServer_CheckedChanged(object sender, EventArgs e)
		{
			labelTextBoxDescription.Text = "Connection string:";
			btnPersistenceBrowse.Text = "Browse...";
			tbPersistenceParameter.Multiline = true;
		}

		private void btnPersistenceBrowse_Click(object sender, EventArgs e)
		{
			if (radioPersistenceCSV.Checked || radioPersistenceSqlite.Checked)
			{
				string filter = radioPersistenceCSV.Checked ? DialogHelper.Filters.CsvFiles : DialogHelper.Filters.SqliteFiles;
				string initialDirectory = string.IsNullOrWhiteSpace(tbPersistenceParameter.Text) ? default(string) : tbPersistenceParameter.Text;
				string selectedFile = DialogHelper.SaveFileDialog(filter, initialDirectory);
				if (!string.IsNullOrWhiteSpace(selectedFile))
				{
					tbPersistenceParameter.Text = selectedFile;
				}
			}
			else if (radioPersistenceSqlServer.Checked)
			{
				DataConnectionDialog dataConnectionDialog = new DataConnectionDialog();
				DataSource.AddStandardDataSources(dataConnectionDialog);
				dataConnectionDialog.SelectedDataSource = DataSource.SqlDataSource;
				dataConnectionDialog.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;";
				if (DataConnectionDialog.Show(dataConnectionDialog) == DialogResult.OK)
				{
					tbPersistenceParameter.Text = dataConnectionDialog.ConnectionString;
				}
			}
		}

		#endregion

	}
}
