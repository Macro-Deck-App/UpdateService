using System.Text.Json.Serialization;
using MacroDeck.UpdateService.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MacroDeck.UpdateService.StartupConfig;

public static class RestInterfaceControllerConfig
{
    public static void RegisterRestInterfaceControllers(this IServiceCollection services)
    {
        var assembly = typeof(UpdateServiceControllerBase).Assembly;
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "AllowAny",
                    builder =>
                        builder.SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .Build());
            });
        services.AddMvc();
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opt.JsonSerializerOptions.Converters.Add(enumConverter);
                opt.JsonSerializerOptions.AllowTrailingCommas = true;
            })
            .PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
    }
}