using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using Logging;
using DataAccessLayer;
using FilePropertiesEnumerator;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;

namespace FilePropertiesBaselineGUI
{
	public partial class MainForm : Form
	{
		private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		private CancellationToken cancelToken;
		private Toggle ProcessingToggle = null;
		private DateTime enumerationStart;

		private int mainformMinimumHeight_Collapsed = 0;
		private int mainformMinimumHeight_Expanded = 0;
		private int mainformHeight_Collapsed = 0;
		private int mainformHeight_Expanded = 0;

		private int yaraPanelHeight_Expanded = 0;
		private int yaraPanelHeightDifference = 0;
		private static int yaraPanelHeight_Collapsed = 10;

		private int mainformWidth_Previous = 0;

		private Font defaultLabelFont;

		private static string _errorMessageTitle_AddYaraCondition = "Error adding new yara condition";
		private static string _yaraBrowseFileDialogFilter_RuleFiles = "YARA rule files (*.yar)|*.yar|All files (*.*)|*.*";
		private static string _yaraBrowseFileDialogFilter_ConfigFiles = "JSON files (*.json)|*.json|All files (*.*)|*.*";

		public MainForm()
		{
			InitializeComponent();

			OutputTextBox = tbOutput;
			Log.LogOutputAction = LogOutput;
			SQLHelper.LogExceptionAction = Log.ExceptionMessage;
			ProcessingToggle = new Toggle(ActivationBehavior, DeactivationBehavior);

			yaraPanelHeight_Expanded = panelYara.Height;
			yaraPanelHeightDifference = yaraPanelHeight_Expanded - yaraPanelHeight_Collapsed;

			mainformHeight_Expanded = this.Height;
			mainformHeight_Collapsed = this.MinimumSize.Height;

			mainformMinimumHeight_Collapsed = this.MinimumSize.Height;
			mainformMinimumHeight_Expanded = this.MinimumSize.Height + yaraPanelHeightDifference;

			mainformWidth_Previous = this.Width;

			defaultLabelFont = (Font)labelYaraBaseRulesFilesDescription.Font.Clone();

			comboConditionType.SelectedIndex = -1;
			comboConditionType.Text = "(select)";
			tbYaraConditionValue.Text = "";

			panelYaraCondition.Visible = false;
			tbYaraConditionValue.Visible = false;
			panelYara.Visible = checkBoxYaraRules.Checked;

			toolTips.SetToolTip(groupBoxYaraBaseRuleset, $"Specify YARA rules that will run against every file enumerated (NOTE: File must match '{labelSearchPatterns.Text}' criteria above).");
			toolTips.SetToolTip(groupBoxYaraConditionalRuleset, "Specify YARA rules that will run only if the associated condition is met. You may have many conditions, each with their own associated ruleset.");
			toolTips.SetToolTip(groupBoxYaraElseRuleset, "Specify YARA rules that will run only if all of the conditional rulesets in the above list are not ran.");
			toolTips.SetToolTip(listBoxYaraConditions, "Use the delete key to remove selected YARA conditions from the list.");

			string connectionString = Settings.Database_ConnectionString;
			if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
			{
				Log.ToAll("ERROR: Connection string not set! Please set the SQL connection string in .config file. Browse button disabled.");
				btnScan.Enabled = false;
				btnSearch.Enabled = false;
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

		private void MainForm_Shown(object sender, EventArgs e)
		{
			checkBoxYaraRules.CheckState = CheckState.Unchecked;
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
				btnScan.Enabled = false;
				ProcessingToggle.SetState(false);
			}
			else
			{
				btnScan.Text = "Cancel";
				ProcessingToggle.SetState(true);

				bool calculateEntropy = checkboxCalculateEntropy.Checked;
				string selectedFolder = tbPath.Text;
				string searchPatterns = tbSearchPatterns.Text;

				YaraScanConfiguration yaraParameters = new YaraScanConfiguration();

				if (checkBoxYaraRules.Checked)
				{
					yaraParameters = yaraConfig;
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
				panelParameters.Enabled = false;
				btnScan.Text = "Cancel";

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
				panelParameters.Enabled = true;
				btnScan.Text = "Search";
				btnScan.Enabled = true;
			}
		}

		#endregion

		#region Yara Shit

		private YaraScanConfiguration yaraConfig = new YaraScanConfiguration();
		private YaraCondition newYaraCondition = new YaraCondition();

		private void checkBoxYaraRules_CheckedChanged(object sender, EventArgs e)
		{
			panelYaraCondition.Visible = false;

			if (!checkBoxYaraRules.Checked)
			{
				// Collapse               
				this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, mainformMinimumHeight_Collapsed);
				panelYara.Height = yaraPanelHeight_Collapsed;
				this.Height = mainformHeight_Collapsed;
				ClearAllYaraSettings();
			}
			else
			{
				// Expand                 
				this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, mainformMinimumHeight_Expanded);
				panelYara.Height = yaraPanelHeight_Expanded;
				this.Height = mainformHeight_Expanded;
				UpdateAllYaraListLabeles();
			}

			panelYara.Visible = checkBoxYaraRules.Checked;
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			int formWidth = this.Width;
			if (formWidth != mainformWidth_Previous)
			{
				int newPanelWidths = formWidth - 16; ;
				this.tableLayoutPanel1.Width = newPanelWidths;
				panelTop.Width = newPanelWidths;
				flowLayoutPanelTop.Width = newPanelWidths;
				panelParameters.Width = newPanelWidths - 2;
				panelYara.Width = newPanelWidths - 2;
				mainformWidth_Previous = formWidth;
			}
		}

