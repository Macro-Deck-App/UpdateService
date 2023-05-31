using AutoMapper;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.Repositories;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.UnitTests.Mocks;
using NUnit.Framework;

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
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.Win, false);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        
        Assert.That(latestVersionFromRepository.VersionState, Is.EqualTo(VersionState.Release));
        Assert.That(latestVersionFromRepository.Downloads,
            Is.EqualTo(versionEntityFromDatabase?.Files.SelectMany(x => x.FileDownloads)
                .LongCount(x => x.DownloadReason == DownloadReason.FirstDownload)));
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.Win), Is.True);
    }
    
    [Test]
    public async Task GetLatestVersion_PreviewVersion_ReturnsCorrectVersion()
    {
        var versionEntities = VersionEntityMock.GetVersionEntities();
        await SeedDatabase(versionEntities);

        var versionRepository = new VersionRepository(UpdateServiceContext, _mapper);
        
        var latestVersionFromRepository = await versionRepository.GetLatestVersion(PlatformIdentifier.Win, true);
        
        Assert.That(latestVersionFromRepository, Is.Not.Null);

        var versionEntityFromDatabase =
            versionEntities.SingleOrDefault(x => x.Version == latestVersionFromRepository.Version);
        
        Assert.That(latestVersionFromRepository.VersionState, Is.EqualTo(VersionState.Preview));
        Assert.That(latestVersionFromRepository.Downloads,
            Is.EqualTo(versionEntityFromDatabase?.Files.SelectMany(x => x.FileDownloads)
                .LongCount(x => x.DownloadReason == DownloadReason.FirstDownload)));
        Assert.That(versionEntityFromDatabase?.Files.Any(x => x.PlatformIdentifier == PlatformIdentifier.Win), Is.True);
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