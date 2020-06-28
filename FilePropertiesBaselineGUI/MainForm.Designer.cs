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
			this.checkboxOnlineCertValidation = new System.Windows.Forms.CheckBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelYara = new System.Windows.Forms.Panel();
			this.btnYaraLoad = new System.Windows.Forms.Button();
			this.btnYaraSave = new System.Windows.Forms.Button();
			this.btnRemoveYaraFilter = new System.Windows.Forms.Button();
			this.btnAddYaraFilter = new System.Windows.Forms.Button();
			this.panelListBox = new System.Windows.Forms.Panel();
			this.listBoxYaraFilters = new System.Windows.Forms.ListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.radioButtonYara_AlwaysRun = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_IsPeFile = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.tbYaraFilterValue = new System.Windows.Forms.TextBox();
			this.radioButtonYara_Extention = new System.Windows.Forms.RadioButton();
			this.tbYaraRuleNoMatchFiles = new System.Windows.Forms.TextBox();
			this.tbYaraRuleMatchFiles = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnBrowseYaraNoMatch = new System.Windows.Forms.Button();
			this.btnBrowseYaraMatch = new System.Windows.Forms.Button();
			this.radioButtonYara_MimeType = new System.Windows.Forms.RadioButton();
			this.checkBoxYaraRules = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.panelYara.SuspendLayout();
			this.panelListBox.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(691, 3);
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
			this.tbPath.Size = new System.Drawing.Size(682, 20);
			this.tbPath.TabIndex = 1;
			this.tbPath.Text = "C:\\";
			this.tbPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(4, 347);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(785, 104);
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
			this.tbSearchPatterns.Size = new System.Drawing.Size(552, 20);
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
			// checkboxOnlineCertValidation
			// 
			this.checkboxOnlineCertValidation.AutoSize = true;
			this.checkboxOnlineCertValidation.Location = new System.Drawing.Point(3, 83);
			this.checkboxOnlineCertValidation.Name = "checkboxOnlineCertValidation";
			this.checkboxOnlineCertValidation.Size = new System.Drawing.Size(127, 17);
			this.checkboxOnlineCertValidation.TabIndex = 6;
			this.checkboxOnlineCertValidation.Text = "Online Cert Validation";
			this.checkboxOnlineCertValidation.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(5, 314);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(198, 27);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.panelYara);
			this.panel1.Controls.Add(this.checkBoxYaraRules);
			this.panel1.Controls.Add(this.btnBrowse);
			this.panel1.Controls.Add(this.tbPath);
			this.panel1.Controls.Add(this.checkboxOnlineCertValidation);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tbSearchPatterns);
			this.panel1.Controls.Add(this.checkboxCalculateEntropy);
			this.panel1.Location = new System.Drawing.Point(2, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(786, 306);
			this.panel1.TabIndex = 8;
			// 
			// panelYara
			// 
			this.panelYara.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYara.Controls.Add(this.btnYaraLoad);
			this.panelYara.Controls.Add(this.btnYaraSave);
			this.panelYara.Controls.Add(this.btnRemoveYaraFilter);
			this.panelYara.Controls.Add(this.btnAddYaraFilter);
			this.panelYara.Controls.Add(this.panelListBox);
			this.panelYara.Controls.Add(this.panel2);
			this.panelYara.Location = new System.Drawing.Point(3, 127);
			this.panelYara.Name = "panelYara";
			this.panelYara.Size = new System.Drawing.Size(783, 175);
			this.panelYara.TabIndex = 9;
			// 
			// btnYaraLoad
			// 
			this.btnYaraLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYaraLoad.Location = new System.Drawing.Point(645, 144);
			this.btnYaraLoad.Name = "btnYaraLoad";
			this.btnYaraLoad.Size = new System.Drawing.Size(114, 23);
			this.btnYaraLoad.TabIndex = 23;
			this.btnYaraLoad.Text = "Load filter";
			this.btnYaraLoad.UseVisualStyleBackColor = true;
			this.btnYaraLoad.Click += new System.EventHandler(this.btnYaraLoad_Click);
			// 
			// btnYaraSave
			// 
			this.btnYaraSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYaraSave.Location = new System.Drawing.Point(645, 119);
			this.btnYaraSave.Name = "btnYaraSave";
			this.btnYaraSave.Size = new System.Drawing.Size(114, 23);
			this.btnYaraSave.TabIndex = 22;
			this.btnYaraSave.Text = "Export filters";
			this.btnYaraSave.UseVisualStyleBackColor = true;
			this.btnYaraSave.Click += new System.EventHandler(this.btnYaraSave_Click);
			// 
			// btnRemoveYaraFilter
			// 
			this.btnRemoveYaraFilter.Location = new System.Drawing.Point(522, 54);
			this.btnRemoveYaraFilter.Name = "btnRemoveYaraFilter";
			this.btnRemoveYaraFilter.Size = new System.Drawing.Size(101, 23);
			this.btnRemoveYaraFilter.TabIndex = 21;
			this.btnRemoveYaraFilter.Text = "<- Remove";
			this.btnRemoveYaraFilter.UseVisualStyleBackColor = true;
			this.btnRemoveYaraFilter.Click += new System.EventHandler(this.btnRemoveYaraFilter_Click);
			// 
			// btnAddYaraFilter
			// 
			this.btnAddYaraFilter.Location = new System.Drawing.Point(522, 25);
			this.btnAddYaraFilter.Name = "btnAddYaraFilter";
			this.btnAddYaraFilter.Size = new System.Drawing.Size(101, 23);
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
			this.panelListBox.Location = new System.Drawing.Point(626, 3);
			this.panelListBox.Name = "panelListBox";
			this.panelListBox.Size = new System.Drawing.Size(151, 113);
			this.panelListBox.TabIndex = 19;
			// 
			// listBoxYaraFilters
			// 
			this.listBoxYaraFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxYaraFilters.FormattingEnabled = true;
			this.listBoxYaraFilters.Location = new System.Drawing.Point(2, 1);
			this.listBoxYaraFilters.Name = "listBoxYaraFilters";
			this.listBoxYaraFilters.Size = new System.Drawing.Size(145, 108);
			this.listBoxYaraFilters.TabIndex = 0;
			this.listBoxYaraFilters.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxYaraFilters_KeyUp);
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.checkBox1);
			this.panel2.Controls.Add(this.radioButtonYara_AlwaysRun);
			this.panel2.Controls.Add(this.radioButtonYara_IsPeFile);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.tbYaraFilterValue);
			this.panel2.Controls.Add(this.radioButtonYara_Extention);
			this.panel2.Controls.Add(this.tbYaraRuleNoMatchFiles);
			this.panel2.Controls.Add(this.tbYaraRuleMatchFiles);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.btnBrowseYaraNoMatch);
			this.panel2.Controls.Add(this.btnBrowseYaraMatch);
			this.panel2.Controls.Add(this.radioButtonYara_MimeType);
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(513, 139);
			this.panel2.TabIndex = 18;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(4, 101);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(132, 17);
			this.checkBox1.TabIndex = 24;
			this.checkBox1.Text = "IF NO filter match files:";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// radioButtonYara_AlwaysRun
			// 
			this.radioButtonYara_AlwaysRun.AutoSize = true;
			this.radioButtonYara_AlwaysRun.Location = new System.Drawing.Point(4, 7);
			this.radioButtonYara_AlwaysRun.Name = "radioButtonYara_AlwaysRun";
			this.radioButtonYara_AlwaysRun.Size = new System.Drawing.Size(76, 17);
			this.radioButtonYara_AlwaysRun.TabIndex = 13;
			this.radioButtonYara_AlwaysRun.TabStop = true;
			this.radioButtonYara_AlwaysRun.Text = "Always run";
			this.radioButtonYara_AlwaysRun.UseVisualStyleBackColor = true;
			// 
			// radioButtonYara_IsPeFile
			// 
			this.radioButtonYara_IsPeFile.AutoSize = true;
			this.radioButtonYara_IsPeFile.Location = new System.Drawing.Point(4, 27);
			this.radioButtonYara_IsPeFile.Name = "radioButtonYara_IsPeFile";
			this.radioButtonYara_IsPeFile.Size = new System.Drawing.Size(64, 17);
			this.radioButtonYara_IsPeFile.TabIndex = 15;
			this.radioButtonYara_IsPeFile.TabStop = true;
			this.radioButtonYara_IsPeFile.Text = "If PE file";
			this.radioButtonYara_IsPeFile.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(110, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 23;
			this.label2.Text = "IF filter match files:";
			// 
			// tbYaraFilterValue
			// 
			this.tbYaraFilterValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraFilterValue.Location = new System.Drawing.Point(149, 70);
			this.tbYaraFilterValue.Name = "tbYaraFilterValue";
			this.tbYaraFilterValue.Size = new System.Drawing.Size(279, 20);
			this.tbYaraFilterValue.TabIndex = 19;
			// 
			// radioButtonYara_Extention
			// 
			this.radioButtonYara_Extention.AutoSize = true;
			this.radioButtonYara_Extention.Location = new System.Drawing.Point(4, 47);
			this.radioButtonYara_Extention.Name = "radioButtonYara_Extention";
			this.radioButtonYara_Extention.Size = new System.Drawing.Size(87, 17);
			this.radioButtonYara_Extention.TabIndex = 16;
			this.radioButtonYara_Extention.TabStop = true;
			this.radioButtonYara_Extention.Text = "By Extention:";
			this.radioButtonYara_Extention.UseVisualStyleBackColor = true;
			// 
			// tbYaraRuleNoMatchFiles
			// 
			this.tbYaraRuleNoMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleNoMatchFiles.Location = new System.Drawing.Point(149, 98);
			this.tbYaraRuleNoMatchFiles.Name = "tbYaraRuleNoMatchFiles";
			this.tbYaraRuleNoMatchFiles.Size = new System.Drawing.Size(279, 20);
			this.tbYaraRuleNoMatchFiles.TabIndex = 11;
			// 
			// tbYaraRuleMatchFiles
			// 
			this.tbYaraRuleMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleMatchFiles.Location = new System.Drawing.Point(149, 24);
			this.tbYaraRuleMatchFiles.Name = "tbYaraRuleMatchFiles";
			this.tbYaraRuleMatchFiles.Size = new System.Drawing.Size(279, 20);
			this.tbYaraRuleMatchFiles.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(121, 51);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Yara filter value:";
			// 
			// btnBrowseYaraNoMatch
			// 
			this.btnBrowseYaraNoMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraNoMatch.Location = new System.Drawing.Point(434, 96);
			this.btnBrowseYaraNoMatch.Name = "btnBrowseYaraNoMatch";
			this.btnBrowseYaraNoMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraNoMatch.TabIndex = 10;
			this.btnBrowseYaraNoMatch.Text = "Browse...";
			this.btnBrowseYaraNoMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraNoMatch.Click += new System.EventHandler(this.btnBrowseYaraNoMatch_Click);
			// 
			// btnBrowseYaraMatch
			// 
			this.btnBrowseYaraMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraMatch.Location = new System.Drawing.Point(434, 21);
			this.btnBrowseYaraMatch.Name = "btnBrowseYaraMatch";
			this.btnBrowseYaraMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraMatch.TabIndex = 7;
			this.btnBrowseYaraMatch.Text = "Browse...";
			this.btnBrowseYaraMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraMatch.Click += new System.EventHandler(this.btnBrowseYaraMatch_Click);
			// 
			// radioButtonYara_MimeType
			// 
			this.radioButtonYara_MimeType.AutoSize = true;
			this.radioButtonYara_MimeType.Location = new System.Drawing.Point(4, 67);
			this.radioButtonYara_MimeType.Name = "radioButtonYara_MimeType";
			this.radioButtonYara_MimeType.Size = new System.Drawing.Size(98, 17);
			this.radioButtonYara_MimeType.TabIndex = 17;
			this.radioButtonYara_MimeType.TabStop = true;
			this.radioButtonYara_MimeType.Text = "By MIME Type:";
			this.radioButtonYara_MimeType.UseVisualStyleBackColor = true;
			// 
			// checkBoxYaraRules
			// 
			this.checkBoxYaraRules.AutoSize = true;
			this.checkBoxYaraRules.Location = new System.Drawing.Point(3, 104);
			this.checkBoxYaraRules.Name = "checkBoxYaraRules";
			this.checkBoxYaraRules.Size = new System.Drawing.Size(101, 17);
			this.checkBoxYaraRules.TabIndex = 10;
			this.checkBoxYaraRules.Text = "Run Yara Rules";
			this.checkBoxYaraRules.UseVisualStyleBackColor = true;
			this.checkBoxYaraRules.CheckedChanged += new System.EventHandler(this.checkBoxYaraRules_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(791, 454);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.tbOutput);
			this.MinimumSize = new System.Drawing.Size(660, 460);
			this.Name = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panelYara.ResumeLayout(false);
			this.panelListBox.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.CheckBox checkboxCalculateEntropy;
		private System.Windows.Forms.TextBox tbSearchPatterns;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkboxOnlineCertValidation;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnBrowseYaraMatch;
		private System.Windows.Forms.TextBox tbYaraRuleMatchFiles;
		private System.Windows.Forms.Panel panelYara;
		private System.Windows.Forms.CheckBox checkBoxYaraRules;
		private System.Windows.Forms.Button btnBrowseYaraNoMatch;
		private System.Windows.Forms.TextBox tbYaraRuleNoMatchFiles;
		private System.Windows.Forms.Button btnAddYaraFilter;
		private System.Windows.Forms.Panel panelListBox;
		private System.Windows.Forms.ListBox listBoxYaraFilters;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton radioButtonYara_MimeType;
		private System.Windows.Forms.RadioButton radioButtonYara_Extention;
		private System.Windows.Forms.RadioButton radioButtonYara_IsPeFile;
		private System.Windows.Forms.RadioButton radioButtonYara_AlwaysRun;
		private System.Windows.Forms.TextBox tbYaraFilterValue;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnRemoveYaraFilter;
		private System.Windows.Forms.Button btnYaraSave;
		private System.Windows.Forms.Button btnYaraLoad;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label2;
	}
}

