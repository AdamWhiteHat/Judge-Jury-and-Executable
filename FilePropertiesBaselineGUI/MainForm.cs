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
using Microsoft.Win32;
using System.Collections;

namespace FilePropertiesBaselineGUI
{
	public enum ComboBoxSelection
	{
		None = -1,
		Always = 0,
		PeFile = 1,
		FileExtension = 2,
		MimeType = 3,
		NoMatches = 4
	}

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

		private static AutoCompleteStringCollection AutoComplete_FileExtensions;
		private static AutoCompleteStringCollection AutoComplete_MimeTypes;

		#region Constructor

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

			AutoComplete_FileExtensions = new AutoCompleteStringCollection();
			AutoComplete_MimeTypes = new AutoCompleteStringCollection();

			AutoComplete_FileExtensions.AddRange(Properties.Settings.Default.AutoComplete_FileExtensions.Cast<string>().ToArray());
			AutoComplete_MimeTypes.AddRange(Properties.Settings.Default.AutoComplete_MimeTypes.Cast<string>().ToArray());

			#endregion

			#region YARA panel members initialization

			yaraPanelHeight_Expanded = panelYaraParameters.Height;
			yaraPanelHeightDifference = yaraPanelHeight_Expanded - yaraPanelHeight_Collapsed;

			mainformHeight_Expanded = this.Height;
			mainformHeight_Collapsed = this.MinimumSize.Height;

			mainformMinimumHeight_Collapsed = this.MinimumSize.Height;
			mainformMinimumHeight_Expanded = this.MinimumSize.Height + yaraPanelHeightDifference;

			mainformWidth_Previous = this.Width;

			tbYaraConditionValue.Visible = false;
			panelYaraCondition.Visible = false;

