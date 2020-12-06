using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilePropertiesDataObject;

namespace CsvDataAccessLayer
{
	public class CsvDataPersistenceLayer : IDataPersistenceLayer
	{
		private string _csvFilePath;

		public CsvDataPersistenceLayer(string csvFilePath)
		{
			if (string.IsNullOrWhiteSpace(csvFilePath))
			{
				throw new ArgumentException($"Argument {nameof(csvFilePath)} cannot be null, empty or whitespace.");
			}
			_csvFilePath = csvFilePath;
			throw new NotImplementedException();
		}

		public bool PersistFileProperties(FileProperties fileProperties)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}
	}
}
