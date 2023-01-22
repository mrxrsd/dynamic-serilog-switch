using Serilog.Core;
using System;
using System.Collections.Generic;

namespace Serilog.Extensions.DynamicSwitch
{
    public class DynamicLoggingLevelSwitch
    {
        public Guid Id { get; set; }
        public string SwitchName { get; set; }
        public LoggingLevelSwitch Switch { get; set; }
        public List<string> Prefixes { get; set; }

        public DynamicLoggingLevelSwitch(string switchName)
        {
            Id         = Guid.NewGuid();
            SwitchName = switchName;
            Prefixes   = new List<string>();
        }

        public void AddPrefix(string prefix)
        {
            Prefixes.Add(prefix);
        }

        internal void AssociateSwitch(LoggingLevelSwitch lls)
        {
            Switch = lls;
        }
    }
}
