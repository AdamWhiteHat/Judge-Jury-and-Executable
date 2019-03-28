using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace NTFSLib.Utilities
{
	public class ULongEqualityComparer : EqualityComparer<ulong>
	{
		private static readonly BigInteger Int32MaxValue = int.MaxValue;
		private static readonly BigInteger UIntMaxValue  = uint.MaxValue;
		private static readonly BigInteger ULongMaxValue = ulong.MaxValue;

		private static readonly BigInteger LargePrime_Int32  = BigInteger.Parse("4460758831");            // Int32.MaxValue  == 2147483647
		private static readonly BigInteger LargePrime_UInt32 = BigInteger.Parse("8844674051");           // UInt32.MaxValue == 4294967295
		private static readonly BigInteger LargePrime_UInt64 = BigInteger.Parse("36925826727152769839"); // UInt64.MaxValue == 18446744073709551615		

		public override bool Equals(ulong x, ulong y)
		{
			return Default.Equals(x, y);
		}

		public static ulong CreateKey(uint id, int filenameHashcode)
		{
			BigInteger unsignedInteger = new BigInteger(id);
			BigInteger signedInteger = new BigInteger(filenameHashcode);

			if (unsignedInteger.Sign == -1)
			{
				unsignedInteger = ((UIntMaxValue * 3) + unsignedInteger);
			}

			if (signedInteger.Sign == -1)
			{
				signedInteger = ((Int32MaxValue * 3) + signedInteger);
			}

			unsignedInteger = BigInteger.Abs(unsignedInteger * LargePrime_UInt32);
			signedInteger = BigInteger.Abs(signedInteger * LargePrime_Int32);

			BigInteger product = (signedInteger * unsignedInteger);
			return (ulong)(product % ULongMaxValue);
		}

		public override int GetHashCode(ulong obj)
		{
			BigInteger keyValue = new BigInteger(obj);

			if (keyValue.Sign == -1)
			{
				keyValue = ((Int32MaxValue * 3) + keyValue);
			}

			return (int)(BigInteger.Abs(keyValue * LargePrime_Int32) % Int32MaxValue);
		}
	}
}
