using System;
using System.IO;

namespace FilePropertiesDataObject
{
	public static class HashHelper
	{
		#region Static part

		private static System.Security.Cryptography.MD5 _md5;
		private static System.Security.Cryptography.SHA1Managed _sha1;
		private static System.Security.Cryptography.SHA256Managed _sha256;

		static HashHelper()
		{
			_md5 = System.Security.Cryptography.MD5.Create();
			_sha1 = new System.Security.Cryptography.SHA1Managed();
			_sha256 = new System.Security.Cryptography.SHA256Managed();
		}

		public static Tuple<string, string, string> GetHashes(string filepath)
		{
			byte[] fileBytes = File.ReadAllBytes(filepath);

			if (fileBytes.Length < 1)
			{
				return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
			}

			Tuple<string, string, string> result = new Tuple<string, string, string>(
				BitConverter.ToString(_sha256.ComputeHash(fileBytes)).Replace("-", String.Empty),
				BitConverter.ToString(_sha1.ComputeHash(fileBytes)).Replace("-", String.Empty),
				BitConverter.ToString(_md5.ComputeHash(fileBytes)).Replace("-", String.Empty)
			);

			fileBytes = null;

			return result;
		}

		#endregion
	}
}
