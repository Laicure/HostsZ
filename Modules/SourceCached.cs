using System.Collections.Generic;

namespace HostsZ.Modules
{
    public class SourceCached
    {
        internal string URL { get; set; }
        internal HashSet<string> Domains { get; set; }
    }
}