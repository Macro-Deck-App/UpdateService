using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class FileDownloadEntity : BaseEntity
{
    public DownloadReason DownloadReason { get; set; }
    public PlatformIdentifier PlatformIdentifier { get; set; }

    public int VersionFileId { get; set; }
    public VersionFileEntity? VersionFile { get; set; }
}