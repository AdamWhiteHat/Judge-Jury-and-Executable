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
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Attribute
	{
		public AttributeType AttributeType;
		public UInt32 Length;
		public byte Nonresident;
		public byte NameLength;
		public UInt16 NameOffset;
		public UInt16 Flags;              /* 0x0001 = Compressed, 0x4000 = Encrypted, 0x8000 = Sparse */
		public UInt16 AttributeNumber;
	}
}
