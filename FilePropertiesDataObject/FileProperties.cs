using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.AccessControl;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesDataObject
{
	public class FileProperties
	{
		#region Public properties

		public uint MFTNumber { get; private set; }
		public ushort SequenceNumber { get; private set; }
		public string Sha256Hash { get; private set; }

		public ulong Length { get; private set; }
		public char DriveLetter { get; private set; }
		public string FullPath { get; private set; }
		public string FileName { get; private set; }
		public string Extension { get; private set; }
		public string DirectoryLocation { get; private set; }

		public DateTime MftTimeAccessed { get; set; }
		public DateTime MftTimeCreation { get; set; }
		public DateTime MftTimeModified { get; set; }
		public DateTime MftTimeMftModified { get; set; }
		public DateTime CreationTime { get; private set; }
		public DateTime LastAccessTime { get; private set; }
		public DateTime LastWriteTime { get; private set; }

		public PeDataObject PeData { get; private set; }
		public AuthenticodeData Authenticode { get; private set; }
		public double? Entropy { get; private set; }
		public string YaraRulesMatched { get; private set; }

		public Attributes Attributes { get; private set; }
		public string Project { get; private set; }
		public string ProviderItemID { get; private set; }
		public string OriginalFileName { get; private set; }
		public string FileOwner { get; private set; }
		public string FileVersion { get; private set; }
		public string FileDescription { get; private set; }
		public string Trademarks { get; private set; }
		public string Copyright { get; private set; }
		public string Company { get; private set; }
		public string ApplicationName { get; private set; }
		public string Comment { get; private set; }
		public string Title { get; private set; }
		public string Link { get; private set; }
		public string ContentType { get; private set; }
		public string InternalName { get; private set; }
		public string ProductName { get; private set; }
		public string Language { get; private set; }
		public string ComputerName { get; private set; }

		public bool IsPeDataPopulated { get { return !(PeData == null); } }
		public bool IsAuthenticodePopulated { get { return !(Authenticode == null); } }
		public bool IsEntropyPopulated { get { return (Entropy.HasValue && Entropy.Value > 0); } }
		public bool IsYaraRulesMatchedPopulated { get { return !(YaraRulesMatched == null); } }

		#endregion

		public FileProperties()
		{
			PeData = null;
			Authenticode = null;
			Entropy = null;
			YaraRulesMatched = null;
		}

		public void PopulateFileProperties(FileEnumeratorParameters parameters, char driveLetter, INode node)
		{
			if (parameters == null) return;
			if (node == null) return;

			CancellationHelper.SetCancellationToken(parameters.CancelToken);
			CancellationHelper.ThrowIfCancelled();

			MFTNumber = node.MFTRecordNumber;
			SequenceNumber = node.SequenceNumber;

			DriveLetter = driveLetter;
			FileName = node.Name;

			MftTimeAccessed = node.LastAccessTime;
			MftTimeCreation = node.CreationTime;
			MftTimeModified = node.LastChangeTime;
			MftTimeMftModified = node.TimeMftModified;

			DirectoryLocation = Path.GetDirectoryName(node.FullName);
			Extension = Path.GetExtension(FileName);
			FullPath = node.FullName;

			this.Length = node.Size;

			CancellationHelper.ThrowIfCancelled();

			bool haveFileReadPermission = true;
			var fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, AccessControlActions.View, FullPath);
			if (fileIOPermission.AllFiles == FileIOPermissionAccess.Read)
			{
				haveFileReadPermission = true;
			}

			if (this.Length != 0)
			{
				var maxLength = ((ulong)NtfsReader.MaxClustersToRead * (ulong)4096);
				// Some entries in the MFT are greater than int.MaxValue !! That or the size is corrupt. Either way, we handle that here.
				if (this.Length > maxLength) //(int.MaxValue - 1))
				{
					IEnumerable<byte[]> fileChunks = node.GetBytes();
					CancellationHelper.ThrowIfCancelled();

					this.Sha256Hash = GetSha256Hash_IEnumerable(fileChunks, this.Length);
					CancellationHelper.ThrowIfCancelled();

					if (haveFileReadPermission)
					{
						this.PeData = PeDataObject.TryGetPeDataObject(FullPath, parameters.OnlineCertValidation);
						CancellationHelper.ThrowIfCancelled();

						this.Authenticode = AuthenticodeData.TryGetAuthenticodeData(FullPath);
						CancellationHelper.ThrowIfCancelled();
					}

					/*
					if (parameters.CalculateEntropy)
					{
						this.Entropy = EntropyHelper.CalculateFileEntropy(fileChunks, this.Length);
						CancellationHelper.ThrowIfCancelled();
					}
					*/

					if (haveFileReadPermission)
					{
						if (!string.IsNullOrWhiteSpace(parameters.YaraRulesFilePath) && File.Exists(parameters.YaraRulesFilePath))
						{
							this.YaraRulesMatched = YaraRules.ScanFile(FullPath, parameters.YaraRulesFilePath);
							CancellationHelper.ThrowIfCancelled();
						}
					}
				}
				else
				{
					byte[] fileBytes = node.GetBytes().SelectMany(chunk => chunk).ToArray();
					CancellationHelper.ThrowIfCancelled();

					this.PeData = PeDataObject.TryGetPeDataObject(fileBytes, parameters.OnlineCertValidation);
					if (IsPeDataPopulated)
					{
						this.Sha256Hash = PeData.SHA256Hash;
					}
					else
					{
						this.Sha256Hash = GetSha256Hash_Array(fileBytes);
					}
					CancellationHelper.ThrowIfCancelled();

					this.Authenticode = AuthenticodeData.TryGetAuthenticodeData(fileBytes);
					CancellationHelper.ThrowIfCancelled();

					if (parameters.CalculateEntropy)
					{
						this.Entropy = EntropyHelper.CalculateFileEntropy(fileBytes);
						CancellationHelper.ThrowIfCancelled();
					}

					if (!string.IsNullOrWhiteSpace(parameters.YaraRulesFilePath) && File.Exists(parameters.YaraRulesFilePath))
					{
						this.YaraRulesMatched = YaraRules.ScanBytes(fileBytes, parameters.YaraRulesFilePath);
						CancellationHelper.ThrowIfCancelled();
					}
				}
			}

			this.Attributes = new Attributes(node);
			CancellationHelper.ThrowIfCancelled();

			if (haveFileReadPermission)
			{
				PopulateFileInfoProperties(FullPath);
				CancellationHelper.ThrowIfCancelled();

				PopulateShellFileInfo(FullPath);
				CancellationHelper.ThrowIfCancelled();
			}
		}

		private void PopulateFileInfoProperties(string fullPath)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(fullPath);
				if (fileInfo != null && fileInfo.Exists)
				{
					PopulateFileTimestamps(fileInfo);
				}
			}
			catch
			{ }
		}

		private void PopulateFileTimestamps(FileInfo fileInfo)
		{
			try
			{
				CreationTime = fileInfo.CreationTimeUtc;
				LastAccessTime = fileInfo.LastAccessTimeUtc;
				LastWriteTime = fileInfo.LastWriteTimeUtc;
			}
			catch
			{ }
		}

		private void PopulateShellFileInfo(string fullPath)
		{
			try
			{
				if (!ShellFile.IsPlatformSupported)
				{
					return;
				}

				using (ShellFile file = ShellFile.FromFilePath(fullPath))
				{
					using (ShellProperties fileProperties = file.Properties)
					{
						ShellProperties.PropertySystem shellProperty = fileProperties.System;

						Project = shellProperty.Project.Value ?? "";
						ProviderItemID = shellProperty.ProviderItemID.Value ?? "";
						OriginalFileName = shellProperty.OriginalFileName.Value ?? "";
						FileOwner = shellProperty.FileOwner.Value ?? "";
						FileVersion = shellProperty.FileVersion.Value ?? "";
						FileDescription = shellProperty.FileDescription.Value ?? "";
						Trademarks = shellProperty.Trademarks.Value ?? "";
						Link = shellProperty.Link.TargetUrl.Value ?? "";
						Copyright = shellProperty.Copyright.Value ?? "";
						Company = shellProperty.Company.Value ?? "";
						ApplicationName = shellProperty.ApplicationName.Value ?? "";
						Comment = shellProperty.Comment.Value ?? "";
						Title = shellProperty.Title.Value ?? "";
						CancellationHelper.ThrowIfCancelled();
						ContentType = shellProperty.ContentType.Value ?? "";
						InternalName = shellProperty.InternalName.Value ?? "";
						ProductName = shellProperty.Software.ProductName.Value ?? "";
						Language = shellProperty.Language.Value ?? "";
						ComputerName = shellProperty.ComputerName.Value ?? "";
					}
				}
			}
			catch
			{ }
		}

		private static int bigChunkSize = 1024 * 256;
		private string GetSha256Hash_Stream(Stream fileStream)
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

		private string GetSha256Hash_IEnumerable(IEnumerable<byte[]> fileChunks, ulong fileSize)
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

		private string GetSha256Hash_Array(byte[] fileBytes)
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
