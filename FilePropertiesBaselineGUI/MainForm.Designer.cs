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
			this.btnBrowseYaraMatch = new System.Windows.Forms.Button();
			this.tbYaraRuleMatchFiles = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panelYara = new System.Windows.Forms.Panel();
			this.btnRemoveYaraFilter = new System.Windows.Forms.Button();
			this.btnAddYaraFilter = new System.Windows.Forms.Button();
			this.panelListBox = new System.Windows.Forms.Panel();
			this.listBoxYaraFilters = new System.Windows.Forms.ListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.tbYaraFilterValue = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.radioButtonYara_MimeType = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_Extention = new System.Windows.Forms.RadioButton();
			this.radioButtonYara_IsPeFile = new System.Windows.Forms.RadioButton();
			this.tbYaraRuleNoMatchFiles = new System.Windows.Forms.TextBox();
			this.radioButtonYara_AlwaysRun = new System.Windows.Forms.RadioButton();
			this.btnBrowseYaraNoMatch = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
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
			this.btnBrowse.Location = new System.Drawing.Point(544, 3);
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
			this.tbPath.Size = new System.Drawing.Size(535, 20);
			this.tbPath.TabIndex = 1;
			this.tbPath.Text = "C:\\";
			this.tbPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(4, 343);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(638, 76);
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
			this.tbSearchPatterns.Size = new System.Drawing.Size(405, 20);
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
			this.btnSearch.Location = new System.Drawing.Point(2, 310);
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
			this.panel1.Size = new System.Drawing.Size(639, 302);
			this.panel1.TabIndex = 8;
			// 
			// btnBrowseYaraMatch
			// 
			this.btnBrowseYaraMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraMatch.Location = new System.Drawing.Point(312, 109);
			this.btnBrowseYaraMatch.Name = "btnBrowseYaraMatch";
			this.btnBrowseYaraMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraMatch.TabIndex = 7;
			this.btnBrowseYaraMatch.Text = "Browse...";
			this.btnBrowseYaraMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraMatch.Click += new System.EventHandler(this.btnBrowseYaraMatch_Click);
			// 
			// tbYaraRuleMatchFiles
			// 
			this.tbYaraRuleMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleMatchFiles.Location = new System.Drawing.Point(128, 111);
			this.tbYaraRuleMatchFiles.Name = "tbYaraRuleMatchFiles";
			this.tbYaraRuleMatchFiles.Size = new System.Drawing.Size(178, 20);
			this.tbYaraRuleMatchFiles.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 114);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "IF rule match files:";
			// 
			// panelYara
			// 
			this.panelYara.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelYara.Controls.Add(this.btnRemoveYaraFilter);
			this.panelYara.Controls.Add(this.btnAddYaraFilter);
			this.panelYara.Controls.Add(this.panelListBox);
			this.panelYara.Controls.Add(this.panel2);
			this.panelYara.Location = new System.Drawing.Point(3, 127);
			this.panelYara.Name = "panelYara";
			this.panelYara.Size = new System.Drawing.Size(636, 172);
			this.panelYara.TabIndex = 9;
			// 
			// btnRemoveYaraFilter
			// 
			this.btnRemoveYaraFilter.Location = new System.Drawing.Point(404, 94);
			this.btnRemoveYaraFilter.Name = "btnRemoveYaraFilter";
			this.btnRemoveYaraFilter.Size = new System.Drawing.Size(101, 23);
			this.btnRemoveYaraFilter.TabIndex = 21;
			this.btnRemoveYaraFilter.Text = "<- Remove";
			this.btnRemoveYaraFilter.UseVisualStyleBackColor = true;
			this.btnRemoveYaraFilter.Click += new System.EventHandler(this.btnRemoveYaraFilter_Click);
			// 
			// btnAddYaraFilter
			// 
			this.btnAddYaraFilter.Location = new System.Drawing.Point(404, 65);
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
			this.panelListBox.Location = new System.Drawing.Point(511, 3);
			this.panelListBox.Name = "panelListBox";
			this.panelListBox.Size = new System.Drawing.Size(119, 165);
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
			this.listBoxYaraFilters.Size = new System.Drawing.Size(113, 160);
			this.listBoxYaraFilters.TabIndex = 0;
			this.listBoxYaraFilters.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxYaraFilters_KeyUp);
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.tbYaraFilterValue);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.radioButtonYara_MimeType);
			this.panel2.Controls.Add(this.tbYaraRuleMatchFiles);
			this.panel2.Controls.Add(this.radioButtonYara_Extention);
			this.panel2.Controls.Add(this.btnBrowseYaraMatch);
			this.panel2.Controls.Add(this.radioButtonYara_IsPeFile);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.tbYaraRuleNoMatchFiles);
			this.panel2.Controls.Add(this.radioButtonYara_AlwaysRun);
			this.panel2.Controls.Add(this.btnBrowseYaraNoMatch);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Location = new System.Drawing.Point(7, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(391, 165);
			this.panel2.TabIndex = 18;
			// 
			// tbYaraFilterValue
			// 
			this.tbYaraFilterValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraFilterValue.Location = new System.Drawing.Point(186, 30);
			this.tbYaraFilterValue.Name = "tbYaraFilterValue";
			this.tbYaraFilterValue.Size = new System.Drawing.Size(191, 20);
			this.tbYaraFilterValue.TabIndex = 19;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(158, 14);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Yara filter value:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 11);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 13);
			this.label4.TabIndex = 14;
			this.label4.Text = "Yara filter:";
			// 
			// radioButtonYara_MimeType
			// 
			this.radioButtonYara_MimeType.AutoSize = true;
			this.radioButtonYara_MimeType.Location = new System.Drawing.Point(38, 87);
			this.radioButtonYara_MimeType.Name = "radioButtonYara_MimeType";
			this.radioButtonYara_MimeType.Size = new System.Drawing.Size(95, 17);
			this.radioButtonYara_MimeType.TabIndex = 17;
			this.radioButtonYara_MimeType.TabStop = true;
			this.radioButtonYara_MimeType.Text = "By MIME Type";
			this.radioButtonYara_MimeType.UseVisualStyleBackColor = true;
			// 
			// radioButtonYara_Extention
			// 
			this.radioButtonYara_Extention.AutoSize = true;
			this.radioButtonYara_Extention.Location = new System.Drawing.Point(38, 67);
			this.radioButtonYara_Extention.Name = "radioButtonYara_Extention";
			this.radioButtonYara_Extention.Size = new System.Drawing.Size(84, 17);
			this.radioButtonYara_Extention.TabIndex = 16;
			this.radioButtonYara_Extention.TabStop = true;
			this.radioButtonYara_Extention.Text = "By Extention";
			this.radioButtonYara_Extention.UseVisualStyleBackColor = true;
			// 
			// radioButtonYara_IsPeFile
			// 
			this.radioButtonYara_IsPeFile.AutoSize = true;
			this.radioButtonYara_IsPeFile.Location = new System.Drawing.Point(38, 47);
			this.radioButtonYara_IsPeFile.Name = "radioButtonYara_IsPeFile";
			this.radioButtonYara_IsPeFile.Size = new System.Drawing.Size(64, 17);
			this.radioButtonYara_IsPeFile.TabIndex = 15;
			this.radioButtonYara_IsPeFile.TabStop = true;
			this.radioButtonYara_IsPeFile.Text = "If PE file";
			this.radioButtonYara_IsPeFile.UseVisualStyleBackColor = true;
			// 
			// tbYaraRuleNoMatchFiles
			// 
			this.tbYaraRuleNoMatchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleNoMatchFiles.Location = new System.Drawing.Point(128, 134);
			this.tbYaraRuleNoMatchFiles.Name = "tbYaraRuleNoMatchFiles";
			this.tbYaraRuleNoMatchFiles.Size = new System.Drawing.Size(178, 20);
			this.tbYaraRuleNoMatchFiles.TabIndex = 11;
			// 
			// radioButtonYara_AlwaysRun
			// 
			this.radioButtonYara_AlwaysRun.AutoSize = true;
			this.radioButtonYara_AlwaysRun.Location = new System.Drawing.Point(38, 27);
			this.radioButtonYara_AlwaysRun.Name = "radioButtonYara_AlwaysRun";
			this.radioButtonYara_AlwaysRun.Size = new System.Drawing.Size(76, 17);
			this.radioButtonYara_AlwaysRun.TabIndex = 13;
			this.radioButtonYara_AlwaysRun.TabStop = true;
			this.radioButtonYara_AlwaysRun.Text = "Always run";
			this.radioButtonYara_AlwaysRun.UseVisualStyleBackColor = true;
			// 
			// btnBrowseYaraNoMatch
			// 
			this.btnBrowseYaraNoMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYaraNoMatch.Location = new System.Drawing.Point(312, 132);
			this.btnBrowseYaraNoMatch.Name = "btnBrowseYaraNoMatch";
			this.btnBrowseYaraNoMatch.Size = new System.Drawing.Size(74, 23);
			this.btnBrowseYaraNoMatch.TabIndex = 10;
			this.btnBrowseYaraNoMatch.Text = "Browse...";
			this.btnBrowseYaraNoMatch.UseVisualStyleBackColor = true;
			this.btnBrowseYaraNoMatch.Click += new System.EventHandler(this.btnBrowseYaraNoMatch_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 137);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "IF NO rules match files:";
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
			this.ClientSize = new System.Drawing.Size(644, 422);
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
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelYara;
		private System.Windows.Forms.CheckBox checkBoxYaraRules;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnBrowseYaraNoMatch;
		private System.Windows.Forms.TextBox tbYaraRuleNoMatchFiles;
		private System.Windows.Forms.Button btnAddYaraFilter;
		private System.Windows.Forms.Panel panelListBox;
		private System.Windows.Forms.ListBox listBoxYaraFilters;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton radioButtonYara_MimeType;
		private System.Windows.Forms.RadioButton radioButtonYara_Extention;
		private System.Windows.Forms.RadioButton radioButtonYara_IsPeFile;
		private System.Windows.Forms.RadioButton radioButtonYara_AlwaysRun;
		private System.Windows.Forms.TextBox tbYaraFilterValue;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnRemoveYaraFilter;
	}
}

