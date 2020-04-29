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


using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using RawDiskLib;
using System.IO.Filesystem.Ntfs.Internal;

namespace System.IO.Filesystem.Ntfs
{
	/// <summary>
	/// Ntfs metadata reader.
	/// 
	/// This class is used to get files & directories information of an NTFS volume.
	/// This is a lot faster than using conventional directory browsing method
	/// particularly when browsing really big directories.
	/// </summary>
	/// <remarks>Admnistrator rights are required in order to use this method.</remarks>
	public partial class NtfsReader
	{
		/// <summary>
		/// NtfsReader constructor.
		/// </summary>
		/// <param name="driveInfo">The drive you want to read metadata from.</param>
		/// <param name="include">Information to retrieve from each node while scanning the disk</param>
		/// <remarks>Streams & Fragments are expensive to store in memory, if you don't need them, don't retrieve them.</remarks>
		public NtfsReader(DriveInfo driveInfo, RetrieveMode retrieveMode)
		{
			if (driveInfo == null)
			{
				throw new ArgumentNullException("driveInfo");
			}

			DriveInfo tmpDriveInfo = driveInfo;

			//try to find if the drive is mapped on a local volume
			if (driveInfo.DriveType != DriveType.Fixed)
			{
				tmpDriveInfo = ResolveLocalMapDrive(driveInfo);
			}

			_rootPath = tmpDriveInfo.Name;

			StringBuilder builder = new StringBuilder(1024);
			GetVolumeNameForVolumeMountPoint(tmpDriveInfo.RootDirectory.Name, builder, builder.Capacity);

			_driveInfo = driveInfo;
			_retrieveMode = retrieveMode;

			string volume = driveInfo.Name;

			if (builder != null)
			{
				string volumeName = builder.ToString().TrimEnd(new char[] { '\\' });
				if (!string.IsNullOrWhiteSpace(volumeName))
				{
					volume = volumeName;
				}
			}

			_volumeHandle =
				CreateFile(
					volume,
					FileAccess.Read,
					FileShare.All,
					IntPtr.Zero,
					FileMode.Open,
					0,
					IntPtr.Zero
					);

			if (_volumeHandle == null || _volumeHandle.IsInvalid)
				throw new IOException(
					string.Format(
						"Unable to open volume {0}. Make sure it exists and that you have Administrator privileges.",
						driveInfo
					)
				);

			InitializeDiskInfo();

			_nodes = ProcessMft();

			//cleanup anything that isn't used anymore
			_nameIndex = null;

			//GC.Collect();
		}

		/// <summary>
		/// Get the drive on which this instance is bound to.
		/// </summary>
		public DriveInfo DriveInfo
		{
			get { return _driveInfo; }
		}

		/// <summary>
		/// Get information about the NTFS volume.
		/// </summary>
		public IDiskInfo DiskInfo
		{
			get { return _diskInfo; }
		}

		/// <summary>
		/// Get a single node that match exactly the given path
		/// </summary>
		public INode GetNode(string fullPath)
		{
			foreach (INode node in GetNodes(fullPath))
				if (string.Equals(node.FullName, fullPath, StringComparison.OrdinalIgnoreCase))
					return node;

			return null;
		}

		/// <summary>
		/// Get all nodes under the specified rootPath.
		/// </summary>
		/// <param name="rootPath">The rootPath must at least contains the drive and may include any number of subdirectories. Wildcards aren't supported.</param>
		public List<INode> GetNodes(string rootPath)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			List<INode> nodes = new List<INode>();

			//TODO use Parallel.Net to process this when it becomes available
			UInt32 nodeCount = (UInt32)_nodes.Length;
			for (UInt32 i = 0; i < nodeCount; ++i)
				if (_nodes[i].NameIndex != 0 && GetNodeFullNameCore(i).StartsWith(rootPath, StringComparison.InvariantCultureIgnoreCase))
					nodes.Add(new NodeWrapper(this, i, _nodes[i]));

			stopwatch.Stop();

			Trace.WriteLine(
				string.Format(
					"{0} node{1} have been retrieved in {2} ms",
					nodes.Count,
					nodes.Count > 1 ? "s" : string.Empty,
					(float)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond
				)
			);

			return nodes;
		}

		public IEnumerable<byte> ReadFileSafe(INode node)
		{
			if (node.Size == 0 || !node.Streams.Any())
			{
				yield break;
			}

			ulong fileBytesRemaining = node.Size;

			using (RawDisk disk = new RawDisk(char.ToUpper(node.FullName[0])))
			{
				foreach (IStream stream in node.Streams)
				{
					ulong streamBytesRemaining = stream.Size;
					ulong lastVcn = 0;

					foreach (IFragment fragment in stream.Fragments)
					{
						if (fragment.Lcn != VIRTUALFRAGMENT)
						{
							int clustersToRead = (int)(fragment.NextVcn - lastVcn);
							byte[] chunk = disk.ReadClusters((long)(fragment.Lcn), clustersToRead);
							ulong chunkSize = (ulong)chunk.Length;
							ulong chunkBytesToRead = chunkSize;

							if (chunkSize > streamBytesRemaining)
							{
								chunkBytesToRead = streamBytesRemaining;
							}
							else if (chunkSize > fileBytesRemaining)
							{
								chunkBytesToRead = fileBytesRemaining;
							}

							fileBytesRemaining = fileBytesRemaining - chunkBytesToRead;
							streamBytesRemaining = streamBytesRemaining - chunkBytesToRead;

							foreach (byte b in chunk.Take((int)chunkBytesToRead))
							{
								yield return b;
							}

							lastVcn = fragment.NextVcn;
						}
					}
				}
			}

			yield break;
		}

		public unsafe byte[] ReadFileUnsafe(INode node)
		{
			UInt64 bytesToRead = node.Size;

			if (bytesToRead == 0 || !node.Streams.Any())
			{
				return new byte[0];
			}

			UInt64 bytesPerCluster = (UInt64)_diskInfo.BytesPerSector * _diskInfo.SectorsPerCluster;

			if (bytesToRead % bytesPerCluster > 0)
			{
				bytesToRead += bytesPerCluster - (bytesToRead % bytesPerCluster);
			}

			byte[] bitmapData = new byte[bytesToRead];

			fixed (byte* bitmapDataPtr = bitmapData)
			{
				UInt64 vcn = 0;
				UInt64 offset = 0;

				foreach (IFragment fragment in node.Streams[0].Fragments)
				{
					if (fragment.Lcn != VIRTUALFRAGMENT)
					{
						UInt64 sizeToRead = (fragment.NextVcn - vcn) * bytesPerCluster;

						ReadFile(
							bitmapDataPtr + offset,
							sizeToRead,
							fragment.Lcn * bytesPerCluster
							);

						offset += sizeToRead;
					}

					vcn = fragment.NextVcn;
				}
			}

			return bitmapData;
		}

		public byte[] GetVolumeBitmap()
		{
			return _bitmapData;
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_volumeHandle != null)
			{
				_volumeHandle.Dispose();
				_volumeHandle = null;
			}
		}

		#endregion
	}
}
