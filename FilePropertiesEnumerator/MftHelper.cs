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

		public static IEnumerable<NtfsFile> EnumerateFiles(char driveLetter, string directory)
		{
			if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("String cannot be null, empty or whitespace.", nameof(directory));

			using (RawDisk disk = new RawDisk(driveLetter, FileAccess.Read))
			{
				NTFSDiskProvider provider = new NTFSDiskProvider(disk);
				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, rawDiskCacheRecordSize);
				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);

				return ntfsDir.ListFiles();
			}
		}

		public static IEnumerable<NtfsDirectory> GetDirectories(char driveLetter, string directory)
		{
			if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("String cannot be null, empty or whitespace.", nameof(directory));

			using (RawDisk disk = new RawDisk(driveLetter, FileAccess.Read))
			{
				NTFSDiskProvider provider = new NTFSDiskProvider(disk);
				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, rawDiskCacheRecordSize);
				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);

				return ntfsDir.ListDirectories();
			}
		}
		/*
		public static string[] GetAlternateDatastreamFiles(char driveLetter, string directory, string filename)
		{
			string[] results = new string[] { };

			using (RawDisk disk = new RawDisk(driveLetter))
			{
				NTFSDiskProvider provider = new NTFSDiskProvider(disk);

				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, 0);

				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);
				NtfsFile ntfsFile = NTFSHelpers.OpenFile(ntfsDir, filename);

				results = ntfsWrapper.ListDatastreams(ntfsFile.MFTRecord);
			}

			return results;
		}

		public static string[] GetAlternateDatastreamDirectories(char driveLetter, string directory)
		{
			string[] results = new string[] { };

			using (RawDisk disk = new RawDisk(driveLetter))
			{
				NTFSDiskProvider provider = new NTFSDiskProvider(disk);

				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, 0);

				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);

				// Check streams
				ntfsWrapper.ListDatastreams(ntfsDir.MFTRecord);
			}

			return results;
		}

		public static NtfsFile SimpleFile(char driveLetter, string directory, string filename)
		{
			using (RawDisk disk = new RawDisk(driveLetter))
			{

				NTFSDiskProvider provider = new NTFSDiskProvider(disk);

				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, 0);

				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);
				return NTFSHelpers.OpenFile(ntfsDir, filename);
			}
		}

		public static AttributeData SparseFile(char driveLetter, string directory, string filename)
		{
			using (RawDisk disk = new RawDisk(driveLetter))
			{

				NTFSDiskProvider provider = new NTFSDiskProvider(disk);

				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, 0);

				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);
				NtfsFile ntfsFile = NTFSHelpers.OpenFile(ntfsDir, filename);

				return ntfsFile.MFTRecord.Attributes.OfType<AttributeData>().Single();
			}
		}

		public static AttributeData CompressedFile(char driveLetter, string directory, string filename)
		{
			using (RawDisk disk = new RawDisk(driveLetter))
			{

				NTFSDiskProvider provider = new NTFSDiskProvider(disk);

				NTFSWrapper ntfsWrapper = new NTFSWrapper(provider, 0);

				NtfsDirectory ntfsDir = NTFSHelpers.OpenDir(ntfsWrapper, directory);
				NtfsFile ntfsFile = NTFSHelpers.OpenFile(ntfsDir, filename);
				return ntfsFile.MFTRecord.Attributes.OfType<AttributeData>().Single();
			}
		}
		*/
	}
}
