namespace HostsZ.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public partial class MainZ : Form
    {
        ////readonly
        private readonly string newLined = Environment.NewLine;
        private readonly List<Modules.SourceCached> sourceCacheList = new List<Modules.SourceCached>();
        private readonly System.Globalization.CultureInfo invarCulture = System.Globalization.CultureInfo.InvariantCulture;
        private readonly string startUpPath = Application.StartupPath + @"\";

        ////inputs
        private bool[] setOptions = { }; ////0=tab, 1=parse
        private string setTargIP = "0.0.0.0";
        private int setDPL = 1;
        private string[] setLoopbacks = { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" };
        private string[] setSources = { };
        private string[] setWhitelist = { };
        private string[] setBlacklist = { };

        ////generation
        private string savePath = string.Empty;
        private DateTime startExec = DateTime.Now;
        private int errCount = 0;
        private string generated = string.Empty;
        private bool genCancel = false;

        ////misc
        private string logz = string.Empty;
        private Uri urx = null;

        public MainZ()
        {
            this.InitializeComponent();
        }

        #region "Auto"

        protected override void SetVisibleCore(bool value)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
                if (Environment.GetCommandLineArgs().LongLength > 1 && Environment.CommandLine.Contains("-auto"))
                {
                    value = false;
                    this.Autoo();
                }
                else
                {
                    value = true;
                }
            }

            base.SetVisibleCore(value);
        }

        private void Autoo()
        {
            ////init
            string argg = Environment.CommandLine.Replace(Environment.GetCommandLineArgs()[0], string.Empty).Trim();
            bool tabb = argg.Contains("-tab");
            bool dpl = Regex.Match(argg, @"(\-dpl)([2-9])").Success;

            ////file checks
            if (!File.Exists(this.startUpPath + "source.txt"))
            {
                File.WriteAllText(this.startUpPath + "source.txt", string.Empty, Encoding.ASCII);
            }

            if (!File.Exists(this.startUpPath + "white.txt"))
            {
                File.WriteAllText(this.startUpPath + "white.txt", string.Empty, Encoding.ASCII);
            }

            if (!File.Exists(this.startUpPath + "black.txt"))
            {
                File.WriteAllText(this.startUpPath + "black.txt", string.Empty, Encoding.ASCII);
            }

            if (!File.Exists(this.startUpPath + "loopback.txt"))
            {
                File.WriteAllLines(this.startUpPath + "loopback.txt", new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" }, Encoding.ASCII);
            }

            ////source check
            this.setSources = File.ReadAllLines(this.startUpPath + "source.txt").Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).ToArray();
            ////exit for invalid source
            if (this.setSources.Count() == 0)
            {
                File.WriteAllText(this.startUpPath + "log.txt", this.LogDate(false) + "[Init] No valid sources parsed!", Encoding.ASCII);
                Environment.Exit(3);
                return;
            }
            ////init
            this.setOptions = new bool[] { tabb, true };
            this.setTargIP = "0.0.0.0";
            this.setDPL = dpl ? Convert.ToInt32(Regex.Replace(argg, @"^.+?(\-dpl)([2-9]).+?$", "$2")) : 1;
            this.setLoopbacks = File.ReadAllLines(this.startUpPath + "loopback.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            this.setWhitelist = File.ReadAllLines(this.startUpPath + "white.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) & !x.Equals("*", StringComparison.InvariantCulture)).Where(x => !this.IsIPAddress(x) & !this.IsLoopback(x)).ToArray();
            this.setBlacklist = File.ReadAllLines(this.startUpPath + "black.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x) & !this.IsLoopback(x) & Uri.TryCreate("http://" + x, UriKind.Absolute, out this.urx)).ToArray();

            ////start
            this.logz = this.LogDate(true) + "[Start]";
            this.generated = string.Empty;
            BgGenerate.RunWorkerAsync();

            this.startExec = DateTime.UtcNow;
            this.errCount = 0;
            HashSet<string> downloadedUnified = new HashSet<string>();

            ////download sources
            for (int i = 0; i <= this.setSources.Count() - 1; i++)
            {
                string sourceUrl = this.setSources[i];
                string downloadedData = string.Empty;
                ////download
                this.logz = this.LogDate(false) + "[Fetch] " + sourceUrl + this.newLined + this.logz;
                try
                {
                    using (var clie = new WebClient())
                    {
                        clie.UseDefaultCredentials = true;
                        downloadedData = clie.DownloadString(sourceUrl);
                    }
                }
                catch (Exception ex)
                {
                    this.logz = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + this.logz;
                    this.errCount += 1;
                }

                if (downloadedData != string.Empty)
                {
                    ////parse
                    HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
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
                            if (this.setOptions[1])
                            {
                                {
                                    this.logz = "> [Invalid] " + domStr + this.newLined + this.logz;
                                }
                            }

                            inval = true;
                            this.errCount += 1;
                        }

                        if (!inval)
                        {
                            string safeHost = urxed.DnsSafeHost;
                            if (!this.IsLoopback(safeHost))
                            {
                                downloadedHash.Add(safeHost);
                            }
                        }
                    }

                    Array.Clear(tempDomains, 0, 0);

                    ////unify
                    downloadedUnified.UnionWith(downloadedHash);
                    this.logz = this.LogDate(false) + "[Parsed] " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + this.logz;
                }
            }

            ////remove whitelisted
            if (this.setWhitelist.Count() > 0)
            {
                this.logz = this.LogDate(false) + "[Clean] Whitelist" + this.newLined + this.logz;
                ////remove non-wildcard
                downloadedUnified.ExceptWith(this.setWhitelist.Where(x => Uri.TryCreate("http://" + x, UriKind.Absolute, out this.urx)));
                downloadedUnified.TrimExcess();

                ////remove wildcarded
                if (this.setWhitelist.Where(x => x.Contains("*")).Count() > 0)
                {
                    string whiteRegex = string.Join("|", this.setWhitelist.Where(x => x.Contains("*")).Select(x => "(^" + Regex.Escape(x).Replace(@"\*", ".+?") + "$)").Distinct());
                    downloadedUnified.ExceptWith(downloadedUnified.Where(x => Regex.Match(x, whiteRegex, RegexOptions.IgnoreCase).Success).ToList());
                    downloadedUnified.TrimExcess();
                }

                ////parse url whitelist
                if (this.setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Count() > 0)
                {
                    string[] whitelistSources = this.setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Distinct().ToArray();
                    for (int i = 0; i <= whitelistSources.Count() - 1; i++)
                    {
                        string whitelistUrl = whitelistSources[i];
                        string downloadedData = string.Empty;
                        this.logz = this.LogDate(false) + "[Fetch] Whitelist - " + whitelistUrl + this.newLined + this.logz;
                        try
                        {
                            using (var clie = new WebClient())
                            {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(whitelistUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.logz = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + this.logz;
                            this.errCount += 1;
                        }

                        if (downloadedData != string.Empty)
                        {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
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
                                    if (this.setOptions[1])
                                    {
                                        this.logz = "> [Invalid] Whitelist - " + domStr + this.newLined + this.logz;
                                    }

                                    inval = true;
                                    this.errCount += 1;
                                }

                                if (!inval)
                                {
                                    string safeHost = urxed.DnsSafeHost;
                                    if (!this.IsLoopback(safeHost))
                                    {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            this.logz = this.LogDate(false) + "[Parsed] Whitelist - " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + this.logz;
                            downloadedUnified.ExceptWith(downloadedHash);
                        }
                    }
                }
            }

            ////remove duplicate blacklist
            HashSet<string> blacks = new HashSet<string>();
            if (this.setBlacklist.Count() > 0)
            {
                this.logz = this.LogDate(false) + "[Clean] Blacklist" + this.newLined + this.logz;
                blacks = new HashSet<string>(this.setBlacklist);
                Array.Clear(this.setBlacklist, 0, 0);
                blacks.ExceptWith(downloadedUnified);
                this.setBlacklist = blacks.ToArray();

                ////parse url blacklist
                if (this.setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Count() > 0)
                {
                    string[] blacklistSources = this.setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Distinct().ToArray();
                    for (int i = 0; i <= blacklistSources.Count() - 1; i++)
                    {
                        string blacklistUrl = blacklistSources[i];
                        string downloadedData = string.Empty;
                        this.logz = this.LogDate(false) + "[Fetch] Blacklist - " + blacklistUrl + this.newLined + this.logz;
                        try
                        {
                            using (var clie = new WebClient())
                            {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(blacklistUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.logz = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + this.logz;
                            this.errCount += 1;
                        }

                        if (downloadedData != string.Empty)
                        {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
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
                                    if (this.setOptions[1])
                                    {
                                        this.logz = "> [Invalid] Blacklist - " + domStr + this.newLined + this.logz;
                                    }

                                    inval = true;
                                    this.errCount += 1;
                                }

                                if (!inval)
                                {
                                    string safeHost = urxed.DnsSafeHost;
                                    if (!this.IsLoopback(safeHost))
                                    {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            this.logz = this.LogDate(false) + "[Parsed] Blacklist - " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + this.logz;
                            blacks.UnionWith(downloadedHash);
                        }
                    }
                }
            }

            if (downloadedUnified.Count() == 0 & blacks.Count() == 0)
            {
                this.logz = this.LogDate(false) + "[Canceled] Nothing to generate!" + this.newLined + this.logz;
                return;
            }

            ////add targetIP
            string tabSpace = (this.setOptions[0] ? "\t" : " ").ToString();
            this.logz = this.LogDate(false) + "[Merge] Target IP and " + (this.setOptions[0] ? "Tab" : "Whitespace").ToString() + this.newLined + this.logz;
            if (this.setDPL == 1)
            {
                downloadedUnified = new HashSet<string>(downloadedUnified.Select(x => this.setTargIP + tabSpace + x));
            }
            else
            {
                string[] unifiedTemp = downloadedUnified.ToArray();
                int unifiedTempCountIndex = unifiedTemp.Count() - 1;
                downloadedUnified.Clear();
                downloadedUnified.TrimExcess();
                string artemp = string.Empty;
                for (int i = 0; i <= unifiedTempCountIndex; i++)
                {
                    artemp = artemp + " " + unifiedTemp[i];
                    if ((i + 1) % this.setDPL == 0)
                    {
                        downloadedUnified.Add(this.setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                    else if (i == unifiedTempCountIndex)
                    {
                        downloadedUnified.Add(this.setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                }

                Array.Clear(unifiedTemp, 0, 0);
            }

            ////finalize
            this.logz = this.LogDate(false) + "[Merge] Finalize list" + this.newLined + this.logz;
            List<string> finalList = new List<string>
               {
                "# Entries: " + downloadedUnified.Count().ToString("#,0", this.invarCulture),
                "# As of " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", this.invarCulture) + " UTC",
                "# Generated using github.com/Laicure/HostsZ",
                string.Empty,
                "# Sources: " + this.setSources.Count().ToString("#,0", this.invarCulture)
               };
            finalList.AddRange(this.setSources.Select(x => "# " + x));
            finalList.Add(string.Empty);
            finalList.AddRange(new string[] { "# Loopbacks", "127.0.0.1" + tabSpace + "localhost", "::1" + tabSpace + "localhost" });
            finalList.Add(string.Empty);
            if (blacks.Count() > 0)
            {
                finalList.Add("# Blacklist");
                finalList.AddRange(blacks.Select(x => this.setTargIP + tabSpace + x));
                finalList.Add(string.Empty);
            }

            finalList.Add("# Start");
            finalList.AddRange(downloadedUnified);
            finalList.Add("# End");
            finalList.Add(string.Empty);

            this.generated = string.Join(this.newLined, finalList);

            if (this.errCount > 0)
            {
                this.logz = this.LogDate(false) + "[Error] Count: " + this.errCount.ToString("#,0", this.invarCulture) + this.newLined + this.logz;
            }

            this.logz = this.LogDate(false) + "[Count] Domains: " + downloadedUnified.Count().ToString("#,0", this.invarCulture) + this.newLined + this.logz;

            ////saveto
            this.logz = this.LogDate(false) + "[End] Took: " + DateTime.UtcNow.Subtract(this.startExec).ToString().Substring(0, 11) + this.newLined + this.logz;

            int exitcod = 0;
            if (this.generated != string.Empty)
            {
                try
                {
                    File.WriteAllText(@"C:\Windows\System32\drivers\etc\hosts", this.generated, System.Text.Encoding.ASCII);
                    this.logz = this.LogDate(false) + @"[End] Extracted to C:\Windows\System32\drivers\etc\hosts" + this.newLined + this.logz;
                }
                catch (Exception ex)
                {
                    this.logz = this.LogDate(false) + "[Error] " + ex.Source + ": " + ex.Message + this.newLined + this.logz;
                    exitcod = 5;
                }
            }
            else
            {
                this.logz = this.LogDate(false) + "[End] Nothing this.generated!" + this.newLined + this.logz;
                exitcod = 1;
            }

            ////write to logs
            File.WriteAllText(this.startUpPath + "log.txt", this.logz, Encoding.ASCII);

            ////success exit
            Environment.Exit(exitcod);
        }

        #endregion "Auto"

        #region "Form"

        private void MainZ_Load(object sender, EventArgs e)
        {
            this.Icon = HostsZ.Properties.Resources.art;
            this.Text = "HostsZ v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            ////init
            ChlOptions.SetItemChecked(1, true);

            ////Load data if present
            if (File.Exists(this.startUpPath + "source.txt"))
            {
                TxSources.Text = File.ReadAllText(this.startUpPath + "source.txt");
            }

            if (File.Exists(this.startUpPath + "white.txt"))
            {
                TxWhitelist.Text = File.ReadAllText(this.startUpPath + "white.txt");
            }

            if (File.Exists(this.startUpPath + "black.txt"))
            {
                TxBlacklist.Text = File.ReadAllText(this.startUpPath + "black.txt");
            }

            if (File.Exists(this.startUpPath + "loopback.txt"))
            {
                TxLoopbacks.Text = File.ReadAllText(this.startUpPath + "loopback.txt");
            }
        }

        private void MainZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BgGenerate.IsBusy)
            {
                if (MessageBox.Show("Are you sure to close the app?", "Are you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion "Form"

        #region "Controls"

        private void LbClearCache_Click(object sender, EventArgs e)
        {
            if (this.sourceCacheList.Count() > 1)
            {
                if (MessageBox.Show("Are you sure to clear cached sources?", "Are you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.sourceCacheList.Clear();
                    this.sourceCacheList.TrimExcess();
                }
            }
        }

        private void Tabber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BgGenerate.IsBusy && Tabber.SelectedIndex == 0)
            {
                Tabber.SelectedIndex = 1;
                ////MessageBox.Show("HostsZ is currently busy generating your request." + this.newLined + "You may cancel the process instead.", "Busy!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Tabber.SelectedIndex == 1)
            {
                if (!Regex.Match(TxTargetIP.Text.Trim(), @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.CultureInvariant).Success)
                {
                    TxTargetIP.Text = "0.0.0.0";
                }

                if (string.IsNullOrWhiteSpace(TxLoopbacks.Text))
                {
                    TxLoopbacks.Text = string.Join(this.newLined, new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" });
                }

                TxSources.Lines = TxSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).ToArray();

                ////init
                this.setOptions = new bool[] { ChlOptions.GetItemChecked(0), ChlOptions.GetItemChecked(1) };
                this.setTargIP = TxTargetIP.Text.Trim();
                this.setDPL = Convert.ToInt32(NumDomainPerLine.Value);
                this.setLoopbacks = TxLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                this.setSources = TxSources.Lines;
                this.setWhitelist = TxWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) & !x.Equals("*", StringComparison.InvariantCulture)).Where(x => !this.IsIPAddress(x) & !this.IsLoopback(x)).ToArray();
                this.setBlacklist = TxBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x) & !this.IsLoopback(x) & Uri.TryCreate("http://" + x, UriKind.Absolute, out this.urx)).ToArray();

                if (this.setSources.Count() == 0)
                {
                    MessageBox.Show("No valid sources parsed!", "Nope, sorry. Nothing.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (!BgGenerate.IsBusy)
                    {
                        using (FolderBrowserDialog saveToBrowse = new FolderBrowserDialog())
                        {
                            {
                                saveToBrowse.Description = "Select a folder to save the this.generated hosts file";
                                saveToBrowse.RootFolder = Environment.SpecialFolder.Desktop;
                                saveToBrowse.ShowNewFolderButton = true;
                                if (saveToBrowse.ShowDialog() == DialogResult.OK)
                                {
                                    this.savePath = saveToBrowse.SelectedPath;
                                    TxLogs.Text = this.LogDate(true) + "[Start]";
                                    this.generated = string.Empty;
                                    LbCancel.Visible = true;
                                    this.genCancel = false;
                                    BgGenerate.RunWorkerAsync();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LbCancel_Click(object sender, EventArgs e)
        {
            if (BgGenerate.IsBusy)
            {
                this.genCancel = true;
                LbCancel.Visible = false;
            }
        }

        #endregion "Controls"

        #region "Worker"

        private void BgGenerate_DoWork(object sender, DoWorkEventArgs e)
        {
            this.startExec = DateTime.UtcNow;
            this.errCount = 0;
            HashSet<string> downloadedUnified = new HashSet<string>();

            ////download sources
            for (int i = 0; i <= this.setSources.Count() - 1; i++)
            {
                ////cancel?
                if (this.genCancel)
                {
                    return;
                }

                string sourceUrl = this.setSources[i];

                ////use cache?
                if (this.sourceCacheList.Any(x => x.URL == sourceUrl))
                {
                    HashSet<string> sourceDomains = new HashSet<string>(this.sourceCacheList.Where(x => x.URL == sourceUrl).Select(x => x.Domains).FirstOrDefault());
                    downloadedUnified.UnionWith(sourceDomains);
                    TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Cached] (" + sourceDomains.Count.ToString("#,0", this.invarCulture) + ") " + sourceUrl + this.newLined + TxLogs.Text));
                }
                else
                {
                    string downloadedData = string.Empty;
                    ////download
                    TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Fetch] " + sourceUrl + this.newLined + TxLogs.Text));
                    try
                    {
                        using (var clie = new WebClient())
                        {
                            clie.UseDefaultCredentials = true;
                            downloadedData = clie.DownloadString(sourceUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + TxLogs.Text));
                        this.errCount += 1;
                    }

                    if (downloadedData != string.Empty)
                    {
                        ////parse
                        HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
                        string[] tempDomains = downloadedHash.ToArray();
                        downloadedHash.Clear();
                        downloadedHash.TrimExcess();
                        for (int y = 0; y <= tempDomains.Count() - 1; y++)
                        {
                            ////cancel?
                            if (this.genCancel)
                            {
                                return;
                            }

                            Uri urxed = null;
                            string domStr = tempDomains[y];
                            bool inval = false;
                            try
                            {
                                urxed = new Uri("http://" + domStr);
                            }
                            catch (Exception)
                            {
                                if (this.setOptions[1])
                                {
                                    TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] " + domStr + this.newLined + TxLogs.Text));
                                }

                                inval = true;
                                this.errCount += 1;
                            }

                            if (!inval)
                            {
                                string safeHost = urxed.DnsSafeHost;
                                if (!this.IsLoopback(safeHost))
                                {
                                    downloadedHash.Add(safeHost);
                                }
                            }
                        }

                        Array.Clear(tempDomains, 0, 0);
                        ////add/re-add to cache
                        if (this.sourceCacheList.Any(x => x.URL == sourceUrl))
                        {
                            this.sourceCacheList.RemoveAll(x => x.URL == sourceUrl);
                        }

                        this.sourceCacheList.Add(new Modules.SourceCached { URL = sourceUrl, Domains = downloadedHash });
                        ////unify
                        downloadedUnified.UnionWith(downloadedHash);
                        TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Parsed] " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + TxLogs.Text));
                    }
                }
            }

            ////remove whitelisted
            if (this.setWhitelist.Count() > 0)
            {
                TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Clean] Whitelist" + this.newLined + TxLogs.Text));
                ////remove non-wildcard
                downloadedUnified.ExceptWith(this.setWhitelist.Where(x => Uri.TryCreate("http://" + x, UriKind.Absolute, out this.urx)));
                downloadedUnified.TrimExcess();

                ////remove wildcarded
                if (this.setWhitelist.Where(x => x.Contains("*")).Count() > 0)
                {
                    string whiteRegex = string.Join("|", this.setWhitelist.Where(x => x.Contains("*")).Select(x => "(^" + Regex.Escape(x).Replace(@"\*", ".+?") + "$)").Distinct());
                    downloadedUnified.ExceptWith(downloadedUnified.Where(x => Regex.Match(x, whiteRegex, RegexOptions.IgnoreCase).Success).ToList());
                    downloadedUnified.TrimExcess();
                }

                ////parse url whitelist
                if (this.setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Count() > 0)
                {
                    string[] whitelistSources = this.setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Distinct().ToArray();
                    for (int i = 0; i <= whitelistSources.Count() - 1; i++)
                    {
                        ////cancel?
                        if (this.genCancel)
                        {
                            return;
                        }

                        string whitelistUrl = whitelistSources[i];
                        string downloadedData = string.Empty;
                        TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Fetch] Whitelist - " + whitelistUrl + this.newLined + TxLogs.Text));
                        try
                        {
                            using (var clie = new WebClient())
                            {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(whitelistUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + TxLogs.Text));
                            this.errCount += 1;
                        }

                        if (downloadedData != string.Empty)
                        {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for (int y = 0; y <= tempDomains.Count() - 1; y++)
                            {
                                ////cancel?
                                if (this.genCancel)
                                {
                                    return;
                                }

                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try
                                {
                                    urxed = new Uri("http://" + domStr);
                                }
                                catch (Exception)
                                {
                                    if (this.setOptions[1])
                                    {
                                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] Whitelist - " + domStr + this.newLined + TxLogs.Text));
                                    }

                                    inval = true;
                                    this.errCount += 1;
                                }

                                if (!inval)
                                {
                                    string safeHost = urxed.DnsSafeHost;
                                    if (!this.IsLoopback(safeHost))
                                    {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Parsed] Whitelist - " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + TxLogs.Text));
                            downloadedUnified.ExceptWith(downloadedHash);
                        }
                    }
                }
            }

            ////remove duplicate blacklist
            HashSet<string> blacks = new HashSet<string>();
            if (this.setBlacklist.Count() > 0)
            {
                TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Clean] Blacklist" + this.newLined + TxLogs.Text));
                blacks = new HashSet<string>(this.setBlacklist);
                Array.Clear(this.setBlacklist, 0, 0);
                blacks.ExceptWith(downloadedUnified);
                this.setBlacklist = blacks.ToArray();

                ////parse url blacklist
                if (this.setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Count() > 0)
                {
                    string[] blacklistSources = this.setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out this.urx)).Distinct().ToArray();
                    for (int i = 0; i <= blacklistSources.Count() - 1; i++)
                    {
                        ////cancel?
                        if (this.genCancel)
                        {
                            return;
                        }

                        string blacklistUrl = blacklistSources[i];
                        string downloadedData = string.Empty;
                        TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Fetch] Blacklist - " + blacklistUrl + this.newLined + TxLogs.Text));
                        try
                        {
                            using (var clie = new WebClient())
                            {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(blacklistUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(this.newLined, " ") + this.newLined + TxLogs.Text));
                            this.errCount += 1;
                        }

                        if (downloadedData != string.Empty)
                        {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { this.newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !this.IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for (int y = 0; y <= tempDomains.Count() - 1; y++)
                            {
                                ////cancel?
                                if (this.genCancel)
                                {
                                    return;
                                }

                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try
                                {
                                    urxed = new Uri("http://" + domStr);
                                }
                                catch (Exception)
                                {
                                    if (this.setOptions[1])
                                    {
                                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] Blacklist - " + domStr + this.newLined + TxLogs.Text));
                                    }

                                    inval = true;
                                    this.errCount += 1;
                                }

                                if (!inval)
                                {
                                    string safeHost = urxed.DnsSafeHost;
                                    if (!this.IsLoopback(safeHost))
                                    {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Parsed] Blacklist - " + downloadedHash.Count().ToString("#,0", this.invarCulture) + " valid domains!" + this.newLined + TxLogs.Text));
                            blacks.UnionWith(downloadedHash);
                        }
                    }
                }
            }

            if (downloadedUnified.Count() == 0 & blacks.Count() == 0)
            {
                TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Canceled] Nothing to generate!" + this.newLined + TxLogs.Text));
                return;
            }

            ////add targetIP
            string tabSpace = (this.setOptions[0] ? "\t" : " ").ToString();
            TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Merge] Target IP and " + (this.setOptions[0] ? "Tab" : "Whitespace").ToString() + this.newLined + TxLogs.Text));
            if (this.setDPL == 1)
            {
                downloadedUnified = new HashSet<string>(downloadedUnified.Select(x => this.setTargIP + tabSpace + x));
            }
            else
            {
                string[] unifiedTemp = downloadedUnified.ToArray();
                int unifiedTempCountIndex = unifiedTemp.Count() - 1;
                downloadedUnified.Clear();
                downloadedUnified.TrimExcess();
                string artemp = string.Empty;
                for (int i = 0; i <= unifiedTempCountIndex; i++)
                {
                    ////cancel?
                    if (this.genCancel)
                    {
                        return;
                    }

                    artemp = artemp + " " + unifiedTemp[i];
                    if ((i + 1) % this.setDPL == 0)
                    {
                        downloadedUnified.Add(this.setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                    else if (i == unifiedTempCountIndex)
                    {
                        downloadedUnified.Add(this.setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                }

                Array.Clear(unifiedTemp, 0, 0);
            }

            ////finalize
            TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Merge] Finalize list" + this.newLined + TxLogs.Text));
            List<string> finalList = new List<string>
   {
    "# Entries: " + downloadedUnified.Count().ToString("#,0", this.invarCulture),
    "# As of " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", this.invarCulture) + " UTC",
    "# this.generated using github.com/Laicure/HostsZ",
    string.Empty,
    "# Sources: " + this.sourceCacheList.Where(x => this.setSources.Contains(x.URL)).Select(x => x.URL).Count().ToString("#,0", this.invarCulture)
   };
            finalList.AddRange(this.sourceCacheList.Where(x => this.setSources.Contains(x.URL)).Select(x => "# [" + x.Domains.Count().ToString("#,0", this.invarCulture) + "] " + x.URL));
            finalList.Add(string.Empty);
            finalList.AddRange(new string[] { "# Loopbacks", "127.0.0.1" + tabSpace + "localhost", "::1" + tabSpace + "localhost" });
            finalList.Add(string.Empty);
            if (blacks.Count() > 0)
            {
                finalList.Add("# Blacklist");
                finalList.AddRange(blacks.Select(x => this.setTargIP + tabSpace + x));
                finalList.Add(string.Empty);
            }

            finalList.Add("# Start");
            finalList.AddRange(downloadedUnified);
            finalList.Add("# End");
            finalList.Add(string.Empty);

            this.generated = string.Join(this.newLined, finalList);

            if (this.errCount > 0)
            {
                TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Error] Count: " + this.errCount.ToString("#,0", this.invarCulture) + this.newLined + TxLogs.Text));
            }

            TxLogs.Invoke(new Action(() => TxLogs.Text = this.LogDate(false) + "[Count] Domains: " + downloadedUnified.Count().ToString("#,0", this.invarCulture) + this.newLined + TxLogs.Text));
        }

        private void BgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LbCancel.Visible = false;
            if (this.genCancel)
            {
                TxLogs.Text = this.LogDate(false) + "[Canceled] Took: " + DateTime.UtcNow.Subtract(this.startExec).ToString().Substring(0, 11) + this.newLined + TxLogs.Text;
                this.genCancel = false;
            }
            else
            {
                TxLogs.Text = this.LogDate(false) + "[End] Took: " + DateTime.UtcNow.Subtract(this.startExec).ToString().Substring(0, 11) + this.newLined + TxLogs.Text;
            }

            ////set parsed Counts
            LbSources.Text = "[" + this.setSources.Count().ToString("#,0", this.invarCulture) + "] Sources";
            LbWhitelist.Text = "[" + this.setWhitelist.Count().ToString("#,0", this.invarCulture) + "] Whitelist";
            LbBlacklist.Text = "[" + this.setBlacklist.Count().ToString("#,0", this.invarCulture) + "] Blacklist";

            if (this.generated != string.Empty)
            {
                try
                {
                    string saveP = this.savePath + @"\hosts " + DateTime.UtcNow.ToString("ffff", this.invarCulture);
                    File.WriteAllText(saveP, this.generated, System.Text.Encoding.ASCII);
                    System.Diagnostics.Process.Start("explorer", "/select, " + saveP);
                }
                catch (Exception ex)
                {
                    TxLogs.Text = this.LogDate(false) + "[Error] " + ex.Source + ": " + ex.Message + this.newLined + TxLogs.Text;
                }
            }
            else
            {
                TxLogs.Text = this.LogDate(false) + "[End] Nothing this.generated!" + this.newLined + TxLogs.Text;
            }
        }

        #endregion "Worker"

        #region "Functions"

        private string LogDate(bool withDate)
        {
            return DateTime.UtcNow.ToString(withDate ? "yyyy-MM-dd HH:mm:ss" : "HH:mm:ss", this.invarCulture) + "> ";
        }

        private bool IsIPAddress(string input)
        {
            return Regex.Match(input, @"^((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)))$").Success;
        }

        private bool IsLoopback(string input)
        {
            return Regex.Match(input, string.Join("|", this.setLoopbacks.Select(x => @"\b^" + Regex.Escape(x) + "$")), RegexOptions.IgnoreCase).Success;
        }

        #endregion "Functions"
    }
}