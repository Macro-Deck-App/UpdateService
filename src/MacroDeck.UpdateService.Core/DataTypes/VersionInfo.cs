using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionInfo
{
    public string Version { get; set; } = string.Empty;
    public Dictionary<PlatformIdentifier, string> Platforms { get; set; } = new();
}