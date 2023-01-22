using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Extensions.DynamicSwitch;

namespace AspnetCoreSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DynamicSwitchController : ControllerBase
    {
        private static Serilog.ILogger Logger = Log.ForContext<DynamicSwitchController>();
        private readonly ISerilogLogSwitcherService _logSwitcherService;
        

        public DynamicSwitchController(ISerilogLogSwitcherService logSwitcherService)
        {
            _logSwitcherService = logSwitcherService;
        }

        [HttpGet]
        public IEnumerable<DynamicLoggingLevelSwitch> Get()
        {
            Logger.Information("Log");
            Logger.Warning("Log");
            Logger.Debug("Log");
            return _logSwitcherService.GetSwitches();
        }
    }
}