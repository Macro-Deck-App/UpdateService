using AutoMapper;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Controllers.Api;

[Route("v2")]
public class ApiV2Controller : ControllerBase
{
    private readonly IVersionManager _versionManager;
    private readonly IVersionFileManager _versionFileManager;
    private readonly IMapper _mapper;

    public ApiV2Controller(
        IVersionManager versionManager,
        IVersionFileManager versionFileManager,
        IMapper mapper)
    {
        _versionManager = versionManager;
        _versionFileManager = versionFileManager;
        _mapper = mapper;
    }

    [HttpGet("next/major")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<Version>> GetNextMajorVersion()
    {
        return await _versionManager.GetNextMajorVersion();
    }
    
    [HttpGet("next/minor")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<Version>> GetNextMinorVersion()
    {
        return await _versionManager.GetNextMinorVersion();
    }
    
    [HttpGet("next/patch")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<Version>> GetNextPatchVersion()
    {
        return await _versionManager.GetNextPatchVersion();
    }
    
    [HttpGet("next/major/beta")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<Version>> GetNextMajorBetaVersion()
    {
        return await _versionManager.GetNextMajorBetaVersion();
    }
    
    [HttpGet("next/minor/beta")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<Version>> GetNextMinorBetaVersion()
    {
        return await _versionManager.GetNextMinorBetaVersion();
    }

    [HttpGet("validate/versionname/{version}")]
    [AllowAnonymous]
    public ActionResult<bool> ValidateVersionName(string version)
    {
        return Version.TryParse(version, out _);
    } 

    [HttpGet("check/{installedVersion}/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<ApiV2CheckResult>> CheckForUpdates(
        string installedVersion,
        PlatformIdentifier platform,
        [FromQuery] bool previewVersions = false)
    {
        var checkResult =
            await _versionManager.CheckForNewerVersion(installedVersion, platform, previewVersions);
        return _mapper.Map<ApiV2CheckResult>(checkResult);
    }

    [HttpGet("latest/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<ApiV2VersionInfo>> GetLatestVersion(
        PlatformIdentifier platform,
        [FromQuery] bool previewVersions = false)
    {
        var versionInfo = await _versionManager.GetLatestVersion(platform, previewVersions);
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