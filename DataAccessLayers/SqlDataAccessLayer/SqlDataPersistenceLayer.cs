using System;
using System.Linq;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlDataAccessLayer
{
	using FilePropertiesDataObject;
	using FilePropertiesDataObject.Helpers;

	public class SqlDataPersistenceLayer : IDataPersistenceLayer
	{
		private string _connectionString;
		private const string TableName = "FileProperties";

		public SqlDataPersistenceLayer(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentException($"Argument {nameof(connectionString)} cannot be null, empty or whitespace.");
			}
			_connectionString = connectionString;

			if (!CheckIfTableExists())
			{
				CreateTable();
			}
		}

		public void Dispose()
		{
		}

		public bool PersistFileProperties(FileProperties fileProperties)
		{
			SqlKey key = new SqlKey(fileProperties.MFTNumber, fileProperties.SequenceNumber, fileProperties.Sha256);

			List<SqlParameter> sqlParameters = new List<SqlParameter>();
			sqlParameters.AddRange(key.Parameters);

			sqlParameters.AddRange(new List<SqlParameter>
			{
				ParameterHelper.GetParameter("DriveLetter", fileProperties.DriveLetter),
				ParameterHelper.GetParameter("FullPath",fileProperties.FullPath),
				ParameterHelper.GetParameter("Filename", fileProperties.FileName),
				ParameterHelper.GetParameter("Extension", fileProperties.Extension),
				ParameterHelper.GetParameter("DirectoryLocation", fileProperties.DirectoryLocation),
				ParameterHelper.GetParameter("Length", fileProperties.Length),

				ParameterHelper.GetParameter("MftTimeCreation", fileProperties.MftTimeCreation),
				ParameterHelper.GetParameter("MftTimeAccessed", fileProperties.MftTimeAccessed),
				ParameterHelper.GetParameter("MftTimeModified", fileProperties.MftTimeModified),
				ParameterHelper.GetParameter("MftTimeMftModified", fileProperties.MftTimeMftModified),
				ParameterHelper.GetParameter("CreationTime", fileProperties.CreationTime),
				ParameterHelper.GetParameter("LastAccessTime", fileProperties.LastAccessTime),
				ParameterHelper.GetParameter("LastWriteTime", fileProperties.LastWriteTime),

				ParameterHelper.GetParameter("Project", fileProperties.Project),
				ParameterHelper.GetParameter("ProviderItemID", fileProperties.ProviderItemID),
				ParameterHelper.GetParameter("OriginalFileName", fileProperties.OriginalFileName),
				ParameterHelper.GetParameter("FileOwner", fileProperties.FileOwner),
				ParameterHelper.GetParameter("FileVersion", fileProperties.FileVersion),
				ParameterHelper.GetParameter("FileDescription", fileProperties.FileDescription),
				ParameterHelper.GetParameter("Trademarks", fileProperties.Trademarks),
				ParameterHelper.GetParameter("Copyright", fileProperties.Copyright),
				ParameterHelper.GetParameter("Company", fileProperties.Company),
				ParameterHelper.GetParameter("ApplicationName", fileProperties.ApplicationName),
				ParameterHelper.GetParameter("Comment", fileProperties.Comment),
				ParameterHelper.GetParameter("Title", fileProperties.Title),
				ParameterHelper.GetParameter("Link", fileProperties.Link),

				ParameterHelper.GetParameter("MimeType", fileProperties.MimeType),
				ParameterHelper.GetParameter("InternalName", fileProperties.InternalName),
				ParameterHelper.GetParameter("ProductName", fileProperties.ProductName),
				ParameterHelper.GetParameter("Language", fileProperties.Language),
				ParameterHelper.GetParameter("ComputerName", fileProperties.ComputerName),

				ParameterHelper.GetParameter("Attributes", fileProperties.Attributes),

				ParameterHelper.GetParameter("SHA1",fileProperties.SHA1),
				ParameterHelper.GetParameter("MD5", fileProperties.MD5 ),
				ParameterHelper.GetParameter("ImpHash", fileProperties.ImpHash ),
				ParameterHelper.GetParameter("IsDll", fileProperties.IsDll),
				ParameterHelper.GetParameter("IsExe", fileProperties.IsExe ),
				ParameterHelper.GetParameter("IsDriver", fileProperties.IsDriver ),
				ParameterHelper.GetParameter("IsSigned", fileProperties.IsSigned),
				ParameterHelper.GetParameter("IsSignatureValid", fileProperties.IsSignatureValid ),
				ParameterHelper.GetParameter("IsValidCertChain", fileProperties.IsValidCertChain ),
				ParameterHelper.GetNewParameterByType("BinaryType", fileProperties.BinaryType.GetValueOrDefault(), SqlDbType.Int),
				ParameterHelper.GetNewParameterByType("CompileDate", fileProperties.CompileDate.GetValueOrDefault(), SqlDbType.DateTime2),
				ParameterHelper.GetParameter("IsTrusted", fileProperties.IsTrusted),

				ParameterHelper.GetParameter("CertSubject", fileProperties.CertSubject),
				ParameterHelper.GetParameter("CertIssuer", fileProperties.CertIssuer),
				ParameterHelper.GetParameter("CertSerialNumber", fileProperties.CertSerialNumber),
				ParameterHelper.GetParameter("CertThumbprint", fileProperties.CertThumbprint),
				ParameterHelper.GetParameter("CertNotBefore", fileProperties.CertNotBefore ),
				ParameterHelper.GetParameter("CertNotAfter", fileProperties.CertNotAfter),

				ParameterHelper.GetParameter("Entropy", fileProperties.Entropy??0)
			});

			return InsertIntoDB(fileProperties, key, sqlParameters);
		}

		private string GetExistingYaraRules(SqlKey key)
		{
			string queryText = $"SELECT TOP 1 [YaraRulesMatched] FROM [{TableName}] WHERE {key.GetWhereClause()} AND [YaraRulesMatched] IS NOT NULL";

			return (string)ExecuteScalar(queryText, key.Parameters);
		}

		private bool InsertIntoDB(FileProperties fileProperties, SqlKey key, List<SqlParameter> sqlParameters)
		{
			string updateText = string.Empty;

			if (!string.IsNullOrWhiteSpace(fileProperties.YaraMatchedRules))
			{
				List<string> newYaraMatchedRules = new List<string>();

				string currentYaraRulesMatchedValue = GetExistingYaraRules(key);
				if (currentYaraRulesMatchedValue != null)
				{
					newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(currentYaraRulesMatchedValue));
				}

				newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(fileProperties.YaraMatchedRules));

				string newYaraRulesMatchedValue = YaraHelper.FormatDelimitedRulesString(newYaraMatchedRules);

				SqlParameter yaraMatchedValueParameter = ParameterHelper.GetNewStringParameter("YaraRulesMatched", newYaraRulesMatchedValue);
				sqlParameters.Add(yaraMatchedValueParameter);

				updateText = $"UPDATE [{TableName}] SET [YaraRulesMatched] = {yaraMatchedValueParameter.ParameterName} WHERE {key.GetWhereClause()}";
			}

			string columnNames = sqlParameters.AsColumnString();
			string values = sqlParameters.AsValuesString();

			string insertStatement =
				$@"	INSERT INTO [{TableName}] 
					({columnNames},[PrevalenceCount],[DateSeen]) 
					VALUES 
					({values},1,GETDATE())";

			string commandText =
