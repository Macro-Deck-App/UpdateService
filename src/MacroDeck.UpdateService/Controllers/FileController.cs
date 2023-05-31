using MacroDeck.UpdateService.Core.Authorization;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.UpdateService.Controllers;

[Route("files")]
public class FileController : UpdateServiceControllerBase
{
    private readonly IVersionFileManager _versionFileManager;

    public FileController(IVersionFileManager versionFileManager)
    {
        _versionFileManager = versionFileManager;
    }

    [HttpPost]
    [TokenAuthorization]
    public async ValueTask<IActionResult> UploadFile(
        IFormFile file,
        [FromQuery] string version,
        PlatformIdentifier platformIdentifier)
    {
        var stream = file.OpenReadStream();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var fileBytes = ms.ToArray();

        return await _versionFileManager.UploadVersionFile(fileBytes, version, platformIdentifier);
    }
}