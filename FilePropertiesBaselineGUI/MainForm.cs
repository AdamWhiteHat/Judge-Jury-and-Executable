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
        private Toggle ProcessingToggle = null;
        private DateTime enumerationStart;

        public MainForm()
        {
            InitializeComponent();

            Logging.FormOutputControl = tbOutput;
            SQLHelper.LogExceptionAction = Logging.LogExceptionMessage;
            ProcessingToggle = new Toggle(ActivationBehavior, DeactivationBehavior);

            panelYara.Enabled = checkBoxYaraRules.Checked;

            string connectionString = Settings.Database_ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "SetMe")
            {
                Logging.ReportOutput("ERROR: Connection string not set! Please set the SQL connection string in .config file. Browse button disabled.");
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
                                                    Logging.ReportOutput, Logging.LogOutput, ReportNumbers, Logging.LogExceptionMessage);

                tbOutput.AppendText(Environment.NewLine);
                Logging.ReportOutput($"Beginning Enumeration of folder: \"{selectedFolder}\"");

                enumerationStart = DateTime.Now;

                FileEnumerator.LaunchFileEnumerator(parameters);
            }
        }

        private void ReportNumbers(List<FailSuccessCount> counts)
        {
            TimeSpan enumerationTimeSpan = DateTime.Now.Subtract(enumerationStart);

            foreach (FailSuccessCount count in counts)
            {
                count.ToStrings().ForEach(s => Logging.ReportOutput(s));
            }
            Logging.ReportOutput($"Enumeration time: {enumerationTimeSpan.ToString()}");
            Logging.ReportOutput();
            Logging.ReportOutput("Enumeration finished!");

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

    }
}
