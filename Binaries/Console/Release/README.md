# Judge Jury and Executable
## A Threat Hunting Forensics Tool


![Judge Jury and Executable Logo](https://github.com/AdamWhiteHat/Judge-Jury-and-Executable/blob/master/Logo.png "It's your environment, you be the judge, jury and executioner") 


## Features:
 - Scan a mounted filesystem for threats right away
 - Or gather a system baseline before an incident, for extra threat hunting ability
 - Can be used before, during or after an incident
 - For one to many workstations
 - Scans the MFT, bypassing file permissions, file locks or OS file protections/hiding/shadowing
 - Up to 51 different properties gathered for every file
 - Scan results go into an SQL table for later searching, aggregating results over many scans and/or many machines, and historical or retrospective analysis
 - Leverage the power of SQL to search file systems, query file properties, answer complex or high-level questions, and hunt for threats or indicators of compromise


## Requirements:
 - .NET Framework v4.8
 - Local or remote SQL database with read/write/create access.
 - Visual studio (if you wish to compile the C# code)
 - Access to the internet (or else how did you get this code??? Also for nuget packages...)
 - Basic knowlege of SQL



___




## Hunt for viruses, malware, and APTs on (multiple) file systems using by writing queries in SQL.


Allow me to elaborate...

You start with a disk or disk images that are potentially dirty with malware, viruses, APT's (advanced persistent threats) or the like, and then scan them with this tool. (Optionally, and assuming you have the wisdom and foresight to do so, you may wish to scan a known good baseline disk image with this tool first (or later--doesn't matter). This is certainly not necessary, but can only serve to aid you.) The forensics-level scanning portion of this tool collects a _bunch_ of properties about each file in a file system (or an image(s) of one), and places these properties in a SQL relational database table. The secret sauce comes from being able to threat hunt, investigate, or ask questions about the data through the use of queries, in the language of SQL, against the database that is created. A key feature here was _NOT_ inventing a proprietary query language. If you know SQL, and how to click a button, then you already know how to use this tool like a boss. Even if you don't, this concept is powerful enough that canned queries (see below) will get you a lot of mileage.


## Forensics-level scanning.
Firstly, the tool creates an entry in the database for each record found in the MFT (master file table--its how NTFS does its record keeping). This bypasses file security permissions, file hiding, stealth or obfuscation techniques, file deletion, or timestamp tampering. These techniques will not prevent the file from being scanned and catalogued.


## Rich, high-level data analytics.
Then all operating-system-level properties, data and meta-data available about each file is collected and augments each entry. As a result of this, even if the file properties from the Framework/Operating System API cannot be accessed due to permissions, file locks (is in use), disk corruption, a zero-byte-length file, or various other reasons, the file will still be recorded, logged and tracked. The entry, however, will simply not contain the information about it that was not accessible to the operating system. Up to 51 different properties may be collected for every file.
  

