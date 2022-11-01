using Serilog.Core;
using Serilog.Events;

namespace Serilog.Extensions.DynamicSwitch
{
    public class DynamicLoggingLevelSwitch
    {
        public LoggingLevelSwitch Switch { get; set; }
        public string Prefix { get; set; }

        public DynamicLoggingLevelSwitch(string prefix, LoggingLevelSwitch lls)
        {
            Prefix = prefix;
            Switch = lls;
        }
    }

    public class LoggingLevelContext
    {
        public string Prefix { get; set; }
        public LogEventLevel Level {get;set;}
    }
}
