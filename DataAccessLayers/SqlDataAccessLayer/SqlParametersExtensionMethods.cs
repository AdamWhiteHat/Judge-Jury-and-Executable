using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlDataAccessLayer
{
	public static class SqlParametersExtensionMethods
	{
		public static string AsColumnString(this IEnumerable<SqlParameter> source)
		{
			return $"[{string.Join("],[", source.Select(sqlParam => sqlParam.ParameterName.Replace("@", "")))}]";
		}

		public static string AsValuesString(this IEnumerable<SqlParameter> source)
		{
			return string.Join(",", source.Select(sqlParam => sqlParam.ParameterName));
		}

		public static string AsWhereString(this IEnumerable<SqlParameter> source)
		{
			return string.Join(" AND ", source.Select(sqlParam => $"[{sqlParam.ParameterName.Replace("@", "")}] = {sqlParam.ParameterName}"));
		}
	}
}
