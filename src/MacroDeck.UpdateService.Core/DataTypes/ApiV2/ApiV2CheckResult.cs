namespace MacroDeck.UpdateService.Core.DataTypes.ApiV2;

public class ApiV2CheckResult
{
    public bool NewerVersionAvailable { get; set; } = false;
    public ApiV2VersionInfo? Version { get; set; }
}