using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilePropertiesDataObject;
using System.Reflection;
using System.IO;

namespace CsvDataAccessLayer
{
	public class CsvDataPersistenceLayer : IDataPersistenceLayer
	{
		private string _csvFilePath;
		private List<string> _columnNames;
		private string _headerRow;
		private static string _delimiter = ",";
		private static char[] _excludeChars = new char[] { '\r', '\n' };

		public CsvDataPersistenceLayer(string csvFilePath)
		{
			if (string.IsNullOrWhiteSpace(csvFilePath))
			{
				throw new ArgumentException($"Argument {nameof(csvFilePath)} cannot be null, empty or whitespace.");
			}

			_csvFilePath = csvFilePath;
			_columnNames = GetPublicPropertyNames<FileProperties>();
			_headerRow = string.Join(_delimiter, _columnNames);
			File.WriteAllLines(_csvFilePath, new string[] { _headerRow });
		}

		public bool PersistFileProperties(FileProperties fileProperties)
		{
			List<string> values = new List<string>();
			foreach (PropertyInfo property in typeof(FileProperties).GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				object value = property.GetValue(fileProperties, null);
				string stringValue = (value == null) ? "" : new string(value.ToString().Except(_excludeChars).ToArray());
				values.Add($"\"{stringValue.Replace("\"", "\"\"")}\"");
			}

			File.AppendAllLines(_csvFilePath, new string[] { string.Join(_delimiter, values) });
			return true;
		}

		public void Dispose()
		{
			_headerRow = "";
			_csvFilePath = "";
			_columnNames.Clear();
		}

		private static List<string> GetPublicPropertyNames<T>() where T : class
		{
			Type classType = typeof(T);
			List<string> result = new List<string>();
			foreach (PropertyInfo property in classType.GetProperties())
			{
				result.Add(property.Name); // Add property name
			}
			return result;
		}
	}
}
