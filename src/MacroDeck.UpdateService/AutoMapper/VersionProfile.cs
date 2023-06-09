using AutoMapper;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.Helper;

namespace MacroDeck.UpdateService.AutoMapper;

public class VersionProfile : Profile
{
    public VersionProfile()
    {
        CreateMap<VersionEntity, VersionInfo>()
            .ConvertUsing((src, _, _) =>
            {
                return new VersionInfo
                {
                    Version = src.Version,
                    IsBeta = src.IsBetaVersion,
                    ChangeNotesUrl = GetChangeNotesUrl(src.Version),
                    Platforms = src.Files.ToDictionary(
                        f => f.PlatformIdentifier, 
                        f => new VersionFileInfo
                        {
                            DownloadUrl = FileProviderUrlBuilder.GetUrl(f.FileProvider, src.Version, f.FileName),
                            FileHash = f.FileHash,
                            FileSize = f.FileSize
                        })
                };
            });
    }

    private static string GetChangeNotesUrl(string version)
    {
        return $"https://github.com/Macro-Deck-App/Macro-Deck/releases/tag/v{version}";
    }
}