using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace FilePropertiesDataObject
{
    public static class EntropyHelper
    {
        public static double? CalculateFileEntropy(byte[] fileBytes)
        {
            double? result = null;
            CancellationHelper.ThrowIfCancelled();
            long uncompressedLength = fileBytes.Length;
            if (uncompressedLength > 20)
            {
                long compressedLength = GetCompressedLength(fileBytes);
                if (compressedLength > 0)
                {
                    CancellationHelper.ThrowIfCancelled();
                    result = CalculateRatio(uncompressedLength, compressedLength);
                }
            }

            return result;
        }

        private static double CalculateRatio(long uncompressedLength, long compressedLength)
        {
            double result = (double)compressedLength / (double)uncompressedLength;
            result = result * 100;

            if (result > 100)
            {
                result = 100;
            }

            return result;
        }

        private static long GetCompressedLength(byte[] fileBytes)
        {
            long result = 0;

            try
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        using (MemoryStream byteStream = new MemoryStream(fileBytes))
                        {
                            byteStream.CopyToAsync(gzip, 81920, CancellationHelper.GetCancellationToken()).Wait(CancellationHelper.GetCancellationToken());
                            result = outStream.Length;
                        }
                    }
                }
            }
            catch
            { }

            return result;
        }
    }
}
