using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using PeNet;
using PeNet.ImpHash;

namespace FilePropertiesDataObject
{
	public class PeDataObject
	{
		public string SHA256Hash { get; private set; }
		public string SHA1Hash { get; private set; }
		public string MD5Hash { get; private set; }

		public string ImpHash { get; private set; }
		public bool IsDll { get; private set; }
		public bool IsPe { get; private set; }
		public bool IsDriver { get; private set; }
		public bool IsSigned { get; private set; }
		public bool IsValidCertChain { get; private set; }

		public DateTime CompileDate { get; private set; }

		public int BinaryType { get; private set; }

		public static PeDataObject TryGetPeDataObject(byte[] fileBytes, bool onlineCertValidation)
		{
			return Internal_TryGetPeDataObject(null, fileBytes, onlineCertValidation);
		}

		public static PeDataObject TryGetPeDataObject(string filename, bool onlineCertValidation)
		{
			return Internal_TryGetPeDataObject(filename, null, onlineCertValidation);
		}

		private static PeDataObject Internal_TryGetPeDataObject(string filename, byte[] fileBytes, bool onlineCertValidation)
		{
			PeDataObject result = null;

			bool useFileBytes;
			if (fileBytes == null)
			{
				if (filename == null)
				{
					return result;
				}
				useFileBytes = false;
			}
			else
			{
				useFileBytes = true;
			}

			try
			{
				if (fileBytes.Length > 0)
				{
					PeFile peFile = useFileBytes ? new PeFile(fileBytes) : new PeFile(filename);
					result = new PeDataObject(peFile, onlineCertValidation);
				}
			}
			catch
			{ }

			return result;
		}

		private PeDataObject(PeFile peFile, bool onlineCertValidation)
		{
			this.SHA256Hash = peFile.SHA256;
			this.SHA1Hash = peFile.SHA1;
			this.MD5Hash = peFile.MD5;
			this.ImpHash = peFile.ImpHash;

			if (!string.IsNullOrWhiteSpace(this.SHA256Hash))
			{
				this.SHA256Hash = this.SHA256Hash.ToUpperInvariant();
			}
			if (!string.IsNullOrWhiteSpace(this.SHA1Hash))
			{
				this.SHA1Hash = this.SHA1Hash.ToUpperInvariant();
			}
			if (!string.IsNullOrWhiteSpace(this.MD5Hash))
			{
				this.MD5Hash = this.MD5Hash.ToUpperInvariant();
			}
			if (!string.IsNullOrWhiteSpace(this.ImpHash))
			{
				this.ImpHash = this.ImpHash.ToUpperInvariant();
			}

			this.IsDll = peFile.IsDLL;
			this.IsPe = peFile.IsValidPeFile;
			this.IsDriver = peFile.IsDriver;
			this.IsSigned = peFile.IsSigned;
			this.IsValidCertChain = peFile.IsValidCertChain(onlineCertValidation);

			if (IsPe || IsDll || IsDriver)
			{
				if (peFile.Is32Bit)
				{
					this.BinaryType = 32;
				}
				else if (peFile.Is64Bit)
				{
					this.BinaryType = 16;
				}
			}

			PeNet.Structures.IMAGE_FILE_HEADER fileHeader = peFile.ImageNtHeaders.FileHeader;

			if (fileHeader != null)
			{
				this.CompileDate = DateTimeOffset.FromUnixTimeSeconds(fileHeader.TimeDateStamp).DateTime;
			}
		}
	}
}
