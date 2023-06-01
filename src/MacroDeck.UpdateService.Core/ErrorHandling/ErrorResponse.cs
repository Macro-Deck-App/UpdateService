namespace MacroDeck.UpdateService.Core.ErrorHandling;

public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public string Error { get; set; } = string.Empty;
    public ErrorCode ErrorCode { get; set; }
}