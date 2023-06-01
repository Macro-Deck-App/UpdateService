using System.Reflection;
using Flurl.Http;
using MacroDeck.UpdateService.Core.DataAccess;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests;

public static class IntegrationTestHelper
{
    public static TestServer TestServer { get; set; } = null!;

    public static IFlurlRequest TestClientRequest => FlurlClient.Request();
    
    private static IFlurlClient FlurlClient
    {
        get
        {
            var flurlClient = new FlurlClient(TestServer.CreateClient());
            flurlClient.OnError(call =>
            {
                var response = call.Exception.StackTrace;
                TestContext.Error.WriteLine(response);
            });
            return flurlClient;
        }
    }

    public static async Task TruncateAllTables()
    {
        var context = TestServer.Services.GetRequiredService<UpdateServiceContext>();
        
        var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var entityList = Assembly.GetAssembly(typeof(BaseEntity))?.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseEntity)))
                .ToList();

            if (entityList == null)
            {
                return;
            }

            var tableNames = entityList.Select(x => context.Model.FindEntityType(x)?.GetTableName());

            var truncateCommand = $"TRUNCATE TABLE {string.Join(",", tableNames)}";
            await context.Database.ExecuteSqlRawAsync(truncateCommand);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}