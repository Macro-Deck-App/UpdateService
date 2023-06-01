using Microsoft.AspNetCore.Http;

namespace MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;

public class CannotParseVersionException : ErrorException
{
    public CannotParseVersionException()
        : base(ErrorMessages.CannotParseVersionString, ErrorCode.CannotParseVersionString, StatusCodes.Status400BadRequest)
    {
    }
}