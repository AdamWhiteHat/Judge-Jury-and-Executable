#include "Stdafx.h"

namespace YaraSharp
{
	YSReport::YSReport()
	{
		files = gcnew Dictionary<String^, List<String^>^>();
	}
	YSReport::~YSReport()
	{
		files->Clear();
		delete files;
		files = nullptr;
	}
	void YSReport::AddReport(String^ file, String^ description)
	{
		if (!files->ContainsKey(file))
		{
			files->Add(file, gcnew List<String^>());
		}

		files[file]->Add(description);

	}
	void YSReport::MergeReports(YSReport^ report)
	{
		Dictionary<String^, List<String^>^>^ reportFiles = report->Dump();

		for each(KeyValuePair<String^, List<String^>^> entry in reportFiles)
		{
			String^ file = entry.Key;
			for each(String^ description in entry.Value)
			{
				AddReport(file, description);
			}
		}
	}
	bool YSReport::IsEmpty()
	{
		return files->Keys->Count == 0 ? true : false;
	}

	List<String^>^ YSReport::GetFiles()
	{
		return gcnew List<String^>(files->Keys);
	}
	Dictionary<String^, List<String^>^>^ YSReport::Dump()
	{
		return files;
	}
}