namespace MacroDeck.UpdateService.Core.DataTypes.ApiV2;

public class ApiV2VersionFileInfo
{
    public string DownloadUrl { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public long FileSize { get; set; }
}