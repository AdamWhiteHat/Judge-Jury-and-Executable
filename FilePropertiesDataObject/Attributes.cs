using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesDataObject
{
	public class Attributes
	{
		public bool Archive { get; private set; }
		public bool ReadOnly { get; private set; }
		public bool Hidden { get; private set; }
		public bool System { get; private set; }
		public bool Encrypted { get; private set; }
		public bool Compressed { get; private set; }
		public bool Temporary { get; private set; }

		public Attributes(string filePath)
		{
			try
			{
				FileAttributes attributes = File.GetAttributes(filePath);

				Archive = ((attributes & FileAttributes.Archive) == FileAttributes.Archive);
				ReadOnly = ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
				Hidden = ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
				System = ((attributes & FileAttributes.System) == FileAttributes.System);
				Encrypted = ((attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted);
				Compressed = ((attributes & FileAttributes.Compressed) == FileAttributes.Compressed);
				Temporary = ((attributes & FileAttributes.Temporary) == FileAttributes.Temporary);
			}
			catch
			{ }
		}

		public override string ToString()
		{
			List<string> attr = new List<string>();
			if (Archive)
			{
				attr.Add("A");
			}
			if (ReadOnly)
			{
				attr.Add("R");
			}
			if (Hidden)
			{
				attr.Add("H");
			}
			if (System)
			{
				attr.Add("S");
			}
			if (Encrypted)
			{
				attr.Add("E");
			}
			if (Compressed)
			{
				attr.Add("C");
			}
			if (Temporary)
			{
				attr.Add("T");
			}

			if (attr.Any())
			{
				return string.Join(" ", attr);
			}

			return "";
		}
	}
}
