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
    /// Add some functionality to the basic node
    /// </summary>
    public sealed class NodeWrapper : INode
    {
        NtfsReader _reader;
        UInt32 _nodeIndex;
        Node _node;
        string _fullName;


        public NodeWrapper(NtfsReader reader, UInt32 nodeIndex, Node node)
        {
            _reader = reader;
            _nodeIndex = nodeIndex;
            _node = node;
        }

        public UInt32 NodeIndex
        {
            get { return _nodeIndex; }
        }

        public UInt32 ParentNodeIndex
        {
            get { return _node.ParentNodeIndex; }
        }

        public Attributes Attributes
        {
            get { return _node.Attributes; }
        }

        public string Name
        {
            get { return _reader.GetNameFromIndex(_node.NameIndex); }
        }

        public UInt64 Size
        {
            get { return _node.Size; }
            set { _node.Size = value; }
        }

        public string FullName
        {
            get
            {
                if (_fullName == null)
                    _fullName = _reader.GetNodeFullNameCore(_nodeIndex);

                return _fullName;
            }
        }

        public IList<IStream> Streams
        {
            get
            {
                if (_reader._streams == null)
                    throw new NotSupportedException("The streams haven't been retrieved. Make sure to use the proper RetrieveMode.");

                Stream[] streams = _reader._streams[_nodeIndex];
                if (streams == null)
                    return null;

                List<IStream> newStreams = new List<IStream>();
                for (int i = 0; i < streams.Length; ++i)
                    newStreams.Add(new StreamWrapper(_reader, this, i));

                return newStreams;
            }
        }

        public UInt32 MFTRecordNumber
        {
            get { return _node.MFTRecordNumber; }
        }

        public UInt16 SequenceNumber
        {
            get { return _node.SequenceNumber; }
        }

        public DateTime CreationTime
        {
            get
            {
                if (_reader._standardInformations == null)
                    throw new NotSupportedException("The StandardInformation haven't been retrieved. Make sure to use the proper RetrieveMode.");

                return DateTime.FromFileTimeUtc((Int64)_reader._standardInformations[_nodeIndex].CreationTime);
            }
        }

        public DateTime LastChangeTime
        {
            get
            {
                if (_reader._standardInformations == null)
                    throw new NotSupportedException("The StandardInformation haven't been retrieved. Make sure to use the proper RetrieveMode.");

                return DateTime.FromFileTimeUtc((Int64)_reader._standardInformations[_nodeIndex].LastChangeTime);
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                if (_reader._standardInformations == null)
                    throw new NotSupportedException("The StandardInformation haven't been retrieved. Make sure to use the proper RetrieveMode.");

                return DateTime.FromFileTimeUtc((Int64)_reader._standardInformations[_nodeIndex].LastAccessTime);
            }
        }

        public DateTime TimeMftModified
        {
            get
            {
                if (_reader._standardInformations == null)
                    throw new NotSupportedException("The StandardInformation haven't been retrieved. Make sure to use the proper RetrieveMode.");

                return DateTime.FromFileTimeUtc((Int64)_reader._standardInformations[_nodeIndex].TimeMftModified);
            }
        }

        public IEnumerable<byte[]> GetBytes()
        {
            return this._reader.ReadFileSafe(this);
        }
    }
}