		private void btnNewAddYaraCondition_Click(object sender, EventArgs e)
		{
			newYaraCondition = new YaraCondition();
			comboConditionType.SelectedIndex = -1;
			comboConditionType.Text = "(select)";
			tbYaraConditionValue.Text = "";
			panelYaraCondition.Visible = true;
			UpdateAllYaraListLabeles();
		}

		private void btnCancelAddYaraCondition_Click(object sender, EventArgs e)
		{
			newYaraCondition = new YaraCondition();
			newYaraCondition = new YaraCondition();
			comboConditionType.SelectedIndex = -1;
			comboConditionType.Text = "(select)";
			tbYaraConditionValue.Text = "";
			UpdateAllYaraListLabeles();
			panelYaraCondition.Visible = false;
		}

		private void panelYaraCondition_VisibleChanged(object sender, EventArgs e)
		{
			if (panelYaraCondition.Visible)
			{
				comboConditionType.SelectedIndex = (int)newYaraCondition.ConditionType;
				comboConditionType.Text = (newYaraCondition.ConditionType == YaraConditionType.None) ? "(select)" : Enum.GetName(typeof(YaraConditionType), newYaraCondition.ConditionType);
				tbYaraConditionValue.Text = newYaraCondition.ConditionValue;
				tbYaraConditionValue.Visible = (newYaraCondition.ConditionType == YaraConditionType.FileExtension || newYaraCondition.ConditionType == YaraConditionType.MimeType);
				comboConditionType.Focus();
			}
			else
			{
				comboConditionType.SelectedIndex = -1;
				comboConditionType.Text = "(select)";
				tbYaraConditionValue.Text = "";
				tbYaraConditionValue.Visible = false;
			}
		}

