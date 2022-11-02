using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Extensions.DynamicSwitch
{
    public interface ISerilogLogSwitcherService
    {
        void SetLevel(string prefix, LogEventLevel level);
        IList<LoggingLevelContext> GetSwitches();

    }
}
