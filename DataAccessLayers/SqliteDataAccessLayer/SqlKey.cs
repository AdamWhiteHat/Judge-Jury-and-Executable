using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;

namespace SqliteDataAccessLayer
{
	public class SqlKey
	{
		public uint MFTNumber { get; set; }
		public ushort SequenceNumber { get; set; }
		public string SHA256 { get; set; }

		public List<SQLiteParameter> Parameters { get; private set; }

		public SqlKey(uint mftNumber, ushort sequenceNumber, string sha256)
		{
			MFTNumber = mftNumber;
			SequenceNumber = sequenceNumber;
			SHA256 = sha256;
			Parameters = new List<SQLiteParameter>
			{
				SqlHelper.GetNewParameterByType("MFTNumber",MFTNumber, DbType.UInt32),
				SqlHelper.GetNewParameterByType("SequenceNumber",SequenceNumber, DbType.UInt16),
				SqlHelper.GetNewParameterByType("SHA256",SHA256, DbType.String)
			};
		}

		public string GetColumnsString() => Parameters.AsColumnString();
		public string GetValuesString() => Parameters.AsValuesString();
		public string GetWhereClause() => Parameters.AsWhereString();
	}
}
