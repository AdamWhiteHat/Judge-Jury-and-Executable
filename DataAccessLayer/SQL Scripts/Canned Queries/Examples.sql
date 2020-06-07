/*


-- Check number of entries (distinct files)
SELECT COUNT(SHA256) FROM [FileProperties]


-- Select the last 1000 file entries to be added to the DB
SELECT TOP 1000 *
FROM [FileProperties]
ORDER BY [DateSeen] DESC




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





        [FileOwner] <> 'TrustedInstaller'
    AND [DirectoryLocation] = ':\Windows\System32'
    AND IsSigned = 0
ORDER BY [PrevalenceCount] DESC





*/
