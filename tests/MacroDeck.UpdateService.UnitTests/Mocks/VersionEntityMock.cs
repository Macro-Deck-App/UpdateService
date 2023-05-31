using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.UnitTests.Mocks;

public static class VersionEntityMock
{
    public static List<VersionEntity> GetVersionEntities()
    {
        return new List<VersionEntity>
        {
            new()
            {
                Id = 1,
                CreatedTimestamp = DateTime.Now.AddMinutes(-10),
                VersionState = VersionState.Release,
                Version = "1.0.0",
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 1,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-10),
                        PlatformIdentifier = PlatformIdentifier.Win,
                        FileHash = "1234",
                        FileDownloads = new List<FileDownloadEntity>()
                        {
                            new()
                            {
                                Id = 1,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-10),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 1
                            }
                        },
                        VersionId = 1
                    }
                }
            },
            new()
            {
                Id = 2,
                CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                VersionState = VersionState.Release,
                Version = "1.1.0",
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 2,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                        PlatformIdentifier = PlatformIdentifier.Win,
                        FileHash = "1234",
                        FileDownloads = new List<FileDownloadEntity>()
                        {
                            new()
                            {
                                Id = 2,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 2
                            },
                            new()
                            {
                                Id = 3,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 2
                            }
                        },
                        VersionId = 2
                    }
                }
            },
            new()
            {
                Id = 3,
                CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                VersionState = VersionState.Release,
                Version = "1.2.0",
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 3,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                        PlatformIdentifier = PlatformIdentifier.Win,
                        FileHash = "1234",
                        FileDownloads = new List<FileDownloadEntity>()
                        {
                            new()
                            {
                                Id = 4,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 3
                            },
                            new()
                            {
                                Id = 5,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                                DownloadReason = DownloadReason.UpdateDownload,
                                VersionFileId = 3
                            }
                        },
                        VersionId = 3
                    }
                }
            },
            new()
            {
                Id = 4,
                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                VersionState = VersionState.Preview,
                Version = "1.3.0-preview1",
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 4,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                        PlatformIdentifier = PlatformIdentifier.Win,
                        FileHash = "1234",
                        FileDownloads = new List<FileDownloadEntity>()
                        {
                            new()
                            {
                                Id = 6,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 4
                            },
                            new()
                            {
                                Id = 7,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                                DownloadReason = DownloadReason.UpdateDownload,
                                VersionFileId = 4
                            },
                            new()
                            {
                                Id = 8,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                                DownloadReason = DownloadReason.UpdateDownload,
                                VersionFileId = 4
                            }
                        },
                        VersionId = 4
                    },
                    new()
                    {
                        Id = 5,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                        PlatformIdentifier = PlatformIdentifier.Linux,
                        FileHash = "1234",
                        FileDownloads = new List<FileDownloadEntity>()
                        {
                            new()
                            {
                                Id = 8,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                                DownloadReason = DownloadReason.FirstDownload,
                                VersionFileId = 5
                            },
                            new()
                            {
                                Id = 9,
                                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                                DownloadReason = DownloadReason.UpdateDownload,
                                VersionFileId = 5
                            }
                        },
                        VersionId = 4
                    }
                }
            }
        };
    }
}