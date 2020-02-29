﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NTFSLib.IO;
using NTFSLib.NTFS;
using System.Diagnostics;

namespace NTFSLib.Helpers
{
	public static class NTFSHelpers
	{
		public static NtfsDirectory OpenDir(NTFSWrapper ntfsWrapper, string path)
		{
			Debug.Assert(Path.IsPathRooted(path));

			string[] dirs = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			NtfsDirectory currDir = ntfsWrapper.GetRootDirectory();

			Debug.Assert(currDir != null);

			foreach (string dir in dirs.Skip(1))        // Skip root (C:\)
			{
				IEnumerable<NtfsDirectory> subDirs = currDir.ListDirectories();
				NtfsDirectory subDir = subDirs.FirstOrDefault(s => s.Name.Equals(dir, StringComparison.InvariantCultureIgnoreCase));

				Debug.Assert(subDir != null);

				currDir = subDir;
			}

			return currDir;
		}
	}
}