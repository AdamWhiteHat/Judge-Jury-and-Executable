/*

TRUNCATE TABLE [FileProperties]

*/


SELECT TOP 1000 *
FROM [FileProperties]
ORDER BY [DateSeen] DESC


--SELECT COUNT(SHA256) FROM [FileProperties]

/*

SELECT 
TOP 1000 * 
FROM  [FileProperties]
WHERE
        [FileOwner] <> 'TrustedInstaller'
    AND [DirectoryLocation] = ':\Windows\System32'
    AND IsSigned = 0
ORDER BY [PrevalenceCount] DESC

*/
