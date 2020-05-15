# Judge-Jury-and-Executable

Requirements:
 - .NET Framework v4.8
 - Local or remote SQL database with read/write/create access
 - Visual studio (if you wish to compile the C# code)
 - Access to the internet (or else how did you get this code? Also for nuget packages.)
 - Basic knowledge of SQL
 - Mounted volume to scan
 - Administrator privileges to read that volume
 

#### Hunt for viruses, malware, and APTs on (multiple) file systems using by writing queries in SQL.


Allow me to elaborate...

You start with a disk or disk images that are potentially dirty with malware, viruses, APT's (advanced persistent threats) or the like, and then scan them with this tool. (Optionally, and assuming you have the wisdom and foresight to do so, you may wish to scan a known good baseline disk image with this tool first (or later--doesn't matter). This is certainly not necessary, but can only serve to aid you.) The forensics-level scanning portion of this tool collects a _bunch_ of properties about each file in a file system (or an image(s) of one), and places these properties in a SQL relational database table. The secret sauce comes from being able to threat hunt, investigate, or ask questions about the data through the use of queries, in the language of SQL, against the database that is created. A key feature here was _NOT_ inventing a proprietary query language. If you know SQL, and how to click a button, then you already know how to use this tool like a boss. Even if you don't, this concept is powerful enough that canned queries (see below) will get you a lot of mileage.


#### Forensics-level scanning.
Firstly, the tool creates an entry in the database for each record found in the MFT (master file table--its how NTFS does its record keeping). This bypasses file security permissions, file hiding, stealth or obfuscation techniques, file deletion, or timestamp tampering. These techniques will not prevent the file from being scanned and cataloged.


#### Rich, high-level data analytics.
Then all operating-system-level properties, data and meta-data available about each file is collected and augments each entry. As a result of this, even if the file properties from the Framework/Operating System API cannot be accessed due to permissions, file locks (is in use), disk corruption, a zero-byte-length file, or various other reasons, the file will still be recorded, logged and tracked. The entry, however, will simply not contain the information about it that was not accessible to the operating system.


#### For each file, information collected includes:
 - SHA256 hash
 - MD5 hash
 - Import table hash (if it exists)
 - MFT Number & Sequence Number
 - MFT Create/Modified/Accessed Dates
 - Create/Modified/Accessed Dates Reported by OS
 - All the 'Standard' OS file properties: location, size, datestamps, attributes, metadata
 - Is a PE or DLL or Driver?
 - Is Authenticode signed?
 - Does the X.509 certificate chain verify?
 - Custom YARA rules (Lists the rule names that match)
 - File entropy
 - VirusTotal score
 


Canned Queries:
======

```SQL

-- Idea: Files who's owner is not TrustedInstaller in directory C:\Windows\System32\ should be an empty set.

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
        [FileOwner] <> 'TrustedInstaller'
    AND [DirectoryLocation] = ':\Windows\System32'
    AND IsSigned = 0
ORDER BY [PrevalenceCount] DESC




-- Idea: The MFT dates and timestamps that do not match the ones reported by the OS meta-data is highly suspicious.

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
	([MftTimeAccessed] <> [LastAccessTime]) OR
	([MftTimeCreation] <> [CreationTime]) OR
	([MftTimeMftModified] <> [LastWriteTime])
ORDER BY [DateSeen] DESC
```
