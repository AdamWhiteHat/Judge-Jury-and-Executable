using System;
using System.Linq;
using System.Collections.Generic;

namespace NTFSLib.Utilities
{
	public static class HashHelper
	{
		public static uint CombineHash(uint u1, uint u2)
		{
			return (u1 << 7 | u1 >> 25) ^ u2;
		}

		public static int GetUInt64HashCode(UInt64 uint64)
		{
			return (int)uint64 ^ (int)(uint64 >> 32);
		}
	}
}
