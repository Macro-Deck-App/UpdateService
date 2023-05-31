using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class VersionDoesNotExistException : ErrorException
{
    public VersionDoesNotExistException() 
        : base(ErrorMessages.VersionDoesNotExist, ErrorCode.VersionDoesNotExist, StatusCodes.Status404NotFound)
    {
    }
}