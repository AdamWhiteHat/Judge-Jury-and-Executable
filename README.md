# Judge-Query-and-Executable
Hunt for viruses, malware, and APTs on (multiple) file systems using by writing queries in SQL.

Allow me to elaborate...

You start with a disk or disk images that are potentially dirty with malware, viruses, APT's (advanced persistent threats) or the like, and then scan them with this tool. (Optionally, and assuming you have the wisdom and foresight to do so, you may wish to scan a known good baseline disk image with this tool first (or later--doesn't matter). This is certainly not necessary, but can only serve to aid you.) The scanning portion of this tool collects a _bunch_ of properties about each file in a file system (or an image(s) of one), and places these properties in a SQL relational database table. The secret sauce comes from being able to threat hunt, investigate, or ask questions about the data through the use of queries, in the language of SQL, against the database that is created. A key feature here was _NOT_ inventing a proprietary query language. If you know SQL, and how to click a button, then you already know how to use this tool like a boss. Even if you don't, this concept is powerful enough that canned queries (see below) will get you a lot of mileage.

 
For each file, the properties, information and metadata collected include (not a comprehensive list):
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
 
 
Firstly, the tool creates an entry in the database for each record found in the MFT (master file table--its how NTFS does its record keeping) before attempting to access all the properties available about the file via the operating system. As a result of this, even if the file properties from the Framework/Operating System API cannot be accessed due to permissions, file locks (is in use), disk corruption, a zero-byte-length file, or various other reasons, the file will still be recorded, logged and tracked. The entry, however, will simply not contain the information about it that was not accessible to the operating system.


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
