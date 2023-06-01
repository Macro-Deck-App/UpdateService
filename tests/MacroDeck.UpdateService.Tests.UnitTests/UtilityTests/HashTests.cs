using MacroDeck.UpdateService.Core.DataAccess.Extensions;
using MacroDeck.UpdateService.Tests.Shared;
using NUnit.Framework;

namespace MacroDeck.UpdateService.UnitTests.UtilityTests;

[TestFixture]
public class HashTests
{
    [Test]
    public void GenerateFileHash_Returns_Correct_Hash()
    {
        var testFile1Path = Path.Combine("TestFiles", "testfile-linux");
        var testFile2Path = Path.Combine("TestFiles", "testfile-mac.dmg");
        var testFile3Path = Path.Combine("TestFiles", "testfile-win.exe");

        var testFile1Bytes = File.ReadAllBytes(testFile1Path);
        var testFile2Bytes = File.ReadAllBytes(testFile2Path);
        var testFile3Bytes = File.ReadAllBytes(testFile3Path);

        var testFile1Hash = testFile1Bytes.GenerateSha256Hash();
        var testFile2Hash = testFile2Bytes.GenerateSha256Hash();
        var testFile3Hash = testFile3Bytes.GenerateSha256Hash();
        Assert.Multiple(() =>
        {
            Assert.That(testFile1Hash, Is.EqualTo(Constants.TestFileLinuxSha256));
            Assert.That(testFile2Hash, Is.EqualTo(Constants.TestFileMacSha256));
            Assert.That(testFile3Hash, Is.EqualTo(Constants.TestFileWinSha256));
        });
    }
}