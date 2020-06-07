using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Security.AccessControl;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesDataObject
{
	using Helpers;
	using Parameters;

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

		public bool IsTrusted { get; private set; }
		public PeDataObject PeData { get; private set; }
		public AuthenticodeData Authenticode { get; private set; }
		public double? Entropy { get; private set; }
		public List<string> YaraRulesMatched { get; private set; }

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
		public string MimeType { get; private set; }
		public string InternalName { get; private set; }
		public string ProductName { get; private set; }
		public string Language { get; private set; }
		public string ComputerName { get; private set; }

		public bool IsPeDataPopulated { get { return !(PeData == null); } }
		public bool IsAuthenticodePopulated { get { return !(Authenticode == null); } }
		public bool IsEntropyPopulated { get { return (Entropy.HasValue && Entropy.Value > 0); } }
		public bool IsYaraRulesMatchedPopulated { get { return (YaraRulesMatched != null && YaraRulesMatched.Any()); } }

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

			if (this.Length == 0) //Workaround for hard links
			{
				if (System.IO.File.Exists(FullPath))
				{
					long length = new System.IO.FileInfo(FullPath).Length;
					if (length > 0)
					{
						this.Length = (ulong)length;
						node.Size = this.Length;

					}
				}
			}

			bool hasFileReadPermissions = true;
			var fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, AccessControlActions.View, FullPath);
			if (fileIOPermission.AllFiles == FileIOPermissionAccess.Read)
			{
				hasFileReadPermissions = true;
			}

			if (this.Length != 0)
			{
				var maxLength = ((ulong)NtfsReader.MaxClustersToRead * (ulong)4096);
				// Some entries in the MFT are greater than int.MaxValue !! That or the size is corrupt. Either way, we handle that here.
				if (this.Length > maxLength) //(int.MaxValue - 1))
				{
					PopulateLargeFile(parameters, node, hasFileReadPermissions);
				}
				else
				{
					PopulateSmallFile(parameters, node, hasFileReadPermissions);
				}
			}

			PopulateIsTrusted();
			CancellationHelper.ThrowIfCancelled();

			this.Attributes = new Attributes(node);
			CancellationHelper.ThrowIfCancelled();

			if (hasFileReadPermissions)
			{
				PopulateFileInfoProperties(FullPath);
				CancellationHelper.ThrowIfCancelled();

				PopulateShellFileInfo(FullPath);
				CancellationHelper.ThrowIfCancelled();
			}
		}

		private void PopulateLargeFile(FileEnumeratorParameters parameters, INode node, bool hasFileReadPermissions)
		{
			IEnumerable<byte[]> fileChunks = node.GetBytes();
			CancellationHelper.ThrowIfCancelled();

			this.Sha256Hash = Sha256Helper.GetSha256Hash_IEnumerable(fileChunks, this.Length);
			CancellationHelper.ThrowIfCancelled();

			if (hasFileReadPermissions)
			{
				this.PeData = PeDataObject.TryGetPeDataObject(FullPath, parameters.OnlineCertValidation);
				if (PeData != null)
				{
					this.Authenticode = AuthenticodeData.GetAuthenticodeData(PeData.Certificate);
				}
				CancellationHelper.ThrowIfCancelled();
			}

			/*
			if (parameters.CalculateEntropy)
			{
				this.Entropy = EntropyHelper.CalculateFileEntropy(fileChunks, this.Length);
				CancellationHelper.ThrowIfCancelled();
			}
			*/

			if (hasFileReadPermissions)
			{
				string yaraIndexFilename = PopulateYaraInfo(parameters.YaraParameters, false);

				if (!string.IsNullOrWhiteSpace(yaraIndexFilename))
				{
					this.YaraRulesMatched = YaraHelper.ScanFile(FullPath, yaraIndexFilename);
				}
			}
		}

		private void PopulateSmallFile(FileEnumeratorParameters parameters, INode node, bool hasFileReadPermissions)
		{
			byte[] fileBytes = new byte[0];
			if (!node.Streams.Any()) //workaround for no file stream such as with hard links
			{
				try
				{

					using (FileStream fsSource = new FileStream(FullPath,
						FileMode.Open, FileAccess.Read))
					{

						// Read the source file into a byte array.
						fileBytes = new byte[fsSource.Length];
						int numBytesToRead = (int)fsSource.Length;
						int numBytesRead = 0;
						while (numBytesToRead > 0)
						{
							// Read may return anything from 0 to numBytesToRead.
							int n = fsSource.Read(fileBytes, numBytesRead, numBytesToRead);

							// Break when the end of the file is reached.
							if (n == 0)
								break;

							numBytesRead += n;
							numBytesToRead -= n;
						}
						numBytesToRead = fileBytes.Length;

					}
				}
				catch (Exception ex)
				{

				}
			}

			else
			{
				fileBytes = node.GetBytes().SelectMany(chunk => chunk).ToArray();
				CancellationHelper.ThrowIfCancelled();
			}
			this.PeData = PeDataObject.TryGetPeDataObject(fileBytes, parameters.OnlineCertValidation);
			if (IsPeDataPopulated)
			{
				this.Sha256Hash = PeData.SHA256Hash;
			}
			else
			{
				this.Sha256Hash = Sha256Helper.GetSha256Hash_Array(fileBytes);
			}
			CancellationHelper.ThrowIfCancelled();

			if (PeData != null)
			{
				this.Authenticode = AuthenticodeData.GetAuthenticodeData(PeData.Certificate);
				CancellationHelper.ThrowIfCancelled();
			}

			if (parameters.CalculateEntropy)
			{
				this.Entropy = EntropyHelper.CalculateFileEntropy(fileBytes);
				CancellationHelper.ThrowIfCancelled();
			}

			if (hasFileReadPermissions)
			{
				string yaraIndexFilename = PopulateYaraInfo(parameters.YaraParameters, true);

				if (!string.IsNullOrWhiteSpace(yaraIndexFilename))
				{
					this.YaraRulesMatched = YaraHelper.ScanBytes(fileBytes, yaraIndexFilename);
				}
			}
		}

		private void PopulateIsTrusted()
		{
			bool result = false;

			try
			{
				result = VerifyTrustHelper.IsTrusted(FullPath);
			}
			catch { }

			this.IsTrusted = result;
		}

		private string PopulateYaraInfo(List<YaraFilter> yaraFilters, bool fileBytes)
		{
			List<string> distinctRulesToRun =
				yaraFilters
				.SelectMany(yf => yf.ProcessRule(this))
				.Distinct()
				.OrderBy(s => s)
				.ToList();

			string yaraIndexContents = YaraHelper.MakeYaraIndexFile(distinctRulesToRun);

			string indexFileHash = Sha256Helper.GetSha256Hash_Array(Encoding.UTF8.GetBytes(yaraIndexContents));

			string yaraIndexFilename = Path.Combine(Path.GetTempPath(), $"{indexFileHash}-index.yar");

			if (!File.Exists(yaraIndexFilename))
			{
				File.WriteAllText(yaraIndexFilename, yaraIndexContents);
			}

			return yaraIndexFilename;
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
						MimeType = shellProperty.ContentType.Value ?? "";
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

	}
}
