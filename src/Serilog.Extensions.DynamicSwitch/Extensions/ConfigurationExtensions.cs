using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Extensions.DynamicSwitch
{
    public static class ConfigurationExtensions
    {
        public static LoggerConfiguration Configuration(this LoggerSettingsConfiguration settingsConfiguration,
                                                        IConfiguration configuration,
                                                        out LoggingLevelSwitchConfiguration llsConfig,
                                                        DependencyContext dependencyContext = null)
        {
            llsConfig = ExtractSwitches(configuration.GetSection("Serilog"));
            return ConfigurationLoggerConfigurationExtensions.Configuration(settingsConfiguration, configuration, dependencyContext);
        }

        private static LoggingLevelSwitchConfiguration ExtractSwitches(IConfigurationSection section)
        {
            var llsConfig = new LoggingLevelSwitchConfiguration();

            var levelSwitchesDirective = section.GetSection("LevelSwitches");
            foreach(var levelSwitchDeclaration in levelSwitchesDirective.GetChildren())
            {
                var switchName = levelSwitchDeclaration.Key;
                var @switch = new DynamicLoggingLevelSwitch(switchName);

                llsConfig.Switches.Add(switchName, @switch);
            }

            var minimumLevelDirective = section.GetSection("MinimumLevel");
            var minLevelControlledByDirective = minimumLevelDirective.GetSection("ControlledBy");
            if (minLevelControlledByDirective.Value != null)
            {
                if (llsConfig.Switches.ContainsKey(minLevelControlledByDirective.Value))
                {
                    var @switch = llsConfig.Switches[minLevelControlledByDirective.Value];
                    @switch.AddPrefix("*");
                }
            }

            foreach(var ovverideDirective in minimumLevelDirective.GetSection("Override").GetChildren())
            {
                var overridePrefix = ovverideDirective.Key;
                var overridenLevelOrSwitch = ovverideDirective.Value;
                if (!Enum.TryParse(overridenLevelOrSwitch, out LogEventLevel _))
                {
                    if (llsConfig.Switches.ContainsKey(overridenLevelOrSwitch))
                    {
                        var @switch = llsConfig.Switches[overridenLevelOrSwitch];
                        @switch.AddPrefix(overridePrefix);
                    }
                }

            }

            return llsConfig;
        }
    }
}
