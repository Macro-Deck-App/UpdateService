namespace MacroDeck.UpdateService.Core.Configuration;

public static partial class UpdateServiceConfiguration
{
    public static string DatabaseConnectionString => GetString("database:connection_string");
    public static string? DatabaseConnectionStringOverride { get; set; }
    public static string AdminAuthenticationToken => GetString("authentication:admin_token");
    public static string DataPath => GetString("paths:data_path");
}