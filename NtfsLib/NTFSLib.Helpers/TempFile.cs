using System;
using System.IO;

namespace NTFSLib.Helpers
{
	public class TempFile : IDisposable
	{
		public FileInfo File { get; set; }

		public TempFile()
		{
			File = new FileInfo(Path.GetTempFileName());
		}

		public virtual void Dispose()
		{
			File.Refresh();
			if (File.Exists)
			{
				File.Delete();
			}
		}
	}
}