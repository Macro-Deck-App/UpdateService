using MacroDeck.UpdateService.Core.Helper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace MacroDeck.UpdateService.StartupConfig;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        if (EnvironmentHelper.IsProduction)
        {
            return;
        }
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "x-admin-token",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "API Key Authorization header",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
            
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Macro Deck UpdateService {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }
        });
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        if (EnvironmentHelper.IsProduction)
        {
            return;
        }
        
        app.UseSwagger();
        
        var apiVersionDescriptionProvider =
            app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);
            }
        });
    }
}