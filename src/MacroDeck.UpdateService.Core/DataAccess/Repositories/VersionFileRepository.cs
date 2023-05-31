using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.UpdateService.Core.DataAccess.Repositories;

public class VersionFileRepository : BaseRepository<VersionFileEntity>, IVersionFileRepository
{
    public VersionFileRepository(UpdateServiceContext context)
        : base(context)
    {
    }

    public async ValueTask<bool> Exists(string version, PlatformIdentifier platformIdentifier)
    {
        return await Context.Set<VersionFileEntity>().AsNoTracking()
            .Include(x => x.Version)
            .Where(x => x.PlatformIdentifier == platformIdentifier)
            .AnyAsync(x => x.Version != null && x.Version.Version.ToLower() == version.ToLower());
    }

    public async ValueTask<VersionFileEntity?> GetVersionFile(string version, PlatformIdentifier platformIdentifier)
    {
        return await Context.Set<VersionFileEntity>().AsNoTracking()
            .Include(x => x.Version)
            .Where(x => x.PlatformIdentifier == platformIdentifier)
            .SingleOrDefaultAsync(x => x.Version != null && x.Version.Version.ToLower() == version.ToLower());
    }
}