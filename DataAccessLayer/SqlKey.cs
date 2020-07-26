using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DataAccessLayer
{
	public class SqlKey
	{
		public SqlParameter MFTNumber { get { return ParameterHelper.GetNewUnsignedInt32Parameter(StringLiteral_MftNumber, _mftNumber); } }
		public SqlParameter SequenceNumber { get { return ParameterHelper.GetNewUnsignedInt16Parameter(StringLiteral_SequenceNumber, _sequenceNumber); } }
		public SqlParameter SHA256 { get { return ParameterHelper.GetNewStringParameter(StringLiteral_SHA256, _sha256Hash); } }

		private uint _mftNumber;
		private ushort _sequenceNumber;
		private string _sha256Hash;

		private static string StringLiteral_MftNumber = "MFTNumber";
		private static string StringLiteral_SequenceNumber = "SequenceNumber";
		private static string StringLiteral_SHA256 = "SHA256";

		public string WhereClause
		{
			get
			{
				return $"WHERE [{StringLiteral_MftNumber}] = @{StringLiteral_MftNumber} AND [{StringLiteral_SequenceNumber}] = @{StringLiteral_SequenceNumber} AND [{StringLiteral_SHA256}] = @{StringLiteral_SHA256}";
			}
		}

		public SqlKey(uint mftNumber, ushort sequenceNumber, string sha256Hash)
		{
			_mftNumber = mftNumber;
			_sequenceNumber = sequenceNumber;
			_sha256Hash = sha256Hash;
		}

		public List<SqlParameter> GetSqlParameters()
		{
			List<SqlParameter> result = new List<SqlParameter>();
			result.Add(SHA256);
			result.Add(MFTNumber);
			result.Add(SequenceNumber);
			return result;
		}
	}
}
