﻿using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using NTFSLib.IO;
using NTFSLib.Objects;
using NTFSLib.Objects.Attributes;
using NTFSLib.Objects.Enums;
using NTFSLib.Objects.Specials;
using NTFSLib.Provider;
using NTFSLib.Utilities;
using Attribute = NTFSLib.Objects.Attributes.Attribute;

namespace NTFSLib.NTFS
{
	public class NTFSWrapper : INTFSInfo
	{
		private uint _sectorsPrRecord;
		private WeakReference[] FileRecords { get; set; }
		private Stream MftStream { get; set; }
		private readonly int _rawDiskCacheSizeRecords;
		private RawDiskCache MftRawCache { get; set; }


		internal IDiskProvider Provider { get; private set; }
		internal NtfsFileCache FileCache { get; private set; }


		public uint BytesPrCluster { get { return (uint)(Boot.BytesPrSector * Boot.SectorsPrCluster); } }
		public uint BytesPrSector { get { return Boot.BytesPrSector; } }
		public byte SectorsPrCluster { get { return Boot.SectorsPrCluster; } }
		public bool OwnsDiskStream { get { return true; } }
		public Stream GetDiskStream() { return Provider.CreateDiskStream(); }
		public uint BytesPrFileRecord { get; private set; }
		public uint FileRecordCount { get; private set; }
		public BootSector Boot { get; private set; }
		public FileRecord FileMFT { get; private set; }
		public Version NTFSVersion { get; private set; }




		public NTFSWrapper(IDiskProvider provider, int rawDiskCacheSizeRecords)
		{
			_rawDiskCacheSizeRecords = rawDiskCacheSizeRecords;
			Provider = provider;
			FileCache = new NtfsFileCache();

			InitializeNTFS();
		}

		private void InitializeNTFS()
		{
			// Read $BOOT
			if (Provider.MftFileOnly)
			{
				Boot = new BootSector();
				Boot.OEMCode = "NTFS";
				Boot.SectorsPrCluster = 2;      // Small cluster
				Boot.BytesPrSector = 512;       // Smallest possible sector

				// Get FileRecord size (read first record's size)
				byte[] mft_data = new byte[512];
				Provider.ReadBytes(mft_data, 0, 0, mft_data.Length);

				Boot.MFTRecordSizeBytes = FileRecord.ParseAllocatedSize(mft_data, 0);

				mft_data = null;

			}
			else
			{
				byte[] drive_data = new byte[512];
				Provider.ReadBytes(drive_data, 0, 0, 512);
				Boot = BootSector.ParseData(drive_data, 512, 0);

				drive_data = null;

				Debug.Assert(Boot.OEMCode == "NTFS");
			}

			// Get FileRecord size
			BytesPrFileRecord = Boot.MFTRecordSizeBytes;
			_sectorsPrRecord = BytesPrFileRecord / BytesPrSector;
			Debug.WriteLine($"Updated BytesPrFileRecord, now set to {BytesPrFileRecord}");

			// Prep cache
			MftRawCache = new RawDiskCache(0);

			// Read $MFT file record

			byte[] record_data = ReadMFTRecordData((uint)SpecialMFTFiles.MFT);
			FileMFT = ParseMFTRecord(record_data);
			record_data = null;


			Debug.Assert(FileMFT != null);

			Debug.Assert(FileMFT.Attributes.Count(s => s.Type == AttributeType.DATA) == 1);
			AttributeData fileMftData = FileMFT.Attributes.OfType<AttributeData>().Single();
			Debug.Assert(fileMftData.NonResidentFlag == ResidentFlag.NonResident);
			Debug.Assert(fileMftData.DataFragments.Length >= 1);

			MftStream = OpenFileRecord(FileMFT);

			// Prep cache
			long maxLength = MftStream.Length;
			long toAllocateForCache = Math.Min(maxLength, _rawDiskCacheSizeRecords * BytesPrFileRecord);
			MftRawCache = new RawDiskCache((int)toAllocateForCache);

			// Get number of FileRecords 
			FileRecordCount = (uint)((fileMftData.DataFragments.Sum(s => (float)s.Clusters)) * (BytesPrCluster * 1f / BytesPrFileRecord));
			FileRecords = new WeakReference[FileRecordCount];

			FileRecords[0] = new WeakReference(FileMFT);

			// Read $VOLUME file record
			FileRecord fileVolume = ReadMFTRecord(SpecialMFTFiles.Volume);

			// Get version
			Attribute versionAttrib = fileVolume.Attributes.SingleOrDefault(s => s.Type == AttributeType.VOLUME_INFORMATION);
			if (versionAttrib != null)
			{
				AttributeVolumeInformation attrib = (AttributeVolumeInformation)versionAttrib;
				NTFSVersion = new Version(attrib.MajorVersion, attrib.MinorVersion);
			}
		}







