using MacroDeck.UpdateService.Core.Authorization;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Controllers;

[Route("files")]
public class FileController : UpdateServiceControllerBase
{
    private readonly IVersionFileManager _versionFileManager;

    public FileController(IVersionFileManager versionFileManager)
    {
        _versionFileManager = versionFileManager;
    }

    [HttpPost("{version}/{platform}")]
    [TokenAuthorization]
    public async ValueTask<IActionResult> CreateFile(
        string version,
        PlatformIdentifier platform,
        [FromBody] CreateFileRequest createFileRequest)
    {
        if (!Version.TryParse(version, out var versionStruct))
        {
            throw new CannotParseVersionException();
        }

        return await _versionFileManager.CreateVersionFile(
            createFileRequest.FileProvider,
            createFileRequest.FileName,
            createFileRequest.FileHash,
            createFileRequest.FileSize,
            versionStruct,
            platform);
    }
}