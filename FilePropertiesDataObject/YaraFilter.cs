using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesDataObject
{
	public enum YaraFilterType
	{
		AlwaysRun,
		MimeType,
		FileExtension,
		IsPeFile
	}

	public class YaraFilter : IEquatable<YaraFilter>
	{
		public YaraFilterType FilterType { get; private set; }
		public string FilterValue { get; private set; }

		public List<string> OnMatchRules { get; private set; }
		public List<string> NoMatchRules { get; private set; }

		public YaraFilter(YaraFilterType filterType, string filterValue, List<string> onMatchRules, List<string> noMatchRules)
		{
			FilterType = filterType;
			FilterValue = filterValue;
			OnMatchRules = onMatchRules;
			NoMatchRules = noMatchRules;
		}

		public List<string> ProcessRule(FileProperties fileProperties)
		{
			if (FilterMatches(fileProperties))
			{
				return OnMatchRules;
			}
			else
			{
				return NoMatchRules;
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
				return (fileProperties.PeData.IsPe || fileProperties.PeData.IsDll || fileProperties.PeData.IsDriver);
			}
			else if (FilterType == YaraFilterType.FileExtension)
			{
				return string.Equals(FilterValue, fileProperties.Extension, StringComparison.InvariantCultureIgnoreCase);
			}
			else if (FilterType == YaraFilterType.MimeType)
			{
				return string.Equals(FilterValue, fileProperties.MimeType, StringComparison.InvariantCultureIgnoreCase);
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

			HashSet<string> noMatchHashSet = new HashSet<string>(NoMatchRules);
			noMatchHashSet.SymmetricExceptWith(other.NoMatchRules);
			if (noMatchHashSet.Any())
			{
				return false;
			}

			return true;
		}

		private static string JoinString = ", ";

		public override string ToString()
		{
			return $"{Enum.GetName(typeof(YaraFilterType), FilterType)}:{FilterValue}[{{{string.Join(JoinString, OnMatchRules.Select(s => Path.GetFileName(s)))}}},{{{string.Join(JoinString, NoMatchRules.Select(s => Path.GetFileName(s)))}}}]";
		}
	}
}
