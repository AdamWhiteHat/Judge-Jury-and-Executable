using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
	public static class BuildConfiguration
	{
		private static bool _isDebug;

		static BuildConfiguration()
		{
			_isDebug = false;
			SetDebugFlag();
		}

		[Conditional("DEBUG")]
		private static void SetDebugFlag()
		{
			_isDebug = true;
		}

		public static bool IsRelease()
		{
			return !(_isDebug);
		}

		public static bool IsDebug()
		{
			return _isDebug;
		}
	}
}
