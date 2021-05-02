#pragma once

namespace YaraSharp
{
	// Code 1 is handled by OutOfMemoryException

	// Code 2
	public ref class YaraAttachToProcessFailureException : public Exception
	{
	public:
		YaraAttachToProcessFailureException();
	};

	// Code 3
	public ref class YaraOpenFileFailureException : public Exception
	{
	public:
		YaraOpenFileFailureException();
	};

	// Code 4
	public ref class YaraMapFileFailureException : public Exception
	{
	public:
		YaraMapFileFailureException();
	};

	// Code 6
	public ref class YaraInvalidFileException : public Exception
	{
	public:
		YaraInvalidFileException();
	};

	// Code 7
	public ref class YaraCorruptFileException : public Exception
	{
	public:
		YaraCorruptFileException();
	};

	// Code 8
	public ref class YaraUnsupportedFileVersionException : public Exception
	{
	public:
		YaraUnsupportedFileVersionException();
	};

	// Code 9
	public ref class YaraInvalidRegularExpressionException : public Exception
	{
	public:
		YaraInvalidRegularExpressionException();
	};

	// Code 10
	public ref class YaraInvalidHexStringException : public Exception
	{
	public:
		YaraInvalidHexStringException();
	};

	// Code 11
	public ref class YaraSyntaxErrorException : public Exception
	{
	public:
		YaraSyntaxErrorException();
	};

	// Code 12
	public ref class YaraLoopNestingLimitExceededException : public Exception
	{
	public:
		YaraLoopNestingLimitExceededException();
	};

	// Code 13
	public ref class YaraDuplicatedLoopIdentifierException : public Exception
	{
	public:
		YaraDuplicatedLoopIdentifierException();
	};

	// Code 14
	public ref class YaraDuplicatedIdentifierException : public Exception
	{
	public:
		YaraDuplicatedIdentifierException();
	};

	// Code 15
	public ref class YaraDuplicatedTagIdentifierException : public Exception
	{
	public:
		YaraDuplicatedTagIdentifierException();
	};

	// Code 16
	public ref class YaraDuplicatedMetaIdentifierException : public Exception
	{
	public:
		YaraDuplicatedMetaIdentifierException();
	};

	// Code 17
	public ref class YaraDuplicatedStringIdentifierException : public Exception
	{
	public:
		YaraDuplicatedStringIdentifierException();
	};

	// Code 18
	public ref class YaraUnreferencedStringException : public Exception
	{
	public:
		YaraUnreferencedStringException();
	};

	// Code 19
	public ref class YaraUndefinedStringException : public Exception
	{
	public:
		YaraUndefinedStringException();
	};

	// Code 20
	public ref class YaraUndefinedIdentifierException : public Exception
	{
	public:
		YaraUndefinedIdentifierException();
	};

	// Code 21
	public ref class YaraMisplacedAnonymousStringException : public Exception
	{
	public:
		YaraMisplacedAnonymousStringException();
	};

	// Code 22
	public ref class YaraCircularReferenceException : public Exception
	{
	public:
		YaraCircularReferenceException();
	};

	// Code 23
	public ref class YaraDepthExceededException : public Exception
	{
	public:
		YaraDepthExceededException();
	};

	// Code 24
	public ref class YaraWrongTypeException : public Exception
	{
	public:
		YaraWrongTypeException();
	};

	// Code 25
	public ref class YaraExecStackOverflowException : public Exception
	{
	public:
		YaraExecStackOverflowException();
	};

	// Code 26
	public ref class YaraScanTimeoutException : public Exception
	{
	public:
		YaraScanTimeoutException();
	};

	// Code 27
	public ref class YaraTooManyScanThreadsException : public Exception
	{
	public:
		YaraTooManyScanThreadsException();
	};

	// Code 28
	public ref class YaraCallbackErrorException : public Exception
	{
	public:
		YaraCallbackErrorException();
	};

	// Code 29
	public ref class YaraInvalidArgumentException : public Exception
	{
	public:
		YaraInvalidArgumentException();
	};

	// Code 30
	public ref class YaraTooManyMatchesException : public Exception
	{
	public:
		YaraTooManyMatchesException();
	};

	// Code 31
	public ref class YaraInternalFatalErrorException : public Exception
	{
	public:
		YaraInternalFatalErrorException();
	};

	// Code 32
	public ref class YaraNestedForOfLoopException : public Exception
	{
	public:
		YaraNestedForOfLoopException();
	};

	// Code 33
	public ref class YaraInvalidFieldNameException : public Exception
	{
	public:
		YaraInvalidFieldNameException();
	};

	// Code 34
	public ref class YaraUnknownModuleException : public Exception
	{
	public:
		YaraUnknownModuleException();
	};

	// Code 35
	public ref class YaraNotAStructureException : public Exception
	{
	public:
		YaraNotAStructureException();
	};

	// Code 36
	public ref class YaraNotIndexableException : public Exception
	{
	public:
		YaraNotIndexableException();
	};

	// Code 37
	public ref class YaraNotAFunctionException : public Exception
	{
	public:
		YaraNotAFunctionException();
	};

	// Code 38
	public ref class YaraInvalidFormatException : public Exception
	{
	public:
		YaraInvalidFormatException();
	};

	// Code 39
	public ref class YaraTooManyArgumentsException : public Exception
	{
	public:
		YaraTooManyArgumentsException();
	};

	// Code 40
	public ref class YaraWrongArgumentsException : public Exception
	{
	public:
		YaraWrongArgumentsException();
	};

	// Code 41
	public ref class YaraWrongReturnTypeException : public Exception
	{
	public:
		YaraWrongReturnTypeException();
	};

	// Code 42
	public ref class YaraDuplicatedStructureMemberException : public Exception
	{
	public:
		YaraDuplicatedStructureMemberException();
	};

	// Code 43
	public ref class YaraEmptyStringException : public Exception
	{
	public:
		YaraEmptyStringException();
	};

	// Code 44
	public ref class YaraDivisionByZeroException : public Exception
	{
	public:
		YaraDivisionByZeroException();
	};

	// Code 45
	public ref class YaraRegularExpressionTooLargeException : public Exception
	{
	public:
		YaraRegularExpressionTooLargeException();
	};

	// Code 46
	public ref class YaraTooManyReFibersException : public Exception
	{
	public:
		YaraTooManyReFibersException();
	};

	// Code 47
	public ref class YaraCouldNotReadProcessMemoryException : public Exception
	{
	public:
		YaraCouldNotReadProcessMemoryException();
	};

	// Code 48
	public ref class YaraInvalidExternalVariableTypeException : public Exception
	{
	public:
		YaraInvalidExternalVariableTypeException();
	};
}