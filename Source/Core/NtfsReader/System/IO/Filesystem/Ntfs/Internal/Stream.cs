﻿/*
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
	public sealed class Stream
	{
		public UInt64 Clusters;                      // Total number of clusters.
		public UInt64 Size;                          // Total number of bytes.
		public AttributeType Type;
		public int NameIndex;
		public List<Fragment> _fragments;

		public Stream(int nameIndex, AttributeType type, UInt64 size)
		{
			NameIndex = nameIndex;
			Type = type;
			Size = size;
		}

		public List<Fragment> Fragments
		{
			get
			{
				if (_fragments == null)
					_fragments = new List<Fragment>(5);

				return _fragments;
			}
		}
	}
}
