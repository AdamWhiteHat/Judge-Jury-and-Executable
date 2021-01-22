using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlDataAccessLayer
{
	public static class ParameterHelper
	{
		public static SqlParameter GetParameter<T>(string name, T value)
		{
			SqlDbType dbType = _typeToDbtypeDictionary[typeof(T)];
			if (dbType == SqlDbType.NVarChar)
			{
				return GetNewStringParameter(name, value as string);
			}
			return GetNewParameterByType(name, value, dbType);
		}

		public static SqlParameter GetNewStringParameter(string name, string value)
		{
			string safeValue = "";
			if (!string.IsNullOrWhiteSpace(value))
			{
				safeValue = value.Replace("'", "");
			}
			return new SqlParameter(string.Concat("@", name), SqlDbType.NVarChar, 250) { Value = safeValue, Direction = ParameterDirection.Input };
		}

		public static SqlParameter GetNewParameterByType<T>(string name, T value, SqlDbType type)
		{
			object parameterValue = (value != null) ? (object)value : DBNull.Value;
			return new SqlParameter(string.Concat("@", name), type) { Value = parameterValue, Direction = ParameterDirection.Input };
		}

		private static Dictionary<Type, SqlDbType> _typeToDbtypeDictionary = new Dictionary<Type, SqlDbType>
		{
			{typeof(string), SqlDbType.NVarChar},
			{typeof(bool), SqlDbType.Bit},
			{typeof(char), SqlDbType.Char},
			{typeof(ushort), SqlDbType.Int},
			{typeof(int), SqlDbType.Int},
			{typeof(uint), SqlDbType.BigInt},
			{typeof(ulong), SqlDbType.BigInt},
			{typeof(double), SqlDbType.Float},
			{typeof(decimal), SqlDbType.Decimal },
			{typeof(DateTime),  SqlDbType.DateTime2},
		};
	}
}
