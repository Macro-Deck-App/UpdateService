using Flurl.Http;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.Helper;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Controller;

[TestFixture]
public class ApiV2ControllerTests : TestBase
{
    private VersionDatabaseSeeder _versionDatabaseSeeder;
    private VersionFileDatabaseSeeder _versionFileDatabaseSeeder;

    public override async Task SetUp()
    {
        await base.SetUp();
        _versionDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionDatabaseSeeder>();
        _versionFileDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionFileDatabaseSeeder>();
    }

    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version?.Version, Is.EqualTo(version2.Version));
    }
    
    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_No_Preview_Versions()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-b1";
            entity.IsBetaVersion = true;
        });

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version?.Version, Is.EqualTo(version2.Version));
    }

    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_Preview_Versions()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-b1";
            entity.IsBetaVersion = true;
        });

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version?.Version, Is.EqualTo(version3.Version));
    }
    
    
    [Test]
    public async Task ApiV2Controller_CheckForUpdates_Returns_True_If_Newer_Version_Is_Available_Correct_Order()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.Version = "1.2.0-b1";
            entity.IsBetaVersion = true;
        });
        var version4 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version?.Version, Is.EqualTo(version4.Version));
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
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("check")
            .AppendPathSegment(version1.Version)
            .AppendPathSegment(platform1.ToString())
            .GetJsonAsync<ApiV2CheckResult>();
        
        Assert.That(result.NewerVersionAvailable, Is.EqualTo(true));
        Assert.That(result.Version?.Version, Is.EqualTo(version2.Version));
    }

    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version2.Version));
        Assert.That(result.Platforms, Contains.Key(platform));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version_Include_Preview_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsBetaVersion = true;
            entity.Version = "1.2.0-b1";
        });
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version3.Version));
        Assert.That(result.Platforms, Contains.Key(platform));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version_No_Preview_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsBetaVersion = true;
            entity.Version = "1.2.0-b1";
        });
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", false)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version2.Version));
        Assert.That(result.Platforms, Contains.Key(platform));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_In_Correct_Order()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsBetaVersion = true;
            entity.Version = "1.2.0-b1";
        });
        var version4 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version4.Version));
        Assert.That(result.Platforms, Contains.Key(platform));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_For_Correct_Platform()
    {
        const PlatformIdentifier platform1 = PlatformIdentifier.WinX64;
        const PlatformIdentifier platform2 = PlatformIdentifier.LinuxX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsBetaVersion = true;
            entity.Version = "1.2.0-b1";
        });
        var version4 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");
        var version5 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.3.0");
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform2);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform2);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform2);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform1);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform2);
        
        // Linux only
        await _versionFileDatabaseSeeder.CreateVersionFile(version5, entity => entity.PlatformIdentifier = platform2);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform2.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version5.Version));
        Assert.That(result.Platforms, !Contains.Key(platform1));
        Assert.That(result.Platforms, Contains.Key(platform2));
    }

    [Test]
    public async Task ApiV2Controller_GetVersion_Returns_Correct_Version()
    {
        const string fileName = "testfile";
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;

        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");

        await _versionFileDatabaseSeeder.CreateVersionFile(version1, update: entity =>
        {
            entity.FileProvider = FileProvider.GitHub;
            entity.PlatformIdentifier = platform;
            entity.FileName = fileName;
        });

        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment(version1.Version)
            .GetJsonAsync<ApiV2VersionInfo>();

        Assert.That(result1.Version, Is.EqualTo(version1.Version));
        Assert.That(result1.Platforms, Contains.Key(platform));
        Assert.That(result1.Platforms[platform],
            Is.EqualTo(FileProviderUrlBuilder.GetUrl(FileProvider.GitHub, version1.Version, fileName)));

        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2VersionBase)
            .AppendPathSegment(version2.Version)
            .GetJsonAsync<ApiV2VersionInfo>();

        Assert.That(result2.Version, Is.EqualTo(version2.Version));
        Assert.That(result2.Platforms, Is.Empty);
    }
}