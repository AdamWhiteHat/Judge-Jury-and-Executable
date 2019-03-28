using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace FilePropertiesBaselineGUI
{
	public static class SettingsReader
	{
		public static T GetSettingValue<T>(string SettingName)
		{
			try
			{
				if (SettingExists(SettingName))
				{
					T result = (T)Convert.ChangeType(ConfigurationManager.AppSettings[SettingName], typeof(T));
					if (result != null)
					{
						return result;
					}
				}
			}
			catch //(Exception ex)
			{
				//Logging.LogException($"{nameof(SettingsReader)}.{nameof(GetSettingValue)} threw an exception.", ex);
			}

			return default(T);
		}

		public static bool SettingExists(string SettingName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(SettingName))
				{
					return false;
				}
				else if (!ConfigurationManager.AppSettings.AllKeys.Contains(SettingName))
				{
					return false;
				}
				else if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[SettingName]))
				{
					return false;
				}

				return true;
			}
			catch //(Exception ex)
			{
				//Logging.LogException($"{nameof(SettingsReader)}.{nameof(SettingExists)} threw an exception.", ex);
				return false;
			}
		}
	}
}