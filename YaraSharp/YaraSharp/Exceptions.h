#pragma once

namespace YaraSharp
{
	public ref class YSException abstract sealed
	{
	public:
		static void ThrowOnError(int error);
		static void ThrowOnError(String^ error);
	};
}
