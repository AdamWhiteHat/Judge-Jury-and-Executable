using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using FilePropertiesDataObject.Helpers;

namespace SqliteDataAccessLayer
{
	public class SqliteDataClient : IDisposable
	{
		private string _dbFilePath;
		private static string _connectionString;
		private Lazy<SQLiteConnection> _connection = new Lazy<SQLiteConnection>(() => OpenConnection(_connectionString));

		public SqliteDataClient(string dbFilePath)
		{
			_dbFilePath = dbFilePath;
			_connectionString = BuildConnectionString(_dbFilePath);

			PrepareDatabase();
		}

		private static string BuildConnectionString(string dbFilePath)
		{
			return $"Data Source=\"{dbFilePath}\";Version=3;";
		}

		private void PrepareDatabase()
		{
			if (!File.Exists(_dbFilePath))
			{
				SQLiteConnection.CreateFile(_dbFilePath);

				try
				{
					using (SQLiteConnection conn = SqliteDataClient.OpenConnection(_connectionString))
					{
						ExecuteNonQuery(conn, SqlStrings.CreateTable);
					}
				}
				catch
				{
					File.Delete(_dbFilePath);
				}
			}
		}

		private static SQLiteConnection OpenConnection(string connectionString)
		{
			SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);

			sqliteConnection.Open();

			return sqliteConnection;
		}

		public void Dispose()
		{
			if (_connection != null && _connection.IsValueCreated)
			{
				SQLiteConnection copy = _connection.Value;
				_connection = null;
				copy.Close();
				copy.Dispose();
				copy = null;
			}
		}

		public int InsertRow(List<SQLiteParameter> sqlParameters)
		{
			string commandText = string.Format(SqlStrings.InsertInto, sqlParameters.AsColumnString(), sqlParameters.AsValuesString());
			return ExecuteNonQuery(commandText, sqlParameters);
		}

		public int GetPrevalenceCount(SqlKey key)
		{
			int prevalenceCount = -1;

			object result = ExecuteScalar(SqlStrings.SelectPrevalenceCount, key.GetSqlParameters());
			if (result != null)
			{
				prevalenceCount = (int)result;
			}

			return prevalenceCount;
		}

		public void UpdatePrevalenceCount(SqlKey key, int newCount)
		{
			string commandText = string.Format(SqlStrings.UpdatePrevalenceCount, newCount);
			ExecuteNonQuery(commandText, key.GetSqlParameters());
		}

		public string GetExistingYaraRules(SqlKey key)
		{
			return (string)ExecuteScalar(SqlStrings.SelectYaraRules, key.GetSqlParameters()) ?? "";
		}

		public void UpdateExistingYaraRule(SqlKey key, List<string> newYaraMatchedRules)
		{
			string newYaraRulesMatchedValue = YaraHelper.FormatDelimitedRulesString(newYaraMatchedRules);
			SQLiteParameter yaraMatchedRulesParameter = SqlHelper.GetParameter("YaraRulesMatched", newYaraRulesMatchedValue);

			List<SQLiteParameter> parameters = key.GetSqlParameters().ToList();
			parameters.Add(yaraMatchedRulesParameter);

			string commandText = string.Format(SqlStrings.UpdateYaraRules, yaraMatchedRulesParameter.ParameterName);
			ExecuteNonQuery(commandText, parameters);
		}

		public int ExecuteNonQuery(string command, IEnumerable<SQLiteParameter> parameters = null)
		{
			return ExecuteNonQuery(_connection.Value, command, parameters);
		}

		public object ExecuteScalar(string command, IEnumerable<SQLiteParameter> parameters = null)
		{
			return ExecuteScalar(_connection.Value, command, parameters);
		}

		private static int ExecuteNonQuery(SQLiteConnection connection, string command, IEnumerable<SQLiteParameter> parameters = null)
		{
			int result = -1;
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = command;
				if (parameters != null && parameters.Any())
				{
					cmd.Parameters.AddRange(parameters.ToArray());
				}
				result = cmd.ExecuteNonQuery();
			}
			return result;
		}

		private static object ExecuteScalar(SQLiteConnection connection, string command, IEnumerable<SQLiteParameter> parameters = null)
		{
			object result = null;
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = command;
				if (parameters != null && parameters.Any())
				{
					cmd.Parameters.AddRange(parameters.ToArray());
				}
				result = cmd.ExecuteScalar();
			}
			return result;
		}
	}
}
