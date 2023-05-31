using System.Security.Cryptography;

namespace MacroDeck.UpdateService.Core.DataAccess.Extensions;

public static class ByteArrayExtensions
{
    public static string GenerateSha256Hash(this byte[] bytes)
    {
        var hashBytes = SHA256.HashData(bytes);
        return hashBytes.Aggregate("", (current, t) => current + t.ToString("X2"));
    }
}