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
		private bool[] setOptions = { };
		private string setTargIP = "0.0.0.0";
		private int setDPL = 1;
		private string[] setLoopbacks = { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" };
		private string[] setSources = { };
		private string[] setWhitelist = { };
		private string[] setBlacklist = { };

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
		private void tabber_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabber.SelectedIndex == 1)
			{
				Uri urx = null;
				if (!Regex.Match(txTargetIP.Text.Trim(), @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", System.Text.RegularExpressions.RegexOptions.CultureInvariant).Success)
				{
					txTargetIP.Text = "0.0.0.0";
				}
				if (string.IsNullOrWhiteSpace(txLoopbacks.Text))
				{
					txLoopbacks.Text = string.Join(System.Environment.NewLine, new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" });
				}
				txSources.Lines = txSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();

				//init
				setOptions = new bool[] { chlOptions.GetItemChecked(0), chlOptions.GetItemChecked(1), chlOptions.GetItemChecked(2), chlOptions.GetItemChecked(3) };
				setTargIP = txTargetIP.Text.Trim();
				setDPL = Convert.ToInt32(numDomainPerLine.Value);
				setLoopbacks = txLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				setSources = txSources.Lines;
				setWhitelist = txWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				setBlacklist = txBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
			}
		}
		#endregion

		#region "Worker"
		private void bgGenerate_DoWork(object sender, DoWorkEventArgs e)
		{

		}

		private void bgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}
		#endregion

		private bool IsIPAddress(string input)
		{
			return Regex.Match(input, @"^((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)))$").Success;
		}

		private bool IsLoopback(string input)
		{
			return Regex.Match(input, string.Join("|", setLoopbacks.Select(x => @"\b^" + Regex.Escape(x) + "$")), RegexOptions.IgnoreCase).Success;
		}

	}

	internal class SourceCache
	{
		internal string URL { get; set; }
		internal string Domains { get; set; }
	}
}
