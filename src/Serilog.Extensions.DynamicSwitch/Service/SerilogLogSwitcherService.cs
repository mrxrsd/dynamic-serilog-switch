using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Extensions.DynamicSwitch
{
    public class SerilogLogSwitcherService : ISerilogLogSwitcherService
    {
        private readonly ILogger _serilogLogger;
        private List<DynamicLoggingLevelSwitch> _switches;

        public SerilogLogSwitcherService(ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger;
        }

        public void SetLevel(string prefix, LogEventLevel level)
        {
            var ls = GetSwitchByPrefix(prefix);
            if (ls != null)
            {
                ls.MinimumLevel = level;
            }
        }

        private LoggingLevelSwitch GetSwitchByPrefix(string prefix)
        {
            if (_switches != null && _switches.Any())
            {
                return _switches.FirstOrDefault(x => x.Prefix == prefix)?.Switch;
            }

            return null;
        }

        internal void Init()
        {
            _switches = new List<DynamicLoggingLevelSwitch>();

            var overridesMapProperty = _serilogLogger.GetType().GetField("_overrideMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var overridesMapValue    = overridesMapProperty.GetValue(_serilogLogger);

            var overridesProperty = overridesMapValue.GetType().GetField("_overrides", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var overridesValue    = overridesProperty.GetValue(overridesMapValue);


            foreach(var overrideValue in (IEnumerable) overridesValue)
            {
                var ctxProperty     = overrideValue.GetType().GetProperty("Context");
                var lsProperty      = overrideValue.GetType().GetProperty("LevelSwitch");

                _switches.Add(new DynamicLoggingLevelSwitch((string) ctxProperty.GetValue(overrideValue), (LoggingLevelSwitch) lsProperty.GetValue(overrideValue)));
            }

        }

        public IList<LoggingLevelContext> GetSwitches()
        {
            return _switches.Select(x => new LoggingLevelContext
            {
                Prefix = x.Prefix,
                Level = x.Switch.MinimumLevel
            })?.ToList();
        }
    }
}
