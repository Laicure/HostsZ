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
			this.LbSources = new System.Windows.Forms.Label();
			this.LbBlacklist = new System.Windows.Forms.Label();
			this.TxBlacklist = new System.Windows.Forms.TextBox();
			this.LbWhitelist = new System.Windows.Forms.Label();
			this.TxWhitelist = new System.Windows.Forms.TextBox();
			this.TxSources = new System.Windows.Forms.TextBox();
			this.TxLoopbacks = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.TxTargetIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ChlOptions = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.NumDomainPerLine = new System.Windows.Forms.NumericUpDown();
			this.TabGenerate = new System.Windows.Forms.TabPage();
			this.LbGenerate = new System.Windows.Forms.Label();
			this.TxLogs = new System.Windows.Forms.TextBox();
			this.BgGenerate = new System.ComponentModel.BackgroundWorker();
			this.LbClearCache = new System.Windows.Forms.Label();
			this.Tabber.SuspendLayout();
			this.TabSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumDomainPerLine)).BeginInit();
			this.TabGenerate.SuspendLayout();
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
			this.Tabber.Size = new System.Drawing.Size(532, 459);
			this.Tabber.TabIndex = 0;
			this.Tabber.TabStop = false;
			this.Tabber.SelectedIndexChanged += new System.EventHandler(this.Tabber_SelectedIndexChanged);
			// 
			// TabSettings
			// 
			this.TabSettings.BackColor = System.Drawing.Color.White;
			this.TabSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TabSettings.Controls.Add(this.LbClearCache);
			this.TabSettings.Controls.Add(this.LbSources);
			this.TabSettings.Controls.Add(this.LbBlacklist);
			this.TabSettings.Controls.Add(this.TxBlacklist);
			this.TabSettings.Controls.Add(this.LbWhitelist);
			this.TabSettings.Controls.Add(this.TxWhitelist);
			this.TabSettings.Controls.Add(this.TxSources);
			this.TabSettings.Controls.Add(this.TxLoopbacks);
			this.TabSettings.Controls.Add(this.label4);
			this.TabSettings.Controls.Add(this.label3);
			this.TabSettings.Controls.Add(this.TxTargetIP);
			this.TabSettings.Controls.Add(this.label2);
			this.TabSettings.Controls.Add(this.ChlOptions);
			this.TabSettings.Controls.Add(this.label1);
			this.TabSettings.Controls.Add(this.NumDomainPerLine);
			this.TabSettings.Location = new System.Drawing.Point(4, 24);
			this.TabSettings.Name = "TabSettings";
			this.TabSettings.Size = new System.Drawing.Size(524, 431);
			this.TabSettings.TabIndex = 0;
			this.TabSettings.Text = "Settings";
			// 
			// LbSources
			// 
			this.LbSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LbSources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LbSources.ForeColor = System.Drawing.Color.Purple;
			this.LbSources.Location = new System.Drawing.Point(198, -1);
			this.LbSources.Margin = new System.Windows.Forms.Padding(0);
			this.LbSources.Name = "LbSources";
			this.LbSources.Size = new System.Drawing.Size(325, 19);
			this.LbSources.TabIndex = 8;
			this.LbSources.Text = "Sources";
			this.LbSources.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LbBlacklist
			// 
			this.LbBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LbBlacklist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LbBlacklist.ForeColor = System.Drawing.Color.Purple;
			this.LbBlacklist.Location = new System.Drawing.Point(360, 212);
			this.LbBlacklist.Margin = new System.Windows.Forms.Padding(0);
			this.LbBlacklist.Name = "LbBlacklist";
			this.LbBlacklist.Size = new System.Drawing.Size(163, 19);
			this.LbBlacklist.TabIndex = 12;
			this.LbBlacklist.Text = "Blacklist";
			this.LbBlacklist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TxBlacklist
			// 
			this.TxBlacklist.BackColor = System.Drawing.Color.White;
			this.TxBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxBlacklist.ForeColor = System.Drawing.Color.Black;
			this.TxBlacklist.Location = new System.Drawing.Point(360, 230);
			this.TxBlacklist.Margin = new System.Windows.Forms.Padding(0);
			this.TxBlacklist.MaxLength = 0;
			this.TxBlacklist.Multiline = true;
			this.TxBlacklist.Name = "TxBlacklist";
			this.TxBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxBlacklist.Size = new System.Drawing.Size(163, 200);
			this.TxBlacklist.TabIndex = 13;
			this.TxBlacklist.TabStop = false;
			this.TxBlacklist.WordWrap = false;
			// 
			// LbWhitelist
			// 
			this.LbWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LbWhitelist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LbWhitelist.ForeColor = System.Drawing.Color.Purple;
			this.LbWhitelist.Location = new System.Drawing.Point(198, 212);
			this.LbWhitelist.Margin = new System.Windows.Forms.Padding(0);
			this.LbWhitelist.Name = "LbWhitelist";
			this.LbWhitelist.Size = new System.Drawing.Size(163, 19);
			this.LbWhitelist.TabIndex = 10;
			this.LbWhitelist.Text = "Whitelist";
			this.LbWhitelist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TxWhitelist
			// 
			this.TxWhitelist.BackColor = System.Drawing.Color.White;
			this.TxWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxWhitelist.ForeColor = System.Drawing.Color.Black;
			this.TxWhitelist.Location = new System.Drawing.Point(198, 230);
			this.TxWhitelist.Margin = new System.Windows.Forms.Padding(0);
			this.TxWhitelist.MaxLength = 0;
			this.TxWhitelist.Multiline = true;
			this.TxWhitelist.Name = "TxWhitelist";
			this.TxWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxWhitelist.Size = new System.Drawing.Size(163, 200);
			this.TxWhitelist.TabIndex = 11;
			this.TxWhitelist.TabStop = false;
			this.TxWhitelist.WordWrap = false;
			// 
			// TxSources
			// 
			this.TxSources.BackColor = System.Drawing.Color.White;
			this.TxSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxSources.ForeColor = System.Drawing.Color.Black;
			this.TxSources.Location = new System.Drawing.Point(198, 17);
			this.TxSources.Margin = new System.Windows.Forms.Padding(0);
			this.TxSources.MaxLength = 0;
			this.TxSources.Multiline = true;
			this.TxSources.Name = "TxSources";
			this.TxSources.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxSources.Size = new System.Drawing.Size(325, 196);
			this.TxSources.TabIndex = 9;
			this.TxSources.TabStop = false;
			this.TxSources.WordWrap = false;
			// 
			// TxLoopbacks
			// 
			this.TxLoopbacks.BackColor = System.Drawing.Color.White;
			this.TxLoopbacks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxLoopbacks.ForeColor = System.Drawing.Color.Black;
			this.TxLoopbacks.Location = new System.Drawing.Point(-1, 169);
			this.TxLoopbacks.Margin = new System.Windows.Forms.Padding(0);
			this.TxLoopbacks.Multiline = true;
			this.TxLoopbacks.Name = "TxLoopbacks";
			this.TxLoopbacks.Size = new System.Drawing.Size(200, 261);
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
			this.label4.ForeColor = System.Drawing.Color.Purple;
			this.label4.Location = new System.Drawing.Point(-1, 151);
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
			this.label3.ForeColor = System.Drawing.Color.Purple;
			this.label3.Location = new System.Drawing.Point(-1, 112);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 19);
			this.label3.TabIndex = 4;
			this.label3.Text = "Domain per Line";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TxTargetIP
			// 
			this.TxTargetIP.BackColor = System.Drawing.Color.White;
			this.TxTargetIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxTargetIP.ForeColor = System.Drawing.Color.Black;
			this.TxTargetIP.Location = new System.Drawing.Point(-1, 90);
			this.TxTargetIP.Margin = new System.Windows.Forms.Padding(0);
			this.TxTargetIP.Name = "TxTargetIP";
			this.TxTargetIP.Size = new System.Drawing.Size(200, 23);
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
			this.label2.ForeColor = System.Drawing.Color.Purple;
			this.label2.Location = new System.Drawing.Point(-1, 72);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 19);
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
			this.ChlOptions.Location = new System.Drawing.Point(-1, 17);
			this.ChlOptions.Margin = new System.Windows.Forms.Padding(0);
			this.ChlOptions.Name = "ChlOptions";
			this.ChlOptions.Size = new System.Drawing.Size(200, 38);
			this.ChlOptions.TabIndex = 1;
			this.ChlOptions.TabStop = false;
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
			// NumDomainPerLine
			// 
			this.NumDomainPerLine.BackColor = System.Drawing.Color.White;
			this.NumDomainPerLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.NumDomainPerLine.ForeColor = System.Drawing.Color.Black;
			this.NumDomainPerLine.Location = new System.Drawing.Point(-1, 130);
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
			this.NumDomainPerLine.Size = new System.Drawing.Size(200, 23);
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
			this.TabGenerate.Controls.Add(this.LbGenerate);
			this.TabGenerate.Controls.Add(this.TxLogs);
			this.TabGenerate.Location = new System.Drawing.Point(4, 24);
			this.TabGenerate.Name = "TabGenerate";
			this.TabGenerate.Size = new System.Drawing.Size(524, 431);
			this.TabGenerate.TabIndex = 1;
			this.TabGenerate.Text = "Generate";
			// 
			// LbGenerate
			// 
			this.LbGenerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LbGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LbGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LbGenerate.ForeColor = System.Drawing.Color.Blue;
			this.LbGenerate.Location = new System.Drawing.Point(-1, -1);
			this.LbGenerate.Margin = new System.Windows.Forms.Padding(0);
			this.LbGenerate.Name = "LbGenerate";
			this.LbGenerate.Size = new System.Drawing.Size(524, 19);
			this.LbGenerate.TabIndex = 11;
			this.LbGenerate.Text = "Generate";
			this.LbGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LbGenerate.Click += new System.EventHandler(this.LbGenerate_Click);
			// 
			// TxLogs
			// 
			this.TxLogs.BackColor = System.Drawing.Color.White;
			this.TxLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TxLogs.ForeColor = System.Drawing.Color.Black;
			this.TxLogs.Location = new System.Drawing.Point(-1, 17);
			this.TxLogs.Margin = new System.Windows.Forms.Padding(0);
			this.TxLogs.MaxLength = 0;
			this.TxLogs.Multiline = true;
			this.TxLogs.Name = "TxLogs";
			this.TxLogs.ReadOnly = true;
			this.TxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxLogs.Size = new System.Drawing.Size(524, 413);
			this.TxLogs.TabIndex = 10;
			this.TxLogs.TabStop = false;
			this.TxLogs.WordWrap = false;
			// 
			// BgGenerate
			// 
			this.BgGenerate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgGenerate_DoWork);
			this.BgGenerate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgGenerate_RunWorkerCompleted);
			// 
			// LbClearCache
			// 
			this.LbClearCache.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LbClearCache.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LbClearCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LbClearCache.ForeColor = System.Drawing.Color.Blue;
			this.LbClearCache.Location = new System.Drawing.Point(-1, 54);
			this.LbClearCache.Margin = new System.Windows.Forms.Padding(0);
			this.LbClearCache.Name = "LbClearCache";
			this.LbClearCache.Size = new System.Drawing.Size(200, 19);
			this.LbClearCache.TabIndex = 14;
			this.LbClearCache.Text = "Clear Cached Sources";
			this.LbClearCache.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LbClearCache.Click += new System.EventHandler(this.LbClearCache_Click);
			// 
			// MainZ
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(534, 461);
			this.Controls.Add(this.Tabber);
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
			this.Tabber.ResumeLayout(false);
			this.TabSettings.ResumeLayout(false);
			this.TabSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumDomainPerLine)).EndInit();
			this.TabGenerate.ResumeLayout(false);
			this.TabGenerate.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl Tabber;
		private System.Windows.Forms.TabPage TabSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage TabGenerate;
		private System.Windows.Forms.TextBox TxTargetIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckedListBox ChlOptions;
		private System.Windows.Forms.Label LbBlacklist;
		private System.Windows.Forms.TextBox TxBlacklist;
		private System.Windows.Forms.Label LbWhitelist;
		private System.Windows.Forms.TextBox TxWhitelist;
		private System.Windows.Forms.TextBox TxSources;
		private System.Windows.Forms.Label LbSources;
		private System.Windows.Forms.TextBox TxLoopbacks;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown NumDomainPerLine;
		private System.ComponentModel.BackgroundWorker BgGenerate;
		private System.Windows.Forms.TextBox TxLogs;
		private System.Windows.Forms.Label LbGenerate;
		private System.Windows.Forms.Label LbClearCache;
	}
}