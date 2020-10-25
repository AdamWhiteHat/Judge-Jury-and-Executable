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
			this.btnBrowse = new System.Windows.Forms.Button();
			this.tbPath = new System.Windows.Forms.TextBox();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.checkboxCalculateEntropy = new System.Windows.Forms.CheckBox();
			this.tbSearchPatterns = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.panelParameters = new System.Windows.Forms.Panel();
			this.linkGitHub = new System.Windows.Forms.LinkLabel();
			this.checkBoxYaraRules = new System.Windows.Forms.CheckBox();
			this.panelYaraParameters = new System.Windows.Forms.Panel();
			this.btnYaraLoad = new System.Windows.Forms.Button();
			this.btnYaraSave = new System.Windows.Forms.Button();
			this.btnRemoveYaraFilter = new System.Windows.Forms.Button();
			this.btnAddYaraFilter = new System.Windows.Forms.Button();
			this.panelListBox = new System.Windows.Forms.Panel();
			this.listBoxYaraFilters = new System.Windows.Forms.ListBox();
			this.panelYara = new System.Windows.Forms.Panel();
			this.radioButtonYara_ElseNoMatch = new System.Windows.Forms.RadioButton();
			this.panelYaraMatchRules = new System.Windows.Forms.Panel();
			this.tbYaraRuleMatchFiles = new System.Windows.Forms.TextBox();
			this.btnBrowseYaraMatch = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.panelYaraFilterValue = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.tbYaraFilterValue = new System.Windows.Forms.TextBox();
			this.radioButtonYara_AlwaysRun = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_IsPeFile = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_Extention = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_MimeType = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelTop = new System.Windows.Forms.Panel();
			this.panelParameters.SuspendLayout();
			this.panelYaraParameters.SuspendLayout();
			this.panelListBox.SuspendLayout();
			this.panelYara.SuspendLayout();
			this.panelYaraMatchRules.SuspendLayout();
			this.panelYaraFilterValue.SuspendLayout();
			this.flowLayoutPanelTop.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(660, 3);
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
			this.tbPath.Size = new System.Drawing.Size(651, 20);
			this.tbPath.TabIndex = 1;
			this.tbPath.Text = "C:\\";
			this.tbPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// tbOutput
			// 
			this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbOutput.Location = new System.Drawing.Point(0, 0);
			this.tbOutput.Margin = new System.Windows.Forms.Padding(1);
			this.tbOutput.MinimumSize = new System.Drawing.Size(759, 60);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(759, 77);
			this.tbOutput.TabIndex = 2;
			this.tbOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbOutput_KeyUp);
			// 
			// checkboxCalculateEntropy
			// 
			this.checkboxCalculateEntropy.AutoSize = true;
			this.checkboxCalculateEntropy.Location = new System.Drawing.Point(3, 62);
			this.checkboxCalculateEntropy.Name = "checkboxCalculateEntropy";
			this.checkboxCalculateEntropy.Size = new System.Drawing.Size(109, 17);
			this.checkboxCalculateEntropy.TabIndex = 3;
			this.checkboxCalculateEntropy.Text = "Calculate Entropy";
			this.checkboxCalculateEntropy.UseVisualStyleBackColor = true;
			// 
			// tbSearchPatterns
			// 
			this.tbSearchPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbSearchPatterns.Location = new System.Drawing.Point(98, 29);
			this.tbSearchPatterns.Name = "tbSearchPatterns";
			this.tbSearchPatterns.Size = new System.Drawing.Size(523, 20);
			this.tbSearchPatterns.TabIndex = 4;
			this.tbSearchPatterns.Text = "*.exe|*.dll|*.drv";
			this.tbSearchPatterns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Search patterns:";
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(3, 262);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(198, 27);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panelParameters
			// 
			this.panelParameters.Controls.Add(this.linkGitHub);
			this.panelParameters.Controls.Add(this.checkBoxYaraRules);
			this.panelParameters.Controls.Add(this.btnBrowse);
			this.panelParameters.Controls.Add(this.tbPath);
			this.panelParameters.Controls.Add(this.label1);
			this.panelParameters.Controls.Add(this.tbSearchPatterns);
			this.panelParameters.Controls.Add(this.checkboxCalculateEntropy);
			this.panelParameters.Location = new System.Drawing.Point(1, 1);
			this.panelParameters.Margin = new System.Windows.Forms.Padding(1);
			this.panelParameters.MinimumSize = new System.Drawing.Size(757, 103);
			this.panelParameters.Name = "panelParameters";
			this.panelParameters.Size = new System.Drawing.Size(757, 103);
			this.panelParameters.TabIndex = 8;
			// 
			// linkGitHub
			// 
			this.linkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.linkGitHub.AutoSize = true;
			this.linkGitHub.Location = new System.Drawing.Point(448, 85);
			this.linkGitHub.Name = "linkGitHub";
			this.linkGitHub.Size = new System.Drawing.Size(305, 13);
			this.linkGitHub.TabIndex = 11;
			this.linkGitHub.TabStop = true;
			this.linkGitHub.Text = "https://github.com/AdamWhiteHat/Judge-Jury-and-Executable";
			this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
			// 
			// checkBoxYaraRules
			// 
			this.checkBoxYaraRules.AutoSize = true;
			this.checkBoxYaraRules.Checked = true;
			this.checkBoxYaraRules.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxYaraRules.Location = new System.Drawing.Point(3, 84);
			this.checkBoxYaraRules.Name = "checkBoxYaraRules";
			this.checkBoxYaraRules.Size = new System.Drawing.Size(101, 17);
			this.checkBoxYaraRules.TabIndex = 10;
			this.checkBoxYaraRules.Text = "Run Yara Rules";
			this.checkBoxYaraRules.UseVisualStyleBackColor = true;
			this.checkBoxYaraRules.CheckedChanged += new System.EventHandler(this.checkBoxYaraRules_CheckedChanged);
			// 
			// panelYaraParameters
			// 
			this.panelYaraParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYaraParameters.Controls.Add(this.btnYaraLoad);
			this.panelYaraParameters.Controls.Add(this.btnYaraSave);
			this.panelYaraParameters.Controls.Add(this.btnRemoveYaraFilter);
			this.panelYaraParameters.Controls.Add(this.btnAddYaraFilter);
			this.panelYaraParameters.Controls.Add(this.panelListBox);
			this.panelYaraParameters.Controls.Add(this.panelYara);
			this.panelYaraParameters.Location = new System.Drawing.Point(1, 106);
			this.panelYaraParameters.Margin = new System.Windows.Forms.Padding(1);
			this.panelYaraParameters.MinimumSize = new System.Drawing.Size(757, 0);
			this.panelYaraParameters.Name = "panelYaraParameters";
			this.panelYaraParameters.Size = new System.Drawing.Size(757, 152);
			this.panelYaraParameters.TabIndex = 9;
			// 
			// btnYaraLoad
			// 
			this.btnYaraLoad.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnYaraLoad.Location = new System.Drawing.Point(626, 127);
			this.btnYaraLoad.Name = "btnYaraLoad";
			this.btnYaraLoad.Size = new System.Drawing.Size(114, 22);
			this.btnYaraLoad.TabIndex = 23;
			this.btnYaraLoad.Text = "Load filter";
			this.btnYaraLoad.UseVisualStyleBackColor = true;
			this.btnYaraLoad.Click += new System.EventHandler(this.btnYaraLoad_Click);
			// 
			// btnYaraSave
			// 
			this.btnYaraSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnYaraSave.Location = new System.Drawing.Point(626, 106);
			this.btnYaraSave.Name = "btnYaraSave";
			this.btnYaraSave.Size = new System.Drawing.Size(114, 22);
			this.btnYaraSave.TabIndex = 22;
			this.btnYaraSave.Text = "Export filters";
			this.btnYaraSave.UseVisualStyleBackColor = true;
			this.btnYaraSave.Click += new System.EventHandler(this.btnYaraSave_Click);
			// 
			// btnRemoveYaraFilter
			// 
			this.btnRemoveYaraFilter.Location = new System.Drawing.Point(522, 56);
			this.btnRemoveYaraFilter.Name = "btnRemoveYaraFilter";
			this.btnRemoveYaraFilter.Size = new System.Drawing.Size(87, 23);
			this.btnRemoveYaraFilter.TabIndex = 21;
			this.btnRemoveYaraFilter.Text = "<- Remove";
			this.btnRemoveYaraFilter.UseVisualStyleBackColor = true;
			this.btnRemoveYaraFilter.Click += new System.EventHandler(this.btnRemoveYaraFilter_Click);
			// 
			// btnAddYaraFilter
			// 
			this.btnAddYaraFilter.Location = new System.Drawing.Point(522, 27);
			this.btnAddYaraFilter.Name = "btnAddYaraFilter";
			this.btnAddYaraFilter.Size = new System.Drawing.Size(87, 23);
			this.btnAddYaraFilter.TabIndex = 20;
			this.btnAddYaraFilter.Text = "Add ->";
			this.btnAddYaraFilter.UseVisualStyleBackColor = true;
			this.btnAddYaraFilter.Click += new System.EventHandler(this.btnAddYaraFilter_Click);
			// 
			// panelListBox
			// 
			this.panelListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelListBox.Controls.Add(this.listBoxYaraFilters);
			this.panelListBox.Location = new System.Drawing.Point(614, 5);
			this.panelListBox.Name = "panelListBox";
			this.panelListBox.Size = new System.Drawing.Size(137, 101);
			this.panelListBox.TabIndex = 19;
			// 
			// listBoxYaraFilters
			// 
			this.listBoxYaraFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxYaraFilters.FormattingEnabled = true;
			this.listBoxYaraFilters.Location = new System.Drawing.Point(2, 2);
			this.listBoxYaraFilters.Name = "listBoxYaraFilters";
			this.listBoxYaraFilters.Size = new System.Drawing.Size(131, 95);
			this.listBoxYaraFilters.TabIndex = 0;
			this.listBoxYaraFilters.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxYaraFilters_KeyUp);
			// 
			// panelYara
			// 
			this.panelYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYara.Controls.Add(this.radioButtonYara_ElseNoMatch);
			this.panelYara.Controls.Add(this.panelYaraMatchRules);
			this.panelYara.Controls.Add(this.panelYaraFilterValue);
			this.panelYara.Controls.Add(this.radioButtonYara_AlwaysRun);
			this.panelYara.Controls.Add(this.radioButtonYara_IsPeFile);
			this.panelYara.Controls.Add(this.radioButtonYara_Extention);
			this.panelYara.Controls.Add(this.radioButtonYara_MimeType);
			this.panelYara.Location = new System.Drawing.Point(3, 4);
			this.panelYara.Margin = new System.Windows.Forms.Padding(0);
			this.panelYara.Name = "panelYara";
			this.panelYara.Size = new System.Drawing.Size(513, 137);
			this.panelYara.TabIndex = 18;
			// 
			// radioButtonYara_ElseNoMatch
			// 
			this.radioButtonYara_ElseNoMatch.AutoSize = true;
			this.radioButtonYara_ElseNoMatch.Location = new System.Drawing.Point(4, 98);
			this.radioButtonYara_ElseNoMatch.Name = "radioButtonYara_ElseNoMatch";
			this.radioButtonYara_ElseNoMatch.Size = new System.Drawing.Size(118, 17);
			this.radioButtonYara_ElseNoMatch.TabIndex = 28;
			this.radioButtonYara_ElseNoMatch.TabStop = true;
			this.radioButtonYara_ElseNoMatch.Text = "ELSE (No matches)";
			this.radioButtonYara_ElseNoMatch.UseVisualStyleBackColor = true;
			this.radioButtonYara_ElseNoMatch.CheckedChanged += new System.EventHandler(this.radioButtonYara_HideFilterValue_CheckedChanged);
			// 
			// panelYaraMatchRules
			// 
			this.panelYaraMatchRules.Controls.Add(this.tbYaraRuleMatchFiles);
			this.panelYaraMatchRules.Controls.Add(this.btnBrowseYaraMatch);
			this.panelYaraMatchRules.Controls.Add(this.label2);
			this.panelYaraMatchRules.Location = new System.Drawing.Point(124, 75);
			this.panelYaraMatchRules.Name = "panelYaraMatchRules";
			this.panelYaraMatchRules.Size = new System.Drawing.Size(384, 45);
			this.panelYaraMatchRules.TabIndex = 27;
			// 
			// tbYaraRuleMatchFiles
			// 
			this.tbYaraRuleMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleMatchFiles.Location = new System.Drawing.Point(48, 22);
			this.tbYaraRuleMatchFiles.Name = "tbYaraRuleMatchFiles";
			this.tbYaraRuleMatchFiles.Size = new System.Drawing.Size(253, 20);
			this.tbYaraRuleMatchFiles.TabIndex = 8;
			// 
			// btnBrowseYaraMatch
			// 
			this.btnBrowseYaraMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraMatch.Location = new System.Drawing.Point(306, 20);
			this.btnBrowseYaraMatch.Name = "btnBrowseYaraMatch";
			this.btnBrowseYaraMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraMatch.TabIndex = 7;
			this.btnBrowseYaraMatch.Text = "Browse...";
			this.btnBrowseYaraMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraMatch.Click += new System.EventHandler(this.btnBrowseYaraMatch_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 5);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 23;
			this.label2.Text = "YARA rules to run:";
			// 
			// panelYaraFilterValue
			// 
			this.panelYaraFilterValue.Controls.Add(this.label5);
			this.panelYaraFilterValue.Controls.Add(this.tbYaraFilterValue);
			this.panelYaraFilterValue.Location = new System.Drawing.Point(124, 5);
			this.panelYaraFilterValue.Name = "panelYaraFilterValue";
			this.panelYaraFilterValue.Size = new System.Drawing.Size(384, 44);
			this.panelYaraFilterValue.TabIndex = 25;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 5);
			this.label5.Margin = new System.Windows.Forms.Padding(3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Yara filter value:";
			// 
			// tbYaraFilterValue
			// 
			this.tbYaraFilterValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraFilterValue.Location = new System.Drawing.Point(48, 22);
			this.tbYaraFilterValue.Name = "tbYaraFilterValue";
			this.tbYaraFilterValue.Size = new System.Drawing.Size(253, 20);
			this.tbYaraFilterValue.TabIndex = 19;
			// 
			// radioButtonYara_AlwaysRun
			// 
			this.radioButtonYara_AlwaysRun.AutoSize = true;
			this.radioButtonYara_AlwaysRun.Location = new System.Drawing.Point(4, 76);
			this.radioButtonYara_AlwaysRun.Name = "radioButtonYara_AlwaysRun";
			this.radioButtonYara_AlwaysRun.Size = new System.Drawing.Size(115, 17);
			this.radioButtonYara_AlwaysRun.TabIndex = 13;
			this.radioButtonYara_AlwaysRun.TabStop = true;
			this.radioButtonYara_AlwaysRun.Text = "IF true (Always run)";
			this.radioButtonYara_AlwaysRun.UseVisualStyleBackColor = true;
			this.radioButtonYara_AlwaysRun.CheckedChanged += new System.EventHandler(this.radioButtonYara_HideFilterValue_CheckedChanged);
			// 
			// radioButtonYara_IsPeFile
			// 
			this.radioButtonYara_IsPeFile.AutoSize = true;
			this.radioButtonYara_IsPeFile.Location = new System.Drawing.Point(4, 54);
			this.radioButtonYara_IsPeFile.Name = "radioButtonYara_IsPeFile";
			this.radioButtonYara_IsPeFile.Size = new System.Drawing.Size(153, 17);
			this.radioButtonYara_IsPeFile.TabIndex = 15;
			this.radioButtonYara_IsPeFile.TabStop = true;
			this.radioButtonYara_IsPeFile.Text = "IF file is an executable (PE)";
			this.radioButtonYara_IsPeFile.UseVisualStyleBackColor = true;
			this.radioButtonYara_IsPeFile.CheckedChanged += new System.EventHandler(this.radioButtonYara_HideFilterValue_CheckedChanged);
			// 
			// radioButtonYara_Extention
			// 
			this.radioButtonYara_Extention.AutoSize = true;
			this.radioButtonYara_Extention.Location = new System.Drawing.Point(4, 32);
			this.radioButtonYara_Extention.Name = "radioButtonYara_Extention";
			this.radioButtonYara_Extention.Size = new System.Drawing.Size(109, 17);
			this.radioButtonYara_Extention.TabIndex = 16;
			this.radioButtonYara_Extention.TabStop = true;
			this.radioButtonYara_Extention.Text = "IF file extention is:";
			this.radioButtonYara_Extention.UseVisualStyleBackColor = true;
			this.radioButtonYara_Extention.CheckedChanged += new System.EventHandler(this.radioButtonYara_ShowFilterValue_CheckedChanged);
			// 
			// radioButtonYara_MimeType
			// 
			this.radioButtonYara_MimeType.AutoSize = true;
			this.radioButtonYara_MimeType.Location = new System.Drawing.Point(4, 10);
			this.radioButtonYara_MimeType.Name = "radioButtonYara_MimeType";
			this.radioButtonYara_MimeType.Size = new System.Drawing.Size(101, 17);
			this.radioButtonYara_MimeType.TabIndex = 17;
			this.radioButtonYara_MimeType.TabStop = true;
			this.radioButtonYara_MimeType.Text = "IF MIME type is:";
			this.radioButtonYara_MimeType.UseVisualStyleBackColor = true;
			this.radioButtonYara_MimeType.CheckedChanged += new System.EventHandler(this.radioButtonYara_ShowFilterValue_CheckedChanged);
			// 
			// flowLayoutPanelTop
			// 
			this.flowLayoutPanelTop.AutoSize = true;
			this.flowLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanelTop.Controls.Add(this.panelParameters);
			this.flowLayoutPanelTop.Controls.Add(this.panelYaraParameters);
			this.flowLayoutPanelTop.Controls.Add(this.btnSearch);
			this.flowLayoutPanelTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanelTop.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanelTop.MinimumSize = new System.Drawing.Size(759, 150);
			this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
			this.flowLayoutPanelTop.Size = new System.Drawing.Size(759, 292);
			this.flowLayoutPanelTop.TabIndex = 9;
			this.flowLayoutPanelTop.Resize += new System.EventHandler(this.MainForm_Resize);
			// 
			// panelBottom
			// 
			this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelBottom.Controls.Add(this.tbOutput);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBottom.Location = new System.Drawing.Point(0, 292);
			this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(759, 77);
			this.panelBottom.TabIndex = 10;
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
			this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(759, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(759, 369);
			this.tableLayoutPanel1.TabIndex = 10;
			// 
			// panelTop
			// 
			this.panelTop.AutoSize = true;
			this.panelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelTop.Controls.Add(this.flowLayoutPanelTop);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Margin = new System.Windows.Forms.Padding(0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(759, 292);
			this.panelTop.TabIndex = 11;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(759, 369);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(775, 300);
			this.Name = "MainForm";
			this.Text = "Judge, Jury, and Executable";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.panelParameters.ResumeLayout(false);
			this.panelParameters.PerformLayout();
			this.panelYaraParameters.ResumeLayout(false);
			this.panelListBox.ResumeLayout(false);
			this.panelYara.ResumeLayout(false);
			this.panelYara.PerformLayout();
			this.panelYaraMatchRules.ResumeLayout(false);
			this.panelYaraMatchRules.PerformLayout();
			this.panelYaraFilterValue.ResumeLayout(false);
			this.panelYaraFilterValue.PerformLayout();
			this.flowLayoutPanelTop.ResumeLayout(false);
			this.panelBottom.ResumeLayout(false);
			this.panelBottom.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panelTop.ResumeLayout(false);
			this.panelTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.CheckBox checkboxCalculateEntropy;
		private System.Windows.Forms.TextBox tbSearchPatterns;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Panel panelParameters;
		private System.Windows.Forms.Button btnBrowseYaraMatch;
		private System.Windows.Forms.TextBox tbYaraRuleMatchFiles;
		private System.Windows.Forms.Panel panelYaraParameters;
		private System.Windows.Forms.CheckBox checkBoxYaraRules;
		private System.Windows.Forms.Button btnAddYaraFilter;
		private System.Windows.Forms.Panel panelListBox;
		private System.Windows.Forms.ListBox listBoxYaraFilters;
		private System.Windows.Forms.Panel panelYara;
		private System.Windows.Forms.RadioButton radioButtonYara_MimeType;
		private System.Windows.Forms.RadioButton radioButtonYara_Extention;
		private System.Windows.Forms.RadioButton radioButtonYara_IsPeFile;
		private System.Windows.Forms.RadioButton radioButtonYara_AlwaysRun;
		private System.Windows.Forms.TextBox tbYaraFilterValue;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnRemoveYaraFilter;
		private System.Windows.Forms.Button btnYaraSave;
		private System.Windows.Forms.Button btnYaraLoad;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Panel panelYaraFilterValue;
		private System.Windows.Forms.Panel panelYaraMatchRules;
		private System.Windows.Forms.RadioButton radioButtonYara_ElseNoMatch;
		private System.Windows.Forms.LinkLabel linkGitHub;
	}
}

