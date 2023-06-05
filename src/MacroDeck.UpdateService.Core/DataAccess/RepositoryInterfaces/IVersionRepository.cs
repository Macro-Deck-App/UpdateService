using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;

public interface IVersionRepository : IBaseRepository<VersionEntity>
{
    public ValueTask<VersionInfo?> GetLatestVersion(PlatformIdentifier? platformIdentifier, bool includePreviewVersions);

    public ValueTask<VersionInfo?> GetVersionInfo(string version);

    public ValueTask<VersionEntity?> GetVersion(string version);

    public ValueTask<VersionEntity?> GetNewerVersion(
        Version currentVersion,
        PlatformIdentifier platformIdentifier,
        bool includePreviewVersions);
}