# HostsZ

<a href="https://github.com/Laicure/HostsZ/releases"><img alt="GitHub All Releases" src="https://img.shields.io/github/downloads/laicure/HostsZ/total.svg"></a>
<a href="https://github.com/Laicure/HostsZ/commits/master"><img alt="GitHub last commit (branch)" src="https://img.shields.io/github/last-commit/laicure/HostsZ/master.svg"></img></a>
<a href="https://github.com/Laicure/HostsZ"><img src="https://img.shields.io/github/repo-size/Laicure/HostsZ.svg"></img></a>
<a href="https://github.com/Laicure/HostsZ/blob/master/LICENSE"><img src="https://img.shields.io/github/license/Laicure/HostsZ.svg"></img></a>

Simple hosts file compiler for Windows.
_Inspired by [Steven Black](https://github.com/StevenBlack)'s [hosts repository](https://github.com/StevenBlack/hosts)._

##### VB.NET version in [HostsY](https://github.com/Laicure/HostsY)

---------
#### Auto Parameters (requires admin privileges):
Required:
* \-auto
	* Initializes Auto Generate State
	* Directly replaces the hosts file in **C:\\Windows\\System32\\drivers\\etc**
	* Required: **source.txt** for the _host sources_
	* Optional: **black.txt** for domain _blacklist_
	* Optional: **white.txt** for domain _whitelist_
	* Optional: **loopback.txt** for domain _loopbacks_
```
HostsZ.exe
black.txt
source.txt
white.txt
loopback.txt
```

###### Possible loopbacks:
```
0.0.0.0
broadcasthost
ip6-allhosts
ip6-allnodes
ip6-allrouters
ip6-localhost
ip6-localnet
ip6-loopback
ip6-mcastprefix
local
localhost
localhost.localdomain
```
---------
Optional Parameter after _-auto_:
* \-dpl<n>
	* Indicates that it will generate <2~9> number of domains per line (e.g. -dpl4) to reduce the file size
---------
###### Check [HostY_host](https://github.com/Laicure/HostsY_hosts) for the generated files
