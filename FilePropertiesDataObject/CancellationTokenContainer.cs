using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilePropertiesDataObject
{
	public static class CancellationHelper
	{
		private static CancellationToken cancelToken = CancellationToken.None;

		public static void SetCancellationToken(CancellationToken cancellationToken)
		{
			cancelToken = cancellationToken;
		}

		public static CancellationToken GetCancellationToken()
		{
			return cancelToken;
		}

		public static void Clear()
		{
			cancelToken = CancellationToken.None;
		}

		public static void ThrowIfCancelled()
		{
			cancelToken.ThrowIfCancellationRequested();
		}
	}
}
