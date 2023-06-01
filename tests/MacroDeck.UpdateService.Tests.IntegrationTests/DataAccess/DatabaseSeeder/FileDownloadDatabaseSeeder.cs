using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;

public class FileDownloadDatabaseSeeder
{
    private readonly ITestRepository<FileDownloadEntity> _fileDownloadRepository;

    public FileDownloadDatabaseSeeder(ITestRepository<FileDownloadEntity> fileDownloadRepository)
    {
        _fileDownloadRepository = fileDownloadRepository;
    }

    public async ValueTask<FileDownloadEntity> CreateFileDownload(
        VersionFileEntity versionFile,
        DownloadReason downloadReason)
    {
        var fileDownloadEntity = new FileDownloadEntity
        {
            DownloadReason = downloadReason,
            VersionFileId = versionFile.Id
        };

        await _fileDownloadRepository.InsertAsync(fileDownloadEntity);
        return fileDownloadEntity;
    }
}