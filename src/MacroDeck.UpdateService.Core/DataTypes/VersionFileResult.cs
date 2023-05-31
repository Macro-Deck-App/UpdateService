namespace MacroDeck.UpdateService.Core.DataTypes;

public class VersionFileResult
{
    public byte[] Bytes { get; set; }

    public string FileHash { get; set; }
    
    public string FileName { get; set; }

    public VersionFileResult(byte[] bytes, string fileHash, string fileName)
    {
        Bytes = bytes;
        FileHash = fileHash;
        FileName = fileName;
    }
}