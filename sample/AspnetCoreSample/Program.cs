using AspnetCoreSample;
using Serilog;
using Serilog.Extensions.DynamicSwitch;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                .AddJsonFile("serilog.json")
                                .Build();

        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration, out var llsConfig)
                            .CreateLogger();

        try
        {
            Log.Information("Starting application...");

            CreateHostBuilder(args, llsConfig).Build().Run();
        }
        catch(Exception ex)
        {
            Log.Fatal(ex, "The application failed to start.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
    }

    public static IHostBuilder CreateHostBuilder(string[] args, LoggingLevelSwitchConfiguration llsConfig) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .AddDynamicLoggingLevel(Log.Logger, llsConfig)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}