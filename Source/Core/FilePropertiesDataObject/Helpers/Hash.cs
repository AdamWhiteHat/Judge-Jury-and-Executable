using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace FilePropertiesDataObject.Helpers
{
	public class Hash
	{
		private static int bigChunkSize = 1024 * 256;

		public static class ByteEnumerable
		{
			public static string Sha256(IEnumerable<byte[]> fileChunks)
			{
				return GetHashFromByteIEnumerable<SHA256Managed>(fileChunks);
			}

			public static string Sha1(IEnumerable<byte[]> fileChunks)
			{
				return GetHashFromByteIEnumerable<SHA1Managed>(fileChunks);
			}

			public static string MD5(IEnumerable<byte[]> fileChunks)
			{
				return GetHashFromByteIEnumerable<MD5Cng>(fileChunks);
			}
		}

		public static class ByteArray
		{
			public static string Sha256(byte[] fileBytes)
			{
				return GetHashFromByteArray<SHA256Managed>(fileBytes);
			}

			public static string Sha1(byte[] fileBytes)
			{
				return GetHashFromByteArray<SHA1Managed>(fileBytes);
			}

			public static string MD5(byte[] fileBytes)
			{
				return GetHashFromByteArray<MD5Cng>(fileBytes);
			}
		}

		#region Private Members

		private static string GetHashFromByteArray<HASHER>(byte[] fileBytes) where HASHER : HashAlgorithm, new()
		{
			string result = string.Empty;
			try
			{
				byte[] hashBytes = null;
				using (HASHER managed = new HASHER())
				{
					hashBytes = managed.ComputeHash(fileBytes);
				}
				result = ByteArrayConverter.ToHexString(hashBytes);
			}
			catch { }

			return result;
		}

		private static string GetHashFromByteIEnumerable<HASHER>(IEnumerable<byte[]> fileChunks) where HASHER : HashAlgorithm, new()
		{
			string result = string.Empty;
			try
			{
				using (HASHER hashAlgorithm = new HASHER())
				{
					foreach (byte[] chunk in fileChunks)
					{
						hashAlgorithm.TransformBlock(chunk, 0, chunk.Length, null, 0);
					}

					hashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);

					result = ByteArrayConverter.ToHexString(hashAlgorithm.Hash);
				}
			}
			catch { }

			return result;
		}

		private static string GetHashFromByteStream<HASHER>(Stream fileStream) where HASHER : HashAlgorithm, new()
		{
			string result = string.Empty;
			try
			{
				using (HASHER hashAlgorithm = new HASHER())
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
							throw new InvalidOperationException("Unexpected end of stream"); // or break;
						}
					}
					hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
					buffer = null;
					result = ByteArrayConverter.ToHexString(hashAlgorithm.Hash);
				}
			}
			catch { }

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

		#endregion

	}
}
