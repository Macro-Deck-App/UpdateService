using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class NoFileUploadedException : ErrorException
{
    public NoFileUploadedException()
        : base(ErrorMessages.NoFileUploaded, ErrorCode.FileNotUploaded, StatusCodes.Status400BadRequest)
    {
    }
}