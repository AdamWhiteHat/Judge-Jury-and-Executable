using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FilePropertiesDataObject.Helpers
{
	public static class SequentialFilename
	{
		public static string GetNextAvailable(string fromFilename)
		{
			string result = fromFilename;
			string fileDir = Path.GetDirectoryName(result); ;
			string fileName = Path.GetFileNameWithoutExtension(result);
			string fileExt = Path.GetExtension(result);

			int counter = 0;

			var sequenceDelimiterIndex = fileName.LastIndexOf('_');
			if (sequenceDelimiterIndex != -1)
			{
				if ((sequenceDelimiterIndex + 3) == (fileName.Length - 1))
				{
					string possibleDigits = fileName.Substring(sequenceDelimiterIndex + 1);
					if (possibleDigits.Length == 3 && possibleDigits.All(c => char.IsDigit(c)))
					{
						if (int.TryParse(possibleDigits, out counter))
						{
							fileName = fileName.Substring(0, sequenceDelimiterIndex);
						}
						else
						{
							counter = 0;
						}
					}
				}
			}

			while (File.Exists(result))
			{
				counter++;
				result = Path.Combine(
							fileDir,
							$"{fileName}_{counter.ToString().PadLeft(3, '0')}{fileExt}"
						);
			}

			return result;
		}
	}
}
