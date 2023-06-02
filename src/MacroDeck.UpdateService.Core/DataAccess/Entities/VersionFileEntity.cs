using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class VersionFileEntity : BaseEntity
{
    public PlatformIdentifier PlatformIdentifier { get; set; }
    public string SavedFileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public ICollection<FileDownloadEntity> FileDownloads { get; set; } = new List<FileDownloadEntity>();

    public int VersionId { get; set; }
    public VersionEntity? Version { get; set; }
}