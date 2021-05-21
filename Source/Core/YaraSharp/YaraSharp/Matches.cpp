#include "Stdafx.h"

//	Matches
namespace YaraSharp
{
	YSMatches::YSMatches()
	{
		Rule = nullptr;
		Matches = gcnew Dictionary<String^, List<YSMatch^>^>();
	}

	YSMatches::YSMatches(YR_RULE * matchingRule)
	{
		Rule = gcnew YSRule(matchingRule);
		Matches = gcnew Dictionary<String^, List<YSMatch^>^>();

		YR_STRING* stringEntry;
		YR_MATCH* matchEntry;

		yr_rule_strings_foreach(matchingRule, stringEntry)
		{
			auto identifier = marshal_as<String^>(stringEntry->identifier);

			yr_string_matches_foreach(stringEntry, matchEntry)
			{
				if (!Matches->ContainsKey(identifier))
					Matches->Add(identifier, gcnew List<YSMatch^>());

				Matches[identifier]->Add(gcnew YSMatch(matchEntry));
			}
		}
	}
}

//	Match
namespace YaraSharp
{
	YSMatch::YSMatch()
	{
		Base = 0;
		Offset = 0;
		Data = gcnew array<uint8_t>(0);
	}

	YSMatch::YSMatch(YR_MATCH* match)
	{
		/*
		int64_t base;              // Base address for the match
		int64_t offset;            // Offset relative to base for the match
		int32_t match_length;      // Match length
		int32_t data_length;
		const uint8_t* data;
		*/

		Base = match->base;
		Offset = match->offset;

		Data = gcnew array<uint8_t>(match->match_length);
		Marshal::Copy(IntPtr((void *)match->data), Data, 0, match->data_length);
	}
}