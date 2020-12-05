using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;
using FilePropertiesDataObject.Helpers;

namespace SqliteDataAccessLayer
{
	public class SqliteDataClient : IDisposable
	{
		private string _dbFilePath;
		private static string _connectionString;
		private Lazy<SQLiteConnection> _connection = new Lazy<SQLiteConnection>(() => SqliteDataClient.OpenConnection(_connectionString));

		public SqliteDataClient(string dbFilePath)
		{
			_dbFilePath = dbFilePath;
			_connectionString = BuildConnectionString(_dbFilePath);

			PrepareDatabase();
		}

		public void Dispose()
		{
			if (_connection != null && _connection.IsValueCreated)
			{
				SQLiteConnection copy = _connection.Value;
				copy.Close();
				copy.Dispose();
				copy = null;
				_connection = null;
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

			string commandText = string.Format(SqlStrings.SelectPrevalenceCount, key.GetWhereClause());

			object result = ExecuteScalar(commandText, key.Parameters);
			if (result != null)
			{
				prevalenceCount = (int)result;
			}

			return prevalenceCount;
		}

		public void UpdatePrevalenceCount(SqlKey key, int newCount)
		{
			string commandText = string.Format(SqlStrings.UpdatePrevalenceCount, newCount, key.GetWhereClause());
			ExecuteNonQuery(commandText, key.Parameters);
		}

		public string GetExistingYaraRules(SqlKey key)
		{
			string commandText = string.Format(SqlStrings.SelectYaraRules, key.GetWhereClause());
			return (string)ExecuteScalar(commandText, key.Parameters) ?? "";
		}

		public void UpdateExistingYaraRule(SqlKey key, List<string> newYaraMatchedRules)
		{
			string newYaraRulesMatchedValue = YaraHelper.FormatDelimitedRulesString(newYaraMatchedRules);
			SQLiteParameter yaraMatchedRulesParameter = SqlHelper.GetParameter("YaraRulesMatched", newYaraRulesMatchedValue);

			List<SQLiteParameter> parameters = key.Parameters.ToList();
			parameters.Add(yaraMatchedRulesParameter);

			string commandText = string.Format(SqlStrings.UpdateYaraRules, yaraMatchedRulesParameter.ParameterName, key.GetWhereClause());
			ExecuteNonQuery(commandText, parameters);
		}

		public int ExecuteNonQuery(string command, List<SQLiteParameter> parameters = null)
		{
			return ExecuteNonQuery(_connection.Value, command, parameters);
		}

		public object ExecuteScalar(string command, List<SQLiteParameter> parameters = null)
		{
			return ExecuteScalar(_connection.Value, command, parameters);
		}

		private static int ExecuteNonQuery(SQLiteConnection connection, string command, List<SQLiteParameter> parameters = null)
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

		private static object ExecuteScalar(SQLiteConnection connection, string command, List<SQLiteParameter> parameters = null)
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

		private static string BuildConnectionString(string dbFilePath)
		{
			return $"Data Source=\"{dbFilePath}\";Version=3;";
		}




		public SqlKey GetTestSqlKeyObject()
		{
			return new SqlKey(33196, 2, "36C601D9AC7B26F47FCE9FAC47AE4B86C294EBBEE9378D286E52CC9EC6E56F69");
		}
	}
}
