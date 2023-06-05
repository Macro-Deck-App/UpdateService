using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using MacroDeck.UpdateService.Core.Extensions;
using MacroDeck.UpdateService.Core.Helper;
using MacroDeck.UpdateService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.Managers;

public class VersionFileManager : IVersionFileManager
{
    private readonly ILogger _logger = Log.ForContext<VersionFileManager>();
    private readonly IVersionManager _versionManager;
    private readonly IVersionFileRepository _versionFileRepository;

    public VersionFileManager(IVersionManager versionManager,
        IVersionFileRepository versionFileRepository)
    {
        _versionManager = versionManager;
        _versionFileRepository = versionFileRepository;
    }

    public async ValueTask<IActionResult> CreateVersionFile(
        FileProvider fileProvider,
        string fileName,
        string fileHash,
        long fileSize,
        Version version,
        PlatformIdentifier platformIdentifier)
    {
        var validateFile = EnvironmentHelper.IsStagingOrProduction;
        if (validateFile)
        {
            var fileValid = await CheckFile(fileProvider, version.ToString(), fileName, fileHash);
            if (!fileValid)
            {
                throw new NoFileUploadedException();
            }
        }

        var versionEntity = await _versionManager.GetOrCreateVersion(version.ToString());
        var fileExists = await _versionFileRepository.Exists(version.ToString(), platformIdentifier);
        if (fileExists)
        {
            throw new FileAlreadyExistsException();
        }

        var versionFileEntity = new VersionFileEntity
        {
            FileProvider = fileProvider,
            PlatformIdentifier = platformIdentifier,
            FileName = fileName,
            FileHash = fileHash,
            FileSize = fileSize,
            VersionId = versionEntity.Id
        };

        await _versionFileRepository.InsertAsync(versionFileEntity);
        return new CreatedResult(fileName, version.ToString());
    }

    public async ValueTask<double> GetFileSizeMb(string version, PlatformIdentifier platformIdentifier)
    {
        var versionFile = await _versionFileRepository.GetVersionFile(version, platformIdentifier);
        if (versionFile == null)
        {
            throw new VersionDoesNotExistException();
        }

        return versionFile.FileSize / 1024f / 1024f;
    }

    private async ValueTask<bool> CheckFile(FileProvider fileProvider, string version, string fileName, string fileHash)
    {
        var url = FileProviderUrlBuilder.GetUrl(fileProvider, version, fileName);
        using var httpClient = new HttpClient();
        using var stream = new MemoryStream();
        try
        {
            var httpStream = await httpClient.GetStreamAsync(url);
            await httpStream.CopyToAsync(stream);
            var calculateSha256Hash = await stream.CalculateSha256Hash();
            if (!calculateSha256Hash.EqualsCryptographically(fileHash))
            {
                _logger.Error(
                    "Failed to validate checksum for {FileName}. Calculated hash: {CalculatedHash}, received hash: {ReceivedHash}",
                    fileName,
                    calculateSha256Hash, 
                    fileHash);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Failed to validate file for {Version}, filename: {FileName}, url: {Url}",
                version,
                fileName,
                url);
            return false;
        }

        return true;
    }
}