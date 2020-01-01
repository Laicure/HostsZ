namespace HostsZ.Forms
{
	partial class MainZ
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
			this.tabber = new System.Windows.Forms.TabControl();
			this.TabSettings = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txBlacklist = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txWhitelist = new System.Windows.Forms.TextBox();
			this.txSources = new System.Windows.Forms.TextBox();
			this.txLoopbacks = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txTargetIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chlOptions = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numDomainPerLine = new System.Windows.Forms.NumericUpDown();
			this.TabGenerate = new System.Windows.Forms.TabPage();
			this.bgGenerate = new System.ComponentModel.BackgroundWorker();
			this.txLogs = new System.Windows.Forms.TextBox();
			this.tabber.SuspendLayout();
			this.TabSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDomainPerLine)).BeginInit();
			this.TabGenerate.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabber
			// 
			this.tabber.Controls.Add(this.TabSettings);
			this.tabber.Controls.Add(this.TabGenerate);
			this.tabber.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabber.Location = new System.Drawing.Point(1, 1);
			this.tabber.Name = "tabber";
			this.tabber.SelectedIndex = 0;
			this.tabber.Size = new System.Drawing.Size(532, 459);
			this.tabber.TabIndex = 0;
			this.tabber.TabStop = false;
			this.tabber.SelectedIndexChanged += new System.EventHandler(this.tabber_SelectedIndexChanged);
			// 
			// TabSettings
			// 
			this.TabSettings.BackColor = System.Drawing.Color.White;
			this.TabSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TabSettings.Controls.Add(this.label5);
			this.TabSettings.Controls.Add(this.label7);
			this.TabSettings.Controls.Add(this.txBlacklist);
			this.TabSettings.Controls.Add(this.label6);
			this.TabSettings.Controls.Add(this.txWhitelist);
			this.TabSettings.Controls.Add(this.txSources);
			this.TabSettings.Controls.Add(this.txLoopbacks);
			this.TabSettings.Controls.Add(this.label4);
			this.TabSettings.Controls.Add(this.label3);
			this.TabSettings.Controls.Add(this.txTargetIP);
			this.TabSettings.Controls.Add(this.label2);
			this.TabSettings.Controls.Add(this.chlOptions);
			this.TabSettings.Controls.Add(this.label1);
			this.TabSettings.Controls.Add(this.numDomainPerLine);
			this.TabSettings.Location = new System.Drawing.Point(4, 24);
			this.TabSettings.Name = "TabSettings";
			this.TabSettings.Size = new System.Drawing.Size(524, 431);
			this.TabSettings.TabIndex = 0;
			this.TabSettings.Text = "Settings";
			// 
			// label5
			// 
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label5.ForeColor = System.Drawing.Color.Purple;
			this.label5.Location = new System.Drawing.Point(198, -1);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(325, 19);
			this.label5.TabIndex = 8;
			this.label5.Text = "Sources";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label7.ForeColor = System.Drawing.Color.Purple;
			this.label7.Location = new System.Drawing.Point(360, 212);
			this.label7.Margin = new System.Windows.Forms.Padding(0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(163, 19);
			this.label7.TabIndex = 12;
			this.label7.Text = "Blacklist";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txBlacklist
			// 
			this.txBlacklist.BackColor = System.Drawing.Color.White;
			this.txBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txBlacklist.ForeColor = System.Drawing.Color.Black;
			this.txBlacklist.Location = new System.Drawing.Point(360, 230);
			this.txBlacklist.Margin = new System.Windows.Forms.Padding(0);
			this.txBlacklist.Multiline = true;
			this.txBlacklist.Name = "txBlacklist";
			this.txBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txBlacklist.Size = new System.Drawing.Size(163, 200);
			this.txBlacklist.TabIndex = 13;
			this.txBlacklist.TabStop = false;
			this.txBlacklist.WordWrap = false;
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label6.ForeColor = System.Drawing.Color.Purple;
			this.label6.Location = new System.Drawing.Point(198, 212);
			this.label6.Margin = new System.Windows.Forms.Padding(0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(163, 19);
			this.label6.TabIndex = 10;
			this.label6.Text = "Whitelist";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txWhitelist
			// 
			this.txWhitelist.BackColor = System.Drawing.Color.White;
			this.txWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txWhitelist.ForeColor = System.Drawing.Color.Black;
			this.txWhitelist.Location = new System.Drawing.Point(198, 230);
			this.txWhitelist.Margin = new System.Windows.Forms.Padding(0);
			this.txWhitelist.Multiline = true;
			this.txWhitelist.Name = "txWhitelist";
			this.txWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txWhitelist.Size = new System.Drawing.Size(163, 200);
			this.txWhitelist.TabIndex = 11;
			this.txWhitelist.TabStop = false;
			this.txWhitelist.WordWrap = false;
			// 
			// txSources
			// 
			this.txSources.BackColor = System.Drawing.Color.White;
			this.txSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txSources.ForeColor = System.Drawing.Color.Black;
			this.txSources.Location = new System.Drawing.Point(198, 17);
			this.txSources.Margin = new System.Windows.Forms.Padding(0);
			this.txSources.Multiline = true;
			this.txSources.Name = "txSources";
			this.txSources.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txSources.Size = new System.Drawing.Size(325, 196);
			this.txSources.TabIndex = 9;
			this.txSources.TabStop = false;
			this.txSources.WordWrap = false;
			// 
			// txLoopbacks
			// 
			this.txLoopbacks.BackColor = System.Drawing.Color.White;
			this.txLoopbacks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txLoopbacks.ForeColor = System.Drawing.Color.Black;
			this.txLoopbacks.Location = new System.Drawing.Point(-1, 187);
			this.txLoopbacks.Margin = new System.Windows.Forms.Padding(0);
			this.txLoopbacks.Multiline = true;
			this.txLoopbacks.Name = "txLoopbacks";
			this.txLoopbacks.Size = new System.Drawing.Size(200, 243);
			this.txLoopbacks.TabIndex = 7;
			this.txLoopbacks.TabStop = false;
			this.txLoopbacks.Text = "0.0.0.0\r\nbroadcasthost\r\nip6-allhosts\r\nip6-allnodes\r\nip6-allrouters\r\nip6-localhost" +
    "\r\nip6-localnet\r\nip6-loopback\r\nip6-mcastprefix\r\nlocal\r\nlocalhost\r\nlocalhost.local" +
    "domain\r\n";
			this.txLoopbacks.WordWrap = false;
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label4.ForeColor = System.Drawing.Color.Purple;
			this.label4.Location = new System.Drawing.Point(-1, 169);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(200, 19);
			this.label4.TabIndex = 6;
			this.label4.Text = "Loopbacks Whitelist";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label3.ForeColor = System.Drawing.Color.Purple;
			this.label3.Location = new System.Drawing.Point(-1, 130);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 19);
			this.label3.TabIndex = 4;
			this.label3.Text = "Domain per Line";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txTargetIP
			// 
			this.txTargetIP.BackColor = System.Drawing.Color.White;
			this.txTargetIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txTargetIP.ForeColor = System.Drawing.Color.Black;
			this.txTargetIP.Location = new System.Drawing.Point(-1, 108);
			this.txTargetIP.Margin = new System.Windows.Forms.Padding(0);
			this.txTargetIP.Name = "txTargetIP";
			this.txTargetIP.Size = new System.Drawing.Size(200, 23);
			this.txTargetIP.TabIndex = 3;
			this.txTargetIP.TabStop = false;
			this.txTargetIP.Text = "0.0.0.0";
			this.txTargetIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txTargetIP.WordWrap = false;
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label2.ForeColor = System.Drawing.Color.Purple;
			this.label2.Location = new System.Drawing.Point(-1, 90);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 19);
			this.label2.TabIndex = 2;
			this.label2.Text = "Target IP";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chlOptions
			// 
			this.chlOptions.BackColor = System.Drawing.Color.White;
			this.chlOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.chlOptions.CheckOnClick = true;
			this.chlOptions.ForeColor = System.Drawing.Color.Black;
			this.chlOptions.FormattingEnabled = true;
			this.chlOptions.Items.AddRange(new object[] {
            "Use tab instead of space",
            "Sort domains",
            "Show parse errors",
            "Use session cache"});
			this.chlOptions.Location = new System.Drawing.Point(-1, 17);
			this.chlOptions.Margin = new System.Windows.Forms.Padding(0);
			this.chlOptions.Name = "chlOptions";
			this.chlOptions.Size = new System.Drawing.Size(200, 74);
			this.chlOptions.TabIndex = 1;
			this.chlOptions.TabStop = false;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label1.ForeColor = System.Drawing.Color.Purple;
			this.label1.Location = new System.Drawing.Point(-1, -1);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Options";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numDomainPerLine
			// 
			this.numDomainPerLine.BackColor = System.Drawing.Color.White;
			this.numDomainPerLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.numDomainPerLine.ForeColor = System.Drawing.Color.Black;
			this.numDomainPerLine.Location = new System.Drawing.Point(-1, 148);
			this.numDomainPerLine.Margin = new System.Windows.Forms.Padding(0);
			this.numDomainPerLine.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numDomainPerLine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numDomainPerLine.Name = "numDomainPerLine";
			this.numDomainPerLine.Size = new System.Drawing.Size(200, 23);
			this.numDomainPerLine.TabIndex = 5;
			this.numDomainPerLine.TabStop = false;
			this.numDomainPerLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numDomainPerLine.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// TabGenerate
			// 
			this.TabGenerate.BackColor = System.Drawing.Color.White;
			this.TabGenerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TabGenerate.Controls.Add(this.txLogs);
			this.TabGenerate.Location = new System.Drawing.Point(4, 24);
			this.TabGenerate.Name = "TabGenerate";
			this.TabGenerate.Size = new System.Drawing.Size(524, 431);
			this.TabGenerate.TabIndex = 1;
			this.TabGenerate.Text = "Generate";
			// 
			// bgGenerate
			// 
			this.bgGenerate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgGenerate_DoWork);
			this.bgGenerate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgGenerate_RunWorkerCompleted);
			// 
			// txLogs
			// 
			this.txLogs.BackColor = System.Drawing.Color.White;
			this.txLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txLogs.ForeColor = System.Drawing.Color.Black;
			this.txLogs.Location = new System.Drawing.Point(53, 59);
			this.txLogs.Margin = new System.Windows.Forms.Padding(0);
			this.txLogs.MaxLength = 0;
			this.txLogs.Multiline = true;
			this.txLogs.Name = "txLogs";
			this.txLogs.ReadOnly = true;
			this.txLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txLogs.Size = new System.Drawing.Size(325, 196);
			this.txLogs.TabIndex = 10;
			this.txLogs.TabStop = false;
			this.txLogs.WordWrap = false;
			// 
			// MainZ
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(534, 461);
			this.Controls.Add(this.tabber);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(550, 500);
			this.MinimumSize = new System.Drawing.Size(550, 500);
			this.Name = "MainZ";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "HostsZ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainZ_FormClosing);
			this.Load += new System.EventHandler(this.MainZ_Load);
			this.tabber.ResumeLayout(false);
			this.TabSettings.ResumeLayout(false);
			this.TabSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDomainPerLine)).EndInit();
			this.TabGenerate.ResumeLayout(false);
			this.TabGenerate.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabber;
		private System.Windows.Forms.TabPage TabSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage TabGenerate;
		private System.Windows.Forms.TextBox txTargetIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckedListBox chlOptions;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txBlacklist;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txWhitelist;
		private System.Windows.Forms.TextBox txSources;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txLoopbacks;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numDomainPerLine;
		private System.ComponentModel.BackgroundWorker bgGenerate;
		private System.Windows.Forms.TextBox txLogs;
	}
}