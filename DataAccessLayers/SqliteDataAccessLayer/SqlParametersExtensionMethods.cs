using System;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

namespace SqliteDataAccessLayer
{
	public static class SqlParametersExtensionMethods
	{
		public static string AsColumnString(this IEnumerable<SQLiteParameter> source)
		{
			return $"[{string.Join("],[", source.Select(sqlParam => sqlParam.ParameterName.Replace("@", "")))}]";
		}

		public static string AsValuesString(this IEnumerable<SQLiteParameter> source)
		{
			return string.Join(",", source.Select(sqlParam => sqlParam.ParameterName));
		}

		public static string AsWhereString(this IEnumerable<SQLiteParameter> source)
		{
			return string.Join(" AND ", source.Select(sqlParam => $"[{sqlParam.ParameterName.Replace("@", "")}] = {sqlParam.ParameterName}"));
		}
	}
}
