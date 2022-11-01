using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions.DynamicSwitch.Service;

namespace Serilog.Extensions.DynamicSwitch.Extensions
{
    public static class HostingExtensions
    {
        public static IHostBuilder AddDynamicLoggingLevel(this IHostBuilder builder, ILogger serilogLogger)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddSingleton<ISerilogLogSwitcherService>(sp =>
                {
                    var service = new SerilogLogSwitcherService(serilogLogger);
                    service.Init();

                    return service;
                });
            });

            return builder;
        }
    }
}
