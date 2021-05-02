#pragma once

namespace YaraSharp
{
	public ref class YSReport
	{
		Dictionary<String^, List<String^>^>^ files;
	public:
		YSReport();
		~YSReport();

		bool IsEmpty();
		List<String^>^ GetFiles();
		Dictionary<String^, List<String^>^>^ Dump();

		void MergeReports(YSReport^ report);
		void AddReport(String^ file, String^ description);
	};
}
