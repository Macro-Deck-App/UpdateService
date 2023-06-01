using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.ManagerInterfaces;

public interface IVersionManager
{
    public ValueTask<VersionInfo> GetLatestVersion(PlatformIdentifier platformIdentifier, bool includePreviewVersions);

    public ValueTask<VersionInfo> GetVersion(string version);

    public ValueTask<VersionEntity> GetOrCreateVersion(string version);

    public ValueTask<CheckResult> CheckForNewerVersion(
        string currentVersion,
        PlatformIdentifier platformIdentifier,
        bool includePreviewVersions);
}