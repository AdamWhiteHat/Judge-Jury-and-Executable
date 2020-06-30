using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilePropertiesDataObject.Parameters
{
    [DataContract]
    public class YaraCondition : IEquatable<YaraCondition>
    {
        [DataMember]
        public YaraConditionType ConditionType { get; set; }
        [DataMember]
        public string ConditionValue { get; set; }
        [DataMember]
        public List<string> ConditionMatchRuleset { get; set; }

        private static char[] _extensionTrimChars = new char[] { '.' };
        private static string _joinString = ", ";

        public YaraCondition()
        {
            ConditionType = YaraConditionType.None;
            ConditionValue = "";
            ConditionMatchRuleset = new List<string>();
        }

        public YaraCondition(YaraConditionType conditionType, string conditionValue, List<string> conditionMatchRuleset)
        {
            ConditionType = conditionType;
            ConditionValue = conditionValue;
            ConditionMatchRuleset = conditionMatchRuleset.ToList();
        }

        public List<string> GetMatchingYaraRules(FileProperties fileProperties)
        {
            if (FileMeetsCondition(fileProperties))
            {
                return ConditionMatchRuleset;
            }
            else
            {
                return new List<string>();
            }
        }

        private bool FileMeetsCondition(FileProperties fileProperties)
        {
            if (ConditionType == YaraConditionType.IsPeFile)
            {
                return fileProperties.IsPeDataPopulated && (fileProperties.PeData.IsExe || fileProperties.PeData.IsDll || fileProperties.PeData.IsDriver);
            }
            else if (ConditionType == YaraConditionType.FileExtension)
            {
                return string.Equals(ConditionValue.TrimStart(_extensionTrimChars), fileProperties.Extension.TrimStart(_extensionTrimChars), StringComparison.InvariantCultureIgnoreCase);
            }
            else if (ConditionType == YaraConditionType.MimeType)
            {
                return string.Equals(ConditionValue, fileProperties.MimeType, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                throw new NotImplementedException($"You must have added a new {nameof(YaraConditionType)} enum without adding the appropriate logic in {nameof(YaraCondition)}.{nameof(FileMeetsCondition)}.");
            }
        }

        public bool Equals(YaraCondition other)
        {
            if (this.ConditionType != other.ConditionType)
            {
                return false;
            }

            if (!string.Equals(this.ConditionValue, other.ConditionValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            HashSet<string> onMatchHashSet = new HashSet<string>(ConditionMatchRuleset);
            onMatchHashSet.SymmetricExceptWith(other.ConditionMatchRuleset);
            if (onMatchHashSet.Any())
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string conditionNameString = Enum.GetName(typeof(YaraConditionType), ConditionType);
            string conditionValueString = (ConditionType == YaraConditionType.IsPeFile) ? "" : $" is {ConditionValue}";
            return $"If {conditionNameString}{conditionValueString} then run: {string.Join(_joinString, ConditionMatchRuleset.Select(s => Path.GetFileName(s)))}";
        }
    }
}
