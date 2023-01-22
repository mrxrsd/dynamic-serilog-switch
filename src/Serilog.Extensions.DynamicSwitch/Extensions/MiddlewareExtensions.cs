using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Serilog.Extensions.DynamicSwitch
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDynamicLoggingLevelHelperPage(this IApplicationBuilder app, string path = "/dynamic-serilog-switch")
        {
            app.Map(new PathString(path), x => x.UseMiddleware<DynamicLoggingLevelPageMiddleware>());
            return app;
        }
    }
}
