using AutoMapper;
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
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Startup)));
        _mapper = config.CreateMapper();
    }

    [Test]
    public async Task GetLatestVersion_NoPreviewVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.WinX64, false);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        
        Assert.That(latestVersionFromRepository.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(latestVersionFromRepository.IsPreviewVersion, Is.EqualTo(false));
        Assert.That(latestVersionFromRepository.Downloads,
            Is.EqualTo(versionEntityFromDatabase?.Files.SelectMany(x => x.FileDownloads)
                .LongCount(x => x.DownloadReason == DownloadReason.FirstDownload)));
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.WinX64), Is.True);
    }
    
    [Test]
    public async Task GetLatestVersion_PreviewVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.WinX64, true);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        
        Assert.That(latestVersionFromRepository.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(latestVersionFromRepository.IsPreviewVersion, Is.EqualTo(true));
        Assert.That(latestVersionFromRepository.Downloads,
            Is.EqualTo(versionEntityFromDatabase?.Files.SelectMany(x => x.FileDownloads)
                .LongCount(x => x.DownloadReason == DownloadReason.FirstDownload)));
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.WinX64), Is.True);
    }

    [Test]
    public async Task GetNewerVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);

        var currentVersion = versionEntities.OrderBy(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreviewNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(currentVersion, PlatformIdentifier.WinX64, false);
        var latestVersion = versionEntities
            .Where(x => x.IsPreviewVersion == false)
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .MaxBy(x => x.Id);
        Assert.NotNull(newerVersion);
        Assert.That(newerVersion.Version, Is.EqualTo(latestVersion.Version));
    }
    
    [Test]
    public async Task GetNewerVersion_Including_Preview_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);

        var currentVersion = versionEntities.OrderBy(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreviewNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(currentVersion, PlatformIdentifier.WinX64, true);
        var latestVersion = versionEntities
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .MaxBy(x => x.Id);
        Assert.NotNull(newerVersion);
        Assert.That(newerVersion.Version, Is.EqualTo(latestVersion.Version));
    }
    
    [Test]
    public async Task GetNewerVersion_Returns_Null_If_No_Newer_Version_Exists()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);
        
        var latestVersion = versionEntities
            .Where(x => x.Files.Any(f => f.PlatformIdentifier == PlatformIdentifier.WinX64))
            .OrderByDescending(x => x.Id)
            .Select(x => new Version(x.Major, x.Minor, x.Patch, x.PreviewNo))
            .FirstOrDefault();

        var newerVersion = await versionRepository.GetNewerVersion(latestVersion, PlatformIdentifier.WinX64, true);
        Assert.Null(newerVersion);
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