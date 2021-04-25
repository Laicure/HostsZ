namespace HostsZ.Forms {

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

    public partial class MainZ : Form {

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

        public MainZ() {
            InitializeComponent();
        }

        #region "Auto"

        protected override void SetVisibleCore(bool value) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            if(!IsHandleCreated) {
                CreateHandle();
                if(Environment.GetCommandLineArgs().LongLength > 1 && Environment.CommandLine.Contains("-auto")) {
                    value = false;
                    Autoo();
                } else {
                    value = true;
                }
            }

            base.SetVisibleCore(value);
        }

        private void Autoo() {
            ////init
            string argg = Environment.CommandLine.Replace(Environment.GetCommandLineArgs()[0], string.Empty).Trim();
            bool tabb = argg.Contains("-tab");
            bool dpl = Regex.Match(argg, @"(\-dpl)([2-9])").Success;

            ////file checks
            if(!File.Exists(startUpPath + "source.txt")) {
                File.WriteAllText(startUpPath + "source.txt", string.Empty, Encoding.ASCII);
            }

            if(!File.Exists(startUpPath + "white.txt")) {
                File.WriteAllText(startUpPath + "white.txt", string.Empty, Encoding.ASCII);
            }

            if(!File.Exists(startUpPath + "black.txt")) {
                File.WriteAllText(startUpPath + "black.txt", string.Empty, Encoding.ASCII);
            }

            if(!File.Exists(startUpPath + "loopback.txt")) {
                File.WriteAllLines(startUpPath + "loopback.txt", new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" }, Encoding.ASCII);
            }

            ////source check
            setSources = File.ReadAllLines(startUpPath + "source.txt").Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();
            ////exit for invalid source
            if(setSources.Count() == 0) {
                File.WriteAllText(startUpPath + "log.txt", LogDate(false) + "[Init] No valid sources parsed!", Encoding.ASCII);
                Environment.Exit(3);
                return;
            }
            ////init
            setOptions = new bool[] { tabb, true };
            setTargIP = "0.0.0.0";
            setDPL = dpl ? Convert.ToInt32(Regex.Replace(argg, @"^.+?(\-dpl)([2-9]).+?$", "$2")) : 1;
            setLoopbacks = File.ReadAllLines(startUpPath + "loopback.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            setWhitelist = File.ReadAllLines(startUpPath + "white.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) & !x.Equals("*", StringComparison.InvariantCulture)).Where(x => !IsIPAddress(x) & !IsLoopback(x)).ToArray();
            setBlacklist = File.ReadAllLines(startUpPath + "black.txt").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x) & !IsLoopback(x) & Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)).ToArray();

            ////start
            logz = LogDate(true) + "[Start]";
            generated = string.Empty;
            BgGenerate.RunWorkerAsync();

            startExec = DateTime.UtcNow;
            errCount = 0;
            HashSet<string> downloadedUnified = new HashSet<string>();

            ////download sources
            for(int i = 0; i <= setSources.Count() - 1; i++) {
                string sourceUrl = setSources[i];
                string downloadedData = string.Empty;
                ////download
                logz = LogDate(false) + "[Fetch] " + sourceUrl + newLined + logz;
                try {
                    using(var clie = new WebClient()) {
                        clie.UseDefaultCredentials = true;
                        downloadedData = clie.DownloadString(sourceUrl);
                    }
                } catch(Exception ex) {
                    logz = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + logz;
                    errCount += 1;
                }

                if(downloadedData != string.Empty) {
                    ////parse
                    HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                    string[] tempDomains = downloadedHash.ToArray();
                    downloadedHash.Clear();
                    downloadedHash.TrimExcess();
                    for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                        Uri urxed = null;
                        string domStr = tempDomains[y];
                        bool inval = false;
                        try {
                            urxed = new Uri("http://" + domStr);
                        } catch(Exception) {
                            if(setOptions[1]) {
                                {
                                    logz = "> [Invalid] " + domStr + newLined + logz;
                                }
                            }

                            inval = true;
                            errCount += 1;
                        }

                        if(!inval) {
                            string safeHost = urxed.DnsSafeHost;
                            if(!IsLoopback(safeHost)) {
                                downloadedHash.Add(safeHost);
                            }
                        }
                    }

                    Array.Clear(tempDomains, 0, 0);

                    ////unify
                    downloadedUnified.UnionWith(downloadedHash);
                    logz = LogDate(false) + "[Parsed] " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + logz;
                }
            }

            ////remove whitelisted
            if(setWhitelist.Count() > 0) {
                logz = LogDate(false) + "[Clean] Whitelist" + newLined + logz;
                ////remove non-wildcard
                downloadedUnified.ExceptWith(setWhitelist.Where(x => Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)));
                downloadedUnified.TrimExcess();

                ////remove wildcarded
                if(setWhitelist.Where(x => x.Contains("*")).Count() > 0) {
                    string whiteRegex = string.Join("|", setWhitelist.Where(x => x.Contains("*")).Select(x => "(^" + Regex.Escape(x).Replace(@"\*", ".+?") + "$)").Distinct());
                    downloadedUnified.ExceptWith(downloadedUnified.Where(x => Regex.Match(x, whiteRegex, RegexOptions.IgnoreCase).Success).ToList());
                    downloadedUnified.TrimExcess();
                }

                ////parse url whitelist
                if(setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Count() > 0) {
                    string[] whitelistSources = setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Distinct().ToArray();
                    for(int i = 0; i <= whitelistSources.Count() - 1; i++) {
                        string whitelistUrl = whitelistSources[i];
                        string downloadedData = string.Empty;
                        logz = LogDate(false) + "[Fetch] Whitelist - " + whitelistUrl + newLined + logz;
                        try {
                            using(var clie = new WebClient()) {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(whitelistUrl);
                            }
                        } catch(Exception ex) {
                            logz = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + logz;
                            errCount += 1;
                        }

                        if(downloadedData != string.Empty) {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try {
                                    urxed = new Uri("http://" + domStr);
                                } catch(Exception) {
                                    if(setOptions[1]) {
                                        logz = "> [Invalid] Whitelist - " + domStr + newLined + logz;
                                    }

                                    inval = true;
                                    errCount += 1;
                                }

                                if(!inval) {
                                    string safeHost = urxed.DnsSafeHost;
                                    if(!IsLoopback(safeHost)) {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            logz = LogDate(false) + "[Parsed] Whitelist - " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + logz;
                            downloadedUnified.ExceptWith(downloadedHash);
                        }
                    }
                }
            }

            ////remove duplicate blacklist
            HashSet<string> blacks = new HashSet<string>();
            if(setBlacklist.Count() > 0) {
                logz = LogDate(false) + "[Clean] Blacklist" + newLined + logz;
                blacks = new HashSet<string>(setBlacklist);
                Array.Clear(setBlacklist, 0, 0);
                blacks.ExceptWith(downloadedUnified);
                setBlacklist = blacks.ToArray();

                ////parse url blacklist
                if(setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Count() > 0) {
                    string[] blacklistSources = setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Distinct().ToArray();
                    for(int i = 0; i <= blacklistSources.Count() - 1; i++) {
                        string blacklistUrl = blacklistSources[i];
                        string downloadedData = string.Empty;
                        logz = LogDate(false) + "[Fetch] Blacklist - " + blacklistUrl + newLined + logz;
                        try {
                            using(var clie = new WebClient()) {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(blacklistUrl);
                            }
                        } catch(Exception ex) {
                            logz = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + logz;
                            errCount += 1;
                        }

                        if(downloadedData != string.Empty) {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try {
                                    urxed = new Uri("http://" + domStr);
                                } catch(Exception) {
                                    if(setOptions[1]) {
                                        logz = "> [Invalid] Blacklist - " + domStr + newLined + logz;
                                    }

                                    inval = true;
                                    errCount += 1;
                                }

                                if(!inval) {
                                    string safeHost = urxed.DnsSafeHost;
                                    if(!IsLoopback(safeHost)) {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            logz = LogDate(false) + "[Parsed] Blacklist - " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + logz;
                            blacks.UnionWith(downloadedHash);
                        }
                    }
                }
            }

            if(downloadedUnified.Count() == 0 & blacks.Count() == 0) {
                logz = LogDate(false) + "[Canceled] Nothing to generate!" + newLined + logz;
                return;
            }

            ////add targetIP
            string tabSpace = (setOptions[0] ? "\t" : " ").ToString();
            logz = LogDate(false) + "[Merge] Target IP and " + (setOptions[0] ? "Tab" : "Whitespace").ToString() + newLined + logz;
            if(setDPL == 1) {
                downloadedUnified = new HashSet<string>(downloadedUnified.Select(x => setTargIP + tabSpace + x));
            } else {
                string[] unifiedTemp = downloadedUnified.ToArray();
                int unifiedTempCountIndex = unifiedTemp.Count() - 1;
                downloadedUnified.Clear();
                downloadedUnified.TrimExcess();
                string artemp = string.Empty;
                for(int i = 0; i <= unifiedTempCountIndex; i++) {
                    artemp = artemp + " " + unifiedTemp[i];
                    if((i + 1) % setDPL == 0) {
                        downloadedUnified.Add(setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    } else if(i == unifiedTempCountIndex) {
                        downloadedUnified.Add(setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                }

                Array.Clear(unifiedTemp, 0, 0);
            }

            ////finalize
            logz = LogDate(false) + "[Merge] Finalize list" + newLined + logz;
            List<string> finalList = new List<string>
               {
                "# Entries: " + downloadedUnified.Count().ToString("#,0", invarCulture),
                "# As of " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", invarCulture) + " UTC",
                "# Generated using github.com/Laicure/HostsZ",
                string.Empty,
                "# Sources: " + setSources.Count().ToString("#,0", invarCulture)
               };
            finalList.AddRange(setSources.Select(x => "# " + x));
            finalList.Add(string.Empty);
            finalList.AddRange(new string[] { "# Loopbacks", "127.0.0.1" + tabSpace + "localhost", "::1" + tabSpace + "localhost" });
            finalList.Add(string.Empty);
            if(blacks.Count() > 0) {
                finalList.Add("# Blacklist");
                finalList.AddRange(blacks.Select(x => setTargIP + tabSpace + x));
                finalList.Add(string.Empty);
            }

            finalList.Add("# Start");
            finalList.AddRange(downloadedUnified);
            finalList.Add("# End");
            finalList.Add(string.Empty);

            generated = string.Join(newLined, finalList);

            if(errCount > 0) {
                logz = LogDate(false) + "[Error] Count: " + errCount.ToString("#,0", invarCulture) + newLined + logz;
            }

            logz = LogDate(false) + "[Count] Domains: " + downloadedUnified.Count().ToString("#,0", invarCulture) + newLined + logz;

            ////saveto
            logz = LogDate(false) + "[End] Took: " + DateTime.UtcNow.Subtract(startExec).ToString().Substring(0, 11) + newLined + logz;

            int exitcod = 0;
            if(generated != string.Empty) {
                try {
                    File.WriteAllText(@"C:\Windows\System32\drivers\etc\hosts", generated, System.Text.Encoding.ASCII);
                    logz = LogDate(false) + @"[End] Extracted to C:\Windows\System32\drivers\etc\hosts" + newLined + logz;
                } catch(Exception ex) {
                    logz = LogDate(false) + "[Error] " + ex.Source + ": " + ex.Message + newLined + logz;
                    exitcod = 5;
                }
            } else {
                logz = LogDate(false) + "[End] Nothing generated!" + newLined + logz;
                exitcod = 1;
            }

            ////write to logs
            File.WriteAllText(startUpPath + "log.txt", logz, Encoding.ASCII);

            ////success exit
            Environment.Exit(exitcod);
        }

        #endregion "Auto"

        #region "Form"

        private void MainZ_Load(object sender, EventArgs e) {
            Icon = HostsZ.Properties.Resources.art;
            Text = "HostsZ v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            ////init
            ChlOptions.SetItemChecked(1, true);

            ////Load data if present
            if(File.Exists(startUpPath + "source.txt")) {
                TxSources.Text = File.ReadAllText(startUpPath + "source.txt");
            }

            if(File.Exists(startUpPath + "white.txt")) {
                TxWhitelist.Text = File.ReadAllText(startUpPath + "white.txt");
            }

            if(File.Exists(startUpPath + "black.txt")) {
                TxBlacklist.Text = File.ReadAllText(startUpPath + "black.txt");
            }

            if(File.Exists(startUpPath + "loopback.txt")) {
                TxLoopbacks.Text = File.ReadAllText(startUpPath + "loopback.txt");
            }
        }

        private void MainZ_FormClosing(object sender, FormClosingEventArgs e) {
            if(BgGenerate.IsBusy) {
                if(MessageBox.Show("Are you sure to close the app?", "Are you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        #endregion "Form"

        #region "Controls"

        private void LbClearCache_Click(object sender, EventArgs e) {
            if(sourceCacheList.Count() > 1) {
                if(MessageBox.Show("Are you sure to clear cached sources?", "Are you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    sourceCacheList.Clear();
                    sourceCacheList.TrimExcess();
                }
            }
        }

        private void Tabber_SelectedIndexChanged(object sender, EventArgs e) {
            if(BgGenerate.IsBusy && Tabber.SelectedIndex == 0) {
                Tabber.SelectedIndex = 1;
                ////MessageBox.Show("HostsZ is currently busy generating your request." + newLined + "You may cancel the process instead.", "Busy!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(Tabber.SelectedIndex == 1) {
                if(!Regex.Match(TxTargetIP.Text.Trim(), @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.CultureInvariant).Success) {
                    TxTargetIP.Text = "0.0.0.0";
                }

                if(string.IsNullOrWhiteSpace(TxLoopbacks.Text)) {
                    TxLoopbacks.Text = string.Join(newLined, new string[] { "0.0.0.0", "broadcasthost", "ip6-allhosts", "ip6-allnodes", "ip6-allrouters", "ip6-localhost", "ip6-localnet", "ip6-loopback", "ip6-mcastprefix", "local", "localhost", "localhost.localdomain" });
                }

                TxSources.Lines = TxSources.Lines.Select(x => x.Trim()).Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).ToArray();

                ////init
                setOptions = new bool[] { ChlOptions.GetItemChecked(0), ChlOptions.GetItemChecked(1) };
                setTargIP = TxTargetIP.Text.Trim();
                setDPL = Convert.ToInt32(NumDomainPerLine.Value);
                setLoopbacks = TxLoopbacks.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                setSources = TxSources.Lines;
                setWhitelist = TxWhitelist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) & !x.Equals("*", StringComparison.InvariantCulture)).Where(x => !IsIPAddress(x) & !IsLoopback(x)).ToArray();
                setBlacklist = TxBlacklist.Lines.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x) & !IsLoopback(x) & Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)).ToArray();

                if(setSources.Count() == 0) {
                    MessageBox.Show("No valid sources parsed!", "Nope, sorry. Nothing.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } else {
                    if(!BgGenerate.IsBusy) {
                        using(FolderBrowserDialog saveToBrowse = new FolderBrowserDialog()) {
                            {
                                saveToBrowse.Description = "Select a folder to save the generated hosts file";
                                saveToBrowse.RootFolder = Environment.SpecialFolder.Desktop;
                                saveToBrowse.ShowNewFolderButton = true;
                                if(saveToBrowse.ShowDialog() == DialogResult.OK) {
                                    savePath = saveToBrowse.SelectedPath;
                                    TxLogs.Text = LogDate(true) + "[Start]";
                                    generated = string.Empty;
                                    LbCancel.Visible = true;
                                    genCancel = false;
                                    BgGenerate.RunWorkerAsync();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LbCancel_Click(object sender, EventArgs e) {
            if(BgGenerate.IsBusy) {
                genCancel = true;
                LbCancel.Visible = false;
            }
        }

        #endregion "Controls"

        #region "Worker"

        private void BgGenerate_DoWork(object sender, DoWorkEventArgs e) {
            startExec = DateTime.UtcNow;
            errCount = 0;
            HashSet<string> downloadedUnified = new HashSet<string>();

            ////download sources
            for(int i = 0; i <= setSources.Count() - 1; i++) {
                ////cancel?
                if(genCancel) {
                    return;
                }

                string sourceUrl = setSources[i];

                ////use cache?
                if(sourceCacheList.Any(x => x.URL == sourceUrl)) {
                    HashSet<string> sourceDomains = new HashSet<string>(sourceCacheList.Where(x => x.URL == sourceUrl).Select(x => x.Domains).FirstOrDefault());
                    downloadedUnified.UnionWith(sourceDomains);
                    TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Cached] (" + sourceDomains.Count.ToString("#,0", invarCulture) + ") " + sourceUrl + newLined + TxLogs.Text));
                } else {
                    string downloadedData = string.Empty;
                    ////download
                    TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Fetch] " + sourceUrl + newLined + TxLogs.Text));
                    try {
                        using(var clie = new WebClient()) {
                            clie.UseDefaultCredentials = true;
                            downloadedData = clie.DownloadString(sourceUrl);
                        }
                    } catch(Exception ex) {
                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + TxLogs.Text));
                        errCount += 1;
                    }

                    if(downloadedData != string.Empty) {
                        ////parse
                        HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                        string[] tempDomains = downloadedHash.ToArray();
                        downloadedHash.Clear();
                        downloadedHash.TrimExcess();
                        for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                            ////cancel?
                            if(genCancel) {
                                return;
                            }

                            Uri urxed = null;
                            string domStr = tempDomains[y];
                            bool inval = false;
                            try {
                                urxed = new Uri("http://" + domStr);
                            } catch(Exception) {
                                if(setOptions[1]) {
                                    TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] " + domStr + newLined + TxLogs.Text));
                                }

                                inval = true;
                                errCount += 1;
                            }

                            if(!inval) {
                                string safeHost = urxed.DnsSafeHost;
                                if(!IsLoopback(safeHost)) {
                                    downloadedHash.Add(safeHost);
                                }
                            }
                        }

                        Array.Clear(tempDomains, 0, 0);
                        ////add/re-add to cache
                        if(sourceCacheList.Any(x => x.URL == sourceUrl)) {
                            sourceCacheList.RemoveAll(x => x.URL == sourceUrl);
                        }

                        sourceCacheList.Add(new Modules.SourceCached { URL = sourceUrl, Domains = downloadedHash });
                        ////unify
                        downloadedUnified.UnionWith(downloadedHash);
                        TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Parsed] " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + TxLogs.Text));
                    }
                }
            }

            ////remove whitelisted
            if(setWhitelist.Count() > 0) {
                TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Clean] Whitelist" + newLined + TxLogs.Text));
                ////remove non-wildcard
                downloadedUnified.ExceptWith(setWhitelist.Where(x => Uri.TryCreate("http://" + x, UriKind.Absolute, out urx)));
                downloadedUnified.TrimExcess();

                ////remove wildcarded
                if(setWhitelist.Where(x => x.Contains("*")).Count() > 0) {
                    string whiteRegex = string.Join("|", setWhitelist.Where(x => x.Contains("*")).Select(x => "(^" + Regex.Escape(x).Replace(@"\*", ".+?") + "$)").Distinct());
                    downloadedUnified.ExceptWith(downloadedUnified.Where(x => Regex.Match(x, whiteRegex, RegexOptions.IgnoreCase).Success).ToList());
                    downloadedUnified.TrimExcess();
                }

                ////parse url whitelist
                if(setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Count() > 0) {
                    string[] whitelistSources = setWhitelist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Distinct().ToArray();
                    for(int i = 0; i <= whitelistSources.Count() - 1; i++) {
                        ////cancel?
                        if(genCancel) {
                            return;
                        }

                        string whitelistUrl = whitelistSources[i];
                        string downloadedData = string.Empty;
                        TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Fetch] Whitelist - " + whitelistUrl + newLined + TxLogs.Text));
                        try {
                            using(var clie = new WebClient()) {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(whitelistUrl);
                            }
                        } catch(Exception ex) {
                            TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + TxLogs.Text));
                            errCount += 1;
                        }

                        if(downloadedData != string.Empty) {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                                ////cancel?
                                if(genCancel) {
                                    return;
                                }

                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try {
                                    urxed = new Uri("http://" + domStr);
                                } catch(Exception) {
                                    if(setOptions[1]) {
                                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] Whitelist - " + domStr + newLined + TxLogs.Text));
                                    }

                                    inval = true;
                                    errCount += 1;
                                }

                                if(!inval) {
                                    string safeHost = urxed.DnsSafeHost;
                                    if(!IsLoopback(safeHost)) {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Parsed] Whitelist - " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + TxLogs.Text));
                            downloadedUnified.ExceptWith(downloadedHash);
                        }
                    }
                }
            }

            ////remove duplicate blacklist
            HashSet<string> blacks = new HashSet<string>();
            if(setBlacklist.Count() > 0) {
                TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Clean] Blacklist" + newLined + TxLogs.Text));
                blacks = new HashSet<string>(setBlacklist);
                Array.Clear(setBlacklist, 0, 0);
                blacks.ExceptWith(downloadedUnified);
                setBlacklist = blacks.ToArray();

                ////parse url blacklist
                if(setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Count() > 0) {
                    string[] blacklistSources = setBlacklist.Where(x => Uri.TryCreate(x, UriKind.Absolute, out urx)).Distinct().ToArray();
                    for(int i = 0; i <= blacklistSources.Count() - 1; i++) {
                        ////cancel?
                        if(genCancel) {
                            return;
                        }

                        string blacklistUrl = blacklistSources[i];
                        string downloadedData = string.Empty;
                        TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Fetch] Blacklist - " + blacklistUrl + newLined + TxLogs.Text));
                        try {
                            using(var clie = new WebClient()) {
                                clie.UseDefaultCredentials = true;
                                downloadedData = clie.DownloadString(blacklistUrl);
                            }
                        } catch(Exception ex) {
                            TxLogs.Invoke(new Action(() => TxLogs.Text = "> [" + ex.Source + "] " + ex.Message.Replace(newLined, " ") + newLined + TxLogs.Text));
                            errCount += 1;
                        }

                        if(downloadedData != string.Empty) {
                            ////parse
                            HashSet<string> downloadedHash = new HashSet<string>(downloadedData.Split(new string[] { newLined, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x.Replace("\t", " "), " {2,}", " ").Trim()).Select(x => Regex.Replace(x, @"\#(.+|$)", string.Empty).Trim()).Select(x => Regex.Replace(x, @"^.+ ", string.Empty).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !IsIPAddress(x)));
                            string[] tempDomains = downloadedHash.ToArray();
                            downloadedHash.Clear();
                            downloadedHash.TrimExcess();
                            for(int y = 0; y <= tempDomains.Count() - 1; y++) {
                                ////cancel?
                                if(genCancel) {
                                    return;
                                }

                                Uri urxed = null;
                                string domStr = tempDomains[y];
                                bool inval = false;
                                try {
                                    urxed = new Uri("http://" + domStr);
                                } catch(Exception) {
                                    if(setOptions[1]) {
                                        TxLogs.Invoke(new Action(() => TxLogs.Text = "> [Invalid] Blacklist - " + domStr + newLined + TxLogs.Text));
                                    }

                                    inval = true;
                                    errCount += 1;
                                }

                                if(!inval) {
                                    string safeHost = urxed.DnsSafeHost;
                                    if(!IsLoopback(safeHost)) {
                                        downloadedHash.Add(safeHost);
                                    }
                                }
                            }

                            Array.Clear(tempDomains, 0, 0);
                            ////exclude
                            TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Parsed] Blacklist - " + downloadedHash.Count().ToString("#,0", invarCulture) + " valid domains!" + newLined + TxLogs.Text));
                            blacks.UnionWith(downloadedHash);
                        }
                    }
                }
            }

            if(downloadedUnified.Count() == 0 & blacks.Count() == 0) {
                TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Canceled] Nothing to generate!" + newLined + TxLogs.Text));
                return;
            }

            ////add targetIP
            string tabSpace = (setOptions[0] ? "\t" : " ").ToString();
            TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Merge] Target IP and " + (setOptions[0] ? "Tab" : "Whitespace").ToString() + newLined + TxLogs.Text));
            if(setDPL == 1) {
                downloadedUnified = new HashSet<string>(downloadedUnified.Select(x => setTargIP + tabSpace + x));
            } else {
                string[] unifiedTemp = downloadedUnified.ToArray();
                int unifiedTempCountIndex = unifiedTemp.Count() - 1;
                downloadedUnified.Clear();
                downloadedUnified.TrimExcess();
                string artemp = string.Empty;
                for(int i = 0; i <= unifiedTempCountIndex; i++) {
                    ////cancel?
                    if(genCancel) {
                        return;
                    }

                    artemp = artemp + " " + unifiedTemp[i];
                    if((i + 1) % setDPL == 0) {
                        downloadedUnified.Add(setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    } else if(i == unifiedTempCountIndex) {
                        downloadedUnified.Add(setTargIP + tabSpace + artemp);
                        artemp = string.Empty;
                    }
                }

                Array.Clear(unifiedTemp, 0, 0);
            }

            ////finalize
            TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Merge] Finalize list" + newLined + TxLogs.Text));
            List<string> finalList = new List<string>
               {
                "# Entries: " + downloadedUnified.Count().ToString("#,0", invarCulture),
                "# As of " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", invarCulture) + " UTC",
                "# generated using github.com/Laicure/HostsZ",
                string.Empty,
                "# Sources: " + sourceCacheList.Where(x => setSources.Contains(x.URL)).Select(x => x.URL).Count().ToString("#,0", invarCulture)
               };
            finalList.AddRange(sourceCacheList.Where(x => setSources.Contains(x.URL)).Select(x => "# [" + x.Domains.Count().ToString("#,0", invarCulture) + "] " + x.URL));
            finalList.Add(string.Empty);
            finalList.AddRange(new string[] { "# Loopbacks", "127.0.0.1" + tabSpace + "localhost", "::1" + tabSpace + "localhost" });
            finalList.Add(string.Empty);
            if(blacks.Count() > 0) {
                finalList.Add("# Blacklist");
                finalList.AddRange(blacks.Select(x => setTargIP + tabSpace + x));
                finalList.Add(string.Empty);
            }

            finalList.Add("# Start");
            finalList.AddRange(downloadedUnified);
            finalList.Add("# End");
            finalList.Add(string.Empty);

            generated = string.Join(newLined, finalList);

            if(errCount > 0) {
                TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Error] Count: " + errCount.ToString("#,0", invarCulture) + newLined + TxLogs.Text));
            }

            TxLogs.Invoke(new Action(() => TxLogs.Text = LogDate(false) + "[Count] Domains: " + downloadedUnified.Count().ToString("#,0", invarCulture) + newLined + TxLogs.Text));
        }

        private void BgGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            LbCancel.Visible = false;
            if(genCancel) {
                TxLogs.Text = LogDate(false) + "[Canceled] Took: " + DateTime.UtcNow.Subtract(startExec).ToString().Substring(0, 11) + newLined + TxLogs.Text;
                genCancel = false;
            } else {
                TxLogs.Text = LogDate(false) + "[End] Took: " + DateTime.UtcNow.Subtract(startExec).ToString().Substring(0, 11) + newLined + TxLogs.Text;
            }

            ////set parsed Counts
            GbSources.Text = "[" + setSources.Count().ToString("#,0", invarCulture) + "] Sources";
            GbWhitelist.Text = "[" + setWhitelist.Count().ToString("#,0", invarCulture) + "] Whitelist";
            GbBlacklist.Text = "[" + setBlacklist.Count().ToString("#,0", invarCulture) + "] Blacklist";

            if(generated != string.Empty) {
                try {
                    string saveP = savePath + @"\hosts " + DateTime.UtcNow.ToString("ffff", invarCulture);
                    File.WriteAllText(saveP, generated, System.Text.Encoding.ASCII);
                    System.Diagnostics.Process.Start("explorer", "/select, " + saveP);
                } catch(Exception ex) {
                    TxLogs.Text = LogDate(false) + "[Error] " + ex.Source + ": " + ex.Message + newLined + TxLogs.Text;
                }
            } else {
                TxLogs.Text = LogDate(false) + "[End] Nothing generated!" + newLined + TxLogs.Text;
            }
        }

        #endregion "Worker"

        #region "Functions"

        private string LogDate(bool withDate) {
            return DateTime.UtcNow.ToString(withDate ? "yyyy-MM-dd HH:mm:ss" : "HH:mm:ss", invarCulture) + "> ";
        }

        private bool IsIPAddress(string input) {
            return Regex.Match(input, @"^((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)))$").Success;
        }

        private bool IsLoopback(string input) {
            return Regex.Match(input, string.Join("|", setLoopbacks.Select(x => @"\b^" + Regex.Escape(x) + "$")), RegexOptions.IgnoreCase).Success;
        }

        #endregion "Functions"
    }
}