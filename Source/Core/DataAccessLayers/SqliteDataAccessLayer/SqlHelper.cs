using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;

namespace SqliteDataAccessLayer
{
	public static class SqlHelper
	{
		public static SQLiteParameter GetParameter<T>(string name, T value)
		{
			DbType dbType = _typeToDbtypeDictionary[typeof(T)];
			return GetNewParameterByType(name, value, dbType);
		}

		public static SQLiteParameter GetNewParameterByType<T>(string name, T value, DbType type)
		{
			object parameterValue = (value != null) ? (object)value : DBNull.Value;
			return new SQLiteParameter(string.Concat("@", name), type) { Value = parameterValue, Direction = ParameterDirection.Input };
		}

		private static Dictionary<Type, DbType> _typeToDbtypeDictionary = new Dictionary<Type, DbType>
		{
			{typeof(bool), DbType.Boolean},
			{typeof(byte),  DbType.Byte},
			{typeof(char), DbType.UInt16},
			{typeof(ushort), DbType.UInt16},
			{typeof(uint), DbType.UInt32},
			{typeof(ulong), DbType.UInt64},
			{typeof(short), DbType.Int16},
			{typeof(int), DbType.Int32},
			{typeof(long), DbType.Int64},
			{typeof(double), DbType.Double},
			{typeof(decimal), DbType.Decimal },
			{typeof(DateTime),  DbType.DateTime2},
			{typeof(string),  DbType.String},
			{typeof(Guid),  DbType.Guid}
		};
	}
}
