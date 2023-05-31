using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class FileAlreadyExistsException : ErrorException
{
    public FileAlreadyExistsException()
        : base(ErrorMessages.FileAlreadyExists, ErrorCode.FileAlreadyExists, StatusCodes.Status409Conflict)
    {
    }
}