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
		private Lazy<SQLiteConnection> _connection = new Lazy<SQLiteConnection>(() => OpenConnection(_connectionString));

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

			// TEMP_STORE - DEFAULT (0), FILE (1), MEMORY (2)
			// When TEMP_STORE is MEMORY (2), temporary tables and indices are kept in as if they were pure in-memory databases memory.
			ExecuteNonQuery(sqliteConnection, "PRAGMA TEMP_STORE = 2");

			// SYNCHRONOUS - EXTRA (3), FULL (2), NORMAL (1), and OFF (0)
			// When SYNCHRONOUS is NORMAL (1), the SQLite database engine will still sync at the most critical moments, but less often than in FULL mode.
			ExecuteNonQuery(sqliteConnection, "PRAGMA SYNCHRONOUS = 1");

			// JOURNAL_MODE - DELETE | TRUNCATE | PERSIST | MEMORY | WAL | OFF
			// When JOURNAL_MODE is MEMORY, journaling mode stores the rollback journal in volatile RAM. If SQLite crashes in the middle of a transaction, then the database file will very likely go corrupt.
			ExecuteNonQuery(sqliteConnection, "PRAGMA JOURNAL_MODE = MEMORY");

			// LOCKING_MODE - NORMAL | EXCLUSIVE
			// When LOCKING_MODE is EXCLUSIVE, the database file is used in exclusive mode. The number of system calls to implement file operations decreases in this case, which increases database performance.
			ExecuteNonQuery(sqliteConnection, "PRAGMA LOCKING_MODE = EXCLUSIVE");

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
