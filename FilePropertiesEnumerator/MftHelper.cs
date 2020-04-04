using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using RawDiskLib;
using NTFSLib.IO;
using NTFSLib.NTFS;
using NTFSLib.Helpers;
using NTFSLib.Objects.Attributes;

namespace FilePropertiesEnumerator
{
	public static class MftHelper
	{
		private static int rawDiskCacheRecordSize = 2048;

		public static IEnumerable<NtfsFileEntry> EnumerateFileEntries(string directory)
		{
			if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("String cannot be null, empty or whitespace.", nameof(directory));

			char driveLetter = directory[0];
			using (RawDisk disk = new RawDisk(driveLetter, FileAccess.Read))
			{
				NTFSDiskProvider provider = new NTFSDiskProvider(disk);
				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, rawDiskCacheRecordSize);

				NtfsDirectory rootDir = ntfsWrapper.GetRootDirectory();

				if (rootDir == null)
				{
					throw new Exception("currDir == null");
				}

				NtfsDirectory dir = ntfsWrapper.NavigateToDirectory(directory);

				foreach (var fileEntry in NTFSWrapper.EnumerateFileEntries(dir))
				{
					yield return fileEntry;
				}
			}

			yield break;
		}
	}
}
