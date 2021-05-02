#include "Stdafx.h"

//	Matches
namespace YaraSharp
{
	//	Rule compilation
	YSCompiler^ YSInstance::CompileFromFiles(List<String^>^ FilePathList, Dictionary<String^, Object^>^ externalVariables)
	{
		//	TODO: add namespace support

		YSCompiler^ compiler = gcnew YSCompiler(externalVariables);
		compiler->AddFiles(FilePathList, externalVariables);

		return compiler;
	}

	//	Scanning region
	List<YSMatches^>^ YSInstance::ScanFile(String^ path, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout)
	{
		YSScanner^ FScanner = gcnew YSScanner(rules, externalVariables);

		try {
			List<YSMatches^>^ results = FScanner->ScanFile(path);
			return results;
		}
		catch (System::Exception^ e) {
			throw e;
		}
		finally{
			delete FScanner;
		}
	}
	//	(not yet tested)
	List<YSMatches^>^ YSInstance::ScanProcess(int pID, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout)
	{
		YSScanner^ PScanner = gcnew YSScanner(rules, externalVariables);

		try {
			List<YSMatches^>^ results = PScanner->ScanProcess(pID);
			return results;
		}
		catch (System::Exception^ e) {
			throw e;
		}
		finally{
			delete PScanner;
		}
	}
	//	(not yet tested)
	List<YSMatches^>^ YSInstance::ScanMemory(uint8_t* buffer, int length, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout)
	{
		YSScanner^ MScanner = gcnew YSScanner(rules, externalVariables);

		try {
			List<YSMatches^>^ results = MScanner->ScanMemory(buffer, length);
			return results;
		}
		catch (System::Exception^ e) {
			throw e;
		}
		finally{
			delete MScanner;
		}
	}
	//	(not yet tested)
	List<YSMatches^>^ YSInstance::ScanMemory(array<uint8_t>^ buffer, YSRules^ rules, Dictionary<String^, Object^>^ externalVariables, int timeout)
	{
		if (buffer == nullptr || buffer->Length == 0)
			return gcnew List<YSMatches^>();
		else
		{
			pin_ptr<uint8_t> bufferPointer = &buffer[0];
			return ScanMemory(bufferPointer, buffer->Length, rules, externalVariables, timeout);
		}
	}

	Version^ YSInstance::GetVersion()
	{
		return Assembly::GetExecutingAssembly()->GetName()->Version;
	}
}