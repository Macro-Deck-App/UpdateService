using MacroDeck.UpdateService.Core.Configuration;
using MacroDeck.UpdateService.Core.Helper;
using MacroDeck.UpdateService.DatabaseMigration;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
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
        
        await UpdateServiceConfiguration.Initialize();
        DatabaseMigrationHelper.MigrateDatabase();
        
        if (EnvironmentHelper.IsGitHubIntegrationTest)
        {
            UpdateServiceConfiguration.DatabaseConnectionStringOverride =
                "User ID=updateservice;Password=test;Database=updateserviceintegrationtest;Host=localhost;Port=5432";
        }

        var webHostBuilder = new WebHostBuilder()
            .UseStartup<Startup>()
            .ConfigureServices(InitializeAdditionalServices);
        
        IntegrationTestHelper.TestServer = new TestServer(webHostBuilder);
        await IntegrationTestHelper.TruncateAllTables();
        
        IntegrationTestHelper.RootScope = IntegrationTestHelper.TestServer.Services.CreateScope();
    }

    private void InitializeAdditionalServices(IServiceCollection services)
    {
        services.AddScoped(typeof(ITestRepository<>), typeof(TestRepository<>));
        services.AddScoped<VersionDatabaseSeeder>();
        services.AddScoped<VersionFileDatabaseSeeder>();
    }

    [OneTimeTearDown]
    public async Task CleanUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
    }
}