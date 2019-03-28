using System;
using System.Diagnostics;
using NTFSLib.Objects.Attributes;
using NTFSLib.Objects.Enums;
using NTFSLib.Objects.Headers;

namespace NTFSLib.Objects
{
	public class IndexEntry : IEquatable<IndexEntry>
	{
		public FileReference FileRefence { get; set; }
		public ushort Size { get; set; }
		public ushort StreamSize { get; set; }
		public MFTIndexEntryFlags Flags { get; set; }
		public byte[] Stream { get; set; }
		public uint FileId { get { return FileRefence.FileId; } }

		public AttributeFileName ChildFileName { get; set; }

		public static IndexEntry ParseData(byte[] data, int maxLength, int offset)
		{
			Debug.Assert(maxLength >= 16);

			IndexEntry res = new IndexEntry();

			res.FileRefence = new FileReference(BitConverter.ToUInt64(data, offset));
			res.Size = BitConverter.ToUInt16(data, offset + 8);
			res.StreamSize = BitConverter.ToUInt16(data, offset + 10);
			res.Flags = (MFTIndexEntryFlags)data[offset + 12];

			Debug.Assert(maxLength >= res.Size);
			Debug.Assert(maxLength >= 16 + res.StreamSize);

			res.Stream = new byte[res.StreamSize];
			Array.Copy(data, offset + 16, res.Stream, 0, res.StreamSize);


			if (res.StreamSize > 66)
			{
				res.ChildFileName = new AttributeFileName();
				res.ChildFileName.ParseAttributeResidentBody(res.Stream, res.StreamSize, 0);

				// Fake the resident header
				res.ChildFileName.ResidentHeader = new AttributeResidentHeader();
				res.ChildFileName.ResidentHeader.ContentLength = res.StreamSize;
				res.ChildFileName.ResidentHeader.ContentOffset = 0;
			}

			return res;
		}

		public bool Equals(IndexEntry other)
		{
			if (other == null) return false;
			if (other.FileRefence == null) return false;
			return (this.FileRefence.FileId == other.FileRefence.FileId);
		}

		private static ulong largePrime = 5261343380158614487;
		public override int GetHashCode()
		{
			unchecked
			{
				UInt64 product = (this.FileRefence.FileId * largePrime);
				return (int)(product % ((uint)Int32.MaxValue));
			}
		}
		public override string ToString()
		{
			return (ChildFileName == null) ? FileRefence.ToString() : FileRefence + " (" + ChildFileName.FileName + ")";
		}

	}
}