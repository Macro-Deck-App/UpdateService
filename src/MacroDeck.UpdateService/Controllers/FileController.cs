using MacroDeck.UpdateService.Core.Authorization;
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
    public async ValueTask<IActionResult> UploadFile(
        IFormFile file,
        string version,
        PlatformIdentifier platform)
    {
        if (!Version.TryParse(version, out var versionStruct))
        {
            throw new CannotParseVersionException();
        }

        if (file == null || file.Length == 0)
        {
            throw new NoFileUploadedException();
        }
        
        var stream = file.OpenReadStream();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        
        var fileBytes = ms.ToArray();
        var fileExtension = Path.GetExtension(file.FileName);

        return await _versionFileManager.UploadVersionFile(fileBytes, fileExtension, versionStruct, platform);
    }
}