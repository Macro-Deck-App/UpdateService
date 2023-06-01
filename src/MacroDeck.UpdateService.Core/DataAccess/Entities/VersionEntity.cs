using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class VersionEntity : BaseEntity
{
    public string Version { get; set; } = string.Empty;
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public int? PreviewNo { get; set; }
    public bool IsPreviewVersion { get; set; }
    public VersionState VersionState { get; set; }
    public ICollection<VersionFileEntity> Files { get; set; } = new List<VersionFileEntity>();
}