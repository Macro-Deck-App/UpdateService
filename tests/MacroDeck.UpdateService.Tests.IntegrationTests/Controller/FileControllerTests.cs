using Flurl.Http;
using MacroDeck.UpdateService.Core.Enums;
using MacroDeck.UpdateService.Tests.IntegrationTests.Extensions;
using NUnit.Framework;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.Controller;

[TestFixture]
public class FileControllerTests : TestBase
{
    [Test]
    public async Task FileController_Upload_Valid_File_Success()
    {
        var testFile = Path.Combine("TestFiles", "testfile-win.exe");
        Assert.True(File.Exists(testFile));
        
        var result = await IntegrationTestHelper.TestClientRequest
            .AppendPathSegment(IntegrationTestConstants.FilesBase)
            .AppendPathSegment("1.0.0")
            .AppendPathSegment(PlatformIdentifier.WinX64.ToString())
            .WithAdminToken()
            .PostMultipartAsync(x => x.AddFile("file", testFile));

        var uploadRequestResponse = await result.GetStringAsync();
        
        Assert.NotNull(uploadRequestResponse);
        Assert.That(uploadRequestResponse, Is.EqualTo("1.0.0"));
    }
}