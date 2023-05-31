using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class NoVersionFoundException : ErrorException
{
    public NoVersionFoundException() 
        : base(ErrorMessages.NoVersionFound, ErrorCode.NoVersionFound, StatusCodes.Status404NotFound)
    {
    }
}