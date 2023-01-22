using Microsoft.AspNetCore.Http;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Extensions.DynamicSwitch
{
    public class DynamicLoggingLevelPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISerilogLogSwitcherService _serilogSwitchService;

        public DynamicLoggingLevelPageMiddleware(RequestDelegate next, ISerilogLogSwitcherService serilogSwitchService)
        {
            _next = next;
            _serilogSwitchService = serilogSwitchService;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var maybeValues = ParseParams(httpContext);

            httpContext.Response.ContentType = "text/html";

            if (maybeValues.Item1)
            {
                if (Guid.TryParse(maybeValues.Item2, out var id))
                {
                    _serilogSwitchService.SetLevel(id, maybeValues.Item3.GetValueOrDefault());
                }
            }

            var switches = _serilogSwitchService.GetSwitches();

            return httpContext.Response.WriteAsync(FormatHtml(switches));
        }

        private string FormatHtml(IList<DynamicLoggingLevelSwitch> switches)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1>Dynamic Serilog Switches</h1>");
            foreach (var sw in switches)
            {
                sb.AppendLine($"<b>Switch..:</b> {sw.Id}<br>");
                sb.AppendLine($"<b>Name...:</b> {sw.SwitchName}<br>");
                sb.AppendLine($"<b>Level....:</b> {sw.Switch.MinimumLevel}<br>");
                sb.AppendLine($"<b>Prefixes:</b><ul>");
                sw.Prefixes?.ForEach(prefix => sb.AppendLine($"<li>{prefix}</li>"));
                sb.AppendLine($"</ul>");
                sb.AppendLine($"<br>");
            }

            return sb.ToString();
        }

        private static Tuple<bool, string, LogEventLevel?> ParseParams(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (!path.HasValue) return new Tuple<bool, string, LogEventLevel?>(false, null, null);

            var segments = path.Value.Split('/');

            if (segments.Length > 3) return new Tuple<bool, string, LogEventLevel?> (false, null, null);

            if (Enum.TryParse(segments[2], true, out LogEventLevel result))
            {
                return new Tuple<bool, string, LogEventLevel?>(true, segments[1], result);
            }

            return new Tuple<bool, string, LogEventLevel?>(false, null, null);
        }
    }
}