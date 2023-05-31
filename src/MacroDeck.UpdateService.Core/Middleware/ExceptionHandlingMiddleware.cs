using System.Text.Json;
using MacroDeck.UpdateService.Core.ErrorHandling;
using MacroDeck.UpdateService.Core.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MacroDeck.UpdateService.Core.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger _logger = Log.ForContext<ExceptionHandlingMiddleware>();
    
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExceptionHandlingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
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
        
        object errorMessage = new
        {
            Success = false,
            Error = message,
            ErrorCode = errorCode
        };

        context.Response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(errorMessage);
        await context.Response.WriteAsync(json);
    }
}