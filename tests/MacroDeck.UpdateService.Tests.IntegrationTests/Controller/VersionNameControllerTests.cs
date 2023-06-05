using Flurl.Http;
using MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess.DatabaseSeeder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Controller;

public class VersionNameControllerTests : TestBase
{
    private VersionDatabaseSeeder _versionDatabaseSeeder;
    private VersionFileDatabaseSeeder _versionFileDatabaseSeeder;

    public override async Task SetUp()
    {
        await base.SetUp();
        _versionDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionDatabaseSeeder>();
        _versionFileDatabaseSeeder = TestScope.ServiceProvider.GetRequiredService<VersionFileDatabaseSeeder>();
    }
    
    [Test]
    public async Task ApiV2Controller_GetNextMajorVersion_Returns_Correct_Version()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.0.0");
        await _versionFileDatabaseSeeder.CreateVersionFile(version1);
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("major")
            .GetJsonAsync<Version>();
        
        Assert.That(result1.ToString(), Is.EqualTo("2.0.0"));
        
        var version2 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "2.0.0-b1");
        await _versionFileDatabaseSeeder.CreateVersionFile(version2);
        
        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("major")
            .GetJsonAsync<Version>();
        
        Assert.That(result2.ToString(), Is.EqualTo("2.0.0"));
    }
    
    [Test]
    public async Task ApiV2Controller_GetNextMinorVersion_Returns_Correct_Version()
    {
        var version = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.0.0");
        await _versionFileDatabaseSeeder.CreateVersionFile(version);
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("minor")
            .GetJsonAsync<Version>();
        
        Assert.That(result1.ToString(), Is.EqualTo("1.1.0"));

        var version2 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.2.0-b1");
        await _versionFileDatabaseSeeder.CreateVersionFile(version2);
        
        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("minor")
            .GetJsonAsync<Version>();
        
        Assert.That(result2.ToString(), Is.EqualTo("1.2.0"));
    }
    
    [Test]
    public async Task ApiV2Controller_GetNextPatchVersion_Returns_Correct_Version()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.0.0");
        await _versionFileDatabaseSeeder.CreateVersionFile(version1);
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("patch")
            .GetJsonAsync<Version>();
        
        Assert.That(result1.ToString(), Is.EqualTo("1.0.1"));
    }
    
    [Test]
    public async Task ApiV2Controller_GetMajorBetaPatchVersion_Returns_Correct_Version()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.0.0");
        await _versionFileDatabaseSeeder.CreateVersionFile(version1);
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("major")
            .AppendPathSegment("beta")
            .GetJsonAsync<Version>();
        
        Assert.That(result1.ToString(), Is.EqualTo("2.0.0-b1"));
        
        var version2 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = result1.ToString());
        await _versionFileDatabaseSeeder.CreateVersionFile(version2);
        
        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("major")
            .AppendPathSegment("beta")
            .GetJsonAsync<Version>();
        
        Assert.That(result2.ToString(), Is.EqualTo("2.0.0-b2"));
    }

    [Test]
    public async Task ApiV2Controller_GetMinorBetaPatchVersion_Returns_Correct_Version()
    {
        var version1 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = "1.0.0");
        await _versionFileDatabaseSeeder.CreateVersionFile(version1);
        
        var result1 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("minor")
            .AppendPathSegment("beta")
            .GetJsonAsync<Version>();
        
        Assert.That(result1.ToString(), Is.EqualTo("1.1.0-b1"));
        
        var version2 = await _versionDatabaseSeeder.CreateVersion(update: entity => entity.Version = result1.ToString());
        await _versionFileDatabaseSeeder.CreateVersionFile(version2);
        
        var result2 = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.VersionNameBase)
            .AppendPathSegment("next")
            .AppendPathSegment("minor")
            .AppendPathSegment("beta")
            .GetJsonAsync<Version>();
        
        Assert.That(result2.ToString(), Is.EqualTo("1.1.0-b2"));
    }
}