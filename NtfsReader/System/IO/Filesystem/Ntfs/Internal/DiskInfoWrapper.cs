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
using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Filesystem.Ntfs;
using System.IO.Filesystem.Ntfs.Internal;

namespace System.IO.Filesystem.Ntfs.Internal
{
	/// <summary>
	/// Simple structure of available disk informations.
	/// </summary>
	public sealed class DiskInfoWrapper : IDiskInfo
	{
		public UInt16 BytesPerSector;
		public byte SectorsPerCluster;
		public UInt64 TotalSectors;
		public UInt64 MftStartLcn;
		public UInt64 Mft2StartLcn;
		public UInt32 ClustersPerMftRecord;
		public UInt32 ClustersPerIndexRecord;
		public UInt64 BytesPerMftRecord;
		public UInt64 BytesPerCluster;
		public UInt64 TotalClusters;

		#region IDiskInfo Members

		ushort IDiskInfo.BytesPerSector
		{
			get { return BytesPerSector; }
		}

		byte IDiskInfo.SectorsPerCluster
		{
			get { return SectorsPerCluster; }
		}

		ulong IDiskInfo.TotalSectors
		{
			get { return TotalSectors; }
		}

		ulong IDiskInfo.MftStartLcn
		{
			get { return MftStartLcn; }
		}

		ulong IDiskInfo.Mft2StartLcn
		{
			get { return Mft2StartLcn; }
		}

		uint IDiskInfo.ClustersPerMftRecord
		{
			get { return ClustersPerMftRecord; }
		}

		uint IDiskInfo.ClustersPerIndexRecord
		{
			get { return ClustersPerIndexRecord; }
		}

		ulong IDiskInfo.BytesPerMftRecord
		{
			get { return BytesPerMftRecord; }
		}

		ulong IDiskInfo.BytesPerCluster
		{
			get { return BytesPerCluster; }
		}

		ulong IDiskInfo.TotalClusters
		{
			get { return TotalClusters; }
		}

		#endregion
	}
}
