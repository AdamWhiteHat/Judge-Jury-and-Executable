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
		private static string TableName = "FileProperties";

		public SqlDataPersistenceLayer(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentException($"Argument {nameof(connectionString)} cannot be null, empty or whitespace.");
			}

			_connectionString = connectionString;
		}

		public void Dispose()
		{
		}

		public bool PersistFileProperties(FileProperties fileProperties)
		{
			SqlKey key = new SqlKey(fileProperties.MFTNumber, fileProperties.SequenceNumber, fileProperties.Sha256Hash);

			List<SqlParameter> sqlParameters = key.Parameters;
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

				ParameterHelper.GetParameter("Attributes", fileProperties.Attributes?.ToString() ?? ""),

				ParameterHelper.GetParameter("SHA1",fileProperties.PeData?.SHA1Hash ?? ""),
				ParameterHelper.GetParameter("MD5", fileProperties.PeData?.MD5Hash ?? ""),
				ParameterHelper.GetParameter("ImpHash", fileProperties.PeData?.ImpHash ?? ""),
				ParameterHelper.GetParameter("IsDll", fileProperties.PeData?.IsDll ?? false),
				ParameterHelper.GetParameter("IsExe", fileProperties.PeData?.IsExe ?? false),
				ParameterHelper.GetParameter("IsDriver", fileProperties.PeData?.IsDriver ?? false),
				ParameterHelper.GetParameter("IsSigned", fileProperties.PeData?.IsSigned ?? false),
				ParameterHelper.GetParameter("IsSignatureValid", fileProperties.PeData?.IsSignatureValid ?? false),
				ParameterHelper.GetParameter("IsValidCertChain", fileProperties.PeData?.IsValidCertChain ?? false),
				ParameterHelper.GetNewParameterByType("BinaryType", (object)fileProperties.PeData?.BinaryType ??  DBNull.Value, SqlDbType.Int),
				ParameterHelper.GetNewParameterByType("CompileDate", (object)fileProperties.PeData?.CompileDate ??  DBNull.Value, SqlDbType.DateTime2),
				ParameterHelper.GetParameter("IsTrusted", fileProperties.IsTrusted),

				ParameterHelper.GetParameter("CertSubject", fileProperties.Authenticode?.CertSubject ?? ""),
				ParameterHelper.GetParameter("CertIssuer", fileProperties.Authenticode?.CertIssuer ?? ""),
				ParameterHelper.GetParameter("CertSerialNumber", fileProperties.Authenticode?.CertSerialNumber ?? ""),
				ParameterHelper.GetParameter("CertThumbprint", fileProperties.Authenticode?.CertThumbprint ?? ""),
				ParameterHelper.GetParameter("CertNotBefore", fileProperties.Authenticode?.CertNotBefore ?? ""),
				ParameterHelper.GetParameter("CertNotAfter", fileProperties.Authenticode?.CertNotAfter ?? ""),

				ParameterHelper.GetParameter("Entropy", fileProperties.Entropy ?? 0)
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

			if (fileProperties.IsYaraRulesMatchedPopulated)
			{
				List<string> newYaraMatchedRules = fileProperties.YaraRulesMatched.ToList();

				string currentYaraRulesMatchedValue = GetExistingYaraRules(key);
				if (currentYaraRulesMatchedValue != null)
				{
					newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(currentYaraRulesMatchedValue));
				}

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

		private bool ExecNonQuery(string commandText, List<SqlParameter> sqlParameters)
		{
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand(commandText))
				{
					sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
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

		private object ExecuteScalar(string commandText, List<SqlParameter> sqlParameters)
		{
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand(commandText))
				{
					sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
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
