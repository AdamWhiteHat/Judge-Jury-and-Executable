using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Collections;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace DataAccessLayer
{
	public static class SQLHelper
	{
		public static Action<string, string, Exception> LogExceptionAction = new Action<string, string, Exception>((msg, ct, ex) => { File.AppendAllText("Exceptions.log", msg + Environment.NewLine + ex?.Message ?? ""); });
		public static readonly string AllowedCharacters = "/()_.-:! abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@";

		public static void SetLogExceptionAction(Action<string, string, Exception> exceptionAction)
		{
			LogExceptionAction = exceptionAction;
		}

		public static void LogException(string message, string commandText, Exception exception)
		{
			if (LogExceptionAction != null)
			{
				LogExceptionAction.Invoke(message, commandText, exception);
			}
		}

		public static bool CheckIfExists(string connectionString, char objectType, string objectName)
		{
			string commandText = string.Empty;
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand())
				{
					sqlCommand.Parameters.Add(new SqlParameter("@ObjectType", SqlDbType.Char, 1) { Value = objectType });
					sqlCommand.Parameters.Add(new SqlParameter("@ObjectName", SqlDbType.NVarChar, 128) { Value = objectName });

					commandText = $@"IF EXISTS(SELECT * FROM sys.objects WHERE type = @ObjectType AND name = @ObjectName)
													SELECT 1
												ELSE
													SELECT 0";

					sqlCommand.CommandText = commandText;

					object scalar = ExecuteScalar(connectionString, sqlCommand);
					if (scalar != null && ((int)scalar) == 1)
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				LogException(nameof(CheckIfExists), commandText, ex);
			}

			return false;
		}

		public static bool ExecuteNonQuery(string connectionString, SqlCommand sqlCommand)
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;


					// Returns how many rows affected. Should be 1.
					int result = sqlCommand.ExecuteNonQuery();
					if (result == 1)
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				LogException(nameof(ExecuteNonQuery), sqlCommand.CommandText, ex);
			}

			return false;
		}

		public static object ExecuteScalar(string connectionString, SqlCommand sqlCommand)
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;

					return sqlCommand.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				LogException(nameof(ExecuteScalar), sqlCommand.CommandText, ex);
			}

			return null;
		}

		public static DataTable ExecuteQuery(string connectionString, SqlCommand sqlCommand)
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;

					using (SqlDataReader reader = sqlCommand.ExecuteReader())
					{
						if (reader.HasRows)
						{
							using (DataTable result = new DataTable())
							{
								result.Load(reader);
								return result;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogException(nameof(ExecuteQuery), sqlCommand.CommandText, ex);
			}

			return null;
		}

		public static object ReadDataColumn(DataTable table, string columnName)
		{
			if (!IsValidDataTable(table)
				|| !table.Columns.Contains(columnName)
				|| table.Rows[0].IsNull(columnName))
			{
				return null;
			}
			else
			{
				return table.Rows[0][columnName];
			}
		}

		public static Nullable<T> ReadDataColumn<T>(DataTable table, string columnName) where T : struct
		{
			try
			{
				object dataValue = ReadDataColumn(table, columnName);
				if (dataValue != null)
				{
					return (T)Convert.ChangeType(dataValue, typeof(T));
				}
			}
			catch (Exception ex)
			{
				LogException(nameof(ReadDataColumn), "", ex);
			}

			return null;
		}

		public static bool IsValidDataTable(DataTable table)
		{
			if (table == null || !table.IsInitialized || table.Rows == null || table.Columns == null)
			{
				return false;
			}
			if (table.Rows.Count < 1 || table.Columns.Count < 1)
			{
				return false;
			}

			return true;
		}

		public static bool ContainsInvalidCharacters(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return true;
			}
			var boolInvalidChars = false;
			boolInvalidChars = !input.All(chr => AllowedCharacters.Contains(chr));
			if (!boolInvalidChars)
			{
				if (input.Contains("..")) { boolInvalidChars = true; }
				if (input.Contains("//")) { boolInvalidChars = true; }
			}
			return boolInvalidChars;
		}

		public static string SanitizeString(string dirtyInput)
		{
			if (string.IsNullOrWhiteSpace(dirtyInput))
			{
				return string.Empty;
			}

			IEnumerable<char> stripped = dirtyInput.Where(chr => AllowedCharacters.Contains(chr));
			return new string(stripped.ToArray());
		}
	}
}