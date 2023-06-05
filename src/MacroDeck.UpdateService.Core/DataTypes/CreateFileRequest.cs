using System.ComponentModel.DataAnnotations;
using MacroDeck.UpdateService.Core.Enums;

namespace MacroDeck.UpdateService.Core.DataTypes;

public class CreateFileRequest
{
    [Required]
    public required string FileName { get; set; }
    
    [Required]
    public required FileProvider FileProvider { get; set; }
    
    [Required]
    public required string FileHash { get; set; }
    
    [Required]
    public required long FileSize { get; set; }
}