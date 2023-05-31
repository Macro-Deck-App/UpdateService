using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.UpdateService.Controllers.Api;

[Route("v2")]
public class ApiV2Controller : ControllerBase
{
    private readonly IVersionManager _versionManager;
    private readonly IVersionFileManager _versionFileManager;

    public ApiV2Controller(IVersionManager versionManager, IVersionFileManager versionFileManager)
    {
        _versionManager = versionManager;
        _versionFileManager = versionFileManager;
    }

    [HttpGet("latest")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<VersionInfo>> GetLatestVersion(
        [FromQuery] PlatformIdentifier platformIdentifier,
        [FromQuery] bool previewVersions = false)
    {
        return await _versionManager.GetLatestVersion(platformIdentifier, previewVersions);
    }

    [HttpGet("{version}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<VersionInfo>> GetVersion(string version)
    {
        return await _versionManager.GetVersion(version);
    }
    
    [HttpGet("{version}/download")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<byte[]>> DownloadVersion(
        string version,
        [FromQuery] PlatformIdentifier platformIdentifier,
        [FromQuery] DownloadReason downloadReason = DownloadReason.FirstDownload)
    {
        var versionFile = await _versionFileManager.GetFile(version, platformIdentifier);
        await _versionFileManager.CountDownload(version, platformIdentifier, downloadReason);

        HttpContext.Response.Headers["x-file-hash"] = versionFile.FileHash;
        return File(versionFile.Bytes, "application/octet-stream", versionFile.FileName);
    }
}