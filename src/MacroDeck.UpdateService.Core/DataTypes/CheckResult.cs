namespace MacroDeck.UpdateService.Core.DataTypes;

public class CheckResult
{
    public bool NewerVersionAvailable { get; set; } = false;
    public string? Version { get; set; }
}