using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Extensions.DynamicSwitch
{
    public class LoggingLevelSwitchConfiguration
    {
        public Dictionary<string, DynamicLoggingLevelSwitch> Switches { get; set; } = new Dictionary<string, DynamicLoggingLevelSwitch>();
    }
}
