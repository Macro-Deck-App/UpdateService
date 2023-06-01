using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.ManagerInterfaces;

public interface IVersionFileManager
{
    public ValueTask<IActionResult> UploadVersionFile(byte[] file, string fileExtension, Version version, PlatformIdentifier platformIdentifier);

    public ValueTask<VersionFileResult> GetFile(string version, PlatformIdentifier platformIdentifier);

    public ValueTask CountDownload(
        string version,
        PlatformIdentifier platformIdentifier,
        DownloadReason downloadReason);
}