using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;

public interface IVersionFileRepository : IBaseRepository<VersionFileEntity>
{
    public ValueTask<bool> Exists(string version, PlatformIdentifier platformIdentifier);

    public ValueTask<VersionFileEntity?> GetVersionFile(string version, PlatformIdentifier platformIdentifier);
}