using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FilePropertiesDataObject.Helpers
{
	public static class EntropyHelper
	{
		private static int chunkSize = 81920;

		#region IEnumerable Iteration

		public static double? CalculateFileEntropy(IEnumerable<byte[]> fileChunks, ulong fileSize)
		{
			double? result = null;
			CancellationHelper.ThrowIfCancelled();
			ulong uncompressedLength = fileSize;
			if (uncompressedLength > 20)
			{
				ulong compressedLength = GetCompressedLength(fileChunks);
				if (compressedLength > 0)
				{
					CancellationHelper.ThrowIfCancelled();
					result = CalculateRatio(uncompressedLength, compressedLength);
				}
			}

			return result;
		}

		private static ulong GetCompressedLength(IEnumerable<byte[]> fileChunks)
		{
			ulong result = 0;

			try
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					using (GZipStream gzip = new GZipStream(outStream, CompressionMode.Compress))
					{
						using (MemoryStream inStream = new MemoryStream(Int16.MaxValue))
						{
							foreach (byte[] chunk in fileChunks)
							{
								inStream.Write(chunk, 0, chunk.Length);
							}

							Task copyTask = inStream.CopyToAsync(gzip, chunkSize, CancellationHelper.GetCancellationToken());
							copyTask.Wait(CancellationHelper.GetCancellationToken());
							result = (ulong)outStream.Length;
						}
					}
				}
			}
			catch
			{ }

			return result;
		}

		#endregion

		#region Static Array

		public static double? CalculateFileEntropy(byte[] fileBytes)
		{
			double? result = null;
			CancellationHelper.ThrowIfCancelled();
			ulong uncompressedLength = (ulong)fileBytes.LongLength;
			if (uncompressedLength > 20)
			{
				ulong compressedLength = GetCompressedLength(fileBytes);
				if (compressedLength > 0)
				{
					CancellationHelper.ThrowIfCancelled();
					result = CalculateRatio(uncompressedLength, compressedLength);
				}
			}

			return result;
		}

		private static ulong GetCompressedLength(byte[] fileBytes)
		{
			ulong result = 0;

			try
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					using (GZipStream gzip = new GZipStream(outStream, CompressionMode.Compress))
					{
						using (MemoryStream inStream = new MemoryStream(fileBytes))
						{
							Task copyTask = inStream.CopyToAsync(gzip, chunkSize, CancellationHelper.GetCancellationToken());
							copyTask.Wait(CancellationHelper.GetCancellationToken());
							result = (ulong)outStream.Length;
						}
					}
				}
			}
			catch
			{ }

			return result;
		}

		#endregion

		private static double CalculateRatio(ulong uncompressedLength, ulong compressedLength)
		{
			double result = (double)compressedLength / (double)uncompressedLength;
			result = result * 100;

			if (result > 100)
			{
				result = 100;
			}

			return result;
		}
	}
}
