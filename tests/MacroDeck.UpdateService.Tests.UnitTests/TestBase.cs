using MacroDeck.UpdateService.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MacroDeck.UpdateService.UnitTests;

public class TestBase
{
    protected UpdateServiceContext UpdateServiceContext { get; set; }

    [SetUp]
    public void SetUpDatabase()
    {
        var options = new DbContextOptionsBuilder<UpdateServiceContext>()
            .UseInMemoryDatabase(databaseName: TestContext.CurrentContext.Test.ID)
            .Options;
        
        UpdateServiceContext = new UpdateServiceContext(options);
    }

    [TearDown]
    public void CleanUp()
    {
        UpdateServiceContext?.Dispose();
    }
}