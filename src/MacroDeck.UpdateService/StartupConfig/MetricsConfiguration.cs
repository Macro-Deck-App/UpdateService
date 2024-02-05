using OpenTelemetry.Metrics;

namespace MacroDeck.UpdateService.StartupConfig;

public static class MetricsConfiguration
{
    public static void AddMetricsConfiguration(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddPrometheusExporter();
            });
    }
}