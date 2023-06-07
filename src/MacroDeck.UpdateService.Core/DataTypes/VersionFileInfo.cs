namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionFileInfo
{
    public string DownloadUrl { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public long FileSize { get; set; }
}