## 
![Screenshot](https://github.com/AdamWhiteHat/Judge-Jury-and-Executable/blob/master/Judge-Jury-and-Executable.PNG "Judge Jury and Executable Application Screenshot")
  
 
## For each file, information collected includes:
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



## Example data row:
| MFTNumber | SequenceNumber | SHA256 | FullPath | Length | FileOwner | Attributes | IsExe | IsDll | IsDriver | BinaryType | IsSigned | IsSignatureValid | IsValidCertChain | IsTrusted | ImpHash | MD5 | SHA1 | CompileDate | MimeType | InternalName | ProductName | OriginalFileName | FileVersion | FileDescription | Copyright | Company | Language | Trademarks | Project | ApplicationName | Comment | Title | Link | ProviderItemID | ComputerName | DriveLetter | DirectoryLocation | Filename | Extension | CertSubject | CertIssuer | CertSerialNumber | CertThumbprint | CertNotBefore | CertNotAfter | PrevalenceCount | Entropy | YaraRulesMatched | DateSeen | MftTimeAccessed | MftTimeCreation | MftTimeModified | MftTimeMftModified | CreationTime | LastAccessTime | LastWriteTime |
|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|
| 18010 | 0 | C67BE7D3F54D44AC264A18E33909482F1F8CA7B7FBAAF5659EF71ED9F8092C34 | C:\Windows\WinSxS\amd64_windows-defender-service-cloudclean_31bf3856ad364e35_6.3.9600.18603_none_73d12e8145b3841b\SymSrv.dll | 149264 | TrustedInstaller | A | 1 | 1 | 0 | 16 | 1 | 1 | 0 | 1 | 5D54F5D721E301667338323AC07578E3 | 65FB3391EB26F5AC647FC40501D8E21D | 4B46DB2A99A47FF6A6EE376F4D79F5298BFF28A2 | 2010-02-01 20:15:48.0000000 | application/x-msdownload | symsrv.dll | Debugging Tools for Windows(R) | symsrv.dll | 6.12.2.633 | Symbol Server | © Microsoft Corporation. All rights reserved. | Microsoft Corporation | English (United States) |  |  |  |  |  |  |  | L | C | C:\Windows\WinSxS\amd64_windows-defender-service-cloudclean_31bf3856ad364e35_6.3.9600.18603_none_73d12e8145b3841b | SymSrv.dll | .dll | CN=Microsoft Corporation, OU=MOPR, O=Microsoft Corporation, L=Redmond, S=Washington, C=US | CN=Microsoft Code Signing PCA, O=Microsoft Corporation, L=Redmond, S=Washington, C=US | 6105F71E000000000032 | D468FAEB5190BF9DECD9827AF470F799C41A769C | 7/13/2009 5:00:18 PM | 10/13/2010 5:10:18 PM | 1 | 0 | NULL | 2020-10-25 06:17:12.0133333 | 2013-06-18 14:43:52.6497911 | 2013-08-22 06:56:50.9086288 | 2013-08-22 06:56:50.9086288 | 2019-01-15 19:13:49.1704756 | 2013-08-22 06:56:50.9086288 | 2013-08-22 06:56:50.9086288 | 2013-06-18 14:43:52.6497911 |



Canned Queries:
======

```SQL

/*
IDEA: All files in the directory C:\Windows\System32\ should be 'owned' by TrustedInstaller.
If a file in the System32 directory is owned by a different user, this indicates an anomaly, 
and that user is likely the user that created that file.
Malware likes to masquerade around as valid Windows system files.
Executables that are placed in the System32 directory not only look more official, as it is a common path for
system files, but an explicit path to that executable does not need to be supplied to execute it from the
command line, windows 'Run' dialog box of the start menu, or the win32 API call ShellExecute.
*/

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
        [FileOwner] <> 'TrustedInstaller'
    AND [DirectoryLocation] = ':\Windows\System32'
    AND IsSigned = 0
ORDER BY [PrevalenceCount] DESC


/*
IDEA: The MFT creation timestamp and the OS creation timestamp should match.
If the MFT creation timestamp occurs after the creation time reported by the OS meta-data,
this indicates an anomaly.
Timestomp is a tool that is part of the Metasploit Framework that allows a user to backdate a file
to an arbitrary time of their choosing. There really isn't a good legitimate reason for doing this
(let me know if you can think of one), and is considered an anti-forensics technique.
*/

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
	([MftTimeAccessed] <> [LastAccessTime]) OR
	([MftTimeCreation] <> [CreationTime]) OR
	([MftTimeMftModified] <> [LastWriteTime])
ORDER BY [DateSeen] DESC

/*
IDEA: The 'CompileDate' property of any executable or dll should always come before the creation timestamp for that file.
Similar logic applies as for the MFT creation timestamp occuring after the creation timestamp. How could a program have been
compiled AFTER the file that holds it was created? This anomaly indicates backdating or timestomping has occurred.
*/


SELECT 
TOP 1000 *
FROM  [FileProperties]
WHERE
	([MftTimeCreation] < [CompileDate]) OR
	([CreationTime] < [CompileDate])
ORDER BY [DateSeen] DESC


```
