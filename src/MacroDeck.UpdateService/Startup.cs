using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess;
using MacroDeck.UpdateService.Core.DataAccess.Interceptors;
using MacroDeck.UpdateService.Core.Middleware;
using MacroDeck.UpdateService.StartupConfig;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MacroDeck.UpdateService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<UpdateServiceContext>(options =>
        {
            var loggerFactory = new LoggerFactory().AddSerilog();
            options.UseLoggerFactory(loggerFactory);
            options.UseNpgsql(UpdateServiceConfiguration.DatabaseConnectionStringOverride
                              ?? UpdateServiceConfiguration.DatabaseConnectionString);
            options.AddInterceptors(new SaveChangesInterceptor());
        });
        services.AddSwagger();
        services.RegisterAutoMapperProfiles();
        services.RegisterRestInterfaceControllers();
        services.RegisterClassesEndsWithAsScoped("Repository");
        services.RegisterClassesEndsWithAsScoped("Manager");
        services.AddMetricsConfiguration();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAny");
        app.UseStaticFiles();
        app.UseRouting();
        app.ConfigureSwagger();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPrometheusScrapingEndpoint();
            endpoints.MapControllers();
        });
    }
}