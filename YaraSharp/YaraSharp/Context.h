#pragma once

namespace YaraSharp
{
	public ref class YSContext sealed
	{
	public:
		YSContext();
		~YSContext();
		void Destroy();
	};
}