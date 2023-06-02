namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionFileResult
{
    public Stream FileStream { get; set; }

    public string FileHash { get; set; }
    
    public string FileName { get; set; }

    public VersionFileResult(Stream fileStream, string fileHash, string fileName)
    {
        FileStream = fileStream;
        FileHash = fileHash;
        FileName = fileName;
    }
}