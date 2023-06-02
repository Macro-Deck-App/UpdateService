using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.Extensions;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using MacroDeck.UpdateService.Core.Extensions;
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
    private readonly IFileDownloadRepository _fileDownloadRepository;

    public VersionFileManager(IVersionManager versionManager,
        IVersionFileRepository versionFileRepository,
        IFileDownloadRepository fileDownloadRepository)
    {
        _versionManager = versionManager;
        _versionFileRepository = versionFileRepository;
        _fileDownloadRepository = fileDownloadRepository;
    }

    public async ValueTask<IActionResult> UploadVersionFile(byte[] data, string fileExtension, Version version, PlatformIdentifier platformIdentifier)
    {
        var versionEntity = await _versionManager.GetOrCreateVersion(version.ToString());
        var fileExists = await _versionFileRepository.Exists(version.ToString(), platformIdentifier);
        if (fileExists)
        {
            throw new FileAlreadyExistsException();
        }

        var saveFileName = Guid.NewGuid().ToString();
        var savePath = Path.Combine(UpdateServiceConfiguration.DataPath, saveFileName);
        EnsureDestinationDirectoryExists(savePath);
        try
        {
            await using var stream = File.Create(savePath);
            stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Cannot write file for version {Version} (Platform: {PlatformIdentifier})",
                version,
                platformIdentifier);
            throw;
        }

        var fileHash = data.GenerateSha256Hash();

        var originalFileName = $"macro-deck-{version}{fileExtension}-{platformIdentifier}".ToLower();

        var versionFileEntity = new VersionFileEntity
        {
            PlatformIdentifier = platformIdentifier,
            SavedFileName = saveFileName,
            OriginalFileName = originalFileName,
            FileHash = fileHash,
            VersionId = versionEntity.Id
        };

        await _versionFileRepository.InsertAsync(versionFileEntity);
        return new CreatedResult(originalFileName, version.ToString());
    }

    public async ValueTask<VersionFileResult> GetFile(string version, PlatformIdentifier platformIdentifier)
    {
        var versionFile = await _versionFileRepository.GetVersionFile(version, platformIdentifier);
        if (versionFile == null)
        {
            throw new VersionDoesNotExistException();
        }

        var filePath = Path.Combine(UpdateServiceConfiguration.DataPath, versionFile.SavedFileName);
        var bytes = await File.ReadAllBytesAsync(filePath);

        var calculatedHash = bytes.GenerateSha256Hash();
        if (!versionFile.FileHash.EqualsCryptographically(calculatedHash))
        {
            _logger.Fatal(
                "Hash for version file {Version} ({Platform}) changed {OriginalHash} -> {NewHash}",
                version,
                platformIdentifier,
                versionFile.FileHash,
                calculatedHash);
            throw new InvalidOperationException("File hash invalid");
        }

        return new VersionFileResult(bytes, versionFile.FileHash, versionFile.OriginalFileName);
    }

    public async ValueTask CountDownload(
        string version,
        PlatformIdentifier platformIdentifier,
        DownloadReason downloadReason)
    {
        var versionFile = await _versionFileRepository.GetVersionFile(version, platformIdentifier);
        if (versionFile == null)
        {
            return;
        }

        var fileDownload = new FileDownloadEntity
        {
            CreatedTimestamp = DateTime.Now,
            DownloadReason = downloadReason,
            VersionFileId = versionFile.Id
        };

        await _fileDownloadRepository.InsertAsync(fileDownload);
    }

    private void EnsureDestinationDirectoryExists(string path)
    {
        var directory = Path.GetDirectoryName(path)
                        ?? throw new InvalidOperationException("Directory cannot be null");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}