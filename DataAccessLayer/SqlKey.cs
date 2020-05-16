using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class SqlKey
	{
		public SqlParameter MFTNumber { get; private set; }
		public SqlParameter SequenceNumber { get; private set; }
		public SqlParameter SHA256 { get; private set; }

		public string WhereClause
		{
			get
			{
				return $"WHERE [MFTNumber] = {MFTNumber} AND [SequenceNumber] = {SequenceNumber} AND [SHA256] = '{SHA256}'";
			}
		}

		public SqlKey(SqlParameter mftNumber, SqlParameter sequenceNumber, SqlParameter sha256)
		{
			MFTNumber = mftNumber;
			SequenceNumber = sequenceNumber;
			SHA256 = sha256;
		}
	}
}
