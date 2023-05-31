using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class GenericErrorException : ErrorException
{
    public GenericErrorException() 
        : base(ErrorMessages.Generic, ErrorCode.InternalError, StatusCodes.Status500InternalServerError)
    {
    }
}