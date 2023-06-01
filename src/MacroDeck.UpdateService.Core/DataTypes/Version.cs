using System.Text.RegularExpressions;

namespace MacroDeck.UpdateService.Core.DataTypes;

public partial struct Version
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public int? PreviewNo { get; set; }

    public Version(int major, int minor, int patch, int? previewNo = null)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        PreviewNo = previewNo;
    }

    public bool IsPreviewVersion => PreviewNo.HasValue;

    public override string ToString()
    {
        return PreviewNo.HasValue
            ? $"{Major}.{Minor}.{Patch}-preview{PreviewNo}"
            : $"{Major}.{Minor}.{Patch}";
    }

    public static bool TryParse(string versionString, out Version result)
    {
        try
        {
            result = Parse(versionString);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
    
    public static Version Parse(string versionString)
    {
        var match = VersionRegex().Match(versionString);
        if (!match.Success)
        {
            throw new FormatException("Invalid version string");
        }

        var major = int.Parse(match.Groups["major"].Value);
        var minor = int.Parse(match.Groups["minor"].Value);
        var patch = int.Parse(match.Groups["patch"].Value);

        int? previewNo = null;
        if (match.Groups["preview"].Success)
        {
            previewNo = int.Parse(match.Groups["preview"].Value);
        }

        return new Version(major, minor, patch, previewNo);
    }

    [GeneratedRegex("^(?<major>\\d+)\\.(?<minor>\\d+)\\.(?<patch>\\d+)(-preview(?<preview>\\d+))?$")]
    private static partial Regex VersionRegex();
}