$@"DECLARE @PREVALENCECOUNT INT;
SET @PREVALENCECOUNT = ( SELECT [PrevalenceCount] FROM [{TableName}] WHERE {key.GetWhereClause()} )
SET @PREVALENCECOUNT = @PREVALENCECOUNT + 1;
IF(@PREVALENCECOUNT IS NOT NULL)
BEGIN
	UPDATE [{TableName}] SET [PrevalenceCount] = @PREVALENCECOUNT WHERE {key.GetWhereClause()}
	{updateText}
END
ELSE
BEGIN
	{insertStatement}
END
";
			return ExecNonQuery(commandText, sqlParameters);
		}

		private bool CheckIfTableExists()
		{
			string commandText =
$@"
IF EXISTS ( 
	SELECT [TABLE_SCHEMA] 
	FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = '{TableName}'
) 
( SELECT 1 )
ELSE
( SELECT 0 )
";

			object result = ExecuteScalar(commandText, null);
			if (result != null)
			{
				int resultValue = (int)result;
				return (resultValue == 1);
			}
			return false;
		}

		private bool CreateTable()
		{
			string commandText =
$@"
CREATE TABLE [{TableName}] (
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
);
";
			ExecNonQuery(commandText, null);

			return true;
		}

		private bool ExecNonQuery(string commandText, List<SqlParameter> sqlParameters)
		{
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand(commandText))
				{
					if (sqlParameters != null && sqlParameters.Any())
					{
						sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
					}
					bool result = SQLHelper.ExecuteNonQuery(_connectionString, sqlCommand);
					return result;
				}
			}
			catch (Exception ex)
			{
				SQLHelper.LogException(nameof(PersistFileProperties), commandText, ex);
			}

			return false;
		}

		private object ExecuteScalar(string commandText, IEnumerable<SqlParameter> sqlParameters)
		{
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand(commandText))
				{
					if (sqlParameters != null && sqlParameters.Any())
					{
						sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
					}
					object result = SQLHelper.ExecuteScalar(_connectionString, sqlCommand);
					return result;
				}
			}
			catch (Exception ex)
			{
				SQLHelper.LogException(nameof(PersistFileProperties), commandText, ex);
			}

			return null;
		}
	}
}
