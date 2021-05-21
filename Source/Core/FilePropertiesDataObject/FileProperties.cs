﻿using System;
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
using YaraSharp;

namespace FilePropertiesDataObject
{
	using Helpers;
	using Parameters;
	using System.Security;

	public class FileProperties
	{
		#region Public properties

		public uint MFTNumber { get; private set; }
		public ushort SequenceNumber { get; private set; }
		public string Sha256 { get; private set; }
		public string SHA1 { get; private set; }
		public string MD5 { get; private set; }
		public string ImpHash { get { return _peData?.ImpHash ?? ""; } }

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
		public string Attributes { get { return _attributes?.ToString() ?? ""; } }

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

		public string CertSubject { get { return _authenticode?.CertSubject ?? ""; } }
		public string CertIssuer { get { return _authenticode?.CertIssuer ?? ""; } }
		public string CertSerialNumber { get { return _authenticode?.CertSerialNumber ?? ""; } }
		public string CertThumbprint { get { return _authenticode?.CertThumbprint ?? ""; } }
		public string CertNotBefore { get { return _authenticode?.CertNotBefore ?? ""; } }
		public string CertNotAfter { get { return _authenticode?.CertNotAfter ?? ""; } }

		public bool IsDll { get { return _peData?.IsDll ?? false; } }
		public bool IsExe { get { return _peData?.IsExe ?? false; } }
		public bool IsDriver { get { return _peData?.IsDriver ?? false; } }
		public bool IsSigned { get { return _peData?.IsSigned ?? false; } }
		public bool IsTrusted { get; private set; }
		public bool IsSignatureValid { get { return _peData?.IsSignatureValid ?? false; } }
		public bool IsValidCertChain { get { return _peData?.IsValidCertChain ?? false; } }

		public int? BinaryType { get { return _peData?.BinaryType ?? null; } }
		public DateTime? CompileDate { get { return _peData?.CompileDate ?? null; } }
		public double? Entropy { get; private set; }
		public string YaraMatchedRules { get { return YaraHelper.FormatDelimitedRulesString(_yaraRulesMatched); } }

		internal List<string> _yaraRulesMatched { get; set; }

		private Attributes _attributes { get; set; }
		private PeDataObject _peData { get; set; }
		private AuthenticodeData _authenticode { get; set; }
		private TimingMetrics _timingMetrics = null;

		// Max number of bytes for a file size before reading a file in chunks
		private UInt64 MaxLength = (ulong)NtfsReader.MaxClustersToRead * (ulong)4096;

		private static Dictionary<string, YSScanner> _yaraCompiledRulesDictionary;

		#endregion

		static FileProperties()
		{
			_yaraCompiledRulesDictionary = new Dictionary<string, YSScanner>();
		}

		public FileProperties()
		{
			_peData = null;
			_authenticode = null;
			Entropy = null;
			_yaraRulesMatched = new List<string>();
		}

		public static void CleanUp()
		{
			if (_yaraCompiledRulesDictionary.Any())
			{
				List<YSScanner> rules = _yaraCompiledRulesDictionary.Values.ToList();

				foreach (YSScanner scanner in rules)
				{
					scanner.Dispose();
				}

				_yaraCompiledRulesDictionary.Clear();
				_yaraCompiledRulesDictionary = null;
			}

			YaraHelper.CleanUp();
		}

		public void PopulateFileProperties(FileEnumeratorParameters parameters, char driveLetter, INode node, TimingMetrics timingMetrics)
		{
			if (parameters == null) return;
			if (node == null) return;

			_timingMetrics = timingMetrics;

			CancellationHelper.SetCancellationToken(parameters.CancelToken);
			CancellationHelper.ThrowIfCancelled();

			_timingMetrics.Start(TimingMetric.MiscFileProperties);

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
				if (File.Exists(FullPath))
				{
					long length = new FileInfo(FullPath).Length;
					if (length > 0)
					{
						this.Length = (ulong)length;
						node.Size = this.Length;
					}
				}
			}

			bool hasFileReadPermissions = false;
			var fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, AccessControlActions.View, FullPath);
			try
			{
				fileIOPermission.Demand();
				hasFileReadPermissions = true;
			}
			catch (SecurityException)
			{
				hasFileReadPermissions = false;
			}

			_timingMetrics.Stop(TimingMetric.MiscFileProperties);

			if (this.Length != 0)
			{
				// Some entries in the MFT are greater than int.MaxValue !! That or the size is corrupt. Either way, we handle that here.
				if (this.Length > MaxLength) //(int.MaxValue - 1))
				{
					PopulateLargeFile(parameters, node, hasFileReadPermissions);
				}
				else
				{
					PopulateSmallFile(parameters, node, hasFileReadPermissions);
				}
			}
			else
			{
				this.Sha256 = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855"; // SHA256 hash of a null or zero-length input
				this.SHA1 = "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709";
				this.MD5 = "D41D8CD98F00B204E9800998ECF8427E";
			}

			_timingMetrics.Start(TimingMetric.MiscFileProperties);

			PopulateIsTrusted();
			CancellationHelper.ThrowIfCancelled();

			this._attributes = new Attributes(node);
			_timingMetrics.Stop(TimingMetric.MiscFileProperties);
			CancellationHelper.ThrowIfCancelled();
		}

		private void PopulateLargeFile(FileEnumeratorParameters parameters, INode node, bool hasFileReadPermissions)
		{
			_timingMetrics.Start(TimingMetric.ReadingMFTBytes);
			IEnumerable<byte[]> fileChunks = node.GetBytes();
			_timingMetrics.Stop(TimingMetric.ReadingMFTBytes);

			_timingMetrics.Start(TimingMetric.FileHashing);
			this.Sha256 = Hash.ByteEnumerable.Sha256(fileChunks);
			if (hasFileReadPermissions)
			{
				this._peData = PeDataObject.TryGetPeDataObject(FullPath);
				if (_peData != null)
				{
					this.SHA1 = _peData.SHA1Hash;
					this.MD5 = _peData.MD5Hash;
				}
				else
				{
					this.SHA1 = Hash.ByteEnumerable.Sha1(fileChunks);
					this.MD5 = Hash.ByteEnumerable.MD5(fileChunks);
				}
			}
			_timingMetrics.Stop(TimingMetric.FileHashing);

			_timingMetrics.Start(TimingMetric.MiscFileProperties);
			if (_peData != null)
			{
				this._authenticode = AuthenticodeData.GetAuthenticodeData(this._peData.Certificate);
			}

			CancellationHelper.ThrowIfCancelled();

			if (hasFileReadPermissions)
			{
				PopulateFileInfoProperties(FullPath);
			}
			_timingMetrics.Stop(TimingMetric.MiscFileProperties);

			if (hasFileReadPermissions)
			{
				PopulateShellFileInfo(FullPath);
				CancellationHelper.ThrowIfCancelled();
			}

			if (hasFileReadPermissions)
			{
				YSScanner compiledRules = GetCompiledYaraRules(parameters);
				if (compiledRules != null)
				{
					_timingMetrics.Start(TimingMetric.YaraScanning);
					try
					{
						_yaraRulesMatched = YaraHelper.ScanFile(FullPath, compiledRules);
					}
					catch (Exception ex)
					{
						parameters.ReportExceptionFunction.Invoke(nameof(PopulateLargeFile), string.Empty, ex);
					}
					_timingMetrics.Stop(TimingMetric.YaraScanning);
				}

				CancellationHelper.ThrowIfCancelled();
			}

			if (parameters.CalculateEntropy) // Should we calculate entropy on really large files?
			{
				_timingMetrics.Start(TimingMetric.CalculatingEntropy);
				this.Entropy = EntropyHelper.CalculateFileEntropy(fileChunks, this.Length);
				_timingMetrics.Stop(TimingMetric.CalculatingEntropy);
				CancellationHelper.ThrowIfCancelled();
			}
		}

		private void PopulateSmallFile(FileEnumeratorParameters parameters, INode node, bool hasFileReadPermissions)
		{
			_timingMetrics.Start(TimingMetric.ReadingMFTBytes);
			byte[] fileBytes = new byte[0];
			if (!node.Streams.Any()) //workaround for no file stream such as with hard links
			{
				try
				{
					using (FileStream fsSource = new FileStream(FullPath, FileMode.Open, FileAccess.Read))
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
							{ break; }

							numBytesRead += n;
							numBytesToRead -= n;
						}
						numBytesToRead = fileBytes.Length;
					}
				}
				catch (System.IO.IOException ioException)
				{
					if (ioException.Message.Contains("contains a virus"))
					{
						string hash = "File access blocked by anti-virus program.";
						this.Sha256 = hash;
						this.SHA1 = hash;
						this.MD5 = hash;
						return;
					}
				}
				catch
				{ }
			}
			else
			{
				fileBytes = node.GetBytes().SelectMany(chunk => chunk).ToArray();
			}
			_timingMetrics.Stop(TimingMetric.ReadingMFTBytes);

			_timingMetrics.Start(TimingMetric.FileHashing);
			this._peData = PeDataObject.TryGetPeDataObject(fileBytes);
			if (this._peData != null)
			{
				this.Sha256 = _peData.SHA256Hash;
				this.SHA1 = _peData.SHA1Hash;
				this.MD5 = _peData.MD5Hash;
			}

			if (this._peData == null || string.IsNullOrWhiteSpace(this.Sha256))
			{
				this.Sha256 = Hash.ByteArray.Sha256(fileBytes);
				this.SHA1 = Hash.ByteArray.Sha1(fileBytes);
				this.MD5 = Hash.ByteArray.MD5(fileBytes);
			}
			_timingMetrics.Stop(TimingMetric.FileHashing);

			_timingMetrics.Start(TimingMetric.MiscFileProperties);
			if (_peData != null)
			{
				_authenticode = AuthenticodeData.GetAuthenticodeData(_peData.Certificate);
			}

			CancellationHelper.ThrowIfCancelled();

			if (hasFileReadPermissions)
			{
				PopulateFileInfoProperties(FullPath);
			}
			_timingMetrics.Stop(TimingMetric.MiscFileProperties);

			if (hasFileReadPermissions)
			{
				PopulateShellFileInfo(FullPath);
				CancellationHelper.ThrowIfCancelled();
			}

			YSScanner compiledRules = GetCompiledYaraRules(parameters);
			if (compiledRules != null)
			{
				_timingMetrics.Start(TimingMetric.YaraScanning);
				try
				{
					_yaraRulesMatched = YaraHelper.ScanBytes(fileBytes, compiledRules);
				}
				catch (Exception ex)
				{
					parameters.ReportExceptionFunction.Invoke(nameof(PopulateSmallFile), string.Empty, ex);

				}
				_timingMetrics.Stop(TimingMetric.YaraScanning);
			}
			CancellationHelper.ThrowIfCancelled();

			if (parameters.CalculateEntropy)
			{
				_timingMetrics.Start(TimingMetric.CalculatingEntropy);
				Entropy = EntropyHelper.CalculateFileEntropy(fileBytes);
				_timingMetrics.Stop(TimingMetric.CalculatingEntropy);
				CancellationHelper.ThrowIfCancelled();
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

		private YSScanner GetCompiledYaraRules(FileEnumeratorParameters parameters)
		{
			_timingMetrics.Start(TimingMetric.YaraRuleCompiling);

			List<YaraFilter> yaraFilters = parameters.YaraParameters;

			List<string> distinctRulesToRun =
				yaraFilters
				.SelectMany(yf => yf.ProcessRule(this))
				.Distinct()
				.ToList();

			if (!distinctRulesToRun.Any())
			{
				distinctRulesToRun =
					yaraFilters
						.Where(yf => yf.FilterType == YaraFilterType.ElseNoMatch)
						.SelectMany(yf => yf.OnMatchRules)
						.Distinct()
						.ToList();
			}

			if (!distinctRulesToRun.Any())
			{
				return null;
			}

			distinctRulesToRun = distinctRulesToRun.OrderBy(s => s).ToList();

			string uniqueRuleCollectionToken = string.Join("|", distinctRulesToRun);
			string ruleCollectionHash = Hash.ByteArray.Sha256(Encoding.UTF8.GetBytes(uniqueRuleCollectionToken));

			YSScanner results = null;

			if (_yaraCompiledRulesDictionary.ContainsKey(ruleCollectionHash))
			{
				results = _yaraCompiledRulesDictionary[ruleCollectionHash];
			}
			else
			{
				try
				{
					results = YaraHelper.CompileRules(distinctRulesToRun, parameters.ReportAndLogOutputFunction);
				}
				catch (Exception ex)
				{
					parameters.ReportExceptionFunction.Invoke(nameof(GetCompiledYaraRules), string.Empty, ex);
				}

				_yaraCompiledRulesDictionary.Add(ruleCollectionHash, results);
			}
			_timingMetrics.Stop(TimingMetric.YaraRuleCompiling);

			return results;
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

				_timingMetrics.Start(TimingMetric.GettingShellInfo);
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
				_timingMetrics.Stop(TimingMetric.GettingShellInfo);
			}
			catch
			{ }
		}

	}
}
