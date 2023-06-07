using AutoMapper;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.UpdateService.Controllers.Api;

[Route("v{apiVersion:apiVersion}/versions")]
[ApiVersion("2.0")]
public class ApiV2VersionController : UpdateServiceControllerBase
{
    private readonly IVersionManager _versionManager;
    private readonly IVersionFileManager _versionFileManager;
    private readonly IMapper _mapper;

    public ApiV2VersionController(
        IVersionManager versionManager,
        IVersionFileManager versionFileManager,
        IMapper mapper)
    {
        _versionManager = versionManager;
        _versionFileManager = versionFileManager;
        _mapper = mapper;
    }

    [HttpGet("check/{installedVersion}/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<ApiV2CheckResult>> CheckForUpdates(
        string installedVersion,
        PlatformIdentifier platform,
        [FromQuery] bool betaVersions = false)
    {
        var checkResult =
            await _versionManager.CheckForNewerVersion(installedVersion, platform, betaVersions);
        return _mapper.Map<ApiV2CheckResult>(checkResult);
    }

    [HttpGet("latest/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<ApiV2VersionInfo>> GetLatestVersion(
        PlatformIdentifier platform,
        [FromQuery] bool betaVersions = false)
    {
        var versionInfo = await _versionManager.GetLatestVersion(platform, betaVersions);
        return _mapper.Map<ApiV2VersionInfo>(versionInfo);
    }

    [HttpGet("{version}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<ApiV2VersionInfo>> GetVersion(string version)
    {
        var versionInfo = await _versionManager.GetVersion(version);
        return _mapper.Map<ApiV2VersionInfo>(versionInfo);
    }

    [HttpGet("{version}/fileSize/{platform}")]
    public async ValueTask<double> GetFileSize(
        string version,
        PlatformIdentifier platform)
    {
        return await _versionFileManager.GetFileSizeMb(version, platform);
    }
}