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
