using AutoMapper;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
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
    private readonly IFileDownloadRepository _fileDownloadRepository;
    private readonly IMapper _mapper;

    public ApiV2Controller(
        IVersionManager versionManager,
        IVersionFileManager versionFileManager,
        IFileDownloadRepository fileDownloadRepository,
        IMapper mapper)
    {
        _versionManager = versionManager;
        _versionFileManager = versionFileManager;
        _fileDownloadRepository = fileDownloadRepository;
        _mapper = mapper;
    }

    [HttpGet("totaldownloads")]
    [AllowAnonymous]
    public async ValueTask<long> GetTotalDownloads()
    {
        return await _fileDownloadRepository.CountAllFirstTimeDownloads();
    }

    [HttpGet("check/{installedVersion}/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ApiV2CheckResult> CheckForUpdates(
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

    [HttpGet("latest/download/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult<byte[]>> DownloadLatestVersion(
        PlatformIdentifier platform,
        [FromQuery] DownloadReason downloadReason = DownloadReason.FirstDownload,
        [FromQuery] bool previewVersions = false)
    {
        var latestVersion = await _versionManager.GetLatestVersion(platform, previewVersions);
        return await DownloadVersion(latestVersion.Version, platform, downloadReason);
    }
    
    [HttpGet("{version}/download/{platform}")]
    [AllowAnonymous]
    public async ValueTask<ActionResult> DownloadVersion(
        string version,
        PlatformIdentifier platform,
        [FromQuery] DownloadReason downloadReason = DownloadReason.FirstDownload)
    {
        var versionFile = await _versionFileManager.GetFile(version, platform);
        await _versionFileManager.CountDownload(version, platform, downloadReason);

        HttpContext.Response.Headers["x-file-hash"] = versionFile.FileHash;
        HttpContext.Response.ContentLength = versionFile.FileStream.Length;
        return new FileStreamResult(versionFile.FileStream, "application/octet-stream") 
        { 
            FileDownloadName = versionFile.FileName
        };
    }
}