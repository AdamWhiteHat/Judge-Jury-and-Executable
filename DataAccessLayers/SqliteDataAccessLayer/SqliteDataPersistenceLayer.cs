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
			SqlKey key = new SqlKey(fileProperties.MFTNumber, fileProperties.SequenceNumber, fileProperties.Sha256);

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

				SqlHelper.GetParameter("Attributes", fileProperties.Attributes),

				SqlHelper.GetParameter("SHA1",fileProperties.SHA1),
				SqlHelper.GetParameter("MD5", fileProperties.MD5),
				SqlHelper.GetParameter("ImpHash", fileProperties.ImpHash),
				SqlHelper.GetParameter("IsDll", fileProperties.IsDll),
				SqlHelper.GetParameter("IsExe", fileProperties.IsExe),
				SqlHelper.GetParameter("IsDriver", fileProperties.IsDriver),
				SqlHelper.GetParameter("IsSigned", fileProperties.IsSigned),
				SqlHelper.GetParameter("IsSignatureValid", fileProperties.IsSignatureValid ),
				SqlHelper.GetParameter("IsValidCertChain", fileProperties.IsValidCertChain),
				SqlHelper.GetNewParameterByType("BinaryType", fileProperties.BinaryType.GetValueOrDefault(), DbType.Int32),
				SqlHelper.GetNewParameterByType("CompileDate", fileProperties.CompileDate.GetValueOrDefault(), DbType.DateTime2),
				SqlHelper.GetParameter("IsTrusted", fileProperties.IsTrusted),

				SqlHelper.GetParameter("CertSubject", fileProperties.CertSubject),
				SqlHelper.GetParameter("CertIssuer", fileProperties.CertIssuer),
				SqlHelper.GetParameter("CertSerialNumber", fileProperties.CertSerialNumber ),
				SqlHelper.GetParameter("CertThumbprint", fileProperties.CertThumbprint ),
				SqlHelper.GetParameter("CertNotBefore", fileProperties.CertNotBefore ),
				SqlHelper.GetParameter("CertNotAfter", fileProperties.CertNotAfter),

				SqlHelper.GetParameter("Entropy", fileProperties.Entropy ?? 0)
			});

			int count = _dataClient.GetPrevalenceCount(key);
			if (count == -1)
			{
				_dataClient.InsertRow(sqlParameters);
				return true;
			}

			count += 1;

			if (!string.IsNullOrWhiteSpace(fileProperties.YaraMatchedRules))
			{
				List<string> newYaraMatchedRules = new List<string>();

				string currentYaraRulesMatchedValue = _dataClient.GetExistingYaraRules(key);
				if (currentYaraRulesMatchedValue != null)
				{
					newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(currentYaraRulesMatchedValue));
				}
				newYaraMatchedRules.AddRange(YaraHelper.ParseDelimitedRulesString(fileProperties.YaraMatchedRules));

				_dataClient.UpdateExistingYaraRule(key, newYaraMatchedRules);
			}

			_dataClient.UpdatePrevalenceCount(key, count);

			return true;
		}
	}
}
