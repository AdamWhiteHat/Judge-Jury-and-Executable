#include "Stdafx.h"
#include "YaraExceptions.h"

namespace YaraSharp
{
	void YSException::ThrowOnError(int error)
	{
		switch (error)
		{
		case ERROR_SUCCESS:
			return;
		case ERROR_INSUFICIENT_MEMORY:
			throw gcnew OutOfMemoryException();
		case ERROR_COULD_NOT_ATTACH_TO_PROCESS:
			throw gcnew YaraAttachToProcessFailureException();
		case ERROR_COULD_NOT_OPEN_FILE:
			throw gcnew YaraOpenFileFailureException();
		case ERROR_COULD_NOT_MAP_FILE:
			throw gcnew YaraMapFileFailureException();
		case ERROR_INVALID_FILE:
			throw gcnew YaraInvalidFileException();
		case ERROR_CORRUPT_FILE:
			throw gcnew YaraCorruptFileException();
		case ERROR_UNSUPPORTED_FILE_VERSION:
			throw gcnew YaraUnsupportedFileVersionException();
		case ERROR_INVALID_REGULAR_EXPRESSION:
			throw gcnew YaraInvalidRegularExpressionException();
		case ERROR_INVALID_HEX_STRING:
			throw gcnew YaraInvalidHexStringException();
		case ERROR_SYNTAX_ERROR:
			throw gcnew YaraSyntaxErrorException();
		case ERROR_LOOP_NESTING_LIMIT_EXCEEDED:
			throw gcnew YaraLoopNestingLimitExceededException();
		case ERROR_DUPLICATED_LOOP_IDENTIFIER:
			throw gcnew YaraDuplicatedLoopIdentifierException();
		case ERROR_DUPLICATED_IDENTIFIER:
			throw gcnew YaraDuplicatedIdentifierException();
		case ERROR_DUPLICATED_TAG_IDENTIFIER:
			throw gcnew YaraDuplicatedTagIdentifierException();
		case ERROR_DUPLICATED_META_IDENTIFIER:
			throw gcnew YaraDuplicatedMetaIdentifierException();
		case ERROR_DUPLICATED_STRING_IDENTIFIER:
			throw gcnew YaraDuplicatedStringIdentifierException();
		case ERROR_UNREFERENCED_STRING:
			throw gcnew YaraUnreferencedStringException();
		case ERROR_UNDEFINED_STRING:
			throw gcnew YaraUndefinedStringException();
		case ERROR_UNDEFINED_IDENTIFIER:
			throw gcnew YaraUndefinedIdentifierException();
		case ERROR_MISPLACED_ANONYMOUS_STRING:
			throw gcnew YaraMisplacedAnonymousStringException();
		case ERROR_INCLUDES_CIRCULAR_REFERENCE:
			throw gcnew YaraCircularReferenceException();
		case ERROR_INCLUDE_DEPTH_EXCEEDED:
			throw gcnew YaraDepthExceededException();
		case ERROR_WRONG_TYPE:
			throw gcnew YaraWrongTypeException();
		case ERROR_EXEC_STACK_OVERFLOW:
			throw gcnew YaraExecStackOverflowException();
		case ERROR_SCAN_TIMEOUT:
			throw gcnew YaraScanTimeoutException();
		case ERROR_TOO_MANY_SCAN_THREADS:
			throw gcnew YaraTooManyScanThreadsException();
		case ERROR_CALLBACK_ERROR:
			throw gcnew YaraCallbackErrorException();
		case ERROR_INVALID_ARGUMENT:
			throw gcnew YaraInvalidArgumentException();
		case ERROR_TOO_MANY_MATCHES:
			throw gcnew YaraTooManyMatchesException();
		case ERROR_INTERNAL_FATAL_ERROR:
			throw gcnew YaraInternalFatalErrorException();
		case ERROR_NESTED_FOR_OF_LOOP:
			throw gcnew YaraNestedForOfLoopException();
		case ERROR_INVALID_FIELD_NAME:
			throw gcnew YaraInvalidFieldNameException();
		case ERROR_UNKNOWN_MODULE:
			throw gcnew YaraUnknownModuleException();
		case ERROR_NOT_A_STRUCTURE:
			throw gcnew YaraNotAStructureException();
		case ERROR_NOT_INDEXABLE:
			throw gcnew YaraNotIndexableException();
		case ERROR_NOT_A_FUNCTION:
			throw gcnew YaraNotAFunctionException();
		case ERROR_INVALID_FORMAT:
			throw gcnew YaraInvalidFormatException();
		case ERROR_TOO_MANY_ARGUMENTS:
			throw gcnew YaraTooManyArgumentsException();
		case ERROR_WRONG_ARGUMENTS:
			throw gcnew YaraWrongArgumentsException();
		case ERROR_WRONG_RETURN_TYPE:
			throw gcnew YaraWrongReturnTypeException();
		case ERROR_DUPLICATED_STRUCTURE_MEMBER:
			throw gcnew YaraDuplicatedStructureMemberException();
		case ERROR_EMPTY_STRING:
			throw gcnew YaraEmptyStringException();
		case ERROR_DIVISION_BY_ZERO:
			throw gcnew YaraDivisionByZeroException();
		case ERROR_REGULAR_EXPRESSION_TOO_LARGE:
			throw gcnew YaraRegularExpressionTooLargeException();
		case ERROR_TOO_MANY_RE_FIBERS:
			throw gcnew YaraTooManyReFibersException();
		case ERROR_COULD_NOT_READ_PROCESS_MEMORY:
			throw gcnew YaraCouldNotReadProcessMemoryException();
		case ERROR_INVALID_EXTERNAL_VARIABLE_TYPE:
			throw gcnew YaraInvalidExternalVariableTypeException();
		default:
			throw gcnew Exception("An unknown exception occurred");
		}
	}
	void YSException::ThrowOnError(String^ error)
	{
		throw gcnew Exception(String::Format("Error: {0}", error));
	}
}
