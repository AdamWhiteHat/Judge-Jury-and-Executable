#include "Stdafx.h"

//	Compiler
namespace YaraSharp
{
	//	Constructor
	YSCompiler::YSCompiler(Dictionary<String^, Object^>^ externalVariables)
	{
		YR_COMPILER* testCompiler;
		YSException::ThrowOnError(yr_compiler_create(&testCompiler));
		compiler = testCompiler;

		// Set up and initialize the error dictionary for errors and warnings
		// Which are populated by the callback handler
		errors = gcnew YSReport();
		warnings = gcnew YSReport();

		SetCompilerExternals(externalVariables);
		SetCompilerCallback();
	}

	YSCompiler::~YSCompiler()
	{
		if (compiler)
		{
			yr_compiler_destroy(compiler);
		}

		callbackHandle.Free();

		delete warnings;
		delete errors;
	}

	//	Rule region
	void YSCompiler::AddFile(String^ filePath)
	{
		BindFileToCompiler(this->compiler, filePath);
	}

	void YSCompiler::TryAddFile(String^ filePath, Dictionary<String^, Object^>^ externalVariables)
	{
		YSCompiler^ testCompiler = gcnew YSCompiler(externalVariables);
		BindFileToCompiler(testCompiler->compiler, filePath);

		bool isFileCorrect = testCompiler->GetErrors()->IsEmpty();

		//	If there are some errors => report to main compiler
		if (!isFileCorrect)
		{
			this->errors->MergeReports(testCompiler->GetErrors());
			//	Warnings are not passed, or else they will be duplicated
		}
		//	If no errors => pass file to main compiler
		else
		{
			AddFile(filePath);
		}

		delete testCompiler;
	}

	void YSCompiler::AddFiles(List<String^>^ filePathList, Dictionary<String^, Object^>^ externalVariables)
	{
		for each (auto filePath in filePathList)
		{
			TryAddFile(filePath, externalVariables);
		}
	}

	YSRules^ YSCompiler::GetRules()
	{
		YR_RULES* ysRules;

		YSException::ThrowOnError(yr_compiler_get_rules(compiler, &ysRules));

		return gcnew YSRules(ysRules);
	}

	YSReport^ YSCompiler::GetErrors()
	{
		return this->errors;
	}

	YSReport^ YSCompiler::GetWarnings()
	{
		return this->warnings;
	}


	void YSCompiler::BindFileToCompiler(YR_COMPILER* compiler, String^ filePath)
	{
		FILE* File;

		auto NativePath = marshal_as<std::string>(filePath);

		auto FileOpenError = fopen_s(&File, NativePath.c_str(), "r");

		if (FileOpenError)
		{
			YSException::ThrowOnError(String::Format("Error opening file: {0}", filePath));
		}

		auto errors = yr_compiler_add_file(compiler, File, nullptr, NativePath.c_str());

		if (File)
		{
			fclose(File);
		}
	}

	//	Set externals
	void YSCompiler::SetCompilerExternals(Dictionary<String^, Object^>^ externalVariables)
	{
		if (externalVariables)
		{
			marshal_context CTX;

			for each (auto externalVariable in externalVariables)
			{
				const char* VariablePointer = CTX.marshal_as<const char*>(externalVariable.Key);
				Type^ VariableType = externalVariable.Value->GetType();
				int ExternalError = ERROR_SUCCESS;

				if (VariableType == Boolean::typeid)
				{
					ExternalError = yr_compiler_define_boolean_variable(compiler, VariablePointer, (bool)externalVariable.Value);
				}
				else if (VariableType == Double::typeid)
				{
					ExternalError = yr_compiler_define_float_variable(compiler, VariablePointer, (double)externalVariable.Value);
				}
				else if (VariableType == Int64::typeid || VariableType == Int32::typeid)
				{
					ExternalError = yr_compiler_define_integer_variable(compiler, VariablePointer, (Int64)externalVariable.Value);
				}
				else if (VariableType == String::typeid)
				{
					ExternalError = yr_compiler_define_string_variable(compiler, VariablePointer, CTX.marshal_as<const char*>((String^)externalVariable.Value));
				}
				else
				{
					throw gcnew NotSupportedException(String::Format("Unsupported external variable: '{0}'", VariableType->Name));
				}

				if (ExternalError != ERROR_SUCCESS)
				{
					YSException::ThrowOnError("(Compiler) Error during external variable initialization");
				}
			}
		}
	}

	//	Callback
	void YSCompiler::SetCompilerCallback()
	{
		YaraCompilerCallback^ compilerCallback = gcnew YaraCompilerCallback(this, &YSCompiler::HandleCompilerCallback);
		callbackHandle = GCHandle::Alloc(compilerCallback);
		YR_COMPILER_CALLBACK_FUNC callbackPointer = (YR_COMPILER_CALLBACK_FUNC)(Marshal::GetFunctionPointerForDelegate(compilerCallback)).ToPointer();
		yr_compiler_set_callback(compiler, callbackPointer, NULL);
	}

	void YSCompiler::HandleCompilerCallback(int errorLevel, const char* filename, int lineNumber, const char* message, void* userData)
	{
		UNREFERENCED_PARAMETER(errorLevel);
		UNREFERENCED_PARAMETER(userData);

		String^ file = filename ? marshal_as<String^>(filename) : "[unknown]";

		auto msg = String::Format("{0} on line {1}", marshal_as<String^>(message), lineNumber);

		switch (errorLevel)
		{
		case YARA_ERROR_LEVEL_WARNING:
			warnings->AddReport(file, msg);
			break;

		case YARA_ERROR_LEVEL_ERROR:
			errors->AddReport(file, msg);
			break;
		}
	}
}