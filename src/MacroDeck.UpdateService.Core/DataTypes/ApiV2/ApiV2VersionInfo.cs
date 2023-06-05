using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes.ApiV2;

public class ApiV2VersionInfo
{
    public string Version { get; set; } = string.Empty;
    public Dictionary<PlatformIdentifier, string> Platforms { get; set; } = new();
}