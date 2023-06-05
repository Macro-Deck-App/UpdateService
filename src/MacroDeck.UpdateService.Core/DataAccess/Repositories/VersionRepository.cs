using AutoMapper;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.DataAccess.Repositories;

public class VersionRepository : BaseRepository<VersionEntity>, IVersionRepository
{
    private readonly IMapper _mapper;

    public VersionRepository(UpdateServiceContext context, IMapper mapper)
        : base(context)
    {
        _mapper = mapper;
    }

    public async ValueTask<VersionInfo?> GetLatestVersion(PlatformIdentifier? platformIdentifier, bool includePreviewVersions)
    {
        var versionEntity = await Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Files.Any(y => platformIdentifier == null || y.PlatformIdentifier == platformIdentifier))
            .Where(x => includePreviewVersions && x.IsBetaVersion || !x.IsBetaVersion)
            .OrderByDescending(x => x.Major)
            .ThenByDescending(x => x.Minor)
            .ThenByDescending(x => x.Patch)
            .ThenBy(x => x.IsBetaVersion)
            .ThenByDescending(x => x.PreReleaseNo ?? 0)
            .Include(x => x.Files)
            .FirstOrDefaultAsync();

        return _mapper.Map<VersionInfo>(versionEntity);
    }

    public async ValueTask<VersionInfo?> GetVersionInfo(string version)
    {
        var versionEntity = await GetVersionBaseQuery(version)
            .SingleOrDefaultAsync();
        
        return _mapper.Map<VersionInfo>(versionEntity);
    }
    
    public async ValueTask<VersionEntity?> GetVersion(string version)
    {
        return await GetVersionBaseQuery(version).SingleOrDefaultAsync();
    }

    public async ValueTask<VersionEntity?> GetNewerVersion(
        Version currentVersion,
        PlatformIdentifier platformIdentifier,
        bool includePreviewVersions)
    {
        return await Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Files.Any(y => y.PlatformIdentifier == platformIdentifier))
            .Where(x => includePreviewVersions && x.IsBetaVersion || !x.IsBetaVersion)
            .Where(x => x.Major > currentVersion.Major
                        || (x.Major == currentVersion.Major && x.Minor > currentVersion.Minor)
                        || (x.Major == currentVersion.Major && x.Minor == currentVersion.Minor &&
                            x.Patch > currentVersion.Patch)
                        || (x.Major == currentVersion.Major && x.Minor == currentVersion.Minor &&
                            x.Patch == currentVersion.Patch
                            && ((x.IsBetaVersion == false && currentVersion.BetaNo.HasValue)
                                || (x.PreReleaseNo.HasValue && x.PreReleaseNo > currentVersion.BetaNo))))
            .OrderByDescending(x => x.Major)
            .ThenByDescending(x => x.Minor)
            .ThenByDescending(x => x.Patch)
            .ThenBy(x => x.IsBetaVersion)
            .ThenByDescending(x => x.PreReleaseNo ?? 0)
            .FirstOrDefaultAsync();
    }

    private IQueryable<VersionEntity> GetVersionBaseQuery(string version)
    {
        return Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Version.ToLower() == version.ToLower())
            .Include(x => x.Files);
    }
}