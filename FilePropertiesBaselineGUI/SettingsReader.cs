using System;
using System.Linq;
using System.Configuration;

namespace FilePropertiesBaselineGUI
{
	public static class SettingsReader
	{
		public static T GetSettingValue<T>(string settingName)
		{
			try
			{
				if (SettingExists(settingName))
				{
					T result = (T)Convert.ChangeType(ConfigurationManager.AppSettings[settingName], typeof(T));
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

		public static bool SettingExists(string settingName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(settingName))
				{
					return false;
				}
				else if (!ConfigurationManager.AppSettings.AllKeys.Contains(settingName))
				{
					return false;
				}
				else if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[settingName]))
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