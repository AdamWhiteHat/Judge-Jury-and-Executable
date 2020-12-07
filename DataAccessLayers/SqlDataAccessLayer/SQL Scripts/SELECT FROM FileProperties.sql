
SELECT TOP 10000 * 
FROM [FileProperties]
ORDER BY [MFTNumber] DESC

/*

SELECT COUNT(SHA256) FROM [FileProperties]

*/


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

/*

-- WARNING: This clears all data rows from the table. The table will still exist, but it will be empty.
TRUNCATE TABLE [FileProperties]

*/
