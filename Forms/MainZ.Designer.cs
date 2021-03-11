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
            this.Tabber = new System.Windows.Forms.TabControl();
            this.TabSettings = new System.Windows.Forms.TabPage();
            this.LbClearCache = new System.Windows.Forms.Label();
            this.TxBlacklist = new System.Windows.Forms.TextBox();
            this.TxWhitelist = new System.Windows.Forms.TextBox();
            this.TxSources = new System.Windows.Forms.TextBox();
            this.TxLoopbacks = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxTargetIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ChlOptions = new System.Windows.Forms.CheckedListBox();
            this.NumDomainPerLine = new System.Windows.Forms.NumericUpDown();
            this.TabGenerate = new System.Windows.Forms.TabPage();
            this.TxLogs = new System.Windows.Forms.TextBox();
            this.BgGenerate = new System.ComponentModel.BackgroundWorker();
            this.LbCancel = new System.Windows.Forms.Label();
            this.GbSources = new System.Windows.Forms.GroupBox();
            this.GbWhitelist = new System.Windows.Forms.GroupBox();
            this.GbBlacklist = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Tabber.SuspendLayout();
            this.TabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDomainPerLine)).BeginInit();
            this.TabGenerate.SuspendLayout();
            this.GbSources.SuspendLayout();
            this.GbWhitelist.SuspendLayout();
            this.GbBlacklist.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabber
            // 
            this.Tabber.Controls.Add(this.TabSettings);
            this.Tabber.Controls.Add(this.TabGenerate);
            this.Tabber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabber.Location = new System.Drawing.Point(1, 1);
            this.Tabber.Name = "Tabber";
            this.Tabber.SelectedIndex = 0;
            this.Tabber.Size = new System.Drawing.Size(582, 370);
            this.Tabber.TabIndex = 0;
            this.Tabber.TabStop = false;
            this.Tabber.SelectedIndexChanged += new System.EventHandler(this.Tabber_SelectedIndexChanged);
            // 
            // TabSettings
            // 
            this.TabSettings.BackColor = System.Drawing.Color.White;
            this.TabSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabSettings.Controls.Add(this.groupBox4);
            this.TabSettings.Controls.Add(this.GbBlacklist);
            this.TabSettings.Controls.Add(this.GbWhitelist);
            this.TabSettings.Controls.Add(this.GbSources);
            this.TabSettings.Location = new System.Drawing.Point(4, 24);
            this.TabSettings.Name = "TabSettings";
            this.TabSettings.Size = new System.Drawing.Size(574, 342);
            this.TabSettings.TabIndex = 0;
            this.TabSettings.Text = "Settings";
            // 
            // LbClearCache
            // 
            this.LbClearCache.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LbClearCache.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbClearCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LbClearCache.ForeColor = System.Drawing.Color.Blue;
            this.LbClearCache.Location = new System.Drawing.Point(3, 312);
            this.LbClearCache.Margin = new System.Windows.Forms.Padding(0);
            this.LbClearCache.Name = "LbClearCache";
            this.LbClearCache.Size = new System.Drawing.Size(200, 23);
            this.LbClearCache.TabIndex = 14;
            this.LbClearCache.Text = "Clear Cached Sources";
            this.LbClearCache.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LbClearCache.Click += new System.EventHandler(this.LbClearCache_Click);
            // 
            // TxBlacklist
            // 
            this.TxBlacklist.BackColor = System.Drawing.Color.White;
            this.TxBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxBlacklist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxBlacklist.ForeColor = System.Drawing.Color.Black;
            this.TxBlacklist.Location = new System.Drawing.Point(3, 19);
            this.TxBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.TxBlacklist.MaxLength = 0;
            this.TxBlacklist.Multiline = true;
            this.TxBlacklist.Name = "TxBlacklist";
            this.TxBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxBlacklist.Size = new System.Drawing.Size(169, 139);
            this.TxBlacklist.TabIndex = 13;
            this.TxBlacklist.WordWrap = false;
            // 
            // TxWhitelist
            // 
            this.TxWhitelist.BackColor = System.Drawing.Color.White;
            this.TxWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxWhitelist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxWhitelist.ForeColor = System.Drawing.Color.Black;
            this.TxWhitelist.Location = new System.Drawing.Point(3, 19);
            this.TxWhitelist.Margin = new System.Windows.Forms.Padding(0);
            this.TxWhitelist.MaxLength = 0;
            this.TxWhitelist.Multiline = true;
            this.TxWhitelist.Name = "TxWhitelist";
            this.TxWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxWhitelist.Size = new System.Drawing.Size(179, 139);
            this.TxWhitelist.TabIndex = 11;
            this.TxWhitelist.WordWrap = false;
            // 
            // TxSources
            // 
            this.TxSources.BackColor = System.Drawing.Color.White;
            this.TxSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxSources.ForeColor = System.Drawing.Color.Black;
            this.TxSources.Location = new System.Drawing.Point(3, 19);
            this.TxSources.Margin = new System.Windows.Forms.Padding(0);
            this.TxSources.MaxLength = 0;
            this.TxSources.Multiline = true;
            this.TxSources.Name = "TxSources";
            this.TxSources.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxSources.Size = new System.Drawing.Size(356, 152);
            this.TxSources.TabIndex = 9;
            this.TxSources.WordWrap = false;
            // 
            // TxLoopbacks
            // 
            this.TxLoopbacks.BackColor = System.Drawing.Color.White;
            this.TxLoopbacks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxLoopbacks.ForeColor = System.Drawing.Color.Black;
            this.TxLoopbacks.Location = new System.Drawing.Point(3, 124);
            this.TxLoopbacks.Margin = new System.Windows.Forms.Padding(0);
            this.TxLoopbacks.Multiline = true;
            this.TxLoopbacks.Name = "TxLoopbacks";
            this.TxLoopbacks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxLoopbacks.Size = new System.Drawing.Size(200, 187);
            this.TxLoopbacks.TabIndex = 7;
            this.TxLoopbacks.TabStop = false;
            this.TxLoopbacks.Text = "0.0.0.0\r\nbroadcasthost\r\nip6-allhosts\r\nip6-allnodes\r\nip6-allrouters\r\nip6-localhost" +
    "\r\nip6-localnet\r\nip6-loopback\r\nip6-mcastprefix\r\nlocal\r\nlocalhost\r\nlocalhost.local" +
    "domain\r\n";
            this.TxLoopbacks.WordWrap = false;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Location = new System.Drawing.Point(3, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Loopback Whitelist";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(3, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Domain per Line";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxTargetIP
            // 
            this.TxTargetIP.BackColor = System.Drawing.Color.White;
            this.TxTargetIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxTargetIP.ForeColor = System.Drawing.Color.Black;
            this.TxTargetIP.Location = new System.Drawing.Point(77, 58);
            this.TxTargetIP.Margin = new System.Windows.Forms.Padding(0);
            this.TxTargetIP.Name = "TxTargetIP";
            this.TxTargetIP.Size = new System.Drawing.Size(126, 23);
            this.TxTargetIP.TabIndex = 3;
            this.TxTargetIP.TabStop = false;
            this.TxTargetIP.Text = "0.0.0.0";
            this.TxTargetIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxTargetIP.WordWrap = false;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target IP";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChlOptions
            // 
            this.ChlOptions.BackColor = System.Drawing.Color.White;
            this.ChlOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChlOptions.CheckOnClick = true;
            this.ChlOptions.ForeColor = System.Drawing.Color.Black;
            this.ChlOptions.FormattingEnabled = true;
            this.ChlOptions.Items.AddRange(new object[] {
            "Use tab instead of space",
            "Show parse errors"});
            this.ChlOptions.Location = new System.Drawing.Point(3, 19);
            this.ChlOptions.Margin = new System.Windows.Forms.Padding(0);
            this.ChlOptions.Name = "ChlOptions";
            this.ChlOptions.Size = new System.Drawing.Size(200, 38);
            this.ChlOptions.TabIndex = 1;
            this.ChlOptions.TabStop = false;
            // 
            // NumDomainPerLine
            // 
            this.NumDomainPerLine.BackColor = System.Drawing.Color.White;
            this.NumDomainPerLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumDomainPerLine.ForeColor = System.Drawing.Color.Black;
            this.NumDomainPerLine.Location = new System.Drawing.Point(122, 82);
            this.NumDomainPerLine.Margin = new System.Windows.Forms.Padding(0);
            this.NumDomainPerLine.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.NumDomainPerLine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumDomainPerLine.Name = "NumDomainPerLine";
            this.NumDomainPerLine.Size = new System.Drawing.Size(81, 23);
            this.NumDomainPerLine.TabIndex = 5;
            this.NumDomainPerLine.TabStop = false;
            this.NumDomainPerLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumDomainPerLine.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TabGenerate
            // 
            this.TabGenerate.BackColor = System.Drawing.Color.White;
            this.TabGenerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabGenerate.Controls.Add(this.TxLogs);
            this.TabGenerate.Location = new System.Drawing.Point(4, 24);
            this.TabGenerate.Name = "TabGenerate";
            this.TabGenerate.Padding = new System.Windows.Forms.Padding(1);
            this.TabGenerate.Size = new System.Drawing.Size(574, 342);
            this.TabGenerate.TabIndex = 1;
            this.TabGenerate.Text = "Generate";
            // 
            // TxLogs
            // 
            this.TxLogs.BackColor = System.Drawing.Color.White;
            this.TxLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxLogs.ForeColor = System.Drawing.Color.Black;
            this.TxLogs.Location = new System.Drawing.Point(1, 1);
            this.TxLogs.Margin = new System.Windows.Forms.Padding(0);
            this.TxLogs.MaxLength = 0;
            this.TxLogs.Multiline = true;
            this.TxLogs.Name = "TxLogs";
            this.TxLogs.ReadOnly = true;
            this.TxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxLogs.Size = new System.Drawing.Size(570, 338);
            this.TxLogs.TabIndex = 10;
            this.TxLogs.TabStop = false;
            this.TxLogs.WordWrap = false;
            // 
            // BgGenerate
            // 
            this.BgGenerate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgGenerate_DoWork);
            this.BgGenerate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgGenerate_RunWorkerCompleted);
            // 
            // LbCancel
            // 
            this.LbCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LbCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LbCancel.ForeColor = System.Drawing.Color.Blue;
            this.LbCancel.Location = new System.Drawing.Point(523, 1);
            this.LbCancel.Margin = new System.Windows.Forms.Padding(0);
            this.LbCancel.Name = "LbCancel";
            this.LbCancel.Size = new System.Drawing.Size(60, 19);
            this.LbCancel.TabIndex = 15;
            this.LbCancel.Text = "Cancel";
            this.LbCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LbCancel.Visible = false;
            this.LbCancel.Click += new System.EventHandler(this.LbCancel_Click);
            // 
            // GbSources
            // 
            this.GbSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbSources.Controls.Add(this.TxSources);
            this.GbSources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GbSources.Location = new System.Drawing.Point(209, 1);
            this.GbSources.Margin = new System.Windows.Forms.Padding(1);
            this.GbSources.Name = "GbSources";
            this.GbSources.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.GbSources.Size = new System.Drawing.Size(362, 175);
            this.GbSources.TabIndex = 15;
            this.GbSources.TabStop = false;
            this.GbSources.Text = "Sources";
            // 
            // GbWhitelist
            // 
            this.GbWhitelist.Controls.Add(this.TxWhitelist);
            this.GbWhitelist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GbWhitelist.Location = new System.Drawing.Point(209, 178);
            this.GbWhitelist.Margin = new System.Windows.Forms.Padding(1);
            this.GbWhitelist.Name = "GbWhitelist";
            this.GbWhitelist.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.GbWhitelist.Size = new System.Drawing.Size(185, 162);
            this.GbWhitelist.TabIndex = 16;
            this.GbWhitelist.TabStop = false;
            this.GbWhitelist.Text = "Whitelist";
            // 
            // GbBlacklist
            // 
            this.GbBlacklist.Controls.Add(this.TxBlacklist);
            this.GbBlacklist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GbBlacklist.Location = new System.Drawing.Point(396, 178);
            this.GbBlacklist.Margin = new System.Windows.Forms.Padding(1);
            this.GbBlacklist.Name = "GbBlacklist";
            this.GbBlacklist.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.GbBlacklist.Size = new System.Drawing.Size(175, 162);
            this.GbBlacklist.TabIndex = 17;
            this.GbBlacklist.TabStop = false;
            this.GbBlacklist.Text = "Blacklist";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.ChlOptions);
            this.groupBox4.Controls.Add(this.TxTargetIP);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.LbClearCache);
            this.groupBox4.Controls.Add(this.NumDomainPerLine);
            this.groupBox4.Controls.Add(this.TxLoopbacks);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox4.Location = new System.Drawing.Point(1, 1);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(206, 339);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options";
            // 
            // MainZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 372);
            this.Controls.Add(this.LbCancel);
            this.Controls.Add(this.Tabber);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainZ";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HostsZ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainZ_FormClosing);
            this.Load += new System.EventHandler(this.MainZ_Load);
            this.Tabber.ResumeLayout(false);
            this.TabSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumDomainPerLine)).EndInit();
            this.TabGenerate.ResumeLayout(false);
            this.TabGenerate.PerformLayout();
            this.GbSources.ResumeLayout(false);
            this.GbSources.PerformLayout();
            this.GbWhitelist.ResumeLayout(false);
            this.GbWhitelist.PerformLayout();
            this.GbBlacklist.ResumeLayout(false);
            this.GbBlacklist.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl Tabber;
		private System.Windows.Forms.TabPage TabSettings;
		private System.Windows.Forms.TabPage TabGenerate;
		private System.Windows.Forms.TextBox TxTargetIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckedListBox ChlOptions;
		private System.Windows.Forms.TextBox TxBlacklist;
		private System.Windows.Forms.TextBox TxWhitelist;
		private System.Windows.Forms.TextBox TxSources;
		private System.Windows.Forms.TextBox TxLoopbacks;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown NumDomainPerLine;
		private System.ComponentModel.BackgroundWorker BgGenerate;
		private System.Windows.Forms.TextBox TxLogs;
		private System.Windows.Forms.Label LbClearCache;
		private System.Windows.Forms.Label LbCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox GbBlacklist;
        private System.Windows.Forms.GroupBox GbWhitelist;
        private System.Windows.Forms.GroupBox GbSources;
    }
}