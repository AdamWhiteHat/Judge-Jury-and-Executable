using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilePropertiesBaselineGUI
{
	public static class Settings
	{
		public static string Database_ConnectionString = SettingsReader.GetSettingValue<string>("Database.ConnectionString");
		public static string GUI_DefaultFolder = SettingsReader.GetSettingValue<string>("GUI.DefaultFolder");
		public static string GUI_SearchPattern = SettingsReader.GetSettingValue<string>("GUI.SearchPattern");
		public static bool FileEnumeration_DisableWorkerThread = SettingsReader.GetSettingValue<bool>("FileEnumeration.DisableWorkerThread");
	}
}