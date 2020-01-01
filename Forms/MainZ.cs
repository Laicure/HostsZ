using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HostsZ.Forms
{
	public partial class MainZ : Form
	{
		//inputs
		private bool[] setOptions = { }; //0=tab, 1=sort, 2=parse, 3=cache
		private string setTargIP = "0.0.0.0";
		private int setDPL = 1;
		private string[] setLoopbacks = { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" };
		private string[] setSources = { };
		private string[] setWhitelist = { };
		private string[] setBlacklist = { };

		//generation
		private string generatedHosts = "";
		private DateTime startExec = DateTime.Now;
		private int errCount = 0;
		internal List<SourceCache> sourceCacheList = new List<SourceCache>();

		//misc
		private readonly string vbCrLf = Environment.NewLine;
		private readonly System.Globalization.CultureInfo invarCulture = System.Globalization.CultureInfo.InvariantCulture;

		public MainZ()
		{
			InitializeComponent();
		}

		#region "Form"

		private void MainZ_Load(object sender, EventArgs e)
		{
			this.Icon = HostsZ.Properties.Resources.art;

			//init
			ChlOptions.SetItemChecked(1, true);
			ChlOptions.SetItemChecked(2, true);
			ChlOptions.SetItemChecked(3, true);
		}

		private void MainZ_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (BgGenerate.IsBusy)
			{
				if (MessageBox.Show("Are you sure to close the app?", "Are you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					e.Cancel = true;
			}
		}

		#endregion "Form"

		#region "Controls"

		private void Tabber_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (BgGenerate.IsBusy && Tabber.SelectedIndex == 0)
			{
				Tabber.SelectedIndex = 1;
				MessageBox.Show("HostsZ is currently busy generating your request." + vbCrLf + "You may cancel the process instead.", "Busy!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (Tabber.SelectedIndex == 1)
			{
				if (!Regex.Match(TxTargetIP.Text.Trim(), @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", System.Text.RegularExpressions.RegexOptions.CultureInvariant).Success)
				{
					TxTargetIP.Text = "0.0.0.0";
				}
				if (string.IsNullOrWhiteSpace(TxLoopbacks.Text))
				{
					TxLoopbacks.Text = string.Join(vbCrLf, new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" });
				}
				Uri urx = null;
				TxSources.Lines = TxSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();

				//init
				setOptions = new bool[] { ChlOptions.GetItemChecked(0), ChlOptions.GetItemChecked(1), ChlOptions.GetItemChecked(2), ChlOptions.GetItemChecked(3) };
				setTargIP = TxTargetIP.Text.Trim();
				setDPL = Convert.ToInt32(NumDomainPerLine.Value);
				setLoopbacks = TxLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				setSources = TxSources.Lines;
				setWhitelist = TxWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				setBlacklist = TxBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
			}

			if (setSources.Count() == 0 && Tabber.SelectedIndex == 1)
			{
				MessageBox.Show("No valid sources parsed!", "Nope, sorry. Nothing.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Tabber.SelectedIndex = 0;
				TxSources.Focus();
			}
		}

		private void LbGenerate_Click(object sender, EventArgs e)
		{
			generatedHosts = "";
			if (!BgGenerate.IsBusy)
			{
				LbGenerate.Text = "Generating...";
				TxLogs.Text = LogDate() + "[Start]";
				BgGenerate.RunWorkerAsync();
			}
		}

		#endregion "Controls"

		#region "Worker"

		private void BgGenerate_DoWork(object sender, DoWorkEventArgs e)
		{
			startExec = DateTime.UtcNow;
			errCount = 0;

			//download sources
			for (int i = 0; i <= setSources.Count() - 1; i++)
			{
				string sourceUrl = setSources[i];

				//use cache?
				if (setOptions[3] && sourceCacheList.Any(x => x.URL == sourceUrl))
				{
					TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Cached] " + sourceUrl + vbCrLf + TxLogs.Text));
				}
				else
				{
					string downloadedData = "";
					//download
					TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Fetch] " + sourceUrl + vbCrLf + TxLogs.Text));
					try
					{
						using (var clie = new System.Net.WebClient())
						{
							clie.UseDefaultCredentials = true;
							downloadedData = clie.DownloadString(sourceUrl);
						}
					}
					catch (Exception ex)
					{
						TxLogs.Invoke(new Action(() => TxLogs.Text = "> " + ex.Source + ": " + ex.Message.Replace(vbCrLf, " ") + vbCrLf + TxLogs.Text));
						errCount += 1;
					}

					//parse
					HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { vbCrLf, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", "").Trim()).Select(x => Regex.Replace(x, @"^.+ ", "").Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
					string[] tempDomains = downloadedHash.ToArray();
					downloadedHash.Clear();
					downloadedHash.TrimExcess();
					for (int y = 0; y <= tempDomains.Count() - 1; y++)
					{
						Uri urxed = null;
						string domStr = tempDomains[y];
						bool inval = false;
						try
						{
							urxed = new Uri("http://" + domStr);
						}
						catch (Exception)
						{
							if (setOptions[2])
								TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] " + domStr + vbCrLf + TxLogs.Text));
							inval = true;
							errCount += 1;
						}
						if (!inval)
						{
							string safeHost = urxed.DnsSafeHost;
							if (!IsLoopback(safeHost))
								downloadedHash.Add(safeHost);
						}
					}
					Array.Clear(tempDomains, 0, 0);
					//add to cache
					if (setOptions[3])
					{
						if (sourceCacheList.Any(x => x.URL == sourceUrl))
							sourceCacheList.RemoveAll(x => x.URL == sourceUrl);
						sourceCacheList.Add(new SourceCache { URL = sourceUrl, Domains = downloadedHash });
					}
					else
					{
						if (sourceCacheList.Count() > 1)
						{
							sourceCacheList.Clear();
							sourceCacheList.TrimExcess();
						}
					}
					TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Parsed] " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + vbCrLf + TxLogs.Text));
				}
			}
		}

		private void BgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		#endregion "Worker"

		#region "Functions"

		private string LogDate()
		{
			return DateTime.UtcNow.ToString("HH:mm:ss", invarCulture) + "> ";
		}

		private bool IsIPAddress(string input)
		{
			return Regex.Match(input, @"^((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)))$").Success;
		}

		private bool IsLoopback(string input)
		{
			return Regex.Match(input, string.Join("|", setLoopbacks.Select(x => @"\b^" + Regex.Escape(x) + "$")), RegexOptions.IgnoreCase).Success;
		}

		#endregion "Functions

	}

	internal class SourceCache
	{
		internal string URL { get; set; }
		internal HashSet<string> Domains { get; set; }
	}
}