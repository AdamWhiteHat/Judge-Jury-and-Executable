#pragma once

namespace YaraSharp
{
	public ref class YSRules sealed
	{
		YR_RULES* rules;

	public:
		YSRules(YR_RULES* rules);
		operator YR_RULES*();
		void Destroy();
		~YSRules();
	};


	public ref class YSRule sealed
	{
	public:
		property String^ Identifier;
		property List<String^>^ Tags;
		property List<String^>^ Strings;
		property Dictionary<String^, Object^>^ Meta;

		YSRule();
		YSRule(YR_RULE* rule);

	private:
		List<String^>^ GetRuleTags(YR_RULE* rule);
		List<String^>^ GetRuleStrings(YR_RULE* rule);
		Dictionary<String^, Object^>^ GetRuleMeta(YR_RULE* rule);
	};
}
