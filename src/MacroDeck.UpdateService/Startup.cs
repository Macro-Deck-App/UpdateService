using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess;
using MacroDeck.UpdateService.Core.DataAccess.Interceptors;
using MacroDeck.UpdateService.Core.Middleware;
using MacroDeck.UpdateService.StartupConfig;
using Microsoft.AspNetCore.Http.Features;
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
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue;
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAny");
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.ConfigureSwagger();
    }
}