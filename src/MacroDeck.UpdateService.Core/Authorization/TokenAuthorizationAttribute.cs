using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MacroDeck.UpdateService.Core.Authorization;

public class TokenAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly ILogger _logger = Log.ForContext<TokenAuthorizationAttribute>();
    private const string AuthorizationHeader = "x-admin-token";
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeader, out var adminToken))
        {
            context.Result = new UnauthorizedResult();
            LogFailedAuth(context);
            return;
        }

        if (adminToken.Count > 1)
        {
            context.Result = new UnauthorizedResult();
            LogFailedAuth(context);
            return;
        }

        context.HttpContext.Request.Headers.Remove(AuthorizationHeader);

        if (!UpdateServiceConfiguration.AdminAuthenticationToken.EqualsCryptographically(adminToken.ToString()))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            LogFailedAuth(context);
        }
    }

    private void LogFailedAuth(AuthorizationFilterContext context)
    {
        _logger.Fatal("Failed login from {RemoteIpAddress}", context.HttpContext.Connection.RemoteIpAddress);
    }
}