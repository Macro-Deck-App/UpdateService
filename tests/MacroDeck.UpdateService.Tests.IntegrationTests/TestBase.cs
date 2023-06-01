using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests;

public class TestBase
{
    [SetUp]
    public async Task BaseSetUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
    }

    [TearDown]
    public async Task BaseCleanUp()
    {
        await IntegrationTestHelper.TruncateAllTables();
    }
}