		private bool InRawDiskCache(uint number)
		{
			if (MftRawCache.Initialized && MftRawCache.DataOffset / BytesPrFileRecord <= number &&
				number <= MftRawCache.DataOffset / BytesPrFileRecord + MftRawCache.Length / BytesPrFileRecord - 1)
			{
				return true;
			}

			return false;
		}

		private FileRecord ParseMFTRecord(byte[] data)
		{
			return FileRecord.Parse(data, 0, Boot.BytesPrSector, _sectorsPrRecord);
		}

		private FileRecord ReadMFTRecord(SpecialMFTFiles file)
		{
			return ReadMFTRecord((uint)file);
		}

		private byte[] ReadMFTRecordData(uint number)
		{
			int length = (int)(BytesPrFileRecord == 0 ? 4096 : BytesPrFileRecord);
			long offset = number * length;

			// Calculate location
			if (InRawDiskCache(number))
			{
				byte[] mftData = new byte[length];
				int cacheOffset = (int)(offset - MftRawCache.DataOffset);

				Array.Copy(MftRawCache.Data, cacheOffset, mftData, 0, mftData.Length);

				Debug.WriteLine($"Read MFT Record {number} via. RAW CACHE; bytes {offset}->{offset + (long)length} ({length} bytes)");
				return mftData;
			}

			if (Provider.MftFileOnly)
			{
				// Is a contiguous file - ignore MFT fragments
				// Offset is still correct.
			}
			else if (FileMFT == null)
			{
				// We haven't got the $MFT yet, ignore MFT fragments
				// Offset into the MFT beginning region
				offset += (long)(Boot.MFTCluster * BytesPrCluster);
			}
			else if (MftStream != null)
			{
				byte[] mftData = new byte[length];

				MftStream.Seek(offset, SeekOrigin.Begin);
				MftStream.Read(mftData, 0, length);

				Debug.WriteLine($"Read MFT Record {number} via. NTFSDISKSTREAM; bytes {offset}->{offset + (long)length} ({length} bytes)");
				return mftData;
			}
			else
			{
				throw new Exception("Shouldn't happen");
			}

			if (!Provider.CanReadBytes((ulong)offset, length))
			{
				Debug.WriteLine($"Couldn't read MFT Record {number}; bytes {offset}->{offset + (long)length} ({length} bytes)");
				return new byte[0];
			}

			Debug.WriteLine($"Read MFT Record {number}; bytes {offset}->{offset + (long)length} ({length} bytes)");

			byte[] data = new byte[length];
			Provider.ReadBytes(data, 0, (ulong)offset, length);

			return data;
		}

		private Stream OpenFileRecord(FileRecord record, string dataStream = "")
		{
			Debug.Assert(record != null);

			if (Provider.MftFileOnly)
			{
				throw new InvalidOperationException("Provider indicates it's providing an MFT file only");
			}

			// Fetch extended data
			ParseAttributeLists(record);

			// Get all DATA attributes
			List<AttributeData> dataAttribs = record.Attributes.OfType<AttributeData>().Where(s => s.AttributeName == dataStream).ToList();

			Debug.Assert(dataAttribs.Count >= 1);

			if (dataAttribs.Count == 1 && dataAttribs[0].NonResidentFlag == ResidentFlag.Resident)
			{
				return new MemoryStream(dataAttribs[0].DataBytes);
			}

			Debug.Assert(dataAttribs.All(s => s.NonResidentFlag == ResidentFlag.NonResident));

			DataFragment[] fragments = dataAttribs.SelectMany(s => s.DataFragments).OrderBy(s => s.StartingVCN).ToArray();
			Stream diskStream = Provider.CreateDiskStream();

			ushort compressionUnitSize = dataAttribs[0].NonResidentHeader.CompressionUnitSize;
			ushort compressionClusterCount = (ushort)(compressionUnitSize == 0 ? 0 : Math.Pow(2, compressionUnitSize));

			return new NtfsDiskStream(diskStream, true, fragments, BytesPrCluster, compressionClusterCount, (long)dataAttribs[0].NonResidentHeader.ContentSize);
		}






		internal void ParseNonResidentAttribute(Attribute attr)
		{
			if (Provider.MftFileOnly)
			{   // Nothing to do about this
				throw new InvalidOperationException("Provider indicates an MFT file is used. Cannot parse non-resident attributes.");
			}

			if (attr.NonResidentHeader.Fragments.Length > 0)
			{   // Get data
				attr.ParseAttributeNonResidentBody(this);
			}
		}

		internal void ParseAttributeLists(FileRecord record)
		{
			if (record.ExternalAttributes.Count > 0)
			{
				return; // Already parsed
			}

			Hashtable externalRecords = new Hashtable();
			foreach (AttributeList listAttr in record.Attributes.OfType<AttributeList>())
			{
				if (listAttr.NonResidentFlag == ResidentFlag.NonResident)
				{
					if (Provider.MftFileOnly)
					{
						// Nothing to do about this
						return;
					}

					// Get data
					listAttr.ParseAttributeNonResidentBody(this);
				}

				foreach (AttributeListItem item in listAttr.Items)
				{
					if (item.BaseFile.Equals(record.FileReference))
					{
						continue; // Skip own attributes
					}

					if (externalRecords.ContainsKey(item.BaseFile))
					{
						continue;
					}

					FileRecord otherRecord = ReadMFTRecord(item.BaseFile.FileId);
					externalRecords[item.BaseFile] = otherRecord;

					Debug.Assert(otherRecord.FileReference.Equals(item.BaseFile));
				}
			}

			// Add all records to the record in question
			foreach (FileRecord externalRecord in externalRecords.Values)
			{
				record.ParseExternalAttributes(externalRecord);
			}
		}

		internal FileRecord ReadMFTRecord(uint number, bool parseAttributeLists = true)
		{
			FileRecord record;
			if (number <= FileRecords.Length && FileRecords[number] != null && (record = FileRecords[number].Target as FileRecord) != null)
			{
				return record;
			}

			byte[] data = ReadMFTRecordData(number);
			record = ParseMFTRecord(data);

			FileRecords[number] = new WeakReference(record);

			// Check size
			if (BytesPrFileRecord == 0)
			{
				// Some checks
				Debug.Assert(record.SizeOfFileRecordAllocated % 512 == 0);
				Debug.Assert(record.SizeOfFileRecordAllocated >= 512);
				Debug.Assert(record.SizeOfFileRecordAllocated <= 4096);

				BytesPrFileRecord = record.SizeOfFileRecordAllocated;
			}

			if (parseAttributeLists)
			{
				ParseAttributeLists(record);
			}

			return record;
		}






		public NtfsDirectory GetRootDirectory()
		{
			return (NtfsDirectory)NtfsFileEntry.CreateEntry(this, (uint)SpecialMFTFiles.RootDir);
		}

		public NtfsDirectory NavigateToDirectory(string path)
		{
			Debug.Assert(Path.IsPathRooted(path));

			string[] dirs = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			NtfsDirectory currDir = GetRootDirectory();

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



		public static IEnumerable<NtfsFileEntry> EnumerateFileEntries(NtfsDirectory homeDir)
		{
			IEnumerable<NtfsFile> subFiles = homeDir.ListFiles();

			foreach (var file in subFiles)
			{
				yield return file;
			}

			IEnumerable<NtfsDirectory> subDirs = homeDir.ListDirectories();
			foreach (var dir in subDirs)
			{
				foreach (var entry in EnumerateFileEntries(dir))
				{
					yield return entry;
				}
			}

			yield break;
		}


	}
}
