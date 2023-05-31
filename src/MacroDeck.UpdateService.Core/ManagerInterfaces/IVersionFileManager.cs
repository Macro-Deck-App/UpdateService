using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.UpdateService.Core.ManagerInterfaces;

public interface IVersionFileManager
{
    public ValueTask<IActionResult> UploadVersionFile(byte[] file, string version, PlatformIdentifier platformIdentifier);

    public ValueTask<VersionFileResult> GetFile(string version, PlatformIdentifier platformIdentifier);

    public ValueTask CountDownload(
        string version,
        PlatformIdentifier platformIdentifier,
        DownloadReason downloadReason);
}