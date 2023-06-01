using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    public async ValueTask<VersionInfo?> GetLatestVersion(PlatformIdentifier platformIdentifier, bool includePreviewVersions)
    {
        var versionInfo = await Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.VersionState == VersionState.Published)
            .Where(x => x.Files.Any(y => y.PlatformIdentifier == platformIdentifier))
            .Where(x => includePreviewVersions && x.IsPreviewVersion || !x.IsPreviewVersion)
            .OrderByDescending(x => x.Major)
            .ThenByDescending(x => x.Minor)
            .ThenByDescending(x => x.Patch)
            .ThenBy(x => x.IsPreviewVersion)
            .ThenByDescending(x => x.PreviewNo ?? 0)
            .Include(x => x.Files)
            .ProjectTo<VersionInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (versionInfo == null)
        {
            return null;
        }

        var downloads = await Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Version == versionInfo.Version)
            .SelectMany(v => v.Files)
            .SelectMany(f => f.FileDownloads)
            .LongCountAsync(fd => fd.DownloadReason == DownloadReason.FirstDownload);

        versionInfo.Downloads = downloads;

        return versionInfo;
    }

    public async ValueTask<VersionInfo?> GetVersionInfo(string version)
    {
        var versionInfo = await GetVersionBaseQuery(version)
            .ProjectTo<VersionInfo>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        if (versionInfo == null)
        {
            return null;
        }

        var downloads = await Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Version == versionInfo.Version)
            .SelectMany(v => v.Files)
            .SelectMany(f => f.FileDownloads)
            .LongCountAsync(fd => fd.DownloadReason == DownloadReason.FirstDownload);

        versionInfo.Downloads = downloads;
        return versionInfo;
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
            .Where(x => x.VersionState == VersionState.Published)
            .Where(x => x.Files.Any(y => y.PlatformIdentifier == platformIdentifier))
            .Where(x => includePreviewVersions && x.IsPreviewVersion || !x.IsPreviewVersion)
            .Where(x => x.Major > currentVersion.Major
                        || (x.Major == currentVersion.Major && x.Minor > currentVersion.Minor)
                        || (x.Major == currentVersion.Major && x.Minor == currentVersion.Minor &&
                            x.Patch > currentVersion.Patch)
                        || (x.Major == currentVersion.Major && x.Minor == currentVersion.Minor &&
                            x.Patch == currentVersion.Patch
                            && ((x.IsPreviewVersion == false && currentVersion.PreviewNo.HasValue)
                                || (x.PreviewNo.HasValue && x.PreviewNo > currentVersion.PreviewNo))))
            .OrderByDescending(x => x.Major)
            .ThenByDescending(x => x.Minor)
            .ThenByDescending(x => x.Patch)
            .ThenBy(x => x.IsPreviewVersion)
            .ThenByDescending(x => x.PreviewNo ?? 0)
            .FirstOrDefaultAsync();
    }

    private IQueryable<VersionEntity> GetVersionBaseQuery(string version)
    {
        return Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Version.ToLower() == version.ToLower())
            .Include(x => x.Files)
            .ThenInclude(x => x.FileDownloads);
    }
}