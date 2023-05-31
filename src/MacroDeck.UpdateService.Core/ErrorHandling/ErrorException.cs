namespace MacroDeck.UpdateService.Core.ErrorHandling;

public class ErrorException : Exception
{
    public string ErrorMessage { get; set; }
    
    public ErrorCode ErrorCode { get; set; }

    public int StatusCode { get; set; }

    protected ErrorException(string errorMessage, ErrorCode errorCode, int statusCode)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}