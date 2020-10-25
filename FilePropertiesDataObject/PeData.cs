using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using PeNet;
using PeNet.ImpHash;
using System.Security.Cryptography.X509Certificates;

namespace FilePropertiesDataObject
{
	public class PeDataObject
	{
		public string SHA256Hash { get; private set; }
		public string SHA1Hash { get; private set; }
		public string MD5Hash { get; private set; }

		public string ImpHash { get; private set; }
		public bool IsDll { get; private set; }
		public bool IsExe { get; private set; }
		public bool IsDriver { get; private set; }
		public bool IsSigned { get; private set; }

		public bool IsSignatureValid { get; private set; }
		public bool IsValidCertChain { get; private set; }

		public DateTime CompileDate { get; private set; }

		public int BinaryType { get; private set; }

		public X509Certificate2 Certificate { get; private set; }

		public static PeDataObject TryGetPeDataObject(byte[] fileBytes)
		{
			return Internal_TryGetPeDataObject(null, fileBytes);
		}

		public static PeDataObject TryGetPeDataObject(string filename)
		{
			return Internal_TryGetPeDataObject(filename, null);
		}

		private static PeDataObject Internal_TryGetPeDataObject(string filename, byte[] fileBytes)
		{
			PeDataObject result = null;

			bool useFileBytes;
			if (filename == null)
			{
				if (fileBytes == null || fileBytes.Length == 0)
				{
					return result;
				}
				useFileBytes = true;
			}
			else
			{
				useFileBytes = false;
			}

			try
			{
				bool peParseSuccessfully = false;

				PeFile peFile = null;
				if (useFileBytes)
				{
					if (PeFile.TryParse(fileBytes, out peFile))
					{
						peParseSuccessfully = true;
					}
				}
				else
				{
					if (PeFile.TryParse(filename, out peFile))
					{
						peParseSuccessfully = true;
					}
				}

				if (peParseSuccessfully)
				{
					result = new PeDataObject(peFile);
				}
			}
			catch
			{ }

			return result;
		}

		private PeDataObject(PeFile peFile)
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
			this.IsExe = peFile.IsEXE;
			this.IsDriver = peFile.IsDriver;
			this.IsSigned = peFile.IsSigned;
			this.IsSignatureValid = peFile.IsSignatureValid;
			this.Certificate = peFile.PKCS7;

			if (this.Certificate != null)
			{
				this.IsValidCertChain = this.Certificate.Verify();
			}

			if (IsExe || IsDll || IsDriver)
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
