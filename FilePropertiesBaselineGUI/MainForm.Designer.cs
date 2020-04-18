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
			this.btnBrowseYara = new System.Windows.Forms.Button();
			this.tbYaraRuleFile = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panelYara = new System.Windows.Forms.Panel();
			this.checkBoxYaraRules = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.panelYara.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(494, 3);
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
			this.tbPath.Size = new System.Drawing.Size(485, 20);
			this.tbPath.TabIndex = 1;
			this.tbPath.Text = "C:\\";
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(4, 230);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(588, 162);
			this.tbOutput.TabIndex = 2;
			this.tbOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbOutput_KeyUp);
			// 
			// checkboxCalculateEntropy
			// 
			this.checkboxCalculateEntropy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkboxCalculateEntropy.AutoSize = true;
			this.checkboxCalculateEntropy.Location = new System.Drawing.Point(459, 29);
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
			this.tbSearchPatterns.Size = new System.Drawing.Size(355, 20);
			this.tbSearchPatterns.TabIndex = 4;
			this.tbSearchPatterns.Text = "*.exe|*.dll|*.drv";
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
			this.checkboxOnlineCertValidation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkboxOnlineCertValidation.AutoSize = true;
			this.checkboxOnlineCertValidation.Location = new System.Drawing.Point(459, 47);
			this.checkboxOnlineCertValidation.Name = "checkboxOnlineCertValidation";
			this.checkboxOnlineCertValidation.Size = new System.Drawing.Size(127, 17);
			this.checkboxOnlineCertValidation.TabIndex = 6;
			this.checkboxOnlineCertValidation.Text = "Online Cert Validation";
			this.checkboxOnlineCertValidation.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSearch.Location = new System.Drawing.Point(230, 197);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(168, 27);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnBrowse);
			this.panel1.Controls.Add(this.tbPath);
			this.panel1.Controls.Add(this.checkboxOnlineCertValidation);
			this.panel1.Controls.Add(this.checkboxCalculateEntropy);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tbSearchPatterns);
			this.panel1.Location = new System.Drawing.Point(2, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(589, 111);
			this.panel1.TabIndex = 8;
			// 
			// btnBrowseYara
			// 
			this.btnBrowseYara.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseYara.Location = new System.Drawing.Point(491, 10);
			this.btnBrowseYara.Name = "btnBrowseYara";
			this.btnBrowseYara.Size = new System.Drawing.Size(95, 23);
			this.btnBrowseYara.TabIndex = 7;
			this.btnBrowseYara.Text = "Browse...";
			this.btnBrowseYara.UseVisualStyleBackColor = true;
			this.btnBrowseYara.Click += new System.EventHandler(this.btnBrowseYara_Click);
			// 
			// tbYaraRuleFile
			// 
			this.tbYaraRuleFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbYaraRuleFile.Location = new System.Drawing.Point(61, 12);
			this.tbYaraRuleFile.Name = "tbYaraRuleFile";
			this.tbYaraRuleFile.Size = new System.Drawing.Size(424, 20);
			this.tbYaraRuleFile.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Rule file:";
			// 
			// panelYara
			// 
			this.panelYara.Controls.Add(this.label2);
			this.panelYara.Controls.Add(this.btnBrowseYara);
			this.panelYara.Controls.Add(this.tbYaraRuleFile);
			this.panelYara.Location = new System.Drawing.Point(5, 145);
			this.panelYara.Name = "panelYara";
			this.panelYara.Size = new System.Drawing.Size(589, 46);
			this.panelYara.TabIndex = 9;
			// 
			// checkBoxYaraRules
			// 
			this.checkBoxYaraRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxYaraRules.AutoSize = true;
			this.checkBoxYaraRules.Location = new System.Drawing.Point(2, 124);
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
			this.ClientSize = new System.Drawing.Size(594, 395);
			this.Controls.Add(this.checkBoxYaraRules);
			this.Controls.Add(this.panelYara);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.tbOutput);
			this.Name = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panelYara.ResumeLayout(false);
			this.panelYara.PerformLayout();
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
		private System.Windows.Forms.Button btnBrowseYara;
		private System.Windows.Forms.TextBox tbYaraRuleFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelYara;
		private System.Windows.Forms.CheckBox checkBoxYaraRules;
	}
}

