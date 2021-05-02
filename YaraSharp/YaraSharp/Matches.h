#pragma once

namespace YaraSharp
{
	public ref class YSMatch sealed
	{
	public:

		property uint64_t Base;
		property uint64_t Offset;
		property array<uint8_t>^ Data;

		YSMatch();
		YSMatch(YR_MATCH* match);
	};

	public ref class YSMatches sealed
	{
	public:

		property YSRule^ Rule;
		property Dictionary<String^, List<YSMatch^>^>^ Matches;

		YSMatches();
		YSMatches(YR_RULE* matchingRule);
	};
}