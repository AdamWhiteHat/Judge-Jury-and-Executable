/*
    The NtfsReader library.

    Copyright (C) 2008 Danny Couture

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
  
    For the full text of the license see the "License.txt" file.

    This library is based on the work of Jeroen Kessels, Author of JkDefrag.
    http://www.kessels.com/Jkdefrag/
    
    Special thanks goes to him.
  
    Danny Couture
    Software Architect
    mailto:zerk666@gmail.com
*/
using System.Text;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Filesystem.Ntfs;
using System.IO.Filesystem.Ntfs.Internal;
using RawDiskLib;

namespace System.IO.Filesystem.Ntfs
{
	using Stream = System.IO.Filesystem.Ntfs.Internal.Stream;
	using Attribute = System.IO.Filesystem.Ntfs.Internal.Attribute;

	public sealed partial class NtfsReader : IDisposable
	{
		#region Constants

		public const UInt64 VIRTUALFRAGMENT = 18446744073709551615; // _UI64_MAX - 1 */
		private const UInt32 ROOTDIRECTORY = 5;

		private readonly byte[] BitmapMasks = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

		#endregion

		//we support map drive that are mapped on a local fixed disk
		//we will resolve the fixed drive and automatically fix paths
		//so everything should be transparent
		string _locallyMappedDriveRootPath;
		string _rootPath;
		SafeFileHandle _volumeHandle;
		DiskInfoWrapper _diskInfo;
		Node[] _nodes;
		internal StandardInformation[] _standardInformations;
		internal Stream[][] _streams;
		DriveInfo _driveInfo;
		List<string> _names = new List<string>();
		RetrieveMode _retrieveMode;
		byte[] _bitmapData;
		RawDisk rawDisk;

		//preallocate a lot of space for the strings to avoid too much dictionary resizing
		//use ordinal comparison to improve performance
		//this will be deallocated once the MFT reading is finished
		Dictionary<string, int> _nameIndex = new Dictionary<string, int>(128 * 1024, StringComparer.Ordinal);

		#region Events

		/// <summary>
		/// Raised once the bitmap data has been read.
		/// </summary>
		public event EventHandler BitmapDataAvailable;

		private void OnBitmapDataAvailable()
		{
			if (BitmapDataAvailable != null)
			{ BitmapDataAvailable(this, EventArgs.Empty); }
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Try to resolve map drive if it points to a local volume.
		/// </summary>
		/// <param name="driveInfo"></param>
		/// <returns></returns>
		private DriveInfo ResolveLocalMapDrive(DriveInfo driveInfo)
		{
			StringBuilder remoteNameBuilder = new StringBuilder(2048);
			int len = remoteNameBuilder.MaxCapacity;

			//get the address on which the map drive is pointing
			WNetGetConnection(driveInfo.Name.TrimEnd(new char[] { '\\' }), remoteNameBuilder, ref len);

			string remoteName = remoteNameBuilder.ToString();
			if (string.IsNullOrEmpty(remoteName))
			{ throw new Exception("The drive is neither a local drive nor a locally mapped network drive, can't open volume."); }

			//by getting all network shares on the local computer
			//we will be able to compare them with the remote address we found earlier.
			NetworkShare[] networkShares = EnumNetShares();

			for (int i = 0; i < networkShares.Length; ++i)
			{
				string networkShare =
					string.Format(@"\\{0}\{1}", Environment.MachineName, networkShares[i].NetworkName);

				if (string.Equals(remoteName, networkShare, StringComparison.OrdinalIgnoreCase)
					&&
					Directory.Exists(networkShares[i].LocalPath))
				{
					_locallyMappedDriveRootPath = networkShares[i].LocalPath;
					break;
				}
			}

			if (_locallyMappedDriveRootPath == null)
			{ throw new Exception("The drive is neither a local drive nor a locally mapped network drive, can't open volume."); }

			return new DriveInfo(Path.GetPathRoot(_locallyMappedDriveRootPath));
		}

		/// <summary>
		/// Allocate or retrieve an existing index for the particular string.
		/// </summary>
		///<remarks>
		/// In order to mimize memory usage, we reuse string as much as possible.
		///</remarks>
		private int GetNameIndex(string name)
		{
			int existingIndex;
			if (_nameIndex.TryGetValue(name, out existingIndex))
			{ return existingIndex; }

			_names.Add(name);
			_nameIndex[name] = _names.Count - 1;

			return _names.Count - 1;
		}

		/// <summary>
		/// Get the string from our stringtable from the given index.
		/// </summary>
		internal string GetNameFromIndex(int nameIndex)
		{
			return nameIndex == 0 ? null : _names[nameIndex];
		}

		private Stream SearchStream(List<Stream> streams, AttributeType streamType)
		{
			//since the number of stream is usually small, we can afford O(n)
			foreach (Stream stream in streams)
			{
				if (stream.Type == streamType)
				{ return stream; }
			}

			return null;
		}

		private Stream SearchStream(List<Stream> streams, AttributeType streamType, int streamNameIndex)
		{
			//since the number of stream is usually small, we can afford O(n)
			foreach (Stream stream in streams)
			{
				if (stream.Type == streamType && stream.NameIndex == streamNameIndex)
				{ return stream; }
			}

			return null;
		}

		#endregion

		#region File Reading Wrappers

		private unsafe void ReadFile(byte* buffer, int len, UInt64 absolutePosition)
		{
			ReadFile(buffer, (UInt64)len, absolutePosition);
		}

		private unsafe void ReadFile(byte* buffer, UInt32 len, UInt64 absolutePosition)
		{
			ReadFile(buffer, (UInt64)len, absolutePosition);
		}

		private unsafe void ReadFile(byte* buffer, UInt64 len, UInt64 absolutePosition)
		{
			NativeOverlapped overlapped = new NativeOverlapped(absolutePosition);

			uint read;
			if (!ReadFile(_volumeHandle, (IntPtr)buffer, (uint)len, out read, ref overlapped))
			{
				return;
				//throw new Exception("Unable to read volume information");
			}

			if (read != (uint)len)
			{
				return;
				//throw new Exception("Unable to read volume information");
			}
		}

		#endregion

		#region Ntfs Interpretor

		/// <summary>
		/// Read the next contiguous block of information on disk
		/// </summary>
		private unsafe bool ReadNextChunk(
			byte* buffer,
			UInt32 bufferSize,
			UInt32 nodeIndex,
			int fragmentIndex,
			Stream dataStream,
			ref UInt64 BlockStart,
			ref UInt64 BlockEnd,
			ref UInt64 Vcn,
			ref UInt64 RealVcn
			)
		{
			BlockStart = nodeIndex;
			BlockEnd = BlockStart + bufferSize / _diskInfo.BytesPerMftRecord;
			if (BlockEnd > dataStream.Size * 8)
				BlockEnd = dataStream.Size * 8;

			UInt64 u1 = 0;

			int fragmentCount = dataStream.Fragments.Count;
			while (fragmentIndex < fragmentCount)
			{
				Fragment fragment = dataStream.Fragments[fragmentIndex];

				/* Calculate Inode at the end of the fragment. */
				u1 = (RealVcn + fragment.NextVcn - Vcn) * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster / _diskInfo.BytesPerMftRecord;

				if (u1 > nodeIndex)
				{
					break;
				}

				do
				{
					if (fragment.Lcn != VIRTUALFRAGMENT)
					{ RealVcn = RealVcn + fragment.NextVcn - Vcn; }

					Vcn = fragment.NextVcn;

					if (++fragmentIndex >= fragmentCount)
					{ break; }

				}
				while (fragment.Lcn == VIRTUALFRAGMENT);
			}

			if (fragmentIndex >= fragmentCount)
			{ return false; }

			if (BlockEnd >= u1)
			{ BlockEnd = u1; }

			ulong position =
				(dataStream.Fragments[fragmentIndex].Lcn - RealVcn) * _diskInfo.BytesPerSector *
					_diskInfo.SectorsPerCluster + BlockStart * _diskInfo.BytesPerMftRecord;

			ReadFile(buffer, (BlockEnd - BlockStart) * _diskInfo.BytesPerMftRecord, position);

			return true;
		}

		/// <summary>
		/// Gather basic disk information we need to interpret data
		/// </summary>
		private unsafe void InitializeDiskInfo()
		{
			byte[] volumeData = new byte[512];

			fixed (byte* ptr = volumeData)
			{
				ReadFile(ptr, volumeData.Length, 0);

				BootSector* bootSector = (BootSector*)ptr;

				if (bootSector->Signature != 0x202020205346544E)
				{ throw new Exception("This is not an NTFS disk."); }

				DiskInfoWrapper diskInfo = new DiskInfoWrapper();
				diskInfo.BytesPerSector = bootSector->BytesPerSector;
				diskInfo.SectorsPerCluster = bootSector->SectorsPerCluster;
				diskInfo.TotalSectors = bootSector->TotalSectors;
				diskInfo.MftStartLcn = bootSector->MftStartLcn;
				diskInfo.Mft2StartLcn = bootSector->Mft2StartLcn;
				diskInfo.ClustersPerMftRecord = bootSector->ClustersPerMftRecord;
				diskInfo.ClustersPerIndexRecord = bootSector->ClustersPerIndexRecord;

				if (bootSector->ClustersPerMftRecord >= 128)
				{ diskInfo.BytesPerMftRecord = ((ulong)1 << (byte)(256 - (byte)bootSector->ClustersPerMftRecord)); }
				else
				{ diskInfo.BytesPerMftRecord = diskInfo.ClustersPerMftRecord * diskInfo.BytesPerSector * diskInfo.SectorsPerCluster; }

				diskInfo.BytesPerCluster = (UInt64)diskInfo.BytesPerSector * (UInt64)diskInfo.SectorsPerCluster;

				if (diskInfo.SectorsPerCluster > 0)
				{ diskInfo.TotalClusters = diskInfo.TotalSectors / diskInfo.SectorsPerCluster; }

				_diskInfo = diskInfo;
			}
		}

		/// <summary>
		/// Used to check/adjust data before we begin to interpret it
		/// </summary>
		private unsafe void FixupRawMftdata(byte* buffer, UInt64 len)
		{
			FileRecordHeader* ntfsFileRecordHeader = (FileRecordHeader*)buffer;

			if (ntfsFileRecordHeader->RecordHeader.Type != RecordType.File)
			{ return; }

			UInt16* wordBuffer = (UInt16*)buffer;

			UInt16* UpdateSequenceArray = (UInt16*)(buffer + ntfsFileRecordHeader->RecordHeader.UsaOffset);
			UInt32 increment = (UInt32)_diskInfo.BytesPerSector / sizeof(UInt16);

			UInt32 Index = increment - 1;

			for (int i = 1; i < ntfsFileRecordHeader->RecordHeader.UsaCount; i++)
			{
				/* Check if we are inside the buffer. */
				if (Index * sizeof(UInt16) >= len)
				{ throw new Exception("USA data indicates that data is missing, the MFT may be corrupt."); }

				// Check if the last 2 bytes of the sector contain the Update Sequence Number.
				if (wordBuffer[Index] != UpdateSequenceArray[0])
				{ throw new Exception("USA fixup word is not equal to the Update Sequence Number, the MFT may be corrupt."); }

				/* Replace the last 2 bytes in the sector with the value from the Usa array. */
				wordBuffer[Index] = UpdateSequenceArray[i];
				Index = Index + increment;
			}
		}

		/// <summary>
		/// Decode the RunLength value.
		/// </summary>
		private static unsafe Int64 ProcessRunLength(byte* runData, UInt32 runDataLength, Int32 runLengthSize, ref UInt32 index)
		{
			Int64 runLength = 0;
			byte* runLengthBytes = (byte*)&runLength;
			for (int i = 0; i < runLengthSize; i++)
			{
				runLengthBytes[i] = runData[index];
				if (++index >= runDataLength)
				{ throw new Exception("Datarun is longer than buffer, the MFT may be corrupt."); }
			}
			return runLength;
		}

		/// <summary>
		/// Decode the RunOffset value.
		/// </summary>
		private static unsafe Int64 ProcessRunOffset(byte* runData, UInt32 runDataLength, Int32 runOffsetSize, ref UInt32 index)
		{
			Int64 runOffset = 0;
			byte* runOffsetBytes = (byte*)&runOffset;

			int i;
			for (i = 0; i < runOffsetSize; i++)
			{
				runOffsetBytes[i] = runData[index];
				if (++index >= runDataLength)
				{ throw new Exception("Datarun is longer than buffer, the MFT may be corrupt."); }
			}

			//process negative values
			if (runOffsetBytes[i - 1] >= 0x80)
			{
				while (i < 8)
				{ runOffsetBytes[i++] = 0xFF; }
			}

			return runOffset;
		}

		/// <summary>
		/// Read the data that is specified in a RunData list from disk into memory,
		/// skipping the first Offset bytes.
		/// </summary>
		private unsafe byte[] ProcessNonResidentData(
			byte* RunData,
			UInt32 RunDataLength,
			UInt64 Offset,         /* Bytes to skip from begin of data. */
			UInt64 WantedLength    /* Number of bytes to read. */
			)
		{
			/* Sanity check. */
			if (RunData == null || RunDataLength == 0)
			{ throw new Exception("nothing to read"); }

			if (WantedLength >= UInt32.MaxValue)
			{ throw new Exception("too many bytes to read"); }

			/* We have to round up the WantedLength to the nearest sector. For some
               reason or other Microsoft has decided that raw reading from disk can
               only be done by whole sector, even though ReadFile() accepts it's
               parameters in bytes. */
			if (WantedLength % _diskInfo.BytesPerSector > 0)
			{ WantedLength += _diskInfo.BytesPerSector - (WantedLength % _diskInfo.BytesPerSector); }

			/* Walk through the RunData and read the requested data from disk. */
			UInt32 Index = 0;
			Int64 Lcn = 0;
			Int64 Vcn = 0;

			byte[] buffer = new byte[WantedLength];

			fixed (byte* bufPtr = buffer)
			{
				while (RunData[Index] != 0)
				{
					/* Decode the RunData and calculate the next Lcn. */
					Int32 RunLengthSize = (RunData[Index] & 0x0F);
					Int32 RunOffsetSize = ((RunData[Index] & 0xF0) >> 4);

					if (++Index >= RunDataLength)
					{ throw new Exception("Error: datarun is longer than buffer, the MFT may be corrupt."); }

					Int64 RunLength = ProcessRunLength(RunData, RunDataLength, RunLengthSize, ref Index);

					Int64 RunOffset = ProcessRunOffset(RunData, RunDataLength, RunOffsetSize, ref Index);

					// Ignore virtual extents.
					if (RunOffset == 0 || RunLength == 0)
					{ continue; }

					Lcn += RunOffset;
					Vcn += RunLength;

					/* Determine how many and which bytes we want to read. If we don't need
                       any bytes from this extent then loop. */
					UInt64 ExtentVcn = (UInt64)((Vcn - RunLength) * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster);
					UInt64 ExtentLcn = (UInt64)(Lcn * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster);
					UInt64 ExtentLength = (UInt64)(RunLength * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster);

					if (Offset >= ExtentVcn + ExtentLength)
					{ continue; }

					if (Offset > ExtentVcn)
					{
						ExtentLcn = ExtentLcn + Offset - ExtentVcn;
						ExtentLength = ExtentLength - (Offset - ExtentVcn);
						ExtentVcn = Offset;
					}

					if (Offset + WantedLength <= ExtentVcn)
					{ continue; }

					if (Offset + WantedLength < ExtentVcn + ExtentLength)
					{ ExtentLength = Offset + WantedLength - ExtentVcn; }

					if (ExtentLength == 0)
					{ continue; }

					ReadFile(bufPtr + ExtentVcn - Offset, ExtentLength, ExtentLcn);
				}
			}

			return buffer;
		}

		/// <summary>
		/// Process each attributes and gather information when necessary
		/// </summary>
		private unsafe void ProcessAttributes(ref Node node, UInt32 nodeIndex, byte* ptr, UInt64 BufLength, UInt16 instance, int depth, List<Stream> streams, bool isMftNode)
		{
			Attribute* attribute = null;
			for (uint AttributeOffset = 0; AttributeOffset < BufLength; AttributeOffset = AttributeOffset + attribute->Length)
			{
				attribute = (Attribute*)(ptr + AttributeOffset);

				// exit the loop if end-marker.
				if ((AttributeOffset + 4 <= BufLength) && (*(UInt32*)attribute == 0xFFFFFFFF))
				{ break; }

				//make sure we did read the data correctly
				if ((AttributeOffset + 4 > BufLength) || attribute->Length < 3 || (AttributeOffset + attribute->Length > BufLength))
				{ throw new Exception("Error: attribute in Inode %I64u is bigger than the data, the MFT may be corrupt."); }

				//attributes list needs to be processed at the end
				if (attribute->AttributeType == AttributeType.AttributeAttributeList)
				{ continue; }

				/* If the Instance does not equal the AttributeNumber then ignore the attribute.
                   This is used when an AttributeList is being processed and we only want a specific
                   instance. */
				if ((instance != 65535) && (instance != attribute->AttributeNumber))
				{ continue; }

				if (attribute->Nonresident == 0)
				{
					ResidentAttribute* residentAttribute = (ResidentAttribute*)attribute;

					switch (attribute->AttributeType)
					{
						case AttributeType.AttributeFileName:
							AttributeFileName* attributeFileName = (AttributeFileName*)(ptr + AttributeOffset + residentAttribute->ValueOffset);

							if (attributeFileName->ParentDirectory.InodeNumberHighPart > 0)
							{ throw new NotSupportedException("48 bits inode are not supported to reduce memory footprint."); }

							//node.ParentNodeIndex = ((UInt64)attributeFileName->ParentDirectory.InodeNumberHighPart << 32) + attributeFileName->ParentDirectory.InodeNumberLowPart;
							node.ParentNodeIndex = attributeFileName->ParentDirectory.InodeNumberLowPart;

							if (attributeFileName->NameType == 1 || node.NameIndex == 0)
							{ node.NameIndex = GetNameIndex(new string(&attributeFileName->Name, 0, attributeFileName->NameLength)); }

							break;

						case AttributeType.AttributeStandardInformation:
							AttributeStandardInformation* attributeStandardInformation = (AttributeStandardInformation*)(ptr + AttributeOffset + residentAttribute->ValueOffset);

							node.Attributes |= (Attributes)attributeStandardInformation->FileAttributes;

							if ((_retrieveMode & RetrieveMode.StandardInformations) == RetrieveMode.StandardInformations)
							{
								_standardInformations[nodeIndex] =
									new StandardInformation(
										attributeStandardInformation->CreationTime,
										attributeStandardInformation->FileChangeTime,
										attributeStandardInformation->LastAccessTime,
										attributeStandardInformation->MftChangeTime
									);
							}

							break;

						case AttributeType.AttributeData:
							node.Size = residentAttribute->ValueLength;
							break;
					}
				}
				else
				{
					NonResidentAttribute* nonResidentAttribute = (NonResidentAttribute*)attribute;

					//save the length (number of bytes) of the data.
					if (attribute->AttributeType == AttributeType.AttributeData && node.Size == 0)
					{ node.Size = nonResidentAttribute->DataSize; }

					if (streams != null)
					{
						//extract the stream name
						int streamNameIndex = 0;
						if (attribute->NameLength > 0)
						{ streamNameIndex = GetNameIndex(new string((char*)(ptr + AttributeOffset + attribute->NameOffset), 0, (int)attribute->NameLength)); }

						//find or create the stream
						Stream stream = SearchStream(streams, attribute->AttributeType, streamNameIndex);

						if (stream == null)
						{
							stream = new Stream(streamNameIndex, attribute->AttributeType, nonResidentAttribute->DataSize);
							streams.Add(stream);
						}
						else if (stream.Size == 0)
						{ stream.Size = nonResidentAttribute->DataSize; }

						//we need the fragment of the MFTNode so retrieve them this time
						//even if fragments aren't normally read
						if (isMftNode || (_retrieveMode & RetrieveMode.Fragments) == RetrieveMode.Fragments)
						{
							ProcessFragments(
								ref node,
								stream,
								ptr + AttributeOffset + nonResidentAttribute->RunArrayOffset,
								attribute->Length - nonResidentAttribute->RunArrayOffset,
								nonResidentAttribute->StartingVcn
							);
						}
					}
				}
			}

			if (streams != null && streams.Count > 0)
			{ node.Size = streams[0].Size; }
		}

		/// <summary>
		/// Process fragments for streams
		/// </summary>
		private unsafe void ProcessFragments(
			ref Node node,
			Stream stream,
			byte* runData,
			UInt32 runDataLength,
			UInt64 StartingVcn)
		{
			if (runData == null)
			{ return; }

			/* Walk through the RunData and add the extents. */
			uint index = 0;
			Int64 lcn = 0;
			Int64 vcn = (Int64)StartingVcn;
			int runOffsetSize = 0;
			int runLengthSize = 0;

			while (runData[index] != 0)
			{
				/* Decode the RunData and calculate the next Lcn. */
				runLengthSize = (runData[index] & 0x0F);
				runOffsetSize = ((runData[index] & 0xF0) >> 4);

				if (++index >= runDataLength)
				{ throw new Exception("Error: datarun is longer than buffer, the MFT may be corrupt."); }

				Int64 runLength = ProcessRunLength(runData, runDataLength, runLengthSize, ref index);
				Int64 runOffset = ProcessRunOffset(runData, runDataLength, runOffsetSize, ref index);

				lcn += runOffset;
				vcn += runLength;

				/* Add the size of the fragment to the total number of clusters.
                   There are two kinds of fragments: real and virtual. The latter do not
                   occupy clusters on disk, but are information used by compressed
                   and sparse files. */
				if (runOffset != 0)
				{ stream.Clusters += (UInt64)runLength; }

				stream.Fragments.Add(
					new Fragment(
						runOffset == 0 ? VIRTUALFRAGMENT : (UInt64)lcn,
						(UInt64)vcn
					)
				);
			}
		}

		/// <summary>
		/// Process an actual MFT record from the buffer
		/// </summary>
		private unsafe bool ProcessMftRecord(byte* buffer, UInt64 length, UInt32 nodeIndex, out Node node, List<Stream> streams, bool isMftNode)
		{
			node = new Node();

			FileRecordHeader* ntfsFileRecordHeader = (FileRecordHeader*)buffer;

			node.MFTRecordNumber = ntfsFileRecordHeader->MFTRecordNumber;

			node.SequenceNumber = ntfsFileRecordHeader->BaseFileRecord.SequenceNumber;

			if (ntfsFileRecordHeader->RecordHeader.Type != RecordType.File)
			{ return false; }

			//the inode is not in use
			if ((ntfsFileRecordHeader->Flags & 1) != 1)
			{ return false; }

			UInt64 baseInode = ((UInt64)ntfsFileRecordHeader->BaseFileRecord.InodeNumberHighPart << 32) + ntfsFileRecordHeader->BaseFileRecord.InodeNumberLowPart;

			//This is an inode extension used in an AttributeAttributeList of another inode, don't parse it
			if (baseInode != 0)
			{ return false; }

			if (ntfsFileRecordHeader->AttributeOffset >= length)
			{ throw new Exception("Error: attributes in Inode %I64u are outside the FILE record, the MFT may be corrupt."); }

			if (ntfsFileRecordHeader->BytesInUse > length)
			{ throw new Exception("Error: in Inode %I64u the record is bigger than the size of the buffer, the MFT may be corrupt."); }

			//make the file appear in the rootdirectory by default
			node.ParentNodeIndex = ROOTDIRECTORY;

			if ((ntfsFileRecordHeader->Flags & 2) == 2)
			{ node.Attributes |= Attributes.Directory; }

			ProcessAttributes(ref node, nodeIndex, buffer + ntfsFileRecordHeader->AttributeOffset, length - ntfsFileRecordHeader->AttributeOffset, 65535, 0, streams, isMftNode);

			return true;
		}

		/// <summary>
		/// Process the bitmap data that contains information on inode usage.
		/// </summary>
		private unsafe byte[] ProcessBitmapData(List<Stream> streams)
		{
			UInt64 Vcn = 0;
			UInt64 MaxMftBitmapBytes = 0;

			Stream bitmapStream = SearchStream(streams, AttributeType.AttributeBitmap);
			if (bitmapStream == null)
			{ throw new Exception("No Bitmap Data"); }

			foreach (Fragment fragment in bitmapStream.Fragments)
			{
				if (fragment.Lcn != VIRTUALFRAGMENT)
				{
					MaxMftBitmapBytes += (fragment.NextVcn - Vcn) * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster;
				}

				Vcn = fragment.NextVcn;
			}

			byte[] bitmapData = new byte[MaxMftBitmapBytes];

			fixed (byte* bitmapDataPtr = bitmapData)
			{
				Vcn = 0;
				UInt64 RealVcn = 0;

				foreach (Fragment fragment in bitmapStream.Fragments)
				{
					if (fragment.Lcn != VIRTUALFRAGMENT)
					{
						ReadFile(
							bitmapDataPtr + RealVcn * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster,
							(fragment.NextVcn - Vcn) * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster,
							fragment.Lcn * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster
							);

						RealVcn = RealVcn + fragment.NextVcn - Vcn;
					}

					Vcn = fragment.NextVcn;
				}
			}

			return bitmapData;
		}

		/// <summary>
		/// Begin the process of interpreting MFT data
		/// </summary>
		private unsafe Node[] ProcessMft()
		{
			//64 KB seems to be optimal for Windows XP, Vista is happier with 256KB...
			uint bufferSize = (Environment.OSVersion.Version.Major >= 6 ? 256u : 64u) * 1024;

			byte[] data = new byte[bufferSize];

			fixed (byte* buffer = data)
			{
				//Read the $MFT record from disk into memory, which is always the first record in the MFT. 
				ReadFile(buffer, _diskInfo.BytesPerMftRecord, _diskInfo.MftStartLcn * _diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster);

				//Fixup the raw data from disk. This will also test if it's a valid $MFT record.
				FixupRawMftdata(buffer, _diskInfo.BytesPerMftRecord);

				List<Stream> mftStreams = new List<Stream>();

				if ((_retrieveMode & RetrieveMode.StandardInformations) == RetrieveMode.StandardInformations)
				{
					_standardInformations = new StandardInformation[1]; //allocate some space for $MFT record
				}

				Node mftNode;
				if (!ProcessMftRecord(buffer, _diskInfo.BytesPerMftRecord, 0, out mftNode, mftStreams, true))
				{ throw new Exception("Can't interpret Mft Record"); }

				//the bitmap data contains all used inodes on the disk
				_bitmapData = ProcessBitmapData(mftStreams);

				OnBitmapDataAvailable();

				Stream dataStream = SearchStream(mftStreams, AttributeType.AttributeData);

				UInt32 maxInode = (UInt32)_bitmapData.Length * 8;
				if (maxInode > (UInt32)(dataStream.Size / _diskInfo.BytesPerMftRecord))
				{
					maxInode = (UInt32)(dataStream.Size / _diskInfo.BytesPerMftRecord);
				}

				Node[] nodes = new Node[maxInode];
				nodes[0] = mftNode;

				if ((_retrieveMode & RetrieveMode.StandardInformations) == RetrieveMode.StandardInformations)
				{
					StandardInformation mftRecordInformation = _standardInformations[0];
					_standardInformations = new StandardInformation[maxInode];
					_standardInformations[0] = mftRecordInformation;
				}

				if ((_retrieveMode & RetrieveMode.Streams) == RetrieveMode.Streams)
				{
					_streams = new Stream[maxInode][];
				}

				/* Read and process all the records in the MFT. The records are read into a
                   buffer and then given one by one to the InterpretMftRecord() subroutine. */

				UInt64 BlockStart = 0, BlockEnd = 0;
				UInt64 RealVcn = 0, Vcn = 0;

				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				ulong totalBytesRead = 0;
				int fragmentIndex = 0;
				int fragmentCount = dataStream.Fragments.Count;
				for (UInt32 nodeIndex = 1; nodeIndex < maxInode; nodeIndex++)
				{
					// Ignore the Inode if the bitmap says it's not in use.
					if ((_bitmapData[nodeIndex >> 3] & BitmapMasks[nodeIndex % 8]) == 0)
					{
						continue;
					}

					if (nodeIndex >= BlockEnd)
					{
						if (!ReadNextChunk(
								buffer,
								bufferSize,
								nodeIndex,
								fragmentIndex,
								dataStream,
								ref BlockStart,
								ref BlockEnd,
								ref Vcn,
								ref RealVcn))
							break;

						totalBytesRead += (BlockEnd - BlockStart) * _diskInfo.BytesPerMftRecord;
					}

					FixupRawMftdata(
							buffer + (nodeIndex - BlockStart) * _diskInfo.BytesPerMftRecord,
							_diskInfo.BytesPerMftRecord
						);

					List<Stream> streams = null;
					if ((_retrieveMode & RetrieveMode.Streams) == RetrieveMode.Streams)
					{
						streams = new List<Stream>();
					}

					Node newNode;
					if (!ProcessMftRecord(
							buffer + (nodeIndex - BlockStart) * _diskInfo.BytesPerMftRecord,
							_diskInfo.BytesPerMftRecord,
							nodeIndex,
							out newNode,
							streams,
							false))
					{ continue; }

					nodes[nodeIndex] = newNode;

					if (streams != null)
					{
						_streams[nodeIndex] = streams.ToArray();
					}
				}

				stopwatch.Stop();

				Trace.WriteLine(
					string.Format(
						"{0:F3} MB of volume metadata has been read in {1:F3} s at {2:F3} MB/s",
						(float)totalBytesRead / (1024 * 1024),
						(float)stopwatch.Elapsed.TotalSeconds,
						((float)totalBytesRead / (1024 * 1024)) / stopwatch.Elapsed.TotalSeconds
					)
				);

				return nodes;
			}
		}

		#endregion
	}
}
