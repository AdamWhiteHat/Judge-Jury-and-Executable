using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SqliteDataAccessLayer
{
	public static class SqlStrings
	{
		private const string TableName = "FileProperties";

		public static string SelectPrevalenceCount =
			$"SELECT [PrevalenceCount] FROM [{TableName}] WHERE {0}";

		public static string UpdatePrevalenceCount =
			$"UPDATE [{TableName}] SET [PrevalenceCount] = {0} WHERE {1}";

		public static string InsertInto =
		   $"INSERT INTO [{TableName}] ({0},[PrevalenceCount],[DateSeen]) VALUES ({1},1,date('now'))";

		public static string SelectYaraRules =
			$"SELECT TOP 1 [YaraRulesMatched] FROM [{TableName}] WHERE {0} AND [YaraRulesMatched] IS NOT NULL";

		public static string UpdateYaraRules =
			$"UPDATE [{TableName}] SET [YaraRulesMatched] = {0} WHERE {1}";

		#region Create Table Script

		public static string CreateTable =
$@"
CREATE TABLE [{TableName}] (
    [MFTNumber]          BIGINT         NOT NULL,
    [SequenceNumber]     INT            NOT NULL,
    [SHA256]             TEXT           NOT NULL,

    [FullPath]           TEXT           NULL,
    [Length]             BIGINT         NULL,
    [FileOwner]          TEXT           NULL,
    [Attributes]         TEXT           NULL,

    [IsExe]              BIT            NULL,
    [IsDll]              BIT            NULL,
    [IsDriver]           BIT            NULL,
    [BinaryType]         INT            NULL,
    [IsSigned]           BIT            NULL,
    [IsSignatureValid]   BIT            NULL,
    [IsValidCertChain]   BIT            NULL,
    [IsTrusted]          BIT            NULL,
    [ImpHash]            TEXT           NULL,
    [MD5]                TEXT           NULL,
    [SHA1]               TEXT           NULL,
    [CompileDate]        DATETIME2 (7)  NULL,

    [MimeType]           TEXT           NULL,
    [InternalName]       TEXT           NULL,
    [ProductName]        TEXT           NULL,
    [OriginalFileName]   TEXT           NULL,
    [FileVersion]        TEXT           NULL,
    [FileDescription]    TEXT           NULL,
    [Copyright]          TEXT           NULL,
    [Company]            TEXT           NULL,
    [Language]           TEXT           NULL,

    [Trademarks]         TEXT           NULL,
    [Project]            TEXT           NULL,
    [ApplicationName]    TEXT           NULL,
    [Comment]            TEXT           NULL,
    [Title]              TEXT           NULL,
    [Link]               TEXT           NULL,
    [ProviderItemID]     TEXT           NULL,

    [ComputerName]       TEXT           NULL,
    [DriveLetter]        CHAR           NULL,
    [DirectoryLocation]  TEXT           NULL,
    [Filename]           TEXT           NULL,
    [Extension]          TEXT           NULL,

    [CertSubject]        TEXT           NULL,
    [CertIssuer]         TEXT           NULL,
    [CertSerialNumber]   TEXT           NULL,
    [CertThumbprint]     TEXT           NULL,
    [CertNotBefore]      TEXT           NULL,
    [CertNotAfter]       TEXT           NULL,

    [PrevalenceCount]    INT            NULL,
    [Entropy]            FLOAT (53)     NULL,
    [YaraRulesMatched]   TEXT NULL,

    [DateSeen]           DATETIME2 (7)  NOT NULL,
    [MftTimeAccessed]    DATETIME2 (7)  NULL,
    [MftTimeCreation]    DATETIME2 (7)  NULL,
    [MftTimeModified]    DATETIME2 (7)  NULL,
    [MftTimeMftModified] DATETIME2 (7)  NULL,
    [CreationTime]       DATETIME2 (7)  NULL,
    [LastAccessTime]     DATETIME2 (7)  NULL,
    [LastWriteTime]      DATETIME2 (7)  NULL,

    CONSTRAINT [PK_FileProperties] PRIMARY KEY ([MFTNumber], [SequenceNumber], [SHA256])
);
";
		#endregion

	}
}
