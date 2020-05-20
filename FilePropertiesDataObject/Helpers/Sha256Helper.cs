using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FilePropertiesDataObject.Helpers
{
	public class Sha256Helper
	{
		private static int bigChunkSize = 1024 * 256;

		public static string GetSha256Hash_Stream(Stream fileStream)
		{
			string result = string.Empty;

			try
			{
				using (SHA256Managed hashAlgorithm = new SHA256Managed())
				{
					long bytesToHash = fileStream.Length;

					byte[] buffer = new byte[bigChunkSize];
					int sizeToRead = buffer.Length;
					while (bytesToHash > 0)
					{
						if (bytesToHash < (long)sizeToRead)
						{
							sizeToRead = (int)bytesToHash;
						}

						int bytesRead = fileStream.ReadAsync(buffer, 0, sizeToRead, CancellationHelper.GetCancellationToken()).Result;
						CancellationHelper.ThrowIfCancelled();
						hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
						bytesToHash -= (long)bytesRead;
						if (bytesRead == 0)
						{
							throw new InvalidOperationException("Unexpected end of stream");
							// or break;
						}
					}
					hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
					buffer = null;
					result = ByteArrayConverter.ToHexString(hashAlgorithm.Hash);
				}
			}
			catch
			{ }

			return result;
		}

		public static string GetSha256Hash_IEnumerable(IEnumerable<byte[]> fileChunks, ulong fileSize)
		{
			string result = string.Empty;

			try
			{
				using (SHA256Managed hashAlgorithm = new SHA256Managed())
				{
					foreach (byte[] chunk in fileChunks)
					{
						hashAlgorithm.TransformBlock(chunk, 0, chunk.Length, null, 0);
					}

					hashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);

					result = ByteArrayConverter.ToHexString(hashAlgorithm.Hash);
				}
			}
			catch
			{ }

			return result;
		}

		public static string GetSha256Hash_Array(byte[] fileBytes)
		{
			string result = string.Empty;

			try
			{
				byte[] hashBytes = null;
				using (SHA256Managed managed = new SHA256Managed())
				{
					hashBytes = managed.ComputeHash(fileBytes);
				}
				result = ByteArrayConverter.ToHexString(hashBytes);
			}
			catch
			{ }

			return result;
		}

		private static class ByteArrayConverter
		{
			private static readonly uint[] _lookup32 = CreateLookup32();

			private static uint[] CreateLookup32()
			{
				string s = null;
				uint[] result = new uint[256];
				for (int i = 0; i < 256; i++)
				{
					s = i.ToString("X2");
					result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
				}
				return result;
			}

			public static string ToHexString(byte[] bytes)
			{
				string result = null;
				//uint[] localCopy_lookup32 = _lookup32;
				char[] buffer = new char[bytes.Length * 2];
				for (int i = 0; i < bytes.Length; i++)
				{
					uint val = _lookup32[bytes[i]]; // localCopy_lookup32[bytes[i]];
					buffer[2 * i] = (char)val;
					buffer[2 * i + 1] = (char)(val >> 16);
				}
				//localCopy_lookup32 = null;
				result = new string(buffer);
				buffer = null;
				return result;
			}
		}
	}
}
