#pragma once

namespace YaraSharp
{
	public ref class YSInstance
	{
	public:
		YSCompiler^ CompileFromFiles(List<String^>^ filePathList, Dictionary<String^, Object^>^ externalVariables);
		
		List<YSMatches^>^ ScanFile(String^ path, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout);
		List<YSMatches^>^ ScanProcess(int pID, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout);
		List<YSMatches^>^ ScanMemory(array<uint8_t>^ buffer, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout);
		List<YSMatches^>^ ScanMemory(uint8_t* buffer, int length, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout);

		static Version^ GetVersion();
	};
}