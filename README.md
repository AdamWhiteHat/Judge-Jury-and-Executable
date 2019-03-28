# Judge-Query-and-Executable
Hunt for viruses, malware, and APTs on (multiple) file systems using by writing queries in SQL.

Allow me to elaborate,

It collects a bunch of properties about each file in a file system (or an image of one), and places these properties in a SQL relational database. The secret sauce comes from being able to threat hunt, investigate, and ask questions about the data through the use of queries. A key feature here was _NOT_ inventing a proprietary query language. If you know SQL, you know how to query it. Even if you don't, this concept is powerful enough, that canned queries  (see below)

 
For each file, the properties it  Collects a bunch of properties, information and metadata, including:
 - SHA256 hash
 - MD5 hash
 - Import table hash (If exists)
 - MFT Number & Sequence Number
 - MFT Create/Modified/Accessed Dates
 - Create/Modified/Accessed Dates Reported by Operating System (QUERY IDEA: One way to look for suspicious files is to compare these dates with the MFT dates. If they dont agree, this is a red flag)
 - All the 'Standard' OS file properties; location, size, datestamps, attributes, metadata
 - Is PE or DLL or Driver?
 - Is Authenticode signed? Does the X.509 certificate chain verify?
 - Custom yara rules (Lists the rule names that match)
 - Mood (okay, not really)

Creates an entry for each record found in the MFT. If the file properties from the Framework/Operating System API cannot be accessed due to permissions or file locks (in use), the enry will simply not be updated with this information. In this way, an entry will still get created for it, even if it cant otherwise access the file. 


Query Ideas:
======

```SQL

-- Files who's owner is not TrustedInstaller in directory C:\Windows\System32\ should be an empty set

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
        [FileOwner] <> 'TrustedInstaller'
    AND [DirectoryLocation] = ':\Windows\System32'
    AND IsSigned = 0
ORDER BY [PrevalenceCount] DESC




-- The MFT dates and timestamps that do not match the ones reported by the OS meta-data is suspicious

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
	([MftTimeAccessed] <> [LastAccessTime]) OR
	([MftTimeCreation] <> [CreationTime]) OR
	([MftTimeMftModified] <> [LastWriteTime])
ORDER BY [DateSeen] DESC
```
