using MacroDeck.UpdateService.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.ManagerInterfaces;

public interface IVersionFileManager
{
    public ValueTask<IActionResult> CreateVersionFile(
        FileProvider fileProvider,
        string fileName,
        string fileHash,
        long fileSize,
        Version version,
        PlatformIdentifier platformIdentifier);

    public ValueTask<double> GetFileSizeMb(string version, PlatformIdentifier platformIdentifier);
}