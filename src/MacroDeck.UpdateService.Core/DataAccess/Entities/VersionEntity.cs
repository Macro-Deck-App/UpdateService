namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class VersionEntity : BaseEntity
{
    public string Version { get; set; } = string.Empty;
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public int? PreReleaseNo { get; set; }
    public bool IsBetaVersion { get; set; }
    public ICollection<VersionFileEntity> Files { get; set; } = new List<VersionFileEntity>();
}