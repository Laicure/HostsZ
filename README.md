# HostsZ
Revamped [HostsY](https://github.com/Laicure/HostsY) in C#

##### Accepts command-line parameters
- **-auto**
   - initializes automatic hosts file generation
   - replaces the current hosts file in *C:\Windows\System32\drivers\etc*
   - **-dpl<2-9>**
       - requires the _-auto_ parameter to take effect
       - indicates the domain per line
       - sample: _-dpl3_

auto-generation requires the following files to be beside the exe app:
- source.txt
- white.txt
- black.txt
- loopback.txt

- log.txt
    - will be auto-generated every after auto-generation of the hosts file
    
accepts urls as whitelist and blacklist too.
