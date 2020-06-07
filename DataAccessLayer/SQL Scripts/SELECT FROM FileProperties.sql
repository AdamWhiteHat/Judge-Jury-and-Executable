/*

TRUNCATE TABLE [FileProperties]

*/

/**/


SELECT TOP 10000 * 
FROM [FileProperties]
ORDER BY [MFTNumber] DESC


/*
SELECT TOP 1 *
FROM [FileProperties]
WHERE 
   -- [MFTNumber] = 752 AND [SequenceNumber] = 0 AND [SHA256] = '1C1EEE77FD0151FEFE27D2C1074EF844CEDA7F909A78480A466228A3647C858E'
  --AND 
  [YaraRulesMatched] IS NOT NULL

*/

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
