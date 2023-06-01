using Flurl.Http;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Controller;

[TestFixture]
public class ApiV2ControllerTests : TestBase
{
    private VersionDatabaseSeeder _versionDatabaseSeeder;
    private VersionFileDatabaseSeeder _versionFileDatabaseSeeder;
    private ITestRepository<VersionEntity> _versionTestRepository;
    private ITestRepository<VersionFileEntity> _versionFileTestRepository;


    public override async Task SetUp()
    {
        await base.SetUp();
        _versionDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionDatabaseSeeder>();
        _versionFileDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionFileDatabaseSeeder>();
        _versionTestRepository = TestScope.ServiceProvider.GetRequiredService<ITestRepository<VersionEntity>>();
        _versionFileTestRepository = TestScope.ServiceProvider.GetRequiredService<ITestRepository<VersionFileEntity>>();
    }

    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");

        var platform = PlatformIdentifier.WinX64;

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version, Is.EqualTo(version2.Version));
    }
    
    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_No_Preview_Versions()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-preview1";
            entity.IsPreviewVersion = true;
        });

        var platform = PlatformIdentifier.WinX64;

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version, Is.EqualTo(version2.Version));
    }

    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_Preview_Versions()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-preview1";
            entity.IsPreviewVersion = true;
        });

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version, Is.EqualTo(version3.Version));
    }
    
    
    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_Correct_Order()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-preview1";
            entity.IsPreviewVersion = true;
        });
        var version4 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version, Is.EqualTo(version4.Version));
    }
    
    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_Correct_Platform()
    {
        const PlatformIdentifier platform1 = PlatformIdentifier.WinX64;
        const PlatformIdentifier platform2 = PlatformIdentifier.LinuxX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform2);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform1.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version, Is.EqualTo(version2.Version));
    }
}