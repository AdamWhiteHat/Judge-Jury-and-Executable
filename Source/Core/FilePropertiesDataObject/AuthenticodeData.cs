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

		public static AuthenticodeData GetAuthenticodeData(X509Certificate2 cert)
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
