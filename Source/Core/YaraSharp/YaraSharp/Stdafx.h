// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>
#include <msclr\lock.h>

using namespace System;
using namespace msclr::interop;
using namespace System::Reflection;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

#include "yara.h"
#include "Report.h"
#include "Exceptions.h"
#include "Context.h"
#include "Rules.h"
#include "Compiler.h"
#include "Matches.h"
#include "Scanner.h"
#include "Instance.h"