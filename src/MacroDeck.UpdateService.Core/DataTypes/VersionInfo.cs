using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionInfo
{
    public string Version { get; set; } = string.Empty;
    public bool IsBeta { get; set; }
    public Dictionary<PlatformIdentifier, VersionFileInfo> Platforms { get; set; } = new();
}