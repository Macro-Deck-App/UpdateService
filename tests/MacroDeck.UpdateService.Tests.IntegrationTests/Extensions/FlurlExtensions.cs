using Flurl.Http;
using MacroDeck.UpdateService.Core.Configuration;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Extensions;

public static class FlurlExtensions
{
    public static IFlurlRequest WithAdminToken(this IFlurlRequest flurlRequest)
    {
        flurlRequest.Headers.Add("x-admin-token", UpdateServiceConfiguration.AdminAuthenticationToken);
        return flurlRequest;
    }
}