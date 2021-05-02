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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.btnBrowse = new System.Windows.Forms.Button();
			this.tbPath = new System.Windows.Forms.TextBox();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.checkboxCalculateEntropy = new System.Windows.Forms.CheckBox();
			this.tbSearchPatterns = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.panelParameters = new System.Windows.Forms.Panel();
			this.checkBoxYaraRules = new System.Windows.Forms.CheckBox();
			this.panelYaraParameters = new System.Windows.Forms.Panel();
			this.splitContainerYara = new System.Windows.Forms.SplitContainer();
			this.panelYaraCondition = new System.Windows.Forms.Panel();
			this.panelYaraFilter_Buttons = new System.Windows.Forms.Panel();
			this.btnCancelAddYaraCondition = new System.Windows.Forms.Button();
			this.btnOkAddYaraCondition = new System.Windows.Forms.Button();
			this.listYaraMatchFiles = new System.Windows.Forms.ListView();
			this.contextMenuYaraMatchFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem_RemoveMatchFile = new System.Windows.Forms.ToolStripMenuItem();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnBrowseYaraMatch = new System.Windows.Forms.Button();
			this.comboConditionType = new System.Windows.Forms.ComboBox();
			this.tbYaraConditionValue = new System.Windows.Forms.TextBox();
			this.btnNewAddYaraCondition = new System.Windows.Forms.Button();
			this.btnYaraSave = new System.Windows.Forms.Button();
			this.btnYaraLoad = new System.Windows.Forms.Button();
			this.treeViewYaraFilters = new System.Windows.Forms.TreeView();
			this.contextMenuYaraTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.removeTreeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
			this.panelSearchButton = new System.Windows.Forms.Panel();
			this.panelPersistenceOptions = new System.Windows.Forms.Panel();
			this.labelTextBoxDescription = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPersistenceBrowse = new System.Windows.Forms.Button();
			this.tbPersistenceParameter = new System.Windows.Forms.TextBox();
			this.radioPersistenceCSV = new System.Windows.Forms.RadioButton();
			this.radioPersistenceSqlite = new System.Windows.Forms.RadioButton();
			this.radioPersistenceSqlServer = new System.Windows.Forms.RadioButton();
			this.linkGitHub = new System.Windows.Forms.LinkLabel();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelTop = new System.Windows.Forms.Panel();
			this.yaraErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.dialogErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.panelParameters.SuspendLayout();
			this.panelYaraParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerYara)).BeginInit();
			this.splitContainerYara.Panel1.SuspendLayout();
			this.splitContainerYara.Panel2.SuspendLayout();
			this.splitContainerYara.SuspendLayout();
			this.panelYaraCondition.SuspendLayout();
			this.panelYaraFilter_Buttons.SuspendLayout();
			this.contextMenuYaraMatchFiles.SuspendLayout();
			this.contextMenuYaraTreeView.SuspendLayout();
			this.flowLayoutPanelTop.SuspendLayout();
			this.panelSearchButton.SuspendLayout();
			this.panelPersistenceOptions.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.yaraErrorProvider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dialogErrorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(660, 3);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(95, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// tbPath
			// 
			this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPath.Location = new System.Drawing.Point(22, 5);
			this.tbPath.Name = "tbPath";
			this.tbPath.Size = new System.Drawing.Size(632, 20);
			this.tbPath.TabIndex = 0;
			this.tbPath.Text = "C:\\";
			this.tbPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// tbOutput
			// 
			this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbOutput.Location = new System.Drawing.Point(0, 0);
			this.tbOutput.Margin = new System.Windows.Forms.Padding(1);
			this.tbOutput.MinimumSize = new System.Drawing.Size(759, 32);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(760, 125);
			this.tbOutput.TabIndex = 0;
			this.tbOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbOutput_KeyUp);
			// 
			// checkboxCalculateEntropy
			// 
			this.checkboxCalculateEntropy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
			this.tbSearchPatterns.TabIndex = 2;
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
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSearch.Location = new System.Drawing.Point(3, 99);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(198, 27);
			this.btnSearch.TabIndex = 0;
			this.btnSearch.Text = "Scan";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panelParameters
			// 
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
			this.panelParameters.TabIndex = 0;
			// 
			// checkBoxYaraRules
			// 
			this.checkBoxYaraRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxYaraRules.AutoSize = true;
			this.checkBoxYaraRules.Checked = true;
			this.checkBoxYaraRules.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxYaraRules.Location = new System.Drawing.Point(3, 84);
			this.checkBoxYaraRules.Name = "checkBoxYaraRules";
			this.checkBoxYaraRules.Size = new System.Drawing.Size(108, 17);
			this.checkBoxYaraRules.TabIndex = 4;
			this.checkBoxYaraRules.Text = "Run YARA Rules";
			this.checkBoxYaraRules.UseVisualStyleBackColor = true;
			this.checkBoxYaraRules.CheckedChanged += new System.EventHandler(this.checkBoxYaraRules_CheckedChanged);
			// 
			// panelYaraParameters
			// 
			this.panelYaraParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelYaraParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYaraParameters.Controls.Add(this.splitContainerYara);
			this.panelYaraParameters.Location = new System.Drawing.Point(1, 106);
			this.panelYaraParameters.Margin = new System.Windows.Forms.Padding(1);
			this.panelYaraParameters.MinimumSize = new System.Drawing.Size(757, 2);
			this.panelYaraParameters.Name = "panelYaraParameters";
			this.panelYaraParameters.Size = new System.Drawing.Size(757, 154);
			this.panelYaraParameters.TabIndex = 0;
			// 
			// splitContainerYara
			// 
			this.splitContainerYara.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerYara.Location = new System.Drawing.Point(0, -1);
			this.splitContainerYara.Name = "splitContainerYara";
			// 
			// splitContainerYara.Panel1
			// 
			this.splitContainerYara.Panel1.Controls.Add(this.panelYaraCondition);
			this.splitContainerYara.Panel1.Controls.Add(this.btnNewAddYaraCondition);
			this.splitContainerYara.Panel1.Controls.Add(this.btnYaraSave);
			this.splitContainerYara.Panel1.Controls.Add(this.btnYaraLoad);
			this.splitContainerYara.Panel1MinSize = 410;
			// 
			// splitContainerYara.Panel2
			// 
			this.splitContainerYara.Panel2.Controls.Add(this.treeViewYaraFilters);
			this.splitContainerYara.Panel2MinSize = 110;
			this.splitContainerYara.Size = new System.Drawing.Size(756, 154);
			this.splitContainerYara.SplitterDistance = 619;
			this.splitContainerYara.TabIndex = 32;
			// 
			// panelYaraCondition
			// 
			this.panelYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelYaraCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYaraCondition.Controls.Add(this.panelYaraFilter_Buttons);
			this.panelYaraCondition.Controls.Add(this.listYaraMatchFiles);
			this.panelYaraCondition.Controls.Add(this.label6);
			this.panelYaraCondition.Controls.Add(this.label5);
			this.panelYaraCondition.Controls.Add(this.label4);
			this.panelYaraCondition.Controls.Add(this.label3);
			this.panelYaraCondition.Controls.Add(this.btnBrowseYaraMatch);
			this.panelYaraCondition.Controls.Add(this.comboConditionType);
			this.panelYaraCondition.Controls.Add(this.tbYaraConditionValue);
			this.panelYaraCondition.Location = new System.Drawing.Point(-1, 0);
			this.panelYaraCondition.MinimumSize = new System.Drawing.Size(400, 100);
			this.panelYaraCondition.Name = "panelYaraCondition";
			this.panelYaraCondition.Size = new System.Drawing.Size(620, 154);
			this.panelYaraCondition.TabIndex = 0;
			// 
			// panelYaraFilter_Buttons
			// 
			this.panelYaraFilter_Buttons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.panelYaraFilter_Buttons.Controls.Add(this.btnCancelAddYaraCondition);
			this.panelYaraFilter_Buttons.Controls.Add(this.btnOkAddYaraCondition);
			this.panelYaraFilter_Buttons.Location = new System.Drawing.Point(464, 118);
			this.panelYaraFilter_Buttons.Name = "panelYaraFilter_Buttons";
			this.panelYaraFilter_Buttons.Size = new System.Drawing.Size(154, 33);
			this.panelYaraFilter_Buttons.TabIndex = 36;
			// 
			// btnCancelAddYaraCondition
			// 
			this.btnCancelAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancelAddYaraCondition.Location = new System.Drawing.Point(93, 5);
			this.btnCancelAddYaraCondition.Name = "btnCancelAddYaraCondition";
			this.btnCancelAddYaraCondition.Size = new System.Drawing.Size(54, 23);
			this.btnCancelAddYaraCondition.TabIndex = 5;
			this.btnCancelAddYaraCondition.Text = "Cancel";
			this.btnCancelAddYaraCondition.UseVisualStyleBackColor = true;
			this.btnCancelAddYaraCondition.Click += new System.EventHandler(this.btnCancelAddYaraCondition_Click);
			// 
			// btnOkAddYaraCondition
			// 
			this.btnOkAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkAddYaraCondition.Location = new System.Drawing.Point(6, 5);
			this.btnOkAddYaraCondition.Name = "btnOkAddYaraCondition";
			this.btnOkAddYaraCondition.Size = new System.Drawing.Size(81, 23);
			this.btnOkAddYaraCondition.TabIndex = 4;
			this.btnOkAddYaraCondition.Text = "OK";
			this.btnOkAddYaraCondition.UseVisualStyleBackColor = true;
			this.btnOkAddYaraCondition.Click += new System.EventHandler(this.btnOkAddYaraCondition_Click);
			// 
			// listYaraMatchFiles
			// 
			this.listYaraMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listYaraMatchFiles.ContextMenuStrip = this.contextMenuYaraMatchFiles;
			this.listYaraMatchFiles.HideSelection = false;
			this.listYaraMatchFiles.Location = new System.Drawing.Point(107, 40);
			this.listYaraMatchFiles.Name = "listYaraMatchFiles";
			this.listYaraMatchFiles.Size = new System.Drawing.Size(425, 77);
			this.listYaraMatchFiles.TabIndex = 2;
			this.listYaraMatchFiles.UseCompatibleStateImageBehavior = false;
			this.listYaraMatchFiles.View = System.Windows.Forms.View.List;
			this.listYaraMatchFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listYaraMatchFiles_KeyUp);
			this.listYaraMatchFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listYaraMatchFiles_MouseUp);
			// 
			// contextMenuYaraMatchFiles
			// 
			this.contextMenuYaraMatchFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_RemoveMatchFile});
			this.contextMenuYaraMatchFiles.Name = "contextMenuYaraMatchFiles";
			this.contextMenuYaraMatchFiles.ShowImageMargin = false;
			this.contextMenuYaraMatchFiles.Size = new System.Drawing.Size(93, 26);
			this.contextMenuYaraMatchFiles.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuListBox_Opening);
			// 
			// toolStripMenuItem_RemoveMatchFile
			// 
			this.toolStripMenuItem_RemoveMatchFile.Name = "toolStripMenuItem_RemoveMatchFile";
			this.toolStripMenuItem_RemoveMatchFile.Size = new System.Drawing.Size(92, 22);
			this.toolStripMenuItem_RemoveMatchFile.Text = "Remove";
			this.toolStripMenuItem_RemoveMatchFile.Click += new System.EventHandler(this.ToolStripMenuItem_RemoveMatchFile_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(2, 43);
			this.label6.Margin = new System.Windows.Forms.Padding(0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 13);
			this.label6.TabIndex = 35;
			this.label6.Text = "2)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(2, 15);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 13);
			this.label5.TabIndex = 34;
			this.label5.Text = "1)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(18, 43);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(81, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Select rule files:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 15);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "Select condition:";
			// 
			// btnBrowseYaraMatch
			// 
			this.btnBrowseYaraMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraMatch.Location = new System.Drawing.Point(538, 40);
			this.btnBrowseYaraMatch.Name = "btnBrowseYaraMatch";
			this.btnBrowseYaraMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraMatch.TabIndex = 3;
			this.btnBrowseYaraMatch.Text = "Browse...";
			this.btnBrowseYaraMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraMatch.Click += new System.EventHandler(this.btnBrowseYaraMatch_Click);
			// 
			// comboConditionType
			// 
			this.comboConditionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConditionType.Items.AddRange(new object[] {
            "Always",
            "PE File",
            "File Extension",
            "MIME Type",
            "Fail/No matches"});
			this.comboConditionType.Location = new System.Drawing.Point(107, 12);
			this.comboConditionType.Name = "comboConditionType";
			this.comboConditionType.Size = new System.Drawing.Size(105, 21);
			this.comboConditionType.TabIndex = 0;
			this.comboConditionType.SelectionChangeCommitted += new System.EventHandler(this.comboConditionType_SelectionChangeCommitted);
			// 
			// tbYaraConditionValue
			// 
			this.tbYaraConditionValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraConditionValue.Location = new System.Drawing.Point(233, 12);
			this.tbYaraConditionValue.Name = "tbYaraConditionValue";
			this.tbYaraConditionValue.Size = new System.Drawing.Size(363, 20);
			this.tbYaraConditionValue.TabIndex = 1;
			// 
			// btnNewAddYaraCondition
			// 
			this.btnNewAddYaraCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.btnNewAddYaraCondition.Location = new System.Drawing.Point(251, 68);
			this.btnNewAddYaraCondition.Name = "btnNewAddYaraCondition";
			this.btnNewAddYaraCondition.Size = new System.Drawing.Size(114, 23);
			this.btnNewAddYaraCondition.TabIndex = 0;
			this.btnNewAddYaraCondition.Text = "Add...";
			this.btnNewAddYaraCondition.UseVisualStyleBackColor = true;
			this.btnNewAddYaraCondition.Click += new System.EventHandler(this.btnNewAddYaraCondition_Click);
			// 
			// btnYaraSave
			// 
			this.btnYaraSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYaraSave.Location = new System.Drawing.Point(517, 35);
			this.btnYaraSave.Name = "btnYaraSave";
			this.btnYaraSave.Size = new System.Drawing.Size(99, 22);
			this.btnYaraSave.TabIndex = 1;
			this.btnYaraSave.Text = "Save filters...";
			this.btnYaraSave.UseVisualStyleBackColor = true;
			this.btnYaraSave.Click += new System.EventHandler(this.btnYaraSave_Click);
			// 
			// btnYaraLoad
			// 
			this.btnYaraLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYaraLoad.Location = new System.Drawing.Point(517, 56);
			this.btnYaraLoad.Name = "btnYaraLoad";
			this.btnYaraLoad.Size = new System.Drawing.Size(99, 22);
			this.btnYaraLoad.TabIndex = 2;
			this.btnYaraLoad.Text = "Load filters...";
			this.btnYaraLoad.UseVisualStyleBackColor = true;
			this.btnYaraLoad.Click += new System.EventHandler(this.btnYaraLoad_Click);
			// 
			// treeViewYaraFilters
			// 
			this.treeViewYaraFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeViewYaraFilters.ContextMenuStrip = this.contextMenuYaraTreeView;
			this.treeViewYaraFilters.FullRowSelect = true;
			this.treeViewYaraFilters.HideSelection = false;
			this.yaraErrorProvider.SetIconAlignment(this.treeViewYaraFilters, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.treeViewYaraFilters.Indent = 12;
			this.treeViewYaraFilters.ItemHeight = 15;
			this.treeViewYaraFilters.Location = new System.Drawing.Point(0, 0);
			this.treeViewYaraFilters.Name = "treeViewYaraFilters";
			this.treeViewYaraFilters.ShowNodeToolTips = true;
			this.treeViewYaraFilters.Size = new System.Drawing.Size(131, 154);
			this.treeViewYaraFilters.TabIndex = 0;
			this.treeViewYaraFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewYaraFilters_KeyDown);
			// 
			// contextMenuYaraTreeView
			// 
			this.contextMenuYaraTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeTreeFilterToolStripMenuItem});
			this.contextMenuYaraTreeView.Name = "contextMenuYaraTreeView";
			this.contextMenuYaraTreeView.Size = new System.Drawing.Size(118, 26);
			this.contextMenuYaraTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuYaraTreeView_Opening);
			// 
			// removeTreeFilterToolStripMenuItem
			// 
			this.removeTreeFilterToolStripMenuItem.Name = "removeTreeFilterToolStripMenuItem";
			this.removeTreeFilterToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.removeTreeFilterToolStripMenuItem.Text = "Remove";
			this.removeTreeFilterToolStripMenuItem.Click += new System.EventHandler(this.removeTreeFilterToolStripMenuItem_Click);
			// 
			// flowLayoutPanelTop
			// 
			this.flowLayoutPanelTop.AutoSize = true;
			this.flowLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanelTop.Controls.Add(this.panelParameters);
			this.flowLayoutPanelTop.Controls.Add(this.panelYaraParameters);
			this.flowLayoutPanelTop.Controls.Add(this.panelSearchButton);
			this.flowLayoutPanelTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanelTop.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanelTop.MinimumSize = new System.Drawing.Size(759, 150);
			this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
			this.flowLayoutPanelTop.Size = new System.Drawing.Size(759, 391);
			this.flowLayoutPanelTop.TabIndex = 9;
			this.flowLayoutPanelTop.Resize += new System.EventHandler(this.MainForm_Resize);
			// 
			// panelSearchButton
			// 
			this.panelSearchButton.AutoSize = true;
			this.panelSearchButton.BackColor = System.Drawing.SystemColors.Control;
			this.panelSearchButton.Controls.Add(this.panelPersistenceOptions);
			this.panelSearchButton.Controls.Add(this.linkGitHub);
			this.panelSearchButton.Controls.Add(this.btnSearch);
			this.panelSearchButton.Location = new System.Drawing.Point(0, 261);
			this.panelSearchButton.Margin = new System.Windows.Forms.Padding(0);
			this.panelSearchButton.MinimumSize = new System.Drawing.Size(757, 27);
			this.panelSearchButton.Name = "panelSearchButton";
			this.panelSearchButton.Size = new System.Drawing.Size(757, 130);
			this.panelSearchButton.TabIndex = 1;
			// 
			// panelPersistenceOptions
			// 
			this.panelPersistenceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelPersistenceOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPersistenceOptions.Controls.Add(this.labelTextBoxDescription);
			this.panelPersistenceOptions.Controls.Add(this.label2);
			this.panelPersistenceOptions.Controls.Add(this.btnPersistenceBrowse);
			this.panelPersistenceOptions.Controls.Add(this.tbPersistenceParameter);
			this.panelPersistenceOptions.Controls.Add(this.radioPersistenceCSV);
			this.panelPersistenceOptions.Controls.Add(this.radioPersistenceSqlite);
			this.panelPersistenceOptions.Controls.Add(this.radioPersistenceSqlServer);
			this.panelPersistenceOptions.Location = new System.Drawing.Point(1, 0);
			this.panelPersistenceOptions.Margin = new System.Windows.Forms.Padding(0);
			this.panelPersistenceOptions.Name = "panelPersistenceOptions";
			this.panelPersistenceOptions.Size = new System.Drawing.Size(756, 96);
			this.panelPersistenceOptions.TabIndex = 0;
			// 
			// labelTextBoxDescription
			// 
			this.labelTextBoxDescription.AutoSize = true;
			this.labelTextBoxDescription.Location = new System.Drawing.Point(94, 11);
			this.labelTextBoxDescription.Name = "labelTextBoxDescription";
			this.labelTextBoxDescription.Size = new System.Drawing.Size(94, 13);
			this.labelTextBoxDescription.TabIndex = 6;
			this.labelTextBoxDescription.Text = "Connection String:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Save results as:";
			// 
			// btnPersistenceBrowse
			// 
			this.btnPersistenceBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPersistenceBrowse.Location = new System.Drawing.Point(658, 25);
			this.btnPersistenceBrowse.Name = "btnPersistenceBrowse";
			this.btnPersistenceBrowse.Size = new System.Drawing.Size(95, 23);
			this.btnPersistenceBrowse.TabIndex = 4;
			this.btnPersistenceBrowse.Text = "Browse...";
			this.btnPersistenceBrowse.UseVisualStyleBackColor = true;
			this.btnPersistenceBrowse.Click += new System.EventHandler(this.btnPersistenceBrowse_Click);
			// 
			// tbPersistenceParameter
			// 
			this.tbPersistenceParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPersistenceParameter.Location = new System.Drawing.Point(111, 27);
			this.tbPersistenceParameter.Multiline = true;
			this.tbPersistenceParameter.Name = "tbPersistenceParameter";
			this.tbPersistenceParameter.Size = new System.Drawing.Size(542, 38);
			this.tbPersistenceParameter.TabIndex = 3;
			this.tbPersistenceParameter.TextChanged += new System.EventHandler(this.tbPersistenceParameter_TextChanged);
			// 
			// radioPersistenceCSV
			// 
			this.radioPersistenceCSV.AutoSize = true;
			this.radioPersistenceCSV.Location = new System.Drawing.Point(15, 26);
			this.radioPersistenceCSV.Name = "radioPersistenceCSV";
			this.radioPersistenceCSV.Size = new System.Drawing.Size(65, 17);
			this.radioPersistenceCSV.TabIndex = 0;
			this.radioPersistenceCSV.Text = "CSV File";
			this.radioPersistenceCSV.UseVisualStyleBackColor = true;
			this.radioPersistenceCSV.CheckedChanged += new System.EventHandler(this.radioPersistenceCSV_CheckedChanged);
			// 
			// radioPersistenceSqlite
			// 
			this.radioPersistenceSqlite.AutoSize = true;
			this.radioPersistenceSqlite.Location = new System.Drawing.Point(15, 48);
			this.radioPersistenceSqlite.Name = "radioPersistenceSqlite";
			this.radioPersistenceSqlite.Size = new System.Drawing.Size(75, 17);
			this.radioPersistenceSqlite.TabIndex = 1;
			this.radioPersistenceSqlite.Text = "SQLite DB";
			this.radioPersistenceSqlite.UseVisualStyleBackColor = true;
			this.radioPersistenceSqlite.CheckedChanged += new System.EventHandler(this.radioPersistenceSqlite_CheckedChanged);
			// 
			// radioPersistenceSqlServer
			// 
			this.radioPersistenceSqlServer.AutoSize = true;
			this.radioPersistenceSqlServer.Location = new System.Drawing.Point(15, 70);
			this.radioPersistenceSqlServer.Name = "radioPersistenceSqlServer";
			this.radioPersistenceSqlServer.Size = new System.Drawing.Size(98, 17);
			this.radioPersistenceSqlServer.TabIndex = 2;
			this.radioPersistenceSqlServer.Text = "SQL Server DB";
			this.radioPersistenceSqlServer.UseVisualStyleBackColor = true;
			this.radioPersistenceSqlServer.CheckedChanged += new System.EventHandler(this.radioPersistenceSqlServer_CheckedChanged);
			// 
			// linkGitHub
			// 
			this.linkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.linkGitHub.AutoSize = true;
			this.linkGitHub.Location = new System.Drawing.Point(450, 113);
			this.linkGitHub.Name = "linkGitHub";
			this.linkGitHub.Size = new System.Drawing.Size(305, 13);
			this.linkGitHub.TabIndex = 1;
			this.linkGitHub.TabStop = true;
			this.linkGitHub.Text = "https://github.com/AdamWhiteHat/Judge-Jury-and-Executable";
			this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
			// 
			// panelBottom
			// 
			this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelBottom.Controls.Add(this.tbOutput);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBottom.Location = new System.Drawing.Point(0, 391);
			this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(760, 125);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(759, 516);
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
			this.panelTop.Size = new System.Drawing.Size(760, 391);
			this.panelTop.TabIndex = 11;
			// 
			// yaraErrorProvider
			// 
			this.yaraErrorProvider.ContainerControl = this;
			// 
			// dialogErrorProvider
			// 
			this.dialogErrorProvider.ContainerControl = this;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(759, 516);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(775, 400);
			this.Name = "MainForm";
			this.Text = "Judge, Jury, and Executable";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.panelParameters.ResumeLayout(false);
			this.panelParameters.PerformLayout();
			this.panelYaraParameters.ResumeLayout(false);
			this.splitContainerYara.Panel1.ResumeLayout(false);
			this.splitContainerYara.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerYara)).EndInit();
			this.splitContainerYara.ResumeLayout(false);
			this.panelYaraCondition.ResumeLayout(false);
			this.panelYaraCondition.PerformLayout();
			this.panelYaraFilter_Buttons.ResumeLayout(false);
			this.contextMenuYaraMatchFiles.ResumeLayout(false);
			this.contextMenuYaraTreeView.ResumeLayout(false);
			this.flowLayoutPanelTop.ResumeLayout(false);
			this.flowLayoutPanelTop.PerformLayout();
			this.panelSearchButton.ResumeLayout(false);
			this.panelSearchButton.PerformLayout();
			this.panelPersistenceOptions.ResumeLayout(false);
			this.panelPersistenceOptions.PerformLayout();
			this.panelBottom.ResumeLayout(false);
			this.panelBottom.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panelTop.ResumeLayout(false);
			this.panelTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.yaraErrorProvider)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dialogErrorProvider)).EndInit();
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
		private System.Windows.Forms.Panel panelYaraParameters;
		private System.Windows.Forms.CheckBox checkBoxYaraRules;
		private System.Windows.Forms.Button btnYaraSave;
		private System.Windows.Forms.Button btnYaraLoad;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Panel panelSearchButton;
		private System.Windows.Forms.LinkLabel linkGitHub;
		private System.Windows.Forms.Panel panelPersistenceOptions;
		private System.Windows.Forms.Label labelTextBoxDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPersistenceBrowse;
		private System.Windows.Forms.TextBox tbPersistenceParameter;
		private System.Windows.Forms.RadioButton radioPersistenceCSV;
		private System.Windows.Forms.RadioButton radioPersistenceSqlite;
		private System.Windows.Forms.RadioButton radioPersistenceSqlServer;
		private System.Windows.Forms.Button btnNewAddYaraCondition;
		private System.Windows.Forms.ErrorProvider yaraErrorProvider;
		private System.Windows.Forms.ContextMenuStrip contextMenuYaraMatchFiles;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_RemoveMatchFile;
		private System.Windows.Forms.TreeView treeViewYaraFilters;
		private System.Windows.Forms.ContextMenuStrip contextMenuYaraTreeView;
		private System.Windows.Forms.ToolStripMenuItem removeTreeFilterToolStripMenuItem;
		private System.Windows.Forms.Panel panelYaraCondition;
		private System.Windows.Forms.ListView listYaraMatchFiles;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnBrowseYaraMatch;
		private System.Windows.Forms.Button btnCancelAddYaraCondition;
		private System.Windows.Forms.Button btnOkAddYaraCondition;
		private System.Windows.Forms.ComboBox comboConditionType;
		private System.Windows.Forms.TextBox tbYaraConditionValue;
		private System.Windows.Forms.SplitContainer splitContainerYara;
		private System.Windows.Forms.ErrorProvider dialogErrorProvider;
		private System.Windows.Forms.Panel panelYaraFilter_Buttons;
	}
}

