using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public enum LoggingVerbosity
    {
        [Description("Logs only when there is an important event, like an error.")]
        Quiet,
        [Description("One line per file.")]   
        Standard,
        [Description("Reports when entering and exiting each section (timing metric category); 14-19 lines per file.")]
        Verbose
    }
}
