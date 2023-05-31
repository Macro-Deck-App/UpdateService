namespace MacroDeck.UpdateService.StartupConfig;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    { 
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}