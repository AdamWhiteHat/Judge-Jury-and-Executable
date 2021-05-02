#include "Stdafx.h"
#include "YaraExceptions.h"

namespace YaraSharp
{
	// Code 1 is handled by OutOfMemoryException

	// Code 2
	YaraAttachToProcessFailureException::YaraAttachToProcessFailureException() : Exception(String::Format("{0} - Code {1}", "ERROR_COULD_NOT_ATTACH_TO_PROCESS", ERROR_COULD_NOT_ATTACH_TO_PROCESS)) {}

	// Code 3
	YaraOpenFileFailureException::YaraOpenFileFailureException() : Exception(String::Format("{0} - Code {1}", "ERROR_COULD_NOT_OPEN_FILE", ERROR_COULD_NOT_OPEN_FILE)) {}

	// Code 4
	YaraMapFileFailureException::YaraMapFileFailureException() : Exception(String::Format("{0} - Code {1}", "ERROR_COULD_NOT_MAP_FILE", ERROR_COULD_NOT_MAP_FILE)) {}

	// Code 6
	YaraInvalidFileException::YaraInvalidFileException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_FILE", ERROR_INVALID_FILE)) {}

	// Code 7
	YaraCorruptFileException::YaraCorruptFileException() : Exception(String::Format("{0} - Code {1}", "ERROR_CORRUPT_FILE", ERROR_CORRUPT_FILE)) {}

	// Code 8
	YaraUnsupportedFileVersionException::YaraUnsupportedFileVersionException() : Exception(String::Format("{0} - Code {1}", "ERROR_UNSUPPORTED_FILE_VERSION", ERROR_UNSUPPORTED_FILE_VERSION)) {}

	// Code 9
	YaraInvalidRegularExpressionException::YaraInvalidRegularExpressionException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_REGULAR_EXPRESSION", ERROR_INVALID_REGULAR_EXPRESSION)) {}

	// Code 10
	YaraInvalidHexStringException::YaraInvalidHexStringException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_HEX_STRING", ERROR_INVALID_HEX_STRING)) {}

	// Code 11
	YaraSyntaxErrorException::YaraSyntaxErrorException() : Exception(String::Format("{0} - Code {1}", "ERROR_SYNTAX_ERROR", ERROR_SYNTAX_ERROR)) {}

	// Code 12
	YaraLoopNestingLimitExceededException::YaraLoopNestingLimitExceededException() : Exception(String::Format("{0} - Code {1}", "ERROR_LOOP_NESTING_LIMIT_EXCEEDED", ERROR_LOOP_NESTING_LIMIT_EXCEEDED)) {}

	// Code 13
	YaraDuplicatedLoopIdentifierException::YaraDuplicatedLoopIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_LOOP_IDENTIFIER", ERROR_DUPLICATED_LOOP_IDENTIFIER)) {}

	// Code 14
	YaraDuplicatedIdentifierException::YaraDuplicatedIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_IDENTIFIER", ERROR_DUPLICATED_IDENTIFIER)) {}

	// Code 15
	YaraDuplicatedTagIdentifierException::YaraDuplicatedTagIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_TAG_IDENTIFIER", ERROR_DUPLICATED_TAG_IDENTIFIER)) {}

	// Code 16
	YaraDuplicatedMetaIdentifierException::YaraDuplicatedMetaIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_META_IDENTIFIER", ERROR_DUPLICATED_META_IDENTIFIER)) {}

	// Code 17
	YaraDuplicatedStringIdentifierException::YaraDuplicatedStringIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_STRING_IDENTIFIER", ERROR_DUPLICATED_STRING_IDENTIFIER)) {}

	// Code 18
	YaraUnreferencedStringException::YaraUnreferencedStringException() : Exception(String::Format("{0} - Code {1}", "ERROR_UNREFERENCED_STRING", ERROR_UNREFERENCED_STRING)) {}

	// Code 19
	YaraUndefinedStringException::YaraUndefinedStringException() : Exception(String::Format("{0} - Code {1}", "ERROR_UNDEFINED_STRING", ERROR_UNDEFINED_STRING)) {}

	// Code 20
	YaraUndefinedIdentifierException::YaraUndefinedIdentifierException() : Exception(String::Format("{0} - Code {1}", "ERROR_UNDEFINED_IDENTIFIER", ERROR_UNDEFINED_IDENTIFIER)) {}

	// Code 21
	YaraMisplacedAnonymousStringException::YaraMisplacedAnonymousStringException() : Exception(String::Format("{0} - Code {1}", "ERROR_MISPLACED_ANONYMOUS_STRING", ERROR_MISPLACED_ANONYMOUS_STRING)) {}

	// Code 22
	YaraCircularReferenceException::YaraCircularReferenceException() : Exception(String::Format("{0} - Code {1}", "ERROR_INCLUDES_CIRCULAR_REFERENCE", ERROR_INCLUDES_CIRCULAR_REFERENCE)) {}

	// Code 23
	YaraDepthExceededException::YaraDepthExceededException() : Exception(String::Format("{0} - Code {1}", "ERROR_INCLUDE_DEPTH_EXCEEDED", ERROR_INCLUDE_DEPTH_EXCEEDED)) {}

	// Code 24
	YaraWrongTypeException::YaraWrongTypeException() : Exception(String::Format("{0} - Code {1}", "ERROR_WRONG_TYPE", ERROR_WRONG_TYPE)) {}

	// Code 25
	YaraExecStackOverflowException::YaraExecStackOverflowException() : Exception(String::Format("{0} - Code {1}", "ERROR_EXEC_STACK_OVERFLOW", ERROR_EXEC_STACK_OVERFLOW)) {}

	// Code 26
	YaraScanTimeoutException::YaraScanTimeoutException() : Exception(String::Format("{0} - Code {1}", "ERROR_SCAN_TIMEOUT", ERROR_SCAN_TIMEOUT)) {}

	// Code 27
	YaraTooManyScanThreadsException::YaraTooManyScanThreadsException() : Exception(String::Format("{0} - Code {1}", "ERROR_TOO_MANY_SCAN_THREADS", ERROR_TOO_MANY_SCAN_THREADS)) {}

	// Code 28
	YaraCallbackErrorException::YaraCallbackErrorException() : Exception(String::Format("{0} - Code {1}", "ERROR_CALLBACK_ERROR", ERROR_CALLBACK_ERROR)) {}

	// Code 29
	YaraInvalidArgumentException::YaraInvalidArgumentException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_ARGUMENT", ERROR_INVALID_ARGUMENT)) {}

	// Code 30
	YaraTooManyMatchesException::YaraTooManyMatchesException() : Exception(String::Format("{0} - Code {1}", "ERROR_TOO_MANY_MATCHES", ERROR_TOO_MANY_MATCHES)) {}

	// Code 31
	YaraInternalFatalErrorException::YaraInternalFatalErrorException() : Exception(String::Format("{0} - Code {1}", "ERROR_INTERNAL_FATAL_ERROR", ERROR_INTERNAL_FATAL_ERROR)) {}

	// Code 32
	YaraNestedForOfLoopException::YaraNestedForOfLoopException() : Exception(String::Format("{0} - Code {1}", "ERROR_NESTED_FOR_OF_LOOP", ERROR_NESTED_FOR_OF_LOOP)) {}

	// Code 33
	YaraInvalidFieldNameException::YaraInvalidFieldNameException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_FIELD_NAME", ERROR_INVALID_FIELD_NAME)) {}

	// Code 34
	YaraUnknownModuleException::YaraUnknownModuleException() : Exception(String::Format("{0} - Code {1}", "ERROR_UNKNOWN_MODULE", ERROR_UNKNOWN_MODULE)) {}

	// Code 35
	YaraNotAStructureException::YaraNotAStructureException() : Exception(String::Format("{0} - Code {1}", "ERROR_NOT_A_STRUCTURE", ERROR_NOT_A_STRUCTURE)) {}

	// Code 36
	YaraNotIndexableException::YaraNotIndexableException() : Exception(String::Format("{0} - Code {1}", "ERROR_NOT_INDEXABLE", ERROR_NOT_INDEXABLE)) {}

	// Code 37
	YaraNotAFunctionException::YaraNotAFunctionException() : Exception(String::Format("{0} - Code {1}", "ERROR_NOT_A_FUNCTION", ERROR_NOT_A_FUNCTION)) {}

	// Code 38
	YaraInvalidFormatException::YaraInvalidFormatException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_FORMAT", ERROR_INVALID_FORMAT)) {}

	// Code 39
	YaraTooManyArgumentsException::YaraTooManyArgumentsException() : Exception(String::Format("{0} - Code {1}", "ERROR_TOO_MANY_ARGUMENTS", ERROR_TOO_MANY_ARGUMENTS)) {}

	// Code 40
	YaraWrongArgumentsException::YaraWrongArgumentsException() : Exception(String::Format("{0} - Code {1}", "ERROR_WRONG_ARGUMENTS", ERROR_WRONG_ARGUMENTS)) {}

	// Code 41
	YaraWrongReturnTypeException::YaraWrongReturnTypeException() : Exception(String::Format("{0} - Code {1}", "ERROR_WRONG_RETURN_TYPE", ERROR_WRONG_RETURN_TYPE)) {}

	// Code 42
	YaraDuplicatedStructureMemberException::YaraDuplicatedStructureMemberException() : Exception(String::Format("{0} - Code {1}", "ERROR_DUPLICATED_STRUCTURE_MEMBER", ERROR_DUPLICATED_STRUCTURE_MEMBER)) {}

	// Code 43
	YaraEmptyStringException::YaraEmptyStringException() : Exception(String::Format("{0} - Code {1}", "ERROR_EMPTY_STRING", ERROR_EMPTY_STRING)) {}

	// Code 44
	YaraDivisionByZeroException::YaraDivisionByZeroException() : Exception(String::Format("{0} - Code {1}", "ERROR_DIVISION_BY_ZERO", ERROR_DIVISION_BY_ZERO)) {}

	// Code 45
	YaraRegularExpressionTooLargeException::YaraRegularExpressionTooLargeException() : Exception(String::Format("{0} - Code {1}", "ERROR_REGULAR_EXPRESSION_TOO_LARGE", ERROR_REGULAR_EXPRESSION_TOO_LARGE)) {}

	// Code 46
	YaraTooManyReFibersException::YaraTooManyReFibersException() : Exception(String::Format("{0} - Code {1}", "ERROR_TOO_MANY_RE_FIBERS", ERROR_TOO_MANY_RE_FIBERS)) {}

	// Code 47
	YaraCouldNotReadProcessMemoryException::YaraCouldNotReadProcessMemoryException() : Exception(String::Format("{0} - Code {1}", "ERROR_COULD_NOT_READ_PROCESS_MEMORY", ERROR_COULD_NOT_READ_PROCESS_MEMORY)) {}

	// Code 48
	YaraInvalidExternalVariableTypeException::YaraInvalidExternalVariableTypeException() : Exception(String::Format("{0} - Code {1}", "ERROR_INVALID_EXTERNAL_VARIABLE_TYPE", ERROR_INVALID_EXTERNAL_VARIABLE_TYPE)) {}
}