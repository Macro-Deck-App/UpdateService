using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.Repositories;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.UnitTests.Mocks;
using NUnit.Framework;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.UnitTests.Repositories;

[TestFixture]
public class VersionRepositoryTests : TestBase
{
    [Test]
    public async Task GetLatestVersion_NoPreviewVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext);
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.WinX64, false);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.WinX64), Is.True);
    }
    
    [Test]
    public async Task GetLatestVersion_PreviewVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext);
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.WinX64, true);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.WinX64), Is.True);
    }

    [Test]
    public async Task GetNewerVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext);

        var currentVersion = versionEntities.OrderBy(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreReleaseNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(currentVersion, PlatformIdentifier.WinX64, false);
        var latestVersion = versionEntities
            .Where(x => x.IsBetaVersion == false)
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .MaxBy(x => x.Id);
        Assert.That(newerVersion, Is.Not.Null);
        Assert.That(newerVersion.Version, Is.EqualTo(latestVersion.Version));
    }
    
    [Test]
    public async Task GetNewerVersion_Including_Preview_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext);

        var currentVersion = versionEntities.OrderBy(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreReleaseNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(currentVersion, PlatformIdentifier.WinX64, true);
        var latestVersion = versionEntities
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .MaxBy(x => x.Id);
        Assert.That(newerVersion, Is.Not.Null);
        Assert.That(newerVersion.Version, Is.EqualTo(latestVersion.Version));
    }
    
    [Test]
    public async Task GetNewerVersion_Returns_Null_If_No_Newer_Version_Exists()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext);
        
        var latestVersion = versionEntities
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .OrderByDescending(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreReleaseNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(latestVersion, PlatformIdentifier.WinX64, true);
        Assert.That(newerVersion, Is.Null);
    }

    private async Task SeedDatabase(IEnumerable<BaseEntity> entities)
    {
        foreach (var entity in entities)
        {
            UpdateServiceContext.Add(entity);
        }
        await UpdateServiceContext.SaveChangesAsync();
    }
}