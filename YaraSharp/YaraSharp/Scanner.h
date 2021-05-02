#pragma once

namespace YaraSharp
{
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	delegate int YaraScanCallback(int message, void* data, void* context);

	public ref class YSScanner sealed
	{
		initonly YR_SCANNER * scanner;
		List<YSMatches^>^ matches;
		GCHandle callbackHandle;

	public:
		YSScanner(YSRules^ rules, Dictionary<String^, Object^>^ externalVariables);
		~YSScanner();

		List<YSMatches^>^ ScanProcess(int pID);
		List<YSMatches^>^ ScanFile(String^ path);
		List<YSMatches^>^ ScanMemory(uint8_t* buffer, int length);
		int HandleScannerCallback(int message, void* data, void* context);
	private:
		void SetScannerCallback();
		void SetScannerExternals(Dictionary<String^, Object^>^ externalVariables);
	};
}