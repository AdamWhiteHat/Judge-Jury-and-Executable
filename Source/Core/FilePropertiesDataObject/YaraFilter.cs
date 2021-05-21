using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesDataObject
{
	public enum YaraFilterType
	{
		AlwaysRun,
		MimeType,
		FileExtension,
		IsPeFile,
		ElseNoMatch
	}

	[DataContract]
	public class YaraFilter : IEquatable<YaraFilter>
	{
		[DataMember]
		public YaraFilterType FilterType { get; private set; }
		[DataMember]
		public string FilterValue { get; private set; }

		[DataMember]
		public List<string> OnMatchRules { get; private set; }

		public YaraFilter()
		{ }

		public YaraFilter(YaraFilterType filterType, string filterValue, List<string> onMatchRules)
		{
			FilterType = filterType;
			OnMatchRules = onMatchRules.ToList();
			FilterValue = filterValue;
			if (FilterValue.Any(c => char.IsWhiteSpace(c)))
			{
				FilterValue = new string(FilterValue.Where(c => !char.IsWhiteSpace(c)).ToArray());
			}
		}

		public List<string> ProcessRule(FileProperties fileProperties)
		{
			if (FilterMatches(fileProperties))
			{
				return OnMatchRules;
			}
			else
			{
				return new List<string>();
			}
		}

		private bool FilterMatches(FileProperties fileProperties)
		{
			if (FilterType == YaraFilterType.AlwaysRun)
			{
				return true;
			}
			else if (FilterType == YaraFilterType.IsPeFile)
			{
				return (fileProperties.IsExe || fileProperties.IsDll || fileProperties.IsDriver);
			}
			else if (FilterType == YaraFilterType.FileExtension)
			{
				return string.Equals(FilterValue.Replace(".", ""), fileProperties.Extension.Replace(".", ""), StringComparison.InvariantCultureIgnoreCase);
			}
			else if (FilterType == YaraFilterType.MimeType)
			{
				return string.Equals(FilterValue, fileProperties.MimeType, StringComparison.InvariantCultureIgnoreCase);
			}
			else if (FilterType == YaraFilterType.ElseNoMatch)
			{
				return false;
			}
			else
			{
				throw new NotImplementedException($"You must have added a new {nameof(YaraFilterType)} enum without adding the appropriate logic in {nameof(YaraFilter)}.{nameof(FilterMatches)}.");
			}
		}

		public bool Equals(YaraFilter other)
		{
			if (this.FilterType != other.FilterType)
			{
				return false;
			}

			if (!string.Equals(this.FilterValue, other.FilterValue, StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			HashSet<string> onMatchHashSet = new HashSet<string>(OnMatchRules);
			onMatchHashSet.SymmetricExceptWith(other.OnMatchRules);
			if (onMatchHashSet.Any())
			{
				return false;
			}

			return true;
		}

		public override string ToString()
		{
			if (FilterType == YaraFilterType.ElseNoMatch)
			{
				return "else(no rules matches)";
			}
			else if (FilterType == YaraFilterType.AlwaysRun)
			{
				return "if(true)";
			}
			else if (FilterType == YaraFilterType.IsPeFile)
			{
				return "if(PE file)";
			}
			else if (FilterType == YaraFilterType.FileExtension)
			{
				return $"if(ext == {FilterValue})";
			}
			else if (FilterType == YaraFilterType.MimeType)
			{
				return $"if(MimeType == {FilterValue})";
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
