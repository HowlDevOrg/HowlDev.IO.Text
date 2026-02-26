using HowlDev.IO.Text.Parsers;
using HowlDev.IO.Text.Parsers.Enums;
using System.Collections;

namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

internal class ParseFileAsOptionTests {
    [Test]
    public async Task AllObjectsMustBeClosedBeforeEnding() {
        // { Lorem: {} 
        PseudoTextParser p = new PseudoTextParser([
            (TextToken.StartObject, ""),
            (TextToken.KeyValue, "Lorem"),
            (TextToken.StartObject, ""),
            (TextToken.EndObject, "")
        ]);
        await Assert.That(() => TextConfigFile.ConvertTokenStreamToConfigOption(p))
            .Throws<InvalidOperationException>()
            .WithMessage("Not all objects were closed. Stack count post-parse: 1");
    }

    [Test]
    public async Task CannotCloseAnArrayWithObject() {
        var p = new PseudoTextParser([
            (TextToken.StartArray, ""),
            (TextToken.Primitive, "Lorem"),
            (TextToken.EndObject, "")
        ]);
        await Assert.That(() => TextConfigFile.ConvertTokenStreamToConfigOption(p))
            .Throws<InvalidOperationException>()
            .WithMessage("Cannot close an Array object with an EndObject token.");
    }

    [Test]
    public async Task CannotCloseAnObjectWithArray() {
        // { Lorem: {} 
        var p = new PseudoTextParser([
            (TextToken.StartObject, ""),
            (TextToken.KeyValue, "Lorem"),
            (TextToken.Primitive, "Lorem2"),
            (TextToken.EndArray, ""),
        ]);
        await Assert.That(() => TextConfigFile.ConvertTokenStreamToConfigOption(p))
            .Throws<InvalidOperationException>()
            .WithMessage("Cannot close an Object object with an EndArray token.");
    }
}

internal class PseudoTextParser(IEnumerable<(TextToken, string)> list) : ITokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        foreach ((TextToken, string) item in list) yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
