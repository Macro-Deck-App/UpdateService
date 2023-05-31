using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;

namespace MacroDeck.UpdateService.Core.DataAccess.Repositories;

public class FileDownloadRepository : BaseRepository<FileDownloadEntity>, IFileDownloadRepository
{
    public FileDownloadRepository(UpdateServiceContext context)
        : base(context)
    {
    }
}