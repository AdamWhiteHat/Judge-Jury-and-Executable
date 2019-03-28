using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace NTFSLib.Utilities
{
	public class UIntEqualityComparer : EqualityComparer<uint>
	{
		private static readonly BigInteger Int32MaxValue = int.MaxValue;
		private static readonly BigInteger LargePrime = BigInteger.Parse("11176778521");

		public override bool Equals(uint x, uint y)
		{
			return Default.Equals(x, y);
		}

		public override int GetHashCode(uint obj)
		{
			BigInteger keyValue = new BigInteger(obj);

			if (keyValue.Sign == -1)
			{
				keyValue = ((Int32MaxValue * 3) + keyValue);
			}

			return (int)(BigInteger.Abs(keyValue * LargePrime) % Int32MaxValue);
		}
	}
}
