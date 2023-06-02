using MacroDeck.UpdateService.Core.Extensions;
using MacroDeck.UpdateService.Tests.Shared;
using NUnit.Framework;

namespace MacroDeck.UpdateService.UnitTests.UtilityTests;

[TestFixture]
public class HashTests
{
    [Test]
    public async Task GenerateFileHash_Returns_Correct_Hash()
    {
        var testFile1Path = Path.Combine("TestFiles", "testfile-linux");
        var testFile2Path = Path.Combine("TestFiles", "testfile-mac.dmg");
        var testFile3Path = Path.Combine("TestFiles", "testfile-win.exe");

        var testFile1Stream = File.OpenRead(testFile1Path);
        var testFile2Stream = File.OpenRead(testFile2Path);
        var testFile3Stream = File.OpenRead(testFile3Path);

        var testFile1Hash = await testFile1Stream.CalculateSha256Hash();
        var testFile2Hash = await testFile2Stream.CalculateSha256Hash();
        var testFile3Hash = await testFile3Stream.CalculateSha256Hash();
        Assert.Multiple(() =>
        {
            Assert.That(testFile1Hash, Is.EqualTo(SharedTestConstants.TestFileLinuxSha256));
            Assert.That(testFile2Hash, Is.EqualTo(SharedTestConstants.TestFileMacSha256));
            Assert.That(testFile3Hash, Is.EqualTo(SharedTestConstants.TestFileWinSha256));
        });
    }
}