using MacroDeck.UpdateService.Core.Utils;
using MacroDeck.UpdateService.Tests.Shared;
using NUnit.Framework;

namespace MacroDeck.UpdateService.UnitTests.UtilityTests;

[TestFixture]
public class Base64Tests
{
    [Test]
    public void Base64Encode_Returns_Correct_Base64_String()
    {
        var testString = Constants.Base64TestStringPlain;
        var testStringBase64 = Base64Utils.Base64Encode(testString);
        
        Assert.That(testStringBase64, Is.EqualTo(Constants.Base64TestStringBase64));
    }

    [Test]
    public void Base64Decode_Returns_Correct_String()
    {
        var testStringBase64 = Constants.Base64TestStringBase64;
        var testString = Base64Utils.Base64Decode(testStringBase64);
        
        Assert.That(testString, Is.EqualTo(Constants.Base64TestStringPlain));
    }
}