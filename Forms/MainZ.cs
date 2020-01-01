using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HostsZ.Forms
{
	public partial class MainZ : Form
	{
		private bool[] SetOptions = { };
		private string SetTargIP = "0.0.0.0";
		private int SetDPL = 1;
		private string[] SetLoopbacks = { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" };
		private string[] SetSources = { };
		private string[] SetWhitelist = { };
		private string[] SetBlacklist = { };

		public MainZ()
		{
			InitializeComponent();
		}
		#region "Form"
		private void MainZ_Load(object sender, EventArgs e)
		{

		}

		private void MainZ_FormClosing(object sender, FormClosingEventArgs e)
		{

		}
		#endregion

		#region "Controls"
		private void Tabber_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Tabber.SelectedIndex == 1)
			{
				Uri urx = null;
				if (!Regex.Match(TxTargetIP.Text.Trim(), @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", System.Text.RegularExpressions.RegexOptions.CultureInvariant).Success)
				{
					TxTargetIP.Text = "0.0.0.0";
				}
				if (string.IsNullOrWhiteSpace(TxLoopbacks.Text))
				{
					TxLoopbacks.Text = string.Join(System.Environment.NewLine, new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" });
				}
				TxSources.Lines = TxSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();

				//init
				SetOptions = new bool[] { ChlOptions.GetItemChecked(0), ChlOptions.GetItemChecked(1), ChlOptions.GetItemChecked(2), ChlOptions.GetItemChecked(3) };
				SetTargIP = TxTargetIP.Text.Trim();
				SetDPL = Convert.ToInt32(NumDomainPerLine.Value);
				SetLoopbacks = TxLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				SetSources = TxSources.Lines;
				SetWhitelist = TxWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				SetBlacklist = TxBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
			}
		}
		#endregion

		#region "Worker"
		private void BgGenerate_DoWork(object sender, DoWorkEventArgs e)
		{

		}

		private void BgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}
		#endregion

		private bool IsIPAddress(string input)
		{
			return Regex.Match(input, @"^((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)))$").Success;
		}

		private bool IsLoopback(string input)
		{
			return Regex.Match(input, string.Join("|", SetLoopbacks.Select(x => @"\b^" + Regex.Escape(x) + "$")), RegexOptions.IgnoreCase).Success;
		}
	}

	internal class SourceCache
	{
		internal string URL { get; set; }
		internal string Domains { get; set; }
	}
}
