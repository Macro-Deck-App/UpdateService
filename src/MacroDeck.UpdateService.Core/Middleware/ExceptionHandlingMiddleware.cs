using System.Text.Json;
using MacroDeck.UpdateService.Core.ErrorHandling;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MacroDeck.UpdateService.Core.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger _logger = Log.ForContext<ExceptionHandlingMiddleware>();
    
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        if (exception is not ErrorException errorException)
        {
            _logger.Fatal(exception, "Unhandled error on request {Request}", context.Request.Path);
            errorException = new GenericErrorException();
        }


        var errorCode = errorException.ErrorCode;
        var statusCode = errorException.StatusCode;
        var message = errorException.ErrorMessage;
        
        var errorResponse = new ErrorResponse
        {
            Success = false,
            Error = message,
            ErrorCode = errorCode
        };

        context.Response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(json);
    }
}