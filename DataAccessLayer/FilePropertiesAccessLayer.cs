using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Collections;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataAccessLayer
{
	using FilePropertiesDataObject;

	public class FilePropertiesAccessLayer
	{
		private static string ConnectionString;

		public static void SetConnectionString(string connectionString)
		{
			if (!string.IsNullOrWhiteSpace(connectionString))
			{
				ConnectionString = connectionString;
			}
		}

		public static bool InsertFileProperties(FileProperties fileProperties)
		{
			SqlParameter parameterSha256 = ParameterHelper.GetNewStringParameter("SHA256", fileProperties.Sha256Hash);
			SqlParameter parameterMFTNumber = ParameterHelper.GetNewUnsignedInt32Parameter("MFTNumber", fileProperties.MFTNumber);
			SqlParameter parameterSequenceNumber = ParameterHelper.GetNewUnsignedInt16Parameter("SequenceNumber", fileProperties.SequenceNumber);

			List<SqlParameter> sqlParameters = new List<SqlParameter>
			{
				parameterSha256,
				parameterMFTNumber,
				parameterSequenceNumber,
				ParameterHelper.GetNewCharParameter("DriveLetter", fileProperties.DriveLetter),
				ParameterHelper.GetNewStringParameter("FullPath",fileProperties.FullPath),
				ParameterHelper.GetNewStringParameter("Filename", fileProperties.FileName),
				ParameterHelper.GetNewStringParameter("Extension", fileProperties.Extension),
				ParameterHelper.GetNewStringParameter("DirectoryLocation", fileProperties.DirectoryLocation),
				ParameterHelper.GetNewULongParameter("Length", fileProperties.Length),

				ParameterHelper.GetNewDateTimeParameter("MftTimeCreation", fileProperties.MftTimeCreation),
				ParameterHelper.GetNewDateTimeParameter("MftTimeAccessed", fileProperties.MftTimeAccessed),
				ParameterHelper.GetNewDateTimeParameter("MftTimeModified", fileProperties.MftTimeModified),
				ParameterHelper.GetNewDateTimeParameter("MftTimeMftModified", fileProperties.MftTimeMftModified),
				ParameterHelper.GetNewDateTimeParameter("CreationTime", fileProperties.CreationTime),
				ParameterHelper.GetNewDateTimeParameter("LastAccessTime", fileProperties.LastAccessTime),
				ParameterHelper.GetNewDateTimeParameter("LastWriteTime", fileProperties.LastWriteTime),

				ParameterHelper.GetNewStringParameter("Project", fileProperties.Project),
				ParameterHelper.GetNewStringParameter("ProviderItemID", fileProperties.ProviderItemID),
				ParameterHelper.GetNewStringParameter("OriginalFileName", fileProperties.OriginalFileName),
				ParameterHelper.GetNewStringParameter("FileOwner", fileProperties.FileOwner),
				ParameterHelper.GetNewStringParameter("FileVersion", fileProperties.FileVersion),
				ParameterHelper.GetNewStringParameter("FileDescription", fileProperties.FileDescription),
				ParameterHelper.GetNewStringParameter("Trademarks", fileProperties.Trademarks),
				ParameterHelper.GetNewStringParameter("Copyright", fileProperties.Copyright),
				ParameterHelper.GetNewStringParameter("Company", fileProperties.Company),
				ParameterHelper.GetNewStringParameter("ApplicationName", fileProperties.ApplicationName),
				ParameterHelper.GetNewStringParameter("Comment", fileProperties.Comment),
				ParameterHelper.GetNewStringParameter("Title", fileProperties.Title),
				ParameterHelper.GetNewStringParameter("Link", fileProperties.Link),

				ParameterHelper.GetNewStringParameter("ContentType", fileProperties.ContentType),
				ParameterHelper.GetNewStringParameter("InternalName", fileProperties.InternalName),
				ParameterHelper.GetNewStringParameter("ProductName", fileProperties.ProductName),
				ParameterHelper.GetNewStringParameter("Language", fileProperties.Language),
				ParameterHelper.GetNewStringParameter("ComputerName", fileProperties.ComputerName),

				ParameterHelper.GetNewStringParameter("Attributes", fileProperties.Attributes?.ToString() ?? "")
			};

			if (fileProperties.IsPeDataPopulated)
			{
				sqlParameters.AddRange(new List<SqlParameter> {
					ParameterHelper.GetNewStringParameter("SHA1",fileProperties.PeData?.SHA1Hash ?? ""),
					ParameterHelper.GetNewStringParameter("MD5", fileProperties.PeData?.MD5Hash ?? ""),
					ParameterHelper.GetNewStringParameter("ImpHash", fileProperties.PeData?.ImpHash ?? ""),
					ParameterHelper.GetNewParameterByType("IsDll", (object)fileProperties.PeData?.IsDll ?? DBNull.Value, SqlDbType.Bit),
					ParameterHelper.GetNewParameterByType("IsPe", (object)fileProperties.PeData?.IsPe ?? DBNull.Value, SqlDbType.Bit),
					ParameterHelper.GetNewParameterByType("IsDriver", (object)fileProperties.PeData?.IsDriver ?? DBNull.Value, SqlDbType.Bit),
					ParameterHelper.GetNewParameterByType("IsSigned", (object)fileProperties.PeData?.IsSigned ?? DBNull.Value, SqlDbType.Bit),
					ParameterHelper.GetNewParameterByType("IsValidCertChain", (object)fileProperties.PeData?.IsValidCertChain ?? DBNull.Value, SqlDbType.Bit),
					ParameterHelper.GetNewParameterByType("BinaryType", (object)fileProperties.PeData?.BinaryType ?? DBNull.Value, SqlDbType.Int),
					ParameterHelper.GetNewParameterByType("CompileDate", (object)fileProperties.PeData?.CompileDate ?? DBNull.Value, SqlDbType.DateTime2)
				});
			}

			if (fileProperties.IsAuthenticodePopulated)
			{
				sqlParameters.AddRange(new List<SqlParameter>
				{
					ParameterHelper.GetNewStringParameter("CertSubject", fileProperties.Authenticode?.CertSubject ?? ""),
					ParameterHelper.GetNewStringParameter("CertIssuer", fileProperties.Authenticode?.CertIssuer ?? ""),
					ParameterHelper.GetNewStringParameter("CertSerialNumber", fileProperties.Authenticode?.CertSerialNumber ?? ""),
					ParameterHelper.GetNewStringParameter("CertThumbprint", fileProperties.Authenticode?.CertThumbprint ?? ""),
					ParameterHelper.GetNewStringParameter("CertNotBefore", fileProperties.Authenticode?.CertNotBefore ?? ""),
					ParameterHelper.GetNewStringParameter("CertNotAfter", fileProperties.Authenticode?.CertNotAfter ?? ""),
				});
			}

			if (fileProperties.IsEntropyPopulated)
			{
				sqlParameters.Add(ParameterHelper.GetNewDoubleParameter("Entropy", fileProperties.Entropy ?? 0));
			}

			if (fileProperties.IsYaraRulesMatchedPopulated)
			{
				sqlParameters.Add(ParameterHelper.GetNewStringParameter("YaraRulesMatched", fileProperties.YaraRulesMatched ?? ""));
			}

			string columnNames = "[" + string.Join("],[", sqlParameters.Select(param => param.ParameterName.Replace("@", ""))) + "]";
			string values = string.Join(",", sqlParameters.Select(param => param.ParameterName));

			string insertStatement = $"INSERT INTO [FileProperties]	({columnNames},[PrevalenceCount],[DateSeen]) VALUES ({values},1,GETDATE())";

			string whereClause = $"WHERE [MFTNumber] = {parameterMFTNumber.Value} AND [SequenceNumber] = {parameterSequenceNumber.Value} AND [SHA256] = '{parameterSha256.Value}'";

			string commandText =
$@"DECLARE @PREVALENCECOUNT INT;
SET @PREVALENCECOUNT = ( SELECT [PrevalenceCount] FROM [FileProperties] {whereClause} )
SET @PREVALENCECOUNT = @PREVALENCECOUNT + 1;
IF(@PREVALENCECOUNT IS NOT NULL)
BEGIN
	UPDATE [FileProperties] SET [PrevalenceCount] = @PREVALENCECOUNT {whereClause}
END
ELSE
BEGIN
	{insertStatement}
END
";
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand(commandText))
				{
					sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
					return SQLHelper.ExecuteNonQuery(ConnectionString, sqlCommand);
				}
			}
			catch (Exception ex)
			{
				SQLHelper.LogException(nameof(InsertFileProperties), commandText, ex);
			}

			return false;
		}
	}
}
