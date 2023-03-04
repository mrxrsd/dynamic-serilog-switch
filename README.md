# Dynamic Serilog Switch
Dynamic Serilog Switch is a extension for serilog that expose a service to retrieve and change LoggingLevelSwitches at runtime.

## Getting started

**1.** Add [the NuGet package](https://www.nuget.org/packages/dynamic-serilog-switch) as a dependency of your project either with the package manager.

**2.** Configure your serilog config file using switches as describe in Serilog Docs.

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console" ],
    "LevelSwitches": {
      "$mainSwitch": "Verbose",
      "$appSwitch":  "Verbose"
    },
    "MinimumLevel": {
      "ControlledBy": "$mainSwitch",
      "Default": "Information",
      "Override": {
        "AspnetCoreSample": "$appSwitch",
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" }
    ]
  }
}
```

**2.** In your `Program` class, call `Configure` overload that exposes `LoggingLevelSwitchConfiguration`:

```csharp
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                .AddJsonFile("serilog.json")
                                .Build();

        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration, out var llsConfig)
                            .CreateLogger();

        CreateHostBuilder(args, llsConfig).Build().Run();
        
    }
```

**3.** Call `AddDynamicLogginLevel` passing serilog logger and switches configuration. That method will inject `ISerilogLogSwitcherService` using dotnet service collection. 

```csharp
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .AddDynamicLoggingLevel(Log.Logger, llsConfig)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
```

**4.** [Optional] Call `UseDynamicLoggingLevelHelperPage` to inject a middleware that you can list all switches and change their logging levels.

```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDynamicLoggingLevelHelperPage();
        }
```

## License

MIT
