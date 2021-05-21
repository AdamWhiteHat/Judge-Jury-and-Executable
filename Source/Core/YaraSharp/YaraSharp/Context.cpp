#include "Stdafx.h"

//	Context
namespace YaraSharp
{
	YSContext::YSContext()
	{ 
		YSException::ThrowOnError(yr_initialize()); 
	}

	YSContext::~YSContext()
	{ 
		YSException::ThrowOnError(yr_finalize()); 
	}

	void YSContext::Destroy()
	{ 
		delete this; 
	}
}