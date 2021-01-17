using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Helpers;

namespace SqliteDataAccessLayer
{
	public class SqliteDataPersistenceLayer : IDataPersistenceLayer
	{
		private string _dbFilePath;
		private SqliteDataClient _dataClient;

		public SqliteDataPersistenceLayer(string dbFilePath)
		{
			if (string.IsNullOrWhiteSpace(dbFilePath))
			{
				throw new ArgumentException($"Argument {nameof(dbFilePath)} cannot be null, empty or whitespace.");
			}
			_dbFilePath = dbFilePath;
			_dataClient = new SqliteDataClient(_dbFilePath);
		}

		public void Dispose()
		{
			_dataClient.Dispose();
			_dataClient = null;
		}

		public bool PersistFileProperties(FileProperties fileProperties)
		{
			SqlKey key = new SqlKey(fileProperties.MFTNumber, fileProperties.SequenceNumber, fileProperties.Sha256Hash);

			List<SQLiteParameter> sqlParameters = new List<SQLiteParameter>();
			sqlParameters.AddRange(key.Parameters);
			sqlParameters.AddRange(new List<SQLiteParameter>
			{
				SqlHelper.GetParameter("DriveLetter", fileProperties.DriveLetter),
				SqlHelper.GetParameter("FullPath",fileProperties.FullPath),
				SqlHelper.GetParameter("Filename", fileProperties.FileName),
				SqlHelper.GetParameter("Extension", fileProperties.Extension),
				SqlHelper.GetParameter("DirectoryLocation", fileProperties.DirectoryLocation),
				SqlHelper.GetParameter("Length", fileProperties.Length),

				SqlHelper.GetParameter("MftTimeCreation", fileProperties.MftTimeCreation),
				SqlHelper.GetParameter("MftTimeAccessed", fileProperties.MftTimeAccessed),
				SqlHelper.GetParameter("MftTimeModified", fileProperties.MftTimeModified),
				SqlHelper.GetParameter("MftTimeMftModified", fileProperties.MftTimeMftModified),
				SqlHelper.GetParameter("CreationTime", fileProperties.CreationTime),
				SqlHelper.GetParameter("LastAccessTime", fileProperties.LastAccessTime),
				SqlHelper.GetParameter("LastWriteTime", fileProperties.LastWriteTime),

				SqlHelper.GetParameter("Project", fileProperties.Project),
				SqlHelper.GetParameter("ProviderItemID", fileProperties.ProviderItemID),
				SqlHelper.GetParameter("OriginalFileName", fileProperties.OriginalFileName),
				SqlHelper.GetParameter("FileOwner", fileProperties.FileOwner),
				SqlHelper.GetParameter("FileVersion", fileProperties.FileVersion),
				SqlHelper.GetParameter("FileDescription", fileProperties.FileDescription),
				SqlHelper.GetParameter("Trademarks", fileProperties.Trademarks),
				SqlHelper.GetParameter("Copyright", fileProperties.Copyright),
				SqlHelper.GetParameter("Company", fileProperties.Company),
				SqlHelper.GetParameter("ApplicationName", fileProperties.ApplicationName),
				SqlHelper.GetParameter("Comment", fileProperties.Comment),
				SqlHelper.GetParameter("Title", fileProperties.Title),
				SqlHelper.GetParameter("Link", fileProperties.Link),

				SqlHelper.GetParameter("MimeType", fileProperties.MimeType),
				SqlHelper.GetParameter("InternalName", fileProperties.InternalName),
				SqlHelper.GetParameter("ProductName", fileProperties.ProductName),
				SqlHelper.GetParameter("Language", fileProperties.Language),
				SqlHelper.GetParameter("ComputerName", fileProperties.ComputerName),

				SqlHelper.GetParameter("Attributes", fileProperties.Attributes?.ToString() ?? ""),

				SqlHelper.GetParameter("SHA1",fileProperties.PeData?.SHA1Hash ?? ""),
				SqlHelper.GetParameter("MD5", fileProperties.PeData?.MD5Hash ?? ""),
				SqlHelper.GetParameter("ImpHash", fileProperties.PeData?.ImpHash ?? ""),
				SqlHelper.GetParameter("IsDll", fileProperties.PeData?.IsDll ?? false),
				SqlHelper.GetParameter("IsExe", fileProperties.PeData?.IsExe ?? false),
				SqlHelper.GetParameter("IsDriver", fileProperties.PeData?.IsDriver ?? false),
				SqlHelper.GetParameter("IsSigned", fileProperties.PeData?.IsSigned ?? false),
				SqlHelper.GetParameter("IsSignatureValid", fileProperties.PeData?.IsSignatureValid ?? false),
				SqlHelper.GetParameter("IsValidCertChain", fileProperties.PeData?.IsValidCertChain ?? false),
				SqlHelper.GetNewParameterByType("BinaryType", (object)fileProperties.PeData?.BinaryType ??  DBNull.Value, DbType.Int32),
				SqlHelper.GetNewParameterByType("CompileDate", (object)fileProperties.PeData?.CompileDate ??  DBNull.Value, DbType.DateTime2),
				SqlHelper.GetParameter("IsTrusted", fileProperties.IsTrusted),

				SqlHelper.GetParameter("CertSubject", fileProperties.Authenticode?.CertSubject ?? ""),
				SqlHelper.GetParameter("CertIssuer", fileProperties.Authenticode?.CertIssuer ?? ""),
				SqlHelper.GetParameter("CertSerialNumber", fileProperties.Authenticode?.CertSerialNumber ?? ""),
				SqlHelper.GetParameter("CertThumbprint", fileProperties.Authenticode?.CertThumbprint ?? ""),
				SqlHelper.GetParameter("CertNotBefore", fileProperties.Authenticode?.CertNotBefore ?? ""),
				SqlHelper.GetParameter("CertNotAfter", fileProperties.Authenticode?.CertNotAfter ?? ""),

				SqlHelper.GetParameter("Entropy", fileProperties.Entropy ?? 0)
			});

			int count = _dataClient.GetPrevalenceCount(key);
			if (count == -1)
			{
				_dataClient.InsertRow(sqlParameters);
				return true;
			}

			count += 1;

			if (fileProperties.IsYaraRulesMatchedPopulated)
			{
				List<string> newYaraMatchedRules = fileProperties.YaraRulesMatched.ToList();

				string currentYaraRulesMatchedValue = _dataClient.GetExistingYaraRules(key);
				if (currentYaraRulesMatchedValue != null)
				{
					newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(currentYaraRulesMatchedValue));

					_dataClient.UpdateExistingYaraRule(key, newYaraMatchedRules);
				}
			}

			_dataClient.UpdatePrevalenceCount(key, count);

			return true;
		}
	}
}
