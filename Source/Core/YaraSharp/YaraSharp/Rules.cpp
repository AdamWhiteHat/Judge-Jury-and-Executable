#include "Stdafx.h"

//	Rules
namespace YaraSharp
{
	YSRules::operator YR_RULES*()
	{ 
		return rules;
	}
	YSRules::YSRules(YR_RULES* rules)
	{
		this->rules = rules;
	}
	YSRules::~YSRules()
	{ 
		if (rules)
		{
			yr_rules_destroy(rules);
		}
	}
	void YSRules::Destroy()
	{
		delete this; 
	}
}
//	YSRule
namespace YaraSharp
{
	YSRule::YSRule()
	{
		Identifier = nullptr;
		Tags = gcnew List<String^>();
		Strings = gcnew List<String^>();
		Meta = gcnew Dictionary<String^, Object^>();
	}
	YSRule::YSRule(YR_RULE* Rule)
	{
		Tags = YSRule::GetRuleTags(Rule);
		Meta = YSRule::GetRuleMeta(Rule);
		Strings = YSRule::GetRuleStrings(Rule);
		Identifier = gcnew String(Rule->identifier);
	}

	List<String^>^ YSRule::GetRuleTags(YR_RULE* rule)
	{
		List<String^>^ result = gcnew List<String^>();

		const char* tagEntry;
		yr_rule_tags_foreach(rule, tagEntry)
			result->Add(gcnew String(tagEntry));

		return result;
	}
	List<String^>^ YSRule::GetRuleStrings(YR_RULE* rule)
	{
		List<String^>^ result = gcnew List<String^>();

		YR_STRING* stringEntry;
		yr_rule_strings_foreach(rule, stringEntry)
			result->Add(gcnew String(stringEntry->identifier));

		return result;
	}
	Dictionary<String^, Object^>^ YSRule::GetRuleMeta(YR_RULE* rule)
	{
		Dictionary<String^, Object^>^ result = gcnew Dictionary<String^, Object^>();

		YR_META* metaEntry;
		yr_rule_metas_foreach(rule, metaEntry)
		{
			if (metaEntry->identifier == NULL)
				throw gcnew NullReferenceException("(Meta) Значение не может быть пустым");

			String^ metaID = gcnew String(metaEntry->identifier);
			Object^ metaValue = nullptr;

			switch (metaEntry->type)
			{
			case META_TYPE_BOOLEAN:
				metaValue = (bool)(metaEntry->integer == 1);
				break;
			case META_TYPE_INTEGER:
				metaValue = (Int32)metaEntry->integer;
				break;
			case META_TYPE_STRING:
				metaValue = gcnew String(metaEntry->string);
				break;
			}

			//	Distinct
			if (!result->ContainsKey(metaID))
				result->Add(metaID, metaValue);
		}

		return result;
	}
}