using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests;

public abstract class TestBase
{
    protected IServiceScope TestScope { get; private set; }
    
    [SetUp]
    public virtual async Task SetUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
        TestScope = IntegrationTestHelper.RootScope.ServiceProvider.CreateScope();
    }

    [TearDown]
    public virtual async Task CleanUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
        TestScope?.Dispose();
    }
}