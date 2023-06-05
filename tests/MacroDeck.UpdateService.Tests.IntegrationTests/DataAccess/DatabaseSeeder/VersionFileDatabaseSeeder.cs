using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Tests.Shared;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;

public class VersionFileDatabaseSeeder
{
    private readonly ITestRepository<VersionFileEntity> _versionFileRepository;

    public VersionFileDatabaseSeeder(ITestRepository<VersionFileEntity> versionFileRepository)
    {
        _versionFileRepository = versionFileRepository;
    }

    public async ValueTask<VersionFileEntity> CreateVersionFile(
        VersionEntity version,
        Action<VersionFileEntity> update = null)
    {
        var versionFileEntity = new VersionFileEntity
        {
            PlatformIdentifier = PlatformIdentifier.WinX64,
            FileName = "testfile-win.exe",
            FileProvider = FileProvider.GitHub,
            FileHash = SharedTestConstants.TestFileWinSha256,
            FileSize = 100,
            VersionId = version.Id
        };
        
        update?.Invoke(versionFileEntity);
        await _versionFileRepository.InsertAsync(versionFileEntity);
        return versionFileEntity;
    }
}