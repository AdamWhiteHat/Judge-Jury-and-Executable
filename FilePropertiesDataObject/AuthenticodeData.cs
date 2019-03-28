using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FilePropertiesDataObject
{
	public class AuthenticodeData
	{
		public string CertSubject { get; private set; }
		public string CertIssuer { get; private set; }
		public string CertSerialNumber { get; private set; }
		public string CertThumbprint { get; private set; }
		public string CertNotBefore { get; private set; }
		public string CertNotAfter { get; private set; }

		// This prevents the class from being instantiated.
		// This makes it behave kinda like a static class
		// (since all the methods on it are static and it cannot be instantiated)
		// but unlike a static class, it has public properties.
		// A static class would just have members instead (no get/set)
		private AuthenticodeData()
		{
		}

		public static AuthenticodeData TryGetAuthenticodeData(string filename)
		{
			return Internal_TryGetAuthenticodeData(filename, null);
		}

		public static AuthenticodeData TryGetAuthenticodeData(byte[] fileBytes)
		{
			return Internal_TryGetAuthenticodeData(null, fileBytes);
		}

		// Instead of writing two identical methods, except for a single line,
		// we will just use an if statement and branch the logic.
		// Having to maintain two versions of essentially the same code sucks. 
		// And you are setting yourself up for the scenario wherein you update one
		// but forget to update the other. This eliminates that possibility.
		// There is a term for this kind of 'mistake-proofing': a poka-yoke.
		private static AuthenticodeData Internal_TryGetAuthenticodeData(string filename, byte[] fileBytes)
		{
			AuthenticodeData result = null;

			bool useFileBytes;
			if (filename == null)
			{
				useFileBytes = true;
			}
			else if (fileBytes == null)
			{
				useFileBytes = false;
			}
			else
			{
				return result;
			}

			try
			{
				if (File.Exists(filename))
				{
					using (X509Certificate cert = useFileBytes ? new X509Certificate(fileBytes) : new X509Certificate(filename))
					{
						if (cert != null)
						{
							result = ExtractCertData(cert);
						}
					}
				}
			}
			catch
			{ }

			return result;
		}

		private static AuthenticodeData ExtractCertData(X509Certificate cert)
		{
			AuthenticodeData result = null;
			if (cert != null)
			{
				result = new AuthenticodeData()
				{					
					CertSubject = cert.Subject,
					CertIssuer = cert.Issuer,
					CertSerialNumber = cert.GetSerialNumberString(),
					CertThumbprint = cert.GetCertHashString(),
					CertNotBefore = cert.GetEffectiveDateString(),
					CertNotAfter = cert.GetExpirationDateString()
				};
			}
			return result;
		}
	}
}