		private void comboConditionType_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (comboConditionType.SelectedIndex == -1)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
			}
			else if (comboConditionType.SelectedIndex == 0)
			{
				tbYaraConditionValue.Visible = false;
				tbYaraConditionValue.Text = "";
			}
			else if (comboConditionType.SelectedIndex == 1)
			{
				tbYaraConditionValue.Visible = true;
				tbYaraConditionValue.Text = ".exe";
				tbYaraConditionValue.Focus();
			}
			else if (comboConditionType.SelectedIndex == 2)
			{
				tbYaraConditionValue.Visible = true;
				tbYaraConditionValue.Text = "application/octet-stream";
				tbYaraConditionValue.Focus();
			}
		}

		private void btnOkAddCondition_Click(object sender, EventArgs e)
		{
			if (comboConditionType.SelectedIndex == -1)
			{
				MessageBox.Show($"You must select a condition from the dropdown box.", _errorMessageTitle_AddYaraCondition, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!newYaraCondition.ConditionMatchRuleset.Any())
			{
				MessageBox.Show($"Must have at least one rule file selected to run if condition is true.", _errorMessageTitle_AddYaraCondition, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (comboConditionType.SelectedIndex == 0)
			{
				newYaraCondition.ConditionType = YaraConditionType.IsPeFile;
				newYaraCondition.ConditionValue = "";
			}
			else if (comboConditionType.SelectedIndex == 1)
			{
				newYaraCondition.ConditionType = YaraConditionType.FileExtension;
				if (string.IsNullOrWhiteSpace(tbYaraConditionValue.Text))
				{
					MessageBox.Show($"You must specify a condition value when '{comboConditionType.SelectedValue.ToString()}' is selected.", _errorMessageTitle_AddYaraCondition, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				newYaraCondition.ConditionValue = tbYaraConditionValue.Text;
			}
			else if (comboConditionType.SelectedIndex == 2)
			{
				newYaraCondition.ConditionType = YaraConditionType.MimeType;
				if (string.IsNullOrWhiteSpace(tbYaraConditionValue.Text))
				{
					MessageBox.Show($"You must specify a condition value when '{comboConditionType.SelectedValue.ToString()}' is selected.", _errorMessageTitle_AddYaraCondition, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				newYaraCondition.ConditionValue = tbYaraConditionValue.Text;
			}

			if (yaraConfig.Conditions.Contains(newYaraCondition))
			{
				MessageBox.Show("Yara condition already exists. Duplicate condition not added.", _errorMessageTitle_AddYaraCondition, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			yaraConfig.AddConditionalRule(newYaraCondition);

			newYaraCondition = new YaraCondition();
			UpdateAllYaraListLabeles();
			panelYaraCondition.Visible = false;
		}

		private void listBoxYaraConditions_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				listBoxYaraConditions_RemoveSelected();
			}
		}

		private void tbYaraConditionValue_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnOkAddCondition_Click(null, new EventArgs());
			}
		}

		private void listBoxYaraConditions_RemoveSelected()
		{
			if (listBoxYaraConditions.SelectedIndex != -1)
			{
				int lastIndex = listBoxYaraConditions.SelectedIndices.Cast<int>().Last();
				var condition = yaraConfig.Conditions.Where(yf => yf.ToString() == listBoxYaraConditions.SelectedItem.ToString()).Single();
				yaraConfig.RemoveConditionalRule(condition);
				UpdateYaraConditionsListbox();
				int newIndex = Math.Min(lastIndex, listBoxYaraConditions.Items.Count - 1);
				listBoxYaraConditions.SelectedIndex = newIndex;
			}
		}

		private void ClearAllYaraSettings()
		{
			yaraConfig = new YaraScanConfiguration();
			newYaraCondition = new YaraCondition();
			UpdateAllYaraListLabeles();
		}

		private void UpdateAllYaraListLabeles()
		{
			UpdateCollectionLabel(labelYaraBaseRulesFilesDescription, yaraConfig.BaseRuleset);
			UpdateCollectionLabel(labelYaraConditionRuleFilesDescription, newYaraCondition.ConditionMatchRuleset);
			UpdateCollectionLabel(labelYaraElseRulesFilesDescription, yaraConfig.ElseRuleset);
			UpdateYaraConditionsListbox();
		}

		private void UpdateYaraConditionsListbox()
		{
			listBoxYaraConditions.SuspendLayout();
			listBoxYaraConditions.Items.Clear();
			foreach (YaraCondition condition in yaraConfig.Conditions)
			{
				listBoxYaraConditions.Items.Add(condition.ToString());
			}
			listBoxYaraConditions.ResumeLayout();
		}

		private void btnYaraSaveConfiguration_Click(object sender, EventArgs e)
		{
			string selectedFilename = DialogHelper.SaveFileDialog(_yaraBrowseFileDialogFilter_ConfigFiles);

			if (!string.IsNullOrWhiteSpace(selectedFilename))
			{
				JsonSerialization.Save.Object(yaraConfig, selectedFilename);
				ClearAllYaraSettings();
			}
		}

		private void btnYaraLoadConfiguration_Click(object sender, EventArgs e)
		{
			string selectedFilename = DialogHelper.BrowseForFileDialog(_yaraBrowseFileDialogFilter_ConfigFiles);
			if (!string.IsNullOrWhiteSpace(selectedFilename) && File.Exists(selectedFilename))
			{
				yaraConfig = JsonSerialization.Load.Generic<YaraScanConfiguration>(selectedFilename);
				newYaraCondition = new YaraCondition();
				UpdateAllYaraListLabeles();
			}
		}

		private void labelRuleFilesDescription_MouseEnter(object sender, EventArgs e)
		{
			Label control = (Label)sender;
			control.Font = new Font(control.Font, FontStyle.Underline); // FontStyle.Bold
			control.ForeColor = Color.Blue;
		}

		private void labelRuleFilesDescription_MouseLeave(object sender, EventArgs e)
		{
			Label control = (Label)sender;
			control.Font = (Font)defaultLabelFont.Clone();
			control.ForeColor = SystemColors.ControlText;
		}

		private void labelYaraBaseRulesFilesDescription_Click(object sender, EventArgs e)
		{
			yaraConfig.SetBaseRuleset(EditRulesCollection(labelYaraBaseRulesFilesDescription, yaraConfig.BaseRuleset));
		}

		private void labelYaraConditionRuleFilesDescription_Click(object sender, EventArgs e)
		{
			newYaraCondition.ConditionMatchRuleset = EditRulesCollection(labelYaraConditionRuleFilesDescription, newYaraCondition.ConditionMatchRuleset);
		}

		private void labelYaraElseRulesFilesDescription_Click(object sender, EventArgs e)
		{
			yaraConfig.SetElseRuleset(EditRulesCollection(labelYaraElseRulesFilesDescription, yaraConfig.ElseRuleset));
		}

		private List<string> EditRulesCollection(Label control, List<string> currentCollection)
		{
			using (FileSelectionForm fileSelectionForm = new FileSelectionForm())
			{
				fileSelectionForm.FileNames = currentCollection.ToList();
				fileSelectionForm.Filter = _yaraBrowseFileDialogFilter_RuleFiles;

				if (fileSelectionForm.ShowDialog() == DialogResult.OK)
				{
					List<string> newCollection = fileSelectionForm.FileNames.ToList();
					UpdateCollectionLabel(control, newCollection);
					return newCollection;
				}
			}
			return currentCollection;
		}

		private void UpdateCollectionLabel(Label control, List<string> collection)
		{
			control.Text = $"{collection.Count} file(s) selected. (Click here to edit)";
		}

		#endregion

	}
}
