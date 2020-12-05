using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlDataAccessLayer
{
	public static class SqlParametersExtensionMethods
	{
		public static string AsColumnString(this List<SqlParameter> source)
		{
			return $"[{string.Join("],[", source.Select(param => param.ParameterName.Replace("@", "")))}]";
		}

		public static string AsValuesString(this List<SqlParameter> source)
		{
			return string.Join(",", source.Select(param => param.ParameterName));
		}

		public static string AsWhereString(this List<SqlParameter> source)
		{
			return string.Join(" AND ", source.Select(param => $"[{param.ParameterName.Replace("@", "")}] = {param.ParameterName}"));
		}
	}
}
