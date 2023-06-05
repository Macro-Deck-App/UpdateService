using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.Managers;

public class VersionManager : IVersionManager
{
    private readonly IVersionRepository _versionRepository;

    public VersionManager(IVersionRepository versionRepository)
    {
        _versionRepository = versionRepository;
    }

    public async ValueTask<Version> GetNextMajorVersion()
    {
        var currentVersion = await GetCurrentVersion(true);
        return new Version
        {
            Major = currentVersion.IsBetaVersion ? currentVersion.Major : currentVersion.Major + 1, 
            Minor = 0,
            Patch = 0,
            BetaNo = null
        };
    }

    public async ValueTask<Version> GetNextMinorVersion()
    {
        var currentVersion = await GetCurrentVersion(true);
        return currentVersion with
        {
            Minor = currentVersion.IsBetaVersion ? currentVersion.Minor : currentVersion.Minor + 1,
            Patch = 0,
            BetaNo = null
        };
    }

    public async ValueTask<Version> GetNextPatchVersion()
    {
        var currentVersion = await GetCurrentVersion();
        return currentVersion with
        {
            Patch = currentVersion.Patch + 1,
            BetaNo = null
        };
    }

    public async ValueTask<Version> GetNextMajorBetaVersion()
    {
        var currentVersion = await GetCurrentVersion(true);
        if (currentVersion.IsBetaVersion)
        {
            return currentVersion with
            {
                BetaNo = currentVersion.BetaNo + 1
            };
        }

        var nextMajorVersion = await GetNextMajorVersion();
        return nextMajorVersion with
        {
            BetaNo = 1
        };
    }

    public async ValueTask<Version> GetNextMinorBetaVersion()
    {
        var currentVersion = await GetCurrentVersion(true);
        if (currentVersion.IsBetaVersion)
        {
            return currentVersion with
            {
                BetaNo = currentVersion.BetaNo + 1
            };
        }

        var nextMinorVersion = await GetNextMinorVersion();
        return nextMinorVersion with
        {
            BetaNo = 1
        };
    }

    public async ValueTask<VersionInfo> GetLatestVersion(PlatformIdentifier platformIdentifier, bool includePreviewVersions)
    {
        return await _versionRepository.GetLatestVersion(platformIdentifier, includePreviewVersions)
               ?? throw new NoVersionFoundException();
    }

    public async ValueTask<VersionInfo> GetVersion(string version)
    {
        return await _versionRepository.GetVersionInfo(version)
               ?? throw new VersionDoesNotExistException();
    }

    public async ValueTask<VersionEntity> GetOrCreateVersion(string version)
    {
        var versionEntity = await _versionRepository.GetVersion(version);
        if (versionEntity != null)
        {
            return versionEntity;
        }

        versionEntity = new VersionEntity
        {
            Version = version
        };

        await _versionRepository.InsertAsync(versionEntity);
        return versionEntity;
    }

    public async ValueTask<CheckResult> CheckForNewerVersion(
        string currentVersion,
        PlatformIdentifier platformIdentifier,
        bool includePreviewVersions)
    {
        if (!Version.TryParse(currentVersion, out var version))
        {
            throw new CannotParseVersionException();
        }

        var newerVersion =
            await _versionRepository.GetNewerVersion(version, platformIdentifier, includePreviewVersions);
        if (newerVersion == null)
        {
            return new CheckResult
            {
                NewerVersionAvailable = false
            };
        }

        return new CheckResult
        {
            NewerVersionAvailable = true,
            Version = newerVersion.Version
        };
    }

    private async ValueTask<Version> GetCurrentVersion(bool includeBetaVersions = false)
    {
        var currentVersionInfo = await _versionRepository.GetLatestVersion(null, includeBetaVersions);
        if (currentVersionInfo == null)
        {
            throw new NoVersionFoundException();
        }
        
        return Version.Parse(currentVersionInfo.Version);
    }
}