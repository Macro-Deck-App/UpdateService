using Flurl.Http;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ErrorHandling;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
using MacroDeck.UpdateService.Tests.IntegrationTests.Extensions;
using MacroDeck.UpdateService.Tests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Controller;

[TestFixture]
public class FileControllerTests : TestBase
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
    public async Task FileController_Upload_Valid_File_Success()
    {
        var createFileRequest = new CreateFileRequest
        {
            FileName = "testfile-win.exe",
            FileProvider = FileProvider.GitHub,
            FileHash = SharedTestConstants.TestFileWinSha256,
            FileSize = 100
        };

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment("1.0.0")
            .AppendPathSegment(PlatformIdentifier.WinX64.ToString())
            .WithAdminToken()
            .PostJsonAsync(createFileRequest);

        var uploadRequestResponse = await result.GetStringAsync();
        
        Assert.NotNull(uploadRequestResponse);
        Assert.That(uploadRequestResponse, Is.EqualTo("1.0.0"));

        var dbVersion = await _versionTestRepository.AsQueryable()
            .Include(x => x.Files)
            .SingleOrDefaultAsync(x => x.Version == "1.0.0");
        Assert.NotNull(dbVersion);
        Assert.That(dbVersion.Files.Count, Is.EqualTo(1));
        Assert.That(dbVersion.Files.SingleOrDefault()?.FileHash, Is.EqualTo(SharedTestConstants.TestFileWinSha256));
    }

    [Test]
    public async Task FileController_Upload_Existing_File_Fails()
    {
        var version = await _versionDatabaseSeeder.CreateVersion();
        var versionFile = await _versionFileDatabaseSeeder.CreateVersionFile(version, update: entity =>
        {
            entity.FileName = "testfile-win.exe";
        });
        
        var createFileRequest = new CreateFileRequest
        {
            FileName = "testfile-win.exe",
            FileProvider = FileProvider.GitHub,
            FileHash = SharedTestConstants.TestFileWinSha256,
            FileSize = 100
        };

        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment(version.Version)
            .AppendPathSegment(versionFile.PlatformIdentifier.ToString())
            .WithAdminToken()
            .AllowAnyHttpStatus()
            .PostJsonAsync(createFileRequest);
        
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        Assert.That(_versionFileTestRepository.AsQueryable().Count(), Is.EqualTo(1));
        
        var errorException = await result.GetJsonAsync<ErrorResponse>();
        Assert.NotNull(errorException);
        Assert.That(errorException.ErrorCode, Is.EqualTo(ErrorCode.FileAlreadyExists));
        Assert.That(errorException.Error, Is.EqualTo(ErrorMessages.FileAlreadyExists));
    }
}