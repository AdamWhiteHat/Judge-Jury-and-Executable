/*

-- This clears all data rows from the table. The table will still exist, but it will be empty.
TRUNCATE TABLE [FileProperties]

-- This deletes the table. The table wont exist, and neither will its data.
DROP TABLE [FileProperties]

*/

IF EXISTS
(
    SELECT [TABLE_SCHEMA] 
    FROM [INFORMATION_SCHEMA].[TABLES] 
    WHERE [TABLE_NAME] = 'FileProperties'
)
BEGIN
    PRINT 'Table Already Exists'
END
ELSE
BEGIN
    CREATE TABLE [FileProperties]
    (
        [MFTNumber]          BIGINT         NOT NULL,
        [SequenceNumber]     INT            NOT NULL,
        [SHA256]             NVARCHAR (64)  NOT NULL,
    		
        [FullPath]           NVARCHAR (MAX) NULL,
    	[Length]             BIGINT         NULL,
    	[FileOwner]          NVARCHAR (250) NULL,
        [Attributes]         NVARCHAR (250) NULL,
    
        [IsExe]              BIT            NULL,
        [IsDll]              BIT            NULL,
        [IsDriver]           BIT            NULL,
    	[BinaryType]         INT            NULL,
        [IsSigned]           BIT            NULL,
        [IsSignatureValid]   BIT            NULL,
    	[IsValidCertChain]   BIT            NULL,
        [IsTrusted]          BIT            NULL,
    	[ImpHash]            NVARCHAR (64)  NULL,
    	[MD5]                NVARCHAR (64)  NULL,
    	[SHA1]               NVARCHAR (64)  NULL,
    	[CompileDate]		 DATETIME2 (7)  NULL,
    
        [MimeType]           NVARCHAR (250) NULL,
        [InternalName]       NVARCHAR (250) NULL,
        [ProductName]        NVARCHAR (250) NULL,
        [OriginalFileName]   NVARCHAR (250) NULL,
        [FileVersion]        NVARCHAR (250) NULL,
        [FileDescription]    NVARCHAR (MAX) NULL,
        [Copyright]          NVARCHAR (MAX) NULL,
        [Company]            NVARCHAR (250) NULL,
        [Language]           NVARCHAR (MAX) NULL,
    	
    	[Trademarks]         NVARCHAR (MAX) NULL,
        [Project]            NVARCHAR (MAX) NULL,
        [ApplicationName]    NVARCHAR (250) NULL,
        [Comment]            NVARCHAR (250) NULL,
        [Title]              NVARCHAR (250) NULL,
        [Link]               NVARCHAR (250) NULL,
    	[ProviderItemID]     NVARCHAR (250) NULL,
    	
    	[ComputerName]       NVARCHAR (250) NULL,
    	[DriveLetter]        CHAR           NULL,
        [DirectoryLocation]  NVARCHAR (MAX) NULL,
        [Filename]           NVARCHAR (MAX) NULL,
        [Extension]          NVARCHAR (250) NULL,
    
    	[CertSubject]        NVARCHAR (MAX) NULL,
        [CertIssuer]         NVARCHAR (MAX) NULL,
        [CertSerialNumber]   NVARCHAR (MAX) NULL,
        [CertThumbprint]     NVARCHAR (MAX) NULL,
        [CertNotBefore]      NVARCHAR (MAX) NULL,
        [CertNotAfter]       NVARCHAR (MAX) NULL,
    
    	[PrevalenceCount]    INT            NULL,
        [Entropy]            FLOAT (53)     NULL,
        [YaraRulesMatched]   NVARCHAR (MAX) NULL,
    
    	[DateSeen]           DATETIME2 (7)  NOT NULL,
    	[MftTimeAccessed]    DATETIME2 (7)  NULL,
    	[MftTimeCreation]    DATETIME2 (7)  NULL,
    	[MftTimeModified]    DATETIME2 (7)  NULL,
    	[MftTimeMftModified] DATETIME2 (7)  NULL,
    	[CreationTime]       DATETIME2 (7)  NULL,
        [LastAccessTime]     DATETIME2 (7)  NULL,
        [LastWriteTime]      DATETIME2 (7)  NULL,
    
        CONSTRAINT [PK_FileProperties] PRIMARY KEY ([MFTNumber], [SequenceNumber], [SHA256])
    )
    PRINT 'Table Created'
END
