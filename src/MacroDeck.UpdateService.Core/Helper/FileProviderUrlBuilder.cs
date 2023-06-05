using MacroDeck.UpdateService.Core.Constants;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.Helper;

public static class FileProviderUrlBuilder
{
    public static string GetUrl(FileProvider fileProvider, string version, string fileName)
    {
        return fileProvider switch
        {
            FileProvider.GitHub => $"{FileProviderBaseUrls.GitHubFileBaseUrl}/v{version}/{fileName}",
            _ => throw new ArgumentOutOfRangeException(nameof(fileProvider), fileProvider, null)
        };
    }
}