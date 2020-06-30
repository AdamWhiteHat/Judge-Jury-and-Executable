using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FilePropertiesDataObject.Parameters
{
    public enum YaraConditionType
    {
        None = -1,
        IsPeFile = 0,
        MimeType = 1,
        FileExtension = 2
    }

    [DataContract]
    public class YaraScanConfiguration
    {
        [DataMember]
        public List<string> BaseRuleset { get; set; }
        [DataMember]
        public List<YaraCondition> Conditions { get; set; }
        [DataMember]
        public List<string> ElseRuleset { get; set; }

        public YaraScanConfiguration()
        {
            BaseRuleset = new List<string>();
            Conditions = new List<YaraCondition>();
            ElseRuleset = new List<string>();
        }

        public YaraScanConfiguration(List<string> baseRuleset, List<YaraCondition> conditions, List<string> elseRuleset)
        {
            SetBaseRuleset(baseRuleset);
            SetConditions(conditions);
            SetElseRuleset(elseRuleset);
        }

        public void AddConditionalRule(YaraCondition conditionalRule)
        {
            Conditions.Add(conditionalRule);
        }

        public void RemoveConditionalRule(YaraCondition conditionalRule)
        {
            Conditions.Remove(conditionalRule);
        }

        public void SetBaseRuleset(List<string> ruleset)
        {
            BaseRuleset = ruleset.ToList();
        }

        public void SetConditions(List<YaraCondition> rulesets)
        {
            Conditions = rulesets.ToList();
        }

        public void SetElseRuleset(List<string> ruleset)
        {
            ElseRuleset = ruleset.ToList();
        }
    }
}
