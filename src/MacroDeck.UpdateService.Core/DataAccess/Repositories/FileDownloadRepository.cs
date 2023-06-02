using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.UpdateService.Core.DataAccess.Repositories;

public class FileDownloadRepository : BaseRepository<FileDownloadEntity>, IFileDownloadRepository
{
    public FileDownloadRepository(UpdateServiceContext context)
        : base(context)
    {
    }

    public async ValueTask<long> CountAllFirstTimeDownloads()
    {
        return await Context.Set<FileDownloadEntity>()
            .CountAsync(x => x.DownloadReason == DownloadReason.FirstDownload);
    }
}