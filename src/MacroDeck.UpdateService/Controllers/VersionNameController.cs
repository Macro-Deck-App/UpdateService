using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Controllers;

[Route("versionname")]
public class VersionNameController : UpdateServiceControllerBase
{
    private readonly IVersionManager _versionManager;

    public VersionNameController(IVersionManager versionManager)
    {
        _versionManager = versionManager;
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

    [HttpGet("validate/{version}")]
    [AllowAnonymous]
    public ActionResult<bool> ValidateVersionName(string version)
    {
        return Version.TryParse(version, out _);
    } 
}