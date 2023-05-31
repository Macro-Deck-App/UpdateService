using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionInfo
{
    public string Version { get; set; } = string.Empty;
    public VersionState VersionState { get; set; }
    public long Downloads { get; set; }
    public PlatformIdentifier[] SupportedPlatforms { get; set; } = Array.Empty<PlatformIdentifier>();
}