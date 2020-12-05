using System;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

namespace SqliteDataAccessLayer
{
	public static class SqlParametersExtensionMethods
	{
		public static string AsColumnString(this List<SQLiteParameter> source)
		{
			return $"[{string.Join("],[", source.Select(param => param.ParameterName.Replace("@", "")))}]";
		}

		public static string AsValuesString(this List<SQLiteParameter> source)
		{
			return string.Join(",", source.Select(param => param.ParameterName));
		}

		public static string AsWhereString(this List<SQLiteParameter> source)
		{
			return string.Join(" AND ", source.Select(param => $"[{param.ParameterName.Replace("@", "")}] = {param.ParameterName}"));
		}
	}
}
