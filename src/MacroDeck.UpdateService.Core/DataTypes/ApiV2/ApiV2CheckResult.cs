namespace MacroDeck.UpdateService.Core.DataTypes.ApiV2;

public class ApiV2CheckResult
{
    public bool NewerVersionAvailable { get; set; } = false;
    public string? Version { get; set; }
}