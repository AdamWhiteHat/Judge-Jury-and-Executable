using System;
using System.Data;
using System.Data.SqlClient;

namespace SqlDataAccessLayer
{
    public static class ParameterHelper
    {
        public static SqlParameter GetNewStringParameter(string name, string value)
        {
            string safeValue = "";
            if (!string.IsNullOrWhiteSpace(value))
            {
                safeValue = value.Replace("'", "");
            }
            return new SqlParameter(string.Concat("@", name), SqlDbType.NVarChar, 250) { Value = safeValue, Direction = ParameterDirection.Input };
        }

        public static SqlParameter GetNewDateTimeParameter(string name, DateTime value)
        {
            return GetNewParameterByType(name, value, SqlDbType.DateTime2);
        }

        public static SqlParameter GetNewIntParameter(string name, int value)
        {
            return GetNewParameterByType(name, value, SqlDbType.Int);
        }

        public static SqlParameter GetNewUnsignedInt32Parameter(string name, uint value)
        {
            return GetNewParameterByType(name, value, SqlDbType.BigInt);
        }

        public static SqlParameter GetNewUnsignedInt16Parameter(string name, ushort value)
        {
            return GetNewParameterByType(name, value, SqlDbType.Int);
        }

        public static SqlParameter GetNewCharParameter(string name, char value)
        {
            return GetNewParameterByType(name, value, SqlDbType.Char);
        }

        public static SqlParameter GetNewULongParameter(string name, ulong value)
        {
            return GetNewParameterByType(name, value, SqlDbType.BigInt);
        }

        public static SqlParameter GetNewDecimalParameter(string name, ulong value)
        {
            return new SqlParameter(string.Concat("@", name), SqlDbType.Decimal) { Value = value, Direction = ParameterDirection.Input, Precision = 21 };
        }

        public static SqlParameter GetNewDoubleParameter(string name, double value)
        {
            return GetNewParameterByType(name, value, SqlDbType.Float);
        }

        public static SqlParameter GetNewBoolParameter(string name, bool value)
        {
            return GetNewParameterByType(name, value, SqlDbType.Bit);
        }

        public static SqlParameter GetNewParameterByType(string name, object value, SqlDbType type)
        {
            return new SqlParameter(string.Concat("@", name), type) { Value = value, Direction = ParameterDirection.Input };
        }
    }
}
