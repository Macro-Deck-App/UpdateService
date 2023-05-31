using AutoMapper;
using AutoMapper.QueryableExtensions;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.EntityFrameworkCore;

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
            .Include(x => x.Files)
            .Where(x => x.Files.Any(y => y.PlatformIdentifier == platformIdentifier))
            .Where(x => (includePreviewVersions == true && x.VersionState == VersionState.Preview) ||
                        x.VersionState == VersionState.Release)
            .OrderByDescending(x => x.CreatedTimestamp)
            .Take(1)
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

    private IQueryable<VersionEntity> GetVersionBaseQuery(string version)
    {
        return Context.Set<VersionEntity>().AsNoTracking()
            .Where(x => x.Version.ToLower() == version.ToLower())
            .Include(x => x.Files)
            .ThenInclude(x => x.FileDownloads);
    }
}