using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class VersionFileEntity : BaseEntity
{
    public FileProvider FileProvider { get; set; }
    public PlatformIdentifier PlatformIdentifier { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public long FileSize { get; set; }
    
    public int VersionId { get; set; }
    public VersionEntity? Version { get; set; }
}