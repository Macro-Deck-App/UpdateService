using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess.Extensions;
using MacroDeck.UpdateService.Core.Helper;
using MacroDeck.UpdateService.StartupConfig;
using Serilog;

namespace MacroDeck.UpdateService;

public static class Program
{
    public static async Task Main(string[] args) 
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

        await UpdateServiceConfiguration.Initialize();
        
        var app = Host.CreateDefaultBuilder(args)
            .ConfigureSerilog()
            .ConfigureWebHostDefaults(hostBuilder =>
            {
                hostBuilder.UseStartup<Startup>();
                hostBuilder.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = long.MaxValue;
                    options.ListenAnyIP(EnvironmentHelper.HostingPort);
                });
            }).Build();

        await app.Services.MigrateDatabaseAsync();
        await app.RunAsync();
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Logger.Fatal(e.ExceptionObject as Exception,
            "Unhandled exception {Terminating}",
            e.IsTerminating
                ? "Terminating"
                : "Not terminating");
    }
}