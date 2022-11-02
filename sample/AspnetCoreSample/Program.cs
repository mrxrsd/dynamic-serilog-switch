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
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

        try
        {
            Log.Information("Starting application...");

            CreateHostBuilder(args).Build().Run();
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

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .AddDynamicLoggingLevel(Log.Logger)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}