# YaraSharp
C# wrapper around the [Yara pattern matching library](https://github.com/VirusTotal/yara).

Use signatures form [Loki](https://github.com/Neo23x0/signature-base/tree/master/yara) or [Yara](https://github.com/Yara-Rules/rules).

Nuget package is [available](https://www.nuget.org/packages/YaraSharp)
## Usage
```C#
//  All API calls happens here
YSInstance YSInstance = new YSInstance();
        
//  Declare external variables (could be null)
Dictionary<string, object> externals = new Dictionary<string, object>()
{
    { "filename", string.Empty },
    { "filepath", string.Empty },
    { "extension", string.Empty }
};

//	Get list of YARA rules
List<string> ruleFilenames = Directory.GetFiles(@"D:\Test\yara", "*.yar", SearchOption.AllDirectories).ToList();

//  Context is where yara is initialized
//  From yr_initialize() to yr_finalize()
using (YSContext context = new YSContext())
{
    //	Compiling rules
    using (YSCompiler compiler = instance.CompileFromFiles(ruleFilenames, externals))
    {
        //  Get compiled rules
        YSRules rules = compiler.GetRules();

        //  Get errors
        YSReport errors = compiler.GetErrors();
        //  Get warnings
        YSReport warnings = compiler.GetWarnings();


        //  Some file to test yara rules
        string Filename = @"";

		//  Get matches
		List<YSMatches> Matches = instance.ScanFile(Filename, rules,
				new Dictionary<string, object>()
				{
					{ "filename", Alphaleonis.Win32.Filesystem.Path.GetFileName(Filename) },
					{ "filepath", Alphaleonis.Win32.Filesystem.Path.GetFullPath(Filename) },
					{ "extension", Alphaleonis.Win32.Filesystem.Path.GetExtension(Filename) }
				}, 
				0);

		//  Iterate over matches
		foreach (YSMatches Match in Matches)
		{
			//...
		}
	}
	//  Log errors
}
```
For async scanning use **must** call destroy methods:
```C#
YaraSharp.CYaraSharp YSInstance = new CYaraSharp();
YaraSharp.CContext YSContext = new YaraSharp.CContext();
YaraSharp.CRules YSRules = YSInstance.CompileFromFiles(RuleFilenames, null, out Errors);

//  Async here

YSRules.Destroy();
YSContext.Destroy();
```
## Reference
[Libyara C API documentation](http://yara.readthedocs.io/en/v3.7.0/capi.html) for a general overview on how to use libyara. 

## Features and limitations

* Metadata supported
* Externals supported
* Async scanning supported
* It seems (through debug sessions) that modules are supported, but i haven't had cases that certanly used them. So this question is opened

## Note
Soultion contains 2 projects:
- yara-master - where you *can* update yara sources for a new version
- YaraSharp - where you *can* modify sources in order to add / repair wrapper features

## Other
Build in vs 2017

Compiled with yara 3.8.1

Yara patched to support unicode paths

You can use or modify the sources however you want

Special thanks to [kallanreed](https://github.com/kallanreed/libyara.NET)
