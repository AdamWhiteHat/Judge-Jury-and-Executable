using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FilePropertiesBaselineGUI
{
	public static class JsonSerialization
	{
		public static class Save
		{
			public static void Object(object obj, string filename)
			{
				string saveJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(filename, saveJson);
			}
		}

		public static class Load
		{
			public static T Generic<T>(string filename)
			{
				string loadJson = File.ReadAllText(filename);
				return JsonConvert.DeserializeObject<T>(loadJson);
			}
		}
	}
}
