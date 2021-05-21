using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesDataObject
{
	public interface IDataPersistenceLayer : IDisposable
	{
		bool PersistFileProperties(FileProperties fileProperties);
	}
}
