using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes.ApiV2;

public class ApiV2VersionInfo
{
    public string Version { get; set; } = string.Empty;
    public bool IsBeta { get; set; }
    public Dictionary<PlatformIdentifier, ApiV2VersionFileInfo> Platforms { get; set; } = new();
}