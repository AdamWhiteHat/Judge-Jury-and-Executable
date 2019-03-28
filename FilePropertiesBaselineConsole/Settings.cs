using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilePropertiesBaselineConsole
{
	public static class Settings
	{
		public static string Database_ConnectionString = SettingsReader.GetSettingValue<string>("Database.ConnectionString");
		public static bool FileEnumeration_DisableWorkerThread = SettingsReader.GetSettingValue<bool>("FileEnumeration.DisableWorkerThread");
	}
}