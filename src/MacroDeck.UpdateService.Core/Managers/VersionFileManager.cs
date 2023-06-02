using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
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

    public async ValueTask<IActionResult> UploadVersionFile(Stream stream, string fileExtension, Version version, PlatformIdentifier platformIdentifier)
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

        string fileHash;
        try
        {
            await using var saveStream = File.Create(savePath);
            await stream.CopyToAsync(saveStream);
            fileHash = await saveStream.CalculateSha256Hash();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Cannot write file for version {Version} (Platform: {PlatformIdentifier})",
                version,
                platformIdentifier);
            throw;
        }


        var originalFileName = $"macro-deck-{version}-{platformIdentifier}{fileExtension}".ToLower();

        var versionFileEntity = new VersionFileEntity
        {
            PlatformIdentifier = platformIdentifier,
            SavedFileName = saveFileName,
            OriginalFileName = originalFileName,
            FileHash = fileHash,
            VersionId = versionEntity.Id,
            FileSize = stream.Length
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
        var fileStream = File.OpenRead(filePath);

        var calculatedHash = await fileStream.CalculateSha256Hash();

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

        return new VersionFileResult(fileStream, versionFile.FileHash, versionFile.OriginalFileName);
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