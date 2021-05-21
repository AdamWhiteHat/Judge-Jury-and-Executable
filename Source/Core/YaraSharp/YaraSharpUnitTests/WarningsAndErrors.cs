using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YaraSharp;

namespace YaraSharpUnitTests
{
    [TestClass]
    public class WarningsAndErrors
    {
        private static string BaseDirectory = Environment.CurrentDirectory;
        private static string TestDataDirectory = Path.Combine(BaseDirectory, "TestData");

        /// <summary>
        /// Ensures warnings are correctly reported along with errors,
        /// but does not prevent scanning from working
        /// </summary>
        [TestMethod]
        public void CheckWarningsStillScan()
        {
            string inputFileBase = "CheckWarningStillScans";
            string yaraRuleFile = Path.Combine(TestDataDirectory, $"{inputFileBase}.yar");
            string yaraInputFile = Path.Combine(TestDataDirectory, $"{inputFileBase}.txt");

            YSInstance yaraInstance = new YSInstance();

            using (YSContext context = new YSContext())
            {
                using (YSCompiler compiler = new YSCompiler(null))
                {
                    compiler.AddFile(yaraRuleFile);
                    YSReport compilerErrors = compiler.GetErrors();
                    YSReport compilerWarnings = compiler.GetWarnings();

                    YSScanner scanner = new YSScanner(compiler.GetRules(), null);
                    Assert.IsTrue(scanner.ScanFile(yaraInputFile).Any(r => r.Rule.Identifier == "WarningRule"));
                }
            }
        }
    }
}
