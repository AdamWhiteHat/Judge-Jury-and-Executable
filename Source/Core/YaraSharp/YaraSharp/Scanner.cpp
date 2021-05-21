#include "Stdafx.h"

//	Scanner
namespace YaraSharp
{
	YSScanner::YSScanner(YSRules^ rules, Dictionary<String^, Object^>^ externalVariables)
	{
		YR_SCANNER* TestScanner;
		YSException::ThrowOnError(yr_scanner_create((YR_RULES*)rules, &TestScanner));
		scanner = TestScanner;

		matches = gcnew List<YSMatches^>();

		//	TODO: add timeout support

		SetScannerExternals(externalVariables);
		SetScannerCallback();
	}

	YSScanner::~YSScanner()
	{
		if (scanner)
		{
			yr_scanner_destroy(scanner);
		}
		callbackHandle.Free();
	}

	//	Scan region
	List<YSMatches^>^ YSScanner::ScanProcess(int pID)
	{
		matches = gcnew List<YSMatches^>();
		YSException::ThrowOnError(yr_scanner_scan_proc(scanner, pID));
		return matches;
	}

	List<YSMatches^>^ YSScanner::ScanFile(String^ Path)
	{
		matches = gcnew List<YSMatches^>();
		YSException::ThrowOnError(yr_scanner_scan_file(scanner, (marshal_as<std::wstring>(Path)).c_str()));
		return matches;
	}

	List<YSMatches^>^ YSScanner::ScanMemory(uint8_t* Buffer, int Length)
	{
		matches = gcnew List<YSMatches^>();
		YSException::ThrowOnError(yr_scanner_scan_mem(scanner, Buffer, Length));
		return matches;
	}

	List<YSMatches^>^ YSScanner::ScanMemory(array<uint8_t>^ buffer)
	{
		if (buffer == nullptr || buffer->Length == 0)
		{
			return gcnew List<YSMatches^>();
		}
		else
		{
			matches = gcnew List<YSMatches^>();
			pin_ptr<uint8_t> bufferPointer = &buffer[0];
			YSException::ThrowOnError(yr_scanner_scan_mem(scanner, bufferPointer, buffer->Length));
			return matches;
		}
	}

	//	Set externals
	void YSScanner::SetScannerExternals(Dictionary<String^, Object^>^ externalVariables)
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
					ExternalError = yr_scanner_define_boolean_variable(scanner, VariablePointer, (bool)externalVariable.Value);
				}
				else if (VariableType == Double::typeid)
				{
					ExternalError = yr_scanner_define_float_variable(scanner, VariablePointer, (double)externalVariable.Value);
				}
				else if (VariableType == Int64::typeid || VariableType == Int32::typeid)
				{
					ExternalError = yr_scanner_define_integer_variable(scanner, VariablePointer, (Int64)externalVariable.Value);
				}
				else if (VariableType == String::typeid)
				{
					ExternalError = yr_scanner_define_string_variable(scanner, VariablePointer, CTX.marshal_as<const char*>((String^)externalVariable.Value));
				}
				else
				{
					throw gcnew NotSupportedException(String::Format("Unsupported external variable: '{0}'", VariableType->Name));
				}

				if (ExternalError != ERROR_SUCCESS)
				{
					YSException::ThrowOnError("(Scanner) Error during external variable initialization");
				}
			}
		}
	}

	//	Callback
	void YSScanner::SetScannerCallback()
	{
		YaraScanCallback^ scannerCallback = gcnew YaraScanCallback(this, &YSScanner::HandleScannerCallback);
		callbackHandle = GCHandle::Alloc(scannerCallback);
		YR_CALLBACK_FUNC CallbackPointer = (YR_CALLBACK_FUNC)Marshal::GetFunctionPointerForDelegate(scannerCallback).ToPointer();
		yr_scanner_set_callback(scanner, CallbackPointer, NULL);
	}

	int YSScanner::HandleScannerCallback(int message, void* data, void* context)
	{
		if (message == CALLBACK_MSG_RULE_MATCHING)
		{
			matches->Add(gcnew YSMatches((YR_RULE*)data));
		}

		return CALLBACK_CONTINUE;
	}
}