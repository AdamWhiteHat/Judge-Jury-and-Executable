using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlDataAccessLayer
{
	public class SqlKey
	{
		private uint _mftNumber;
		private ushort _sequenceNumber;
		private string _sha256Hash;

		private static string StringLiteral_MftNumber = "MFTNumber";
		private static string StringLiteral_SequenceNumber = "SequenceNumber";
		private static string StringLiteral_SHA256 = "SHA256";

		public SqlKey(uint mftNumber, ushort sequenceNumber, string sha256Hash)
		{
			_mftNumber = mftNumber;
			_sequenceNumber = sequenceNumber;
			_sha256Hash = sha256Hash;
		}

		public SqlParameter[] GetSqlParameters()
		{
			return new SqlParameter[]
			{
				ParameterHelper.GetNewStringParameter(StringLiteral_SHA256, _sha256Hash),
				ParameterHelper.GetParameter(StringLiteral_MftNumber, _mftNumber),
				ParameterHelper.GetParameter(StringLiteral_SequenceNumber, _sequenceNumber)
			};
		}

		public string GetWhereClause() => "[SHA256] = @SHA256 AND [MFTNumber] = @MFTNumber AND [SequenceNumber] = @SequenceNumber";
	}
}
