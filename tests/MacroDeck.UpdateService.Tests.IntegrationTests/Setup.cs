using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.DataAccess.Extensions;
using MacroDeck.UpdateService.Core.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests;

[SetUpFixture]
public class Setup
{

    [OneTimeSetUp]
    public async Task SetupIntegrationTestEnvironment()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        if (EnvironmentHelper.IsGitHubIntegrationTest)
        {
            UpdateServiceConfiguration.DatabaseConnectionStringOverride = "";   
        }

        var webHostBuilder = new WebHostBuilder()
            .UseStartup<Startup>()
            .ConfigureServices(InitializeAdditionalServices);
        
        IntegrationTestHelper.TestServer = new TestServer(webHostBuilder);
        await IntegrationTestHelper.TestServer.Services.MigrateDatabaseAsync();
        await IntegrationTestHelper.TruncateAllTables();
    }

    private void InitializeAdditionalServices(IServiceCollection serviceCollection)
    {
    }

    [OneTimeTearDown]
    public async Task CleanUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
    }
}