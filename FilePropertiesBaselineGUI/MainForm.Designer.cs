namespace FilePropertiesBaselineGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.components = new System.ComponentModel.Container();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.panelParameters = new System.Windows.Forms.Panel();
            this.checkBoxYaraRules = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.checkboxOnlineCertValidation = new System.Windows.Forms.CheckBox();
            this.labelSearchPatterns = new System.Windows.Forms.Label();
            this.tbSearchPatterns = new System.Windows.Forms.TextBox();
            this.checkboxCalculateEntropy = new System.Windows.Forms.CheckBox();
            this.panelYara = new System.Windows.Forms.Panel();
            this.groupBoxYaraElseRuleset = new System.Windows.Forms.GroupBox();
            this.labelYaraElseRulesFilesDescription = new System.Windows.Forms.Label();
            this.groupBoxYaraConditionalRuleset = new System.Windows.Forms.GroupBox();
            this.panelYaraCondition = new System.Windows.Forms.Panel();
            this.labelYaraConditionRuleFilesDescription = new System.Windows.Forms.Label();
            this.btnCancelAddYaraCondition = new System.Windows.Forms.Button();
            this.btnOkAddYaraCondition = new System.Windows.Forms.Button();
            this.comboConditionType = new System.Windows.Forms.ComboBox();
            this.tbYaraConditionValue = new System.Windows.Forms.TextBox();
            this.listBoxYaraConditions = new System.Windows.Forms.ListBox();
            this.btnNewAddYaraCondition = new System.Windows.Forms.Button();
            this.groupBoxYaraBaseRuleset = new System.Windows.Forms.GroupBox();
            this.labelYaraBaseRulesFilesDescription = new System.Windows.Forms.Label();
            this.btnYaraLoadConfig = new System.Windows.Forms.Button();
            this.btnYaraSaveConfig = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.flowLayoutPanelTop.SuspendLayout();
            this.panelParameters.SuspendLayout();
            this.panelYara.SuspendLayout();
            this.groupBoxYaraElseRuleset.SuspendLayout();
            this.groupBoxYaraConditionalRuleset.SuspendLayout();
            this.panelYaraCondition.SuspendLayout();
            this.groupBoxYaraBaseRuleset.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTop
            // 
            this.flowLayoutPanelTop.AutoSize = true;
            this.flowLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelTop.Controls.Add(this.panelParameters);
            this.flowLayoutPanelTop.Controls.Add(this.panelYara);
            this.flowLayoutPanelTop.Controls.Add(this.btnScan);
            this.flowLayoutPanelTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTop.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
            this.flowLayoutPanelTop.Size = new System.Drawing.Size(644, 413);
            this.flowLayoutPanelTop.TabIndex = 13;
            this.flowLayoutPanelTop.WrapContents = false;
            // 
            // panelParameters
            // 
            this.panelParameters.Controls.Add(this.checkBoxYaraRules);
            this.panelParameters.Controls.Add(this.btnBrowse);
            this.panelParameters.Controls.Add(this.tbPath);
            this.panelParameters.Controls.Add(this.checkboxOnlineCertValidation);
            this.panelParameters.Controls.Add(this.labelSearchPatterns);
            this.panelParameters.Controls.Add(this.tbSearchPatterns);
            this.panelParameters.Controls.Add(this.checkboxCalculateEntropy);
            this.panelParameters.Location = new System.Drawing.Point(1, 1);
            this.panelParameters.Margin = new System.Windows.Forms.Padding(1);
            this.panelParameters.Name = "panelParameters";
            this.panelParameters.Size = new System.Drawing.Size(642, 117);
            this.panelParameters.TabIndex = 8;
            // 
            // checkBoxYaraRules
            // 
            this.checkBoxYaraRules.AutoSize = true;
            this.checkBoxYaraRules.Checked = true;
            this.checkBoxYaraRules.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxYaraRules.Location = new System.Drawing.Point(3, 96);
            this.checkBoxYaraRules.Name = "checkBoxYaraRules";
            this.checkBoxYaraRules.Size = new System.Drawing.Size(114, 17);
            this.checkBoxYaraRules.TabIndex = 10;
            this.checkBoxYaraRules.Text = "Scan with YARA...";
            this.checkBoxYaraRules.UseVisualStyleBackColor = true;
            this.checkBoxYaraRules.CheckedChanged += new System.EventHandler(this.checkBoxYaraRules_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(543, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(95, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(3, 5);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(530, 20);
            this.tbPath.TabIndex = 1;
            this.tbPath.Text = "C:\\";
            // 
            // checkboxOnlineCertValidation
            // 
            this.checkboxOnlineCertValidation.AutoSize = true;
            this.checkboxOnlineCertValidation.Location = new System.Drawing.Point(3, 75);
            this.checkboxOnlineCertValidation.Name = "checkboxOnlineCertValidation";
            this.checkboxOnlineCertValidation.Size = new System.Drawing.Size(170, 17);
            this.checkboxOnlineCertValidation.TabIndex = 6;
            this.checkboxOnlineCertValidation.Text = "File certificate online validation";
            this.checkboxOnlineCertValidation.UseVisualStyleBackColor = true;
            // 
            // labelSearchPatterns
            // 
            this.labelSearchPatterns.AutoSize = true;
            this.labelSearchPatterns.Location = new System.Drawing.Point(7, 32);
            this.labelSearchPatterns.Name = "labelSearchPatterns";
            this.labelSearchPatterns.Size = new System.Drawing.Size(85, 13);
            this.labelSearchPatterns.TabIndex = 5;
            this.labelSearchPatterns.Text = "Search patterns:";
            // 
            // tbSearchPatterns
            // 
            this.tbSearchPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchPatterns.Location = new System.Drawing.Point(98, 29);
            this.tbSearchPatterns.Name = "tbSearchPatterns";
            this.tbSearchPatterns.Size = new System.Drawing.Size(408, 20);
            this.tbSearchPatterns.TabIndex = 4;
            this.tbSearchPatterns.Text = "*.exe|*.dll|*.drv";
            // 
            // checkboxCalculateEntropy
            // 
            this.checkboxCalculateEntropy.AutoSize = true;
            this.checkboxCalculateEntropy.Location = new System.Drawing.Point(3, 54);
            this.checkboxCalculateEntropy.Name = "checkboxCalculateEntropy";
            this.checkboxCalculateEntropy.Size = new System.Drawing.Size(124, 17);
            this.checkboxCalculateEntropy.TabIndex = 3;
            this.checkboxCalculateEntropy.Text = "Calculate file entropy";
            this.checkboxCalculateEntropy.UseVisualStyleBackColor = true;
            // 
            // panelYara
            // 
            this.panelYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelYara.Controls.Add(this.groupBoxYaraElseRuleset);
            this.panelYara.Controls.Add(this.groupBoxYaraConditionalRuleset);
            this.panelYara.Controls.Add(this.groupBoxYaraBaseRuleset);
            this.panelYara.Controls.Add(this.btnYaraLoadConfig);
            this.panelYara.Controls.Add(this.btnYaraSaveConfig);
            this.panelYara.Location = new System.Drawing.Point(1, 120);
            this.panelYara.Margin = new System.Windows.Forms.Padding(1);
            this.panelYara.Name = "panelYara";
            this.panelYara.Size = new System.Drawing.Size(642, 259);
            this.panelYara.TabIndex = 9;
            // 
            // groupBoxYaraElseRuleset
            // 
            this.groupBoxYaraElseRuleset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxYaraElseRuleset.Controls.Add(this.labelYaraElseRulesFilesDescription);
            this.groupBoxYaraElseRuleset.Location = new System.Drawing.Point(6, 177);
            this.groupBoxYaraElseRuleset.Name = "groupBoxYaraElseRuleset";
            this.groupBoxYaraElseRuleset.Size = new System.Drawing.Size(626, 45);
            this.groupBoxYaraElseRuleset.TabIndex = 26;
            this.groupBoxYaraElseRuleset.TabStop = false;
            this.groupBoxYaraElseRuleset.Text = "Else YARA ruleset:";
            // 
            // labelYaraElseRulesFilesDescription
            // 
            this.labelYaraElseRulesFilesDescription.AutoSize = true;
            this.labelYaraElseRulesFilesDescription.Location = new System.Drawing.Point(13, 20);
            this.labelYaraElseRulesFilesDescription.Name = "labelYaraElseRulesFilesDescription";
            this.labelYaraElseRulesFilesDescription.Size = new System.Drawing.Size(177, 13);
            this.labelYaraElseRulesFilesDescription.TabIndex = 11;
            this.labelYaraElseRulesFilesDescription.Text = "0 file(s) selected. (Click here to edit) ";
            this.labelYaraElseRulesFilesDescription.Click += new System.EventHandler(this.labelYaraElseRulesFilesDescription_Click);
            this.labelYaraElseRulesFilesDescription.MouseEnter += new System.EventHandler(this.labelRuleFilesDescription_MouseEnter);
            this.labelYaraElseRulesFilesDescription.MouseLeave += new System.EventHandler(this.labelRuleFilesDescription_MouseLeave);
            // 
            // groupBoxYaraConditionalRuleset
            // 
            this.groupBoxYaraConditionalRuleset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxYaraConditionalRuleset.Controls.Add(this.panelYaraCondition);
            this.groupBoxYaraConditionalRuleset.Controls.Add(this.listBoxYaraConditions);
            this.groupBoxYaraConditionalRuleset.Controls.Add(this.btnNewAddYaraCondition);
            this.groupBoxYaraConditionalRuleset.Location = new System.Drawing.Point(6, 56);
            this.groupBoxYaraConditionalRuleset.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxYaraConditionalRuleset.Name = "groupBoxYaraConditionalRuleset";
            this.groupBoxYaraConditionalRuleset.Size = new System.Drawing.Size(626, 115);
            this.groupBoxYaraConditionalRuleset.TabIndex = 25;
            this.groupBoxYaraConditionalRuleset.TabStop = false;
            this.groupBoxYaraConditionalRuleset.Text = "Conditional YARA rulesets:";
            // 
            // panelYaraCondition
            // 
            this.panelYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelYaraCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelYaraCondition.Controls.Add(this.labelYaraConditionRuleFilesDescription);
            this.panelYaraCondition.Controls.Add(this.btnCancelAddYaraCondition);
            this.panelYaraCondition.Controls.Add(this.btnOkAddYaraCondition);
            this.panelYaraCondition.Controls.Add(this.comboConditionType);
            this.panelYaraCondition.Controls.Add(this.tbYaraConditionValue);
            this.panelYaraCondition.Location = new System.Drawing.Point(254, 19);
            this.panelYaraCondition.Name = "panelYaraCondition";
            this.panelYaraCondition.Size = new System.Drawing.Size(366, 89);
            this.panelYaraCondition.TabIndex = 26;
            // 
            // labelYaraConditionRuleFilesDescription
            // 
            this.labelYaraConditionRuleFilesDescription.AutoSize = true;
            this.labelYaraConditionRuleFilesDescription.Location = new System.Drawing.Point(27, 47);
            this.labelYaraConditionRuleFilesDescription.Name = "labelYaraConditionRuleFilesDescription";
            this.labelYaraConditionRuleFilesDescription.Size = new System.Drawing.Size(177, 13);
            this.labelYaraConditionRuleFilesDescription.TabIndex = 32;
            this.labelYaraConditionRuleFilesDescription.Text = "0 file(s) selected. (Click here to edit) ";
            this.labelYaraConditionRuleFilesDescription.Click += new System.EventHandler(this.labelYaraConditionRuleFilesDescription_Click);
            this.labelYaraConditionRuleFilesDescription.MouseEnter += new System.EventHandler(this.labelRuleFilesDescription_MouseEnter);
            this.labelYaraConditionRuleFilesDescription.MouseLeave += new System.EventHandler(this.labelRuleFilesDescription_MouseLeave);
            // 
            // btnCancelAddYaraCondition
            // 
            this.btnCancelAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelAddYaraCondition.Location = new System.Drawing.Point(299, 55);
            this.btnCancelAddYaraCondition.Name = "btnCancelAddYaraCondition";
            this.btnCancelAddYaraCondition.Size = new System.Drawing.Size(54, 23);
            this.btnCancelAddYaraCondition.TabIndex = 31;
            this.btnCancelAddYaraCondition.Text = "Cancel";
            this.btnCancelAddYaraCondition.UseVisualStyleBackColor = true;
            this.btnCancelAddYaraCondition.Click += new System.EventHandler(this.btnCancelAddYaraCondition_Click);
            // 
            // btnOkAddYaraCondition
            // 
            this.btnOkAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkAddYaraCondition.Location = new System.Drawing.Point(244, 55);
            this.btnOkAddYaraCondition.Name = "btnOkAddYaraCondition";
            this.btnOkAddYaraCondition.Size = new System.Drawing.Size(54, 23);
            this.btnOkAddYaraCondition.TabIndex = 20;
            this.btnOkAddYaraCondition.Text = "OK";
            this.btnOkAddYaraCondition.UseVisualStyleBackColor = true;
            this.btnOkAddYaraCondition.Click += new System.EventHandler(this.btnOkAddCondition_Click);
            // 
            // comboConditionType
            // 
            this.comboConditionType.FormattingEnabled = true;
            this.comboConditionType.Items.AddRange(new object[] {
            "PE File",
            "File Extension",
            "MIME Type"});
            this.comboConditionType.Location = new System.Drawing.Point(14, 12);
            this.comboConditionType.Name = "comboConditionType";
            this.comboConditionType.Size = new System.Drawing.Size(101, 21);
            this.comboConditionType.TabIndex = 28;
            this.comboConditionType.Text = "(select)";
            this.comboConditionType.SelectionChangeCommitted += new System.EventHandler(this.comboConditionType_SelectionChangeCommitted);
            // 
            // tbYaraConditionValue
            // 
            this.tbYaraConditionValue.Location = new System.Drawing.Point(121, 12);
            this.tbYaraConditionValue.Name = "tbYaraConditionValue";
            this.tbYaraConditionValue.Size = new System.Drawing.Size(232, 20);
            this.tbYaraConditionValue.TabIndex = 19;
            this.tbYaraConditionValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbYaraConditionValue_KeyUp);
            // 
            // listBoxYaraConditions
            // 
            this.listBoxYaraConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxYaraConditions.FormattingEnabled = true;
            this.listBoxYaraConditions.IntegralHeight = false;
            this.listBoxYaraConditions.Location = new System.Drawing.Point(6, 19);
            this.listBoxYaraConditions.Name = "listBoxYaraConditions";
            this.listBoxYaraConditions.Size = new System.Drawing.Size(242, 89);
            this.listBoxYaraConditions.TabIndex = 0;
            this.listBoxYaraConditions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxYaraConditions_KeyUp);
            // 
            // btnNewAddYaraCondition
            // 
            this.btnNewAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewAddYaraCondition.Location = new System.Drawing.Point(254, 19);
            this.btnNewAddYaraCondition.Name = "btnNewAddYaraCondition";
            this.btnNewAddYaraCondition.Size = new System.Drawing.Size(95, 23);
            this.btnNewAddYaraCondition.TabIndex = 27;
            this.btnNewAddYaraCondition.Text = "Add...";
            this.btnNewAddYaraCondition.UseVisualStyleBackColor = true;
            this.btnNewAddYaraCondition.Click += new System.EventHandler(this.btnNewAddYaraCondition_Click);
            // 
            // groupBoxYaraBaseRuleset
            // 
            this.groupBoxYaraBaseRuleset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxYaraBaseRuleset.Controls.Add(this.labelYaraBaseRulesFilesDescription);
            this.groupBoxYaraBaseRuleset.Location = new System.Drawing.Point(6, 5);
            this.groupBoxYaraBaseRuleset.Name = "groupBoxYaraBaseRuleset";
            this.groupBoxYaraBaseRuleset.Size = new System.Drawing.Size(626, 45);
            this.groupBoxYaraBaseRuleset.TabIndex = 24;
            this.groupBoxYaraBaseRuleset.TabStop = false;
            this.groupBoxYaraBaseRuleset.Text = "Base YARA ruleset:";
            // 
            // labelYaraBaseRulesFilesDescription
            // 
            this.labelYaraBaseRulesFilesDescription.AutoSize = true;
            this.labelYaraBaseRulesFilesDescription.Location = new System.Drawing.Point(18, 20);
            this.labelYaraBaseRulesFilesDescription.Name = "labelYaraBaseRulesFilesDescription";
            this.labelYaraBaseRulesFilesDescription.Size = new System.Drawing.Size(177, 13);
            this.labelYaraBaseRulesFilesDescription.TabIndex = 10;
            this.labelYaraBaseRulesFilesDescription.Text = "0 file(s) selected. (Click here to edit) ";
            this.labelYaraBaseRulesFilesDescription.Click += new System.EventHandler(this.labelYaraBaseRulesFilesDescription_Click);
            this.labelYaraBaseRulesFilesDescription.MouseEnter += new System.EventHandler(this.labelRuleFilesDescription_MouseEnter);
            this.labelYaraBaseRulesFilesDescription.MouseLeave += new System.EventHandler(this.labelRuleFilesDescription_MouseLeave);
            // 
            // btnYaraLoadConfig
            // 
            this.btnYaraLoadConfig.Location = new System.Drawing.Point(154, 229);
            this.btnYaraLoadConfig.Name = "btnYaraLoadConfig";
            this.btnYaraLoadConfig.Size = new System.Drawing.Size(142, 23);
            this.btnYaraLoadConfig.TabIndex = 23;
            this.btnYaraLoadConfig.Text = "Load YARA settings...";
            this.btnYaraLoadConfig.UseVisualStyleBackColor = true;
            this.btnYaraLoadConfig.Click += new System.EventHandler(this.btnYaraLoadConfiguration_Click);
            // 
            // btnYaraSaveConfig
            // 
            this.btnYaraSaveConfig.Location = new System.Drawing.Point(6, 229);
            this.btnYaraSaveConfig.Name = "btnYaraSaveConfig";
            this.btnYaraSaveConfig.Size = new System.Drawing.Size(142, 23);
            this.btnYaraSaveConfig.TabIndex = 22;
            this.btnYaraSaveConfig.Text = "Save YARA settings...";
            this.btnYaraSaveConfig.UseVisualStyleBackColor = true;
            this.btnYaraSaveConfig.Click += new System.EventHandler(this.btnYaraSaveConfiguration_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(3, 383);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(198, 27);
            this.btnScan.TabIndex = 7;
            this.btnScan.Text = "Begin scan...";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutput.Location = new System.Drawing.Point(0, 0);
            this.tbOutput.Margin = new System.Windows.Forms.Padding(1);
            this.tbOutput.MinimumSize = new System.Drawing.Size(4, 60);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(644, 64);
            this.tbOutput.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panelTop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelBottom, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(644, 477);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTop.Controls.Add(this.flowLayoutPanelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(644, 413);
            this.panelTop.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.Controls.Add(this.tbOutput);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 413);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(644, 64);
            this.panelBottom.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 477);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(660, 270);
            this.Name = "MainForm";
            this.Text = "Judge, Jury, and Executable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.flowLayoutPanelTop.ResumeLayout(false);
            this.panelParameters.ResumeLayout(false);
            this.panelParameters.PerformLayout();
            this.panelYara.ResumeLayout(false);
            this.groupBoxYaraElseRuleset.ResumeLayout(false);
            this.groupBoxYaraElseRuleset.PerformLayout();
            this.groupBoxYaraConditionalRuleset.ResumeLayout(false);
            this.panelYaraCondition.ResumeLayout(false);
            this.panelYaraCondition.PerformLayout();
            this.groupBoxYaraBaseRuleset.ResumeLayout(false);
            this.groupBoxYaraBaseRuleset.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
        private System.Windows.Forms.Panel panelParameters;
        private System.Windows.Forms.CheckBox checkBoxYaraRules;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.CheckBox checkboxOnlineCertValidation;
        private System.Windows.Forms.Label labelSearchPatterns;
        private System.Windows.Forms.TextBox tbSearchPatterns;
        private System.Windows.Forms.CheckBox checkboxCalculateEntropy;
        private System.Windows.Forms.Panel panelYara;
        private System.Windows.Forms.GroupBox groupBoxYaraElseRuleset;
        private System.Windows.Forms.Label labelYaraElseRulesFilesDescription;
        private System.Windows.Forms.GroupBox groupBoxYaraConditionalRuleset;
        private System.Windows.Forms.Panel panelYaraCondition;
        private System.Windows.Forms.Label labelYaraConditionRuleFilesDescription;
        private System.Windows.Forms.Button btnCancelAddYaraCondition;
        private System.Windows.Forms.Button btnOkAddYaraCondition;
        private System.Windows.Forms.ComboBox comboConditionType;
        private System.Windows.Forms.TextBox tbYaraConditionValue;
        private System.Windows.Forms.ListBox listBoxYaraConditions;
        private System.Windows.Forms.Button btnNewAddYaraCondition;
        private System.Windows.Forms.GroupBox groupBoxYaraBaseRuleset;
        private System.Windows.Forms.Label labelYaraBaseRulesFilesDescription;
        private System.Windows.Forms.Button btnYaraLoadConfig;
        private System.Windows.Forms.Button btnYaraSaveConfig;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
    }
}

