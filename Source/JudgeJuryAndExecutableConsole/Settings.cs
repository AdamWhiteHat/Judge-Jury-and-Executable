using System;

namespace JudgeJuryAndExecutableConsole
{
	public static class Settings
	{
		public static string Database_ConnectionString = SettingsReader.GetSettingValue<string>("Database.ConnectionString");
	}
}