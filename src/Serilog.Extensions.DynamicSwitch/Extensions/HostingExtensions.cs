using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Serilog.Extensions.DynamicSwitch
{
    public static class HostingExtensions
    {
        public static IHostBuilder AddDynamicLoggingLevel(this IHostBuilder builder, ILogger serilogLogger, LoggingLevelSwitchConfiguration llsConfiguration)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddSingleton<ISerilogLogSwitcherService>(sp =>
                {
                    var service = new SerilogLogSwitcherService(serilogLogger);
                    service.Init(llsConfiguration);

                    return service;
                });
            });

            return builder;
        }
    }
}