			#endregion

		}

		#endregion

		#region Search/Cancel/Reset Behavior

		private void btnSearch_Click(object sender, EventArgs e)
		{

			if (string.IsNullOrWhiteSpace(tbPersistenceParameter.Text))
			{
				string message = "";
				if (radioPersistenceCSV.Checked || radioPersistenceSqlite.Checked)
				{
					message = "A output file path is required.";
				}
				else if (radioPersistenceSqlServer.Checked)
				{
					message = "You must select a SQL server or supply a SQL connection string.";
				}

				MessageBox.Show(message, "Judge, Jury, and Executable", MessageBoxButtons.OK, MessageBoxIcon.Information);
				BrowseForPersistenceMethod();
				if (string.IsNullOrWhiteSpace(tbPersistenceParameter.Text))
				{
					return;
				}
			}

			if (radioPersistenceCSV.Checked || radioPersistenceSqlite.Checked)
			{
				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(tbPersistenceParameter.Text));
				}
				catch
				{
					MessageBox.Show("Could not create folder path: \"" + tbPersistenceParameter.Text + "\"" + "\n\nPlease update the " + char.ToLower(labelTextBoxDescription.Text[0]) + labelTextBoxDescription.Text.Substring(1, labelTextBoxDescription.Text.Length - 2) + " to a valid location that this application can write to.", "Judge, Jury, and Executable", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tbPersistenceParameter.Select();
					return;
				}
			}

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

		private void ReportNumbers(FileEnumeratorReport report)
		{
			TimeSpan enumerationTimeSpan = DateTime.Now.Subtract(enumerationStart);

			foreach (FailSuccessCount count in report.Counts)
			{
				Log.ToAll($"Succeeded: {count.SucceededCount} {count.Description}.");
			}
			foreach (FailSuccessCount count in report.Counts)
			{
				Log.ToAll($"Failed: {count.FailedCount} {count.Description}.");
			}

			Log.ToAll($"Enumeration time: {enumerationTimeSpan.ToString()}");
			Log.ToAll();
			foreach (string line in report.Timings.GetReport())
			{
				Log.ToAll(line);
			}
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
				btnSearch.Text = "Scan";
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
				panelYaraCondition.Visible = false;
				DiscardAllUnsavedYaraRules();
			}
			else
			{
				// Expand                 
				this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, mainformMinimumHeight_Expanded);
				panelYaraParameters.Height = yaraPanelHeight_Expanded;
				this.Height = mainformHeight_Expanded;
				panelYaraCondition.Visible = false;
			}

			yaraErrorProvider.Clear();
			panelYaraParameters.Visible = checkBoxYaraRules.Checked;
		}

		#endregion

		#region Yara Filter Configure

		private void comboConditionType_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.None)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
				SetAutoCompleteSource(tbYaraConditionValue);
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.Always)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
				SetAutoCompleteSource(tbYaraConditionValue);
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.PeFile)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
				SetAutoCompleteSource(tbYaraConditionValue);
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.FileExtension)
			{
				tbYaraConditionValue.Visible = true;
				tbYaraConditionValue.Text = ".exe";
				SetAutoCompleteSource(tbYaraConditionValue, AutoComplete_FileExtensions);
				tbYaraConditionValue.Focus();
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.MimeType)
			{
				tbYaraConditionValue.Visible = true;
				tbYaraConditionValue.Text = "application/octet-stream";
				SetAutoCompleteSource(tbYaraConditionValue, AutoComplete_MimeTypes);
				tbYaraConditionValue.Focus();
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.NoMatches)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
				SetAutoCompleteSource(tbYaraConditionValue);
			}

			yaraErrorProvider.SetError(comboConditionType, string.Empty);
			yaraErrorProvider.SetError(tbYaraConditionValue, string.Empty);
		}

		private static void SetAutoCompleteSource(TextBox textBox, AutoCompleteStringCollection autoCompleteCollection = null)
		{
			if (autoCompleteCollection == null || autoCompleteCollection.Count == 0)
			{
				textBox.AutoCompleteMode = AutoCompleteMode.None;
				textBox.AutoCompleteSource = AutoCompleteSource.None;
				textBox.AutoCompleteCustomSource = new AutoCompleteStringCollection();
			}
			else
			{
				textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
				textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
				textBox.AutoCompleteCustomSource = autoCompleteCollection;
			}
		}

		private void ClearYaraControls()
		{
			tbYaraConditionValue.Clear();
			listYaraMatchFiles.Clear();
			yaraMatchFiles.Clear();

			comboConditionType.SelectedIndex = (int)ComboBoxSelection.None;
			comboConditionType.Text = "(select)";

			yaraErrorProvider.SetError(comboConditionType, string.Empty);
			yaraErrorProvider.SetError(tbYaraConditionValue, string.Empty);
			yaraErrorProvider.SetError(listYaraMatchFiles, string.Empty);
		}

		private void DiscardAllUnsavedYaraRules()
		{
			ClearYaraControls();

			currentYaraFilters.Clear();

			treeViewYaraFilters.Nodes.Clear();
		}

		#endregion

		#region Yara Match Rule Files

		private void btnBrowseYaraMatch_Click(object sender, EventArgs e)
		{
			BrowseYaraMatchFiles();
		}

		private void listYaraMatchFiles_MouseUp(object sender, MouseEventArgs e)
		{
			if (listYaraMatchFiles.Items.Count == 0)
			{
				BrowseYaraMatchFiles();
			}
		}

		private void ToolStripMenuItem_RemoveMatchFile_Click(object sender, EventArgs e)
		{
			DeleteSelectedYaraMatchFiles();
		}

		private void listYaraMatchFiles_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				DeleteSelectedYaraMatchFiles();
			}
		}

		private void BrowseYaraMatchFiles()
		{
			string[] selectedFiles = DialogHelper.BrowseForFilesDialog(DialogHelper.Filters.YaraFiles);
			if (selectedFiles.Any())
			{
				yaraMatchFiles.AddRange(selectedFiles.ToList());
				yaraErrorProvider.SetError(listYaraMatchFiles, string.Empty);
				UpdateYaraMatchFiles();
			}
		}

		private void DeleteSelectedYaraMatchFiles()
		{
			foreach (ListViewItem item in listYaraMatchFiles.SelectedItems)
			{
				string file = (string)item.Tag;
				yaraMatchFiles.Remove(file);
			}

			UpdateYaraMatchFiles();
		}

		private void UpdateYaraMatchFiles()
		{
			listYaraMatchFiles.Clear();
			foreach (string file in yaraMatchFiles.ToList())
			{
				ListViewItem item = new ListViewItem(Path.GetFileName(file));
				item.Tag = file;
				listYaraMatchFiles.Items.Add(item);
			}
		}

		#endregion

		#region Yara Filters TreeView 

		private void UpdateYaraFilterTreeView()
		{
			treeViewYaraFilters.SuspendLayout();
			treeViewYaraFilters.Nodes.Clear();

			foreach (YaraFilter filter in currentYaraFilters)
			{
				List<TreeNode> childNodes = new List<TreeNode>();

				foreach (string file in filter.OnMatchRules.ToList())
				{
					TreeNode child = new TreeNode(Path.GetFileName(file));
					child.ToolTipText = file;
					childNodes.Add(child);
				}

				string nodeText = "";

				if (filter.FilterType == YaraFilterType.FileExtension || filter.FilterType == YaraFilterType.MimeType)
				{
					nodeText = filter.FilterValue;
				}
				else if (filter.FilterType == YaraFilterType.AlwaysRun)
				{
					nodeText = "Always run rules";
				}
				else if (filter.FilterType == YaraFilterType.IsPeFile)
				{
					nodeText = "Is PE file";
				}
				else if (filter.FilterType == YaraFilterType.ElseNoMatch)
				{
					nodeText = "No matches";
				}

				TreeNode parentNode = new TreeNode(nodeText);
				parentNode.ToolTipText = filter.ToString();
				parentNode.Nodes.AddRange(childNodes.ToArray());
				parentNode.Tag = filter;
				parentNode.Collapse();

				treeViewYaraFilters.Nodes.Add(parentNode);
			}

			treeViewYaraFilters.ResumeLayout();
		}

		private void contextMenuYaraTreeView_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (treeViewYaraFilters.Nodes.Count == 0)
			{
				e.Cancel = true;
			}
		}

		private void removeTreeFilterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			treeView_RemoveSelected();
		}

		private void treeViewYaraFilters_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				treeView_RemoveSelected();
			}
		}

		private void treeView_RemoveSelected()
		{
			TreeNode selectedNode = treeViewYaraFilters.SelectedNode;
			int level = selectedNode.Level;

			if (level == 1)
			{
				TreeNode parent = selectedNode.Parent;
				YaraFilter filter = (YaraFilter)parent.Tag;

				filter.OnMatchRules.Remove(selectedNode.ToolTipText);
				selectedNode.Remove();

				if (!filter.OnMatchRules.Any())
				{
					currentYaraFilters.Remove(filter);
					parent.Remove();
				}

				return;
			}
			else
			{
				YaraFilter filter = (YaraFilter)selectedNode.Tag;
				currentYaraFilters.Remove(filter);
				selectedNode.Remove();
			}
		}

		#endregion

		#region Add/Remove Yara Filters

		private void btnNewAddYaraCondition_Click(object sender, EventArgs e)
		{
			panelYaraCondition.Visible = true;
		}

		private void btnCancelAddYaraCondition_Click(object sender, EventArgs e)
		{
			ClearYaraControls();
			panelYaraCondition.Visible = false;
		}

		private void btnOkAddYaraCondition_Click(object sender, EventArgs e)
		{
			if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.None)
			{
				yaraErrorProvider.SetError(comboConditionType, "Missing filter type");
				return;
			}

			if (!yaraMatchFiles.Any())
			{
				yaraErrorProvider.SetError(listYaraMatchFiles, "Missing rule file(s)");
				//MessageBox.Show($"You must select at least 1 rule file.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			YaraFilterType filterType = YaraFilterType.AlwaysRun;
			string filterValue = string.Empty;

			if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.Always)
			{
				filterType = YaraFilterType.AlwaysRun;
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.PeFile)
			{
				filterType = YaraFilterType.IsPeFile;
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.FileExtension)
			{
				filterType = YaraFilterType.FileExtension;
				if (string.IsNullOrWhiteSpace(tbYaraConditionValue.Text))
				{
					yaraErrorProvider.SetError(tbYaraConditionValue, "Missing file extension");
					//MessageBox.Show($"You forgot to enter a file extension to filter by in the text box.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				filterValue = tbYaraConditionValue.Text;

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
						yaraErrorProvider.SetError(tbYaraConditionValue, "File extensions should start with a period ('.')");
						//MessageBox.Show($"You are attempting to add a FILE EXTENSION filter, yet the YARA filter value does not contain the required character '.'.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.MimeType)
			{
				filterType = YaraFilterType.MimeType;
				if (string.IsNullOrWhiteSpace(tbYaraConditionValue.Text))
				{
					yaraErrorProvider.SetError(tbYaraConditionValue, "Missing MIME type");
					//MessageBox.Show($"You forgot to enter a MIME type to filter by in the text box.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				filterValue = tbYaraConditionValue.Text;

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
						yaraErrorProvider.SetError(tbYaraConditionValue, "MIME types contain a slash ('/')");
						//MessageBox.Show($"You are attempting to add a MIME type filter, yet the YARA filter value does not contain the required character '/'.\n\nFilter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			else if (comboConditionType.SelectedIndex == (int)ComboBoxSelection.NoMatches)
			{
				filterType = YaraFilterType.ElseNoMatch;
				filterValue = "";
			}

			YaraFilter yaraFilter = new YaraFilter(filterType, filterValue, yaraMatchFiles);

			if (currentYaraFilters.Contains(yaraFilter))
			{
				MessageBox.Show("YARA filter already exists.\n\nDuplicate filter not added.", AddYaraRuleErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			currentYaraFilters.Add(yaraFilter);

			UpdateYaraFilterTreeView();
			ClearYaraControls();
			panelYaraCondition.Visible = false;
		}

		private void ContextMenuListBox_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ContextMenuStrip contextMenu = (ContextMenuStrip)sender;
			if (contextMenu.SourceControl is ListView)
			{
				ListView listView = (ListView)contextMenu.SourceControl;
				if (listView.Items.Count == 0)
				{
					e.Cancel = true;
				}
			}
		}

		private void tbYaraConditionValue_TextChanged(object sender, EventArgs e)
		{
			yaraErrorProvider.SetError(tbYaraConditionValue, string.Empty);
		}

		#endregion

		#region Load/Save Yara Rules Filters

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
				UpdateYaraFilterTreeView();
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

		private void tbPersistenceParameter_TextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(tbPersistenceParameter.Text))
			{
				if (File.Exists(tbPersistenceParameter.Text))
				{
					DialogResult mbResut =
						MessageBox.Show(
							$"{Path.GetFileName(tbPersistenceParameter.Text)} already exists.\nDo you want to replace it?",
							"Confirm Save File",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Warning);

					if (mbResut == DialogResult.No)
					{
						tbPersistenceParameter.Clear();
						return;
					}
				}
			}
		}

		private void btnPersistenceBrowse_Click(object sender, EventArgs e)
		{
			BrowseForPersistenceMethod();
		}

		private void BrowseForPersistenceMethod()
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

				string presetConnectionString = "(LocalDB)\\MSSQLLocalDB";

				string[] sqlInstances = GetInstalledSQLInstances();
				if (sqlInstances.Any())
				{
					string computerName = SystemInformation.ComputerName;
					presetConnectionString = $"{computerName}\\{sqlInstances.First()}";
				}

				dataConnectionDialog.ConnectionString = $@"Data Source={presetConnectionString};Integrated Security=True;";
				if (DataConnectionDialog.Show(dataConnectionDialog) == DialogResult.OK)
				{
					tbPersistenceParameter.Text = dataConnectionDialog.ConnectionString;
				}
			}
		}

		private static string[] GetInstalledSQLInstances()
		{
			string[] results = new string[0];
			try
			{
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"))
				{
					if (key != null)
					{
						results = key.GetValueNames();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return results;
		}

		#endregion

	}
}
