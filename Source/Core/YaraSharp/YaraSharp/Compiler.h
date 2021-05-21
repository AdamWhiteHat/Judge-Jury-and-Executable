#pragma once

namespace YaraSharp
{
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	delegate void YaraCompilerCallback(int errorLevel, const char* filename, int lineNumber, const char* message, void* userData);

	public ref class YSCompiler sealed
	{
		YSReport^ errors;
		YSReport^ warnings;
		initonly YR_COMPILER* compiler;
		GCHandle callbackHandle;

	public:
		YSCompiler(Dictionary<String^, Object^>^ externalVariables);
		~YSCompiler();

		YSRules^ GetRules();
		YSReport^ GetErrors();
		YSReport^ GetWarnings();

		void AddFile(String^ FilePath);
		void TryAddFile(String^ FilePath, Dictionary<String^, Object^>^ externalVariables);
		void AddFiles(List<String^>^ filePathList, Dictionary<String^, Object^>^ externalVariables);
		
	private:
		void BindFileToCompiler(YR_COMPILER* compiler, String^ filePath);

		void SetCompilerCallback();
		void SetCompilerExternals(Dictionary<String^, Object^>^ externalVariables);
		void HandleCompilerCallback(int errorLevel, const char* filename, int lineNumber, const char* message, void* userData);
	};
}