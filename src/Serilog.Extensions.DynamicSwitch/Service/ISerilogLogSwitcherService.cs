using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Serilog.Extensions.DynamicSwitch
{
    public interface ISerilogLogSwitcherService
    {
        void SetLevel(string prefix, LogEventLevel level);
        void SetLevel(Guid guid, LogEventLevel level);
        IList<DynamicLoggingLevelSwitch> GetSwitches();

    }
}
