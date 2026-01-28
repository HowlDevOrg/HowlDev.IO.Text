using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Exceptions;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests;

public class OptionMappingOptionsTests {
    [Test]
    public async Task AsStrictOverridesPassedInMappingOption() {
        OptionMappingOptions o1 = new() { StrictMatching = false, UseConstructors = true };

        // Strict test
        string txt = """
        name: Jane
        id: 23
        lorem: breaks
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrict<PersonRecord>(o1))
            .Throws<StrictMappingException>();
    }
}