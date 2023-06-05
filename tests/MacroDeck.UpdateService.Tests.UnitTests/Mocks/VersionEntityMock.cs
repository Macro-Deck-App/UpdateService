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
                Version = "1.0.0",
                Major = 1,
                Minor = 0,
                Patch = 0,
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 1,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-10),
                        PlatformIdentifier = PlatformIdentifier.WinX64,
                        FileHash = "1234",
                        VersionId = 1,
                        FileProvider = FileProvider.GitHub
                    }
                }
            },
            new()
            {
                Id = 2,
                CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                Version = "1.1.0",
                Major = 1,
                Minor = 1,
                Patch = 0,
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 2,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-9),
                        PlatformIdentifier = PlatformIdentifier.WinX64,
                        FileHash = "1234",
                        VersionId = 2,
                        FileProvider = FileProvider.GitHub
                    }
                }
            },
            new()
            {
                Id = 3,
                CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                Version = "1.2.0",
                Major = 1,
                Minor = 2,
                Patch = 0,
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 3,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-8),
                        PlatformIdentifier = PlatformIdentifier.WinX64,
                        FileHash = "1234",
                        VersionId = 3,
                        FileProvider = FileProvider.GitHub
                    }
                }
            },
            new()
            {
                Id = 4,
                CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                Version = "1.3.0-b1",
                Major = 1,
                Minor = 3,
                Patch = 0,
                PreReleaseNo = 1,
                IsBetaVersion = true,
                Files = new List<VersionFileEntity>
                {
                    new()
                    {
                        Id = 5,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                        PlatformIdentifier = PlatformIdentifier.WinX64,
                        FileHash = "1234",
                        VersionId = 4,
                        FileProvider = FileProvider.GitHub
                    },
                    new()
                    {
                        Id = 6,
                        CreatedTimestamp = DateTime.Now.AddMinutes(-7),
                        PlatformIdentifier = PlatformIdentifier.LinuxX64,
                        FileHash = "1234",
                        VersionId = 4,
                        FileProvider = FileProvider.GitHub
                    }
                }
            }
        };
    }
}