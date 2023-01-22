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
        private IList<DynamicLoggingLevelSwitch> _switches;

        public SerilogLogSwitcherService(ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger;
        }

        public void SetLevel(string prefix, LogEventLevel level)
        {
            var ls = GetSwitchByPrefix(prefix)?.Switch;
            SetLevelInternal(ls, level);
        }
        public IList<DynamicLoggingLevelSwitch> GetSwitches()
        {
            return _switches ?? Array.Empty<DynamicLoggingLevelSwitch>();
        }

        public void SetLevel(Guid id, LogEventLevel level)
        {
            var ls = _switches?.FirstOrDefault(x => x.Id == id)?.Switch;
            SetLevelInternal(ls, level);
        }

        private void SetLevelInternal(LoggingLevelSwitch lls, LogEventLevel level)
        {
            if (lls != null)
            {
                lls.MinimumLevel = level;
            }
        }

        private DynamicLoggingLevelSwitch GetSwitchByPrefix(string prefix)
        {
            if (_switches != null && _switches.Any())
            {
                return _switches.FirstOrDefault(x => x.Prefixes != null && x.Prefixes.Exists(y => y == prefix));
            }

            return null;
        }

        internal void Init(LoggingLevelSwitchConfiguration llsConfiguration)
        {
            _switches = new List<DynamicLoggingLevelSwitch>();

            var overridesMapProperty = _serilogLogger.GetType().GetField("_overrideMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var overridesMapValue    = overridesMapProperty.GetValue(_serilogLogger);

            var defaultSwitchProperty = overridesMapValue.GetType().GetField("_defaultLevelSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var defaultSwitchValue    = (LoggingLevelSwitch) defaultSwitchProperty.GetValue(_serilogLogger);

            var defaultSwitchContext = GetSwitchByPrefix("*");
            defaultSwitchContext.AssociateSwitch(defaultSwitchValue);

            var overridesProperty = overridesMapValue.GetType().GetField("_overrides", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var overridesValue    = overridesProperty.GetValue(overridesMapValue);


            foreach(var overrideValue in (IEnumerable) overridesValue)
            {
                var ctxProperty     = overrideValue.GetType().GetProperty("Context");
                var lsProperty      = overrideValue.GetType().GetProperty("LevelSwitch");

                var serilogSwitch = (LoggingLevelSwitch)lsProperty.GetValue(overrideValue);
                var serilogPrefix = (string)ctxProperty.GetValue(overrideValue);

                var currSwitchContext = GetSwitchByPrefix(serilogPrefix);

                if (currSwitchContext != null)
                {
                    currSwitchContext.AssociateSwitch(serilogSwitch);
                }
            }

        }
    }
}
