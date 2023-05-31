using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class VersionEntity : BaseEntity
{
    public string Version { get; set; } = string.Empty;
    public VersionState VersionState { get; set; }
    public ICollection<VersionFileEntity> Files { get; set; } = new List<VersionFileEntity>();
}