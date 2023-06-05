using MacroDeck.UpdateService.Core.DataAccess.Entities;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;

public class VersionDatabaseSeeder
{
    private readonly ITestRepository<VersionEntity> _versionRepository;

    public VersionDatabaseSeeder(ITestRepository<VersionEntity> versionRepository)
    {
        _versionRepository = versionRepository;
    }

    public async ValueTask<VersionEntity> CreateVersion(Action<VersionEntity> update = null)
    {
        var versionEntity = new VersionEntity
        {
            Version = "1.0.0",
            Major = 1,
            Minor = 0,
            Patch = 0,
            PreReleaseNo = null,
            IsBetaVersion = false
        };
        
        update?.Invoke(versionEntity);
        await _versionRepository.InsertAsync(versionEntity);
        return versionEntity;
    }
}