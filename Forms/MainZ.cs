using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace HostsZ.Forms
{
	public partial class MainZ : Form
	{
		//inputs
		private bool[] setOptions = { }; //0=tab, 1=parse, 2=cache
		private string setTargIP = "0.0.0.0";
		private int setDPL = 1;
		private string[] setLoopbacks = { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" };
		private string[] setSources = { };
		private string[] setWhitelist = { };
		private string[] setBlacklist = { };

		//generation
		private string savePath = "";
		private DateTime startExec = DateTime.Now;
		private int errCount = 0;
		internal List<SourceCache> sourceCacheList = new List<SourceCache>();
		private string generated = "";

		//misc
		private Uri urx = null;
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
				//MessageBox.Show("HostsZ is currently busy generating your request." + vbCrLf + "You may cancel the process instead.", "Busy!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				TxSources.Lines = TxSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();

				//init
				setOptions = new bool[] { ChlOptions.GetItemChecked(0), ChlOptions.GetItemChecked(1), ChlOptions.GetItemChecked(2) };
				setTargIP = TxTargetIP.Text.Trim();
				setDPL = Convert.ToInt32(NumDomainPerLine.Value);
				setLoopbacks = TxLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
				setSources = TxSources.Lines;
				setWhitelist = TxWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) & !x.Equals("*", StringComparison.InvariantCulture)).Where(x => !IsIPAddress(x) & !IsLoopback(x)).ToArray();
				setBlacklist = TxBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x) & !IsLoopback(x) & Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)).ToArray();
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
			if (!BgGenerate.IsBusy)
			{
				using (FolderBrowserDialog SaveToBrowse = new FolderBrowserDialog())
				{
					{
						SaveToBrowse.Description = "Select a folder to save the generated hosts file";
						SaveToBrowse.RootFolder = Environment.SpecialFolder.Desktop;
						SaveToBrowse.ShowNewFolderButton = true;
						if (SaveToBrowse.ShowDialog() == DialogResult.OK)
						{
							savePath = SaveToBrowse.SelectedPath;

							LbGenerate.Text = "Generating...";
							TxLogs.Text = LogDate() + "[Start]";
							BgGenerate.RunWorkerAsync();
						}
					}
				}

			}
		}

		#endregion "Controls"

		#region "Worker"

		private void BgGenerate_DoWork(object sender, DoWorkEventArgs e)
		{
			startExec = DateTime.UtcNow;
			errCount = 0;
			HashSet<string> downloadedUnified = new HashSet<string>();

			//download sources
			for (int i = 0; i <= setSources.Count() - 1; i++)
			{
				string sourceUrl = setSources[i];

				//use cache?
				if (setOptions[2] && sourceCacheList.Any(x => x.URL == sourceUrl))
				{
					HashSet<string> sourceDomains = new HashSet<string>(sourceCacheList.Where(x => x.URL == sourceUrl).Select(x => x.Domains).FirstOrDefault());
					downloadedUnified.UnionWith(sourceDomains);
					TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Cached] (" + sourceDomains.Count.ToString("#,0", invarCulture) + ") " + sourceUrl + vbCrLf + TxLogs.Text));
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
						TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(vbCrLf, " ") + vbCrLf + TxLogs.Text));
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
							if (setOptions[1])
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
					if (setOptions[2])
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
					downloadedUnified.UnionWith(downloadedHash);
					TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Parsed] " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + vbCrLf + TxLogs.Text));
				}
			}

			//remove duplicate blacklist
			HashSet<string> blacks = new HashSet<string>();
			if (setBlacklist.Count() > 0)
			{
				TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Clean] Blacklist" + vbCrLf + TxLogs.Text));
				blacks = new HashSet<string>(setBlacklist);
				Array.Clear(setBlacklist, 0, 0);
				blacks.ExceptWith(downloadedUnified);
				setBlacklist = blacks.ToArray();
			}
			//remove whitelisted
			if (setWhitelist.Count() > 0)
			{
				TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Clean] Whitelist" + vbCrLf + TxLogs.Text));
				//remove non-wildcard
				downloadedUnified.ExceptWith(setWhitelist.Where(x => Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)));
				downloadedUnified.TrimExcess();

				//remove wildcarded
				if (setWhitelist.Contains("*"))
				{
					string whiteRegex = string.Join("|", setWhitelist.Where(x => x.Contains("*")).Select(x => "(^" + Regex.Escape(x).Replace(@"\*", ".+?") + "$)").Distinct());
					downloadedUnified.ExceptWith(downloadedUnified.Where(x => Regex.Match(x, whiteRegex, RegexOptions.IgnoreCase).Success));
					downloadedUnified.TrimExcess();
				}
			}

			//add targetIP
			string tabSpace = ((setOptions[0]) ? "\t" : " ").ToString();
			TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Merge] Target IP and " + ((setOptions[0]) ? "Tab" : "Whitespace").ToString() + vbCrLf + TxLogs.Text));
			if (setDPL == 1)
			{
				downloadedUnified = new HashSet<string>(downloadedUnified.Select(x => setTargIP + tabSpace + x));
			}
			else
			{
				string[] unifiedTemp = downloadedUnified.ToArray();
				int unifiedTempCountIndex = unifiedTemp.Count() - 1;
				downloadedUnified.Clear();
				downloadedUnified.TrimExcess();
				string artemp = "";
				for (int i = 0; i <= unifiedTempCountIndex; i++)
				{
					artemp = artemp + " " + unifiedTemp[i];
					if ((i + 1) % setDPL == 0)
					{
						downloadedUnified.Add(setTargIP + tabSpace + artemp);
						artemp = "";
					}
					else if (i == unifiedTempCountIndex)
					{
						downloadedUnified.Add(setTargIP + tabSpace + artemp);
						artemp = "";
					}
				}
				Array.Clear(unifiedTemp, 0, 0);
			}

			//finalize
			TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Merge] Finalize list" + vbCrLf + TxLogs.Text));
			List<string> finalList = new List<string>
			{
				"# Entries: " + downloadedUnified.Count().ToString("#,0", invarCulture),
				"# As of " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", invarCulture) + " UTC",
				"# Generated using github.com/Laicure/HostsZ",
				"",
				"# Sources: " + setSources.Count().ToString("#,0", invarCulture)
			};
			finalList.AddRange(setSources.Select(x => "# " + x));
			finalList.Add("");
			finalList.AddRange(new string[] { "# Loopbacks", "127.0.0.1" + tabSpace + "localhost", "::1" + tabSpace + "localhost" });
			finalList.Add("");
			if (setBlacklist.Count() > 0)
			{
				finalList.Add("# Blacklist");
				finalList.AddRange(setBlacklist.Select(x => setTargIP + tabSpace + x));
				finalList.Add("");
			}
			finalList.Add("# Start");
			finalList.AddRange(downloadedUnified);
			finalList.Add("# End");
			finalList.Add("");

			generated = string.Join(vbCrLf, finalList);

			if (errCount > 0)
				TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Error] Count: " + errCount.ToString("#,0", invarCulture) + vbCrLf + TxLogs.Text));
			TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate() + "[Domains] Count: " + downloadedUnified.Count().ToString("#,0", invarCulture) + vbCrLf + TxLogs.Text));
		}

		private void BgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LbGenerate.Text = "Generate";
			TxLogs.Text = LogDate() + "[End] Took: " + DateTime.UtcNow.Subtract(startExec).ToString().Substring(0, 11) + vbCrLf + TxLogs.Text;

			if (!string.IsNullOrWhiteSpace(generated))
			{
				string saveP = savePath + @"\hosts " + DateTime.UtcNow.ToString("ffff", invarCulture);
				File.WriteAllText(saveP, generated, System.Text.Encoding.ASCII);
				System.Diagnostics.Process.Start("explorer", "/select, " + saveP);
			}
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

		#endregion "Functions"
	}

	internal class SourceCache
	{
		internal string URL { get; set; }
		internal HashSet<string> Domains { get; set; }
	}
}