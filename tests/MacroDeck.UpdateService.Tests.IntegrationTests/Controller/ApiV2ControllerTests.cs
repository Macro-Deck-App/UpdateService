using Flurl.Http;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
using MacroDeck.UpdateService.Tests.IntegrationTests.Extensions;
using MacroDeck.UpdateService.Tests.Shared;
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
    private ITestRepository<FileDownloadEntity> _fileDownloadTestRepository;

    public override async Task SetUp()
    {
        await base.SetUp();
        _versionDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionDatabaseSeeder>();
        _versionFileDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionFileDatabaseSeeder>();
        _versionTestRepository = TestScope.ServiceProvider.GetRequiredService<ITestRepository<VersionEntity>>();
        _versionFileTestRepository = TestScope.ServiceProvider.GetRequiredService<ITestRepository<VersionFileEntity>>();
        _fileDownloadTestRepository = TestScope.ServiceProvider.GetRequiredService<ITestRepository<FileDownloadEntity>>();
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

    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version2.Version));
        Assert.That(result.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result.Downloads, Is.EqualTo(0));
        Assert.That(result.SupportedPlatforms, Is.EqualTo(new[] { platform }));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version_Include_Preview_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsPreviewVersion = true;
            entity.Version = "1.2.0-preview1";
        });
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version3.Version));
        Assert.That(result.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result.Downloads, Is.EqualTo(0));
        Assert.That(result.SupportedPlatforms, Is.EqualTo(new[] { platform }));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_Latest_Version_No_Preview_Version()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsPreviewVersion = true;
            entity.Version = "1.2.0-preview1";
        });
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", false)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version2.Version));
        Assert.That(result.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result.Downloads, Is.EqualTo(0));
        Assert.That(result.SupportedPlatforms, Is.EqualTo(new[] { platform }));
    }
    
    [Test]
    public async Task ApiV2Controller_GetLatestVersion_Returns_In_Correct_Order()
    {
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        var version3 = await _versionDatabaseSeeder.CreateVersion(entity =>
        {
            entity.IsPreviewVersion = true;
            entity.Version = "1.2.0-preview1";
        });
        var version4 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.2.0");
        
        await _versionFileDatabaseSeeder.CreateVersionFile(version1, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version2, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version3, entity => entity.PlatformIdentifier = platform);
        await _versionFileDatabaseSeeder.CreateVersionFile(version4, entity => entity.PlatformIdentifier = platform);

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version4.Version));
        Assert.That(result.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result.Downloads, Is.EqualTo(0));
        Assert.That(result.SupportedPlatforms, Is.EqualTo(new[] { platform }));
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
            entity.IsPreviewVersion = true;
            entity.Version = "1.2.0-preview1";
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
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment("latest")
            .AppendPathSegment(platform1.ToString())
            .SetQueryParam("previewVersions", true)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result.Version, Is.EqualTo(version4.Version));
        Assert.That(result.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result.Downloads, Is.EqualTo(0));
        Assert.That(result.SupportedPlatforms, Does.Contain(platform1));
    }

    [Test]
    public async Task ApiV2Controller_GetVersion_Returns_Correct_Version()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.0.0");
        var version2 = await _versionDatabaseSeeder.CreateVersion(entity => entity.Version = "1.1.0");
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment(version1.Version)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result1.Version, Is.EqualTo(version1.Version));
        Assert.That(result1.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result1.Downloads, Is.EqualTo(0));
        Assert.That(result1.SupportedPlatforms, Is.Empty);
        
        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment(version2.Version)
            .GetJsonAsync<ApiV2VersionInfo>();
        
        Assert.That(result2.Version, Is.EqualTo(version2.Version));
        Assert.That(result2.VersionState, Is.EqualTo(VersionState.Published));
        Assert.That(result2.Downloads, Is.EqualTo(0));
        Assert.That(result2.SupportedPlatforms, Is.Empty);
    }

    [Test]
    public async Task ApiV2Controller_Download_Returns_Correct_Data()
    {
        var testFile = Path.Combine("TestFiles", "testfile-win.exe");
        Assert.True(File.Exists(testFile));

        var originalBytes = await File.ReadAllBytesAsync(testFile);

        const string version = "1.0.0";
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment(version)
            .AppendPathSegment(platform.ToString())
            .WithAdminToken()
            .PostMultipartAsync(x => x.AddFile("file", testFile));

        var downloadResponse = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
            .AppendPathSegment(version)
            .AppendPathSegment("download")
            .AppendPathSegment(platform.ToString())
            .GetAsync();

        var downloadBytes = await downloadResponse.GetBytesAsync();

        Assert.That(downloadResponse.Headers.GetAll("x-file-hash").SingleOrDefault(),
            Is.EqualTo(SharedTestConstants.TestFileWinSha256));
        Assert.That(downloadBytes, Has.Length.EqualTo(originalBytes.Length));
        for (var i = 0; i < originalBytes.Length; i++)
        {
            Assert.That(downloadBytes[i], Is.EqualTo(originalBytes[i]));
        }
    }

    [Test]
    public async Task ApiV2Controller_Download_Counts_Downloads_Returns_Downloads()
    {
        var testFile = Path.Combine("TestFiles", "testfile-win.exe");
        Assert.True(File.Exists(testFile));
        
        const string version = "1.0.0";
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment(version)
            .AppendPathSegment(platform.ToString())
            .WithAdminToken()
            .PostMultipartAsync(x => x.AddFile("file", testFile));

        for (var i = 0; i < 10; i++)
        {
            await IntegrationTestHelper.TestClientRequest
                .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
                .AppendPathSegment(version)
                .AppendPathSegment("download")
                .AppendPathSegment(platform.ToString())
                .GetAsync();

            var versionInfo = await IntegrationTestHelper.TestClientRequest
                .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
                .AppendPathSegment(version)
                .GetJsonAsync<ApiV2VersionInfo>();
            Assert.That(versionInfo.Downloads, Is.EqualTo(i + 1));
        }
    }
    
    [Test]
    public async Task ApiV2Controller_Download_Counts_Downloads_Returns_Downloads_Ignores_Updates()
    {
        var testFile = Path.Combine("TestFiles", "testfile-win.exe");
        Assert.True(File.Exists(testFile));
        
        const string version = "1.0.0";
        const PlatformIdentifier platform = PlatformIdentifier.WinX64;
        
        await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment(version)
            .AppendPathSegment(platform.ToString())
            .WithAdminToken()
            .PostMultipartAsync(x => x.AddFile("file", testFile));

        for (var i = 0; i < 10; i++)
        {
            await IntegrationTestHelper.TestClientRequest
                .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
                .AppendPathSegment(version)
                .AppendPathSegment("download")
                .AppendPathSegment(platform.ToString())
                .GetAsync();
            
            await IntegrationTestHelper.TestClientRequest
                .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
                .AppendPathSegment(version)
                .AppendPathSegment("download")
                .AppendPathSegment(platform.ToString())
                .SetQueryParam("downloadReason", DownloadReason.UpdateDownload)
                .GetAsync();

            var versionInfo = await IntegrationTestHelper.TestClientRequest
                .AppendPathSegment(IntegrationTestConstants.ApiV2Base)
                .AppendPathSegment(version)
                .GetJsonAsync<ApiV2VersionInfo>();
            Assert.That(versionInfo.Downloads, Is.EqualTo(i + 1));
        }
    }
}