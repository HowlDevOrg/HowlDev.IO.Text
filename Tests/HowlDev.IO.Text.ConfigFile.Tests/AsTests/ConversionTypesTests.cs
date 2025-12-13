using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;
using HowlDev.IO.Text.ConfigFile.Primitives;
using System;

namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests;

// Tests for all IConvertible conversion types supported by PrimitiveConfigOption
// This includes all conversion methods from the IConvertible interface
internal class ConversionTypesTests {
    [Test]
    public async Task ItAllWorks() {
        // Make a txt file that includes all the types a Primitive can parse to
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsProperties<PersonRecord>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task AllSupportedConversionTypes() {
        // Test all conversion types that work without throwing errors
        
        // String conversion
        var stringPrimitive = new PrimitiveConfigOption("Hello World");
        await Assert.That(stringPrimitive.ToString(null)).IsEqualTo("Hello World");
        await Assert.That(stringPrimitive.AsString()).IsEqualTo("Hello World");
        await Assert.That(stringPrimitive.GetTypeCode()).IsEqualTo(TypeCode.String);
        
        // Integer conversions
        var intPrimitive = new PrimitiveConfigOption("42");
        await Assert.That(intPrimitive.ToInt32(null)).IsEqualTo(42);
        await Assert.That(intPrimitive.AsInt()).IsEqualTo(42);
        await Assert.That(intPrimitive.ToInt16(null)).IsEqualTo((short)42);
        await Assert.That(intPrimitive.ToUInt16(null)).IsEqualTo((ushort)42);
        await Assert.That(intPrimitive.ToUInt32(null)).IsEqualTo((uint)42);
        await Assert.That(intPrimitive.ToUInt64(null)).IsEqualTo((ulong)42);
        
        // Double/Decimal conversions
        var doublePrimitive = new PrimitiveConfigOption("3.14159");
        await Assert.That(doublePrimitive.ToDouble(null)).IsEqualTo(3.14159);
        await Assert.That(doublePrimitive.AsDouble()).IsEqualTo(3.14159);
        await Assert.That(doublePrimitive.ToDecimal(null)).IsEqualTo((decimal)3.14159);
        
        // Boolean conversions
        var boolPrimitive = new PrimitiveConfigOption("true");
        await Assert.That(boolPrimitive.ToBoolean(null)).IsEqualTo(true);
        await Assert.That(boolPrimitive.AsBool()).IsEqualTo(true);
        
        var falseBoolPrimitive = new PrimitiveConfigOption("false");
        await Assert.That(falseBoolPrimitive.ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task AllUnsupportedConversionTypesThrowExceptions() {
        var primitive = new PrimitiveConfigOption("test value");
        
        // Unsupported IConvertible methods
        await Assert.That(() => primitive.ToByte(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToByte not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToChar(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToChar not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToDateTime(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToDateTime not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToInt64(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToInt64 not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToSByte(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToSByte not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToSingle(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToSingle not supported on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.ToType(typeof(string), null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToType not supported on type PrimitiveConfigOption");
        
        // List returning methods
        await Assert.That(() => primitive.AsStringList())
            .Throws<InvalidOperationException>()
            .WithMessage("List returning not allowed on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.AsIntList())
            .Throws<InvalidOperationException>()
            .WithMessage("List returning not allowed on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.AsDoubleList())
            .Throws<InvalidOperationException>()
            .WithMessage("List returning not allowed on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.AsBoolList())
            .Throws<InvalidOperationException>()
            .WithMessage("List returning not allowed on type PrimitiveConfigOption");
        
        // Object/Dictionary operations
        IBaseConfigOption outValue;
        await Assert.That(() => primitive.TryGet("key", out outValue))
            .Throws<InvalidOperationException>()
            .WithMessage("TryGet not allowed on type PrimitiveConfigOption");
        
        await Assert.That(() => primitive.Contains("key"))
            .Throws<InvalidOperationException>()
            .WithMessage("Contains not allowed on type PrimitiveConfigOption");
        
        // Property access that should throw
        await Assert.That(() => primitive.Keys)
            .Throws<InvalidOperationException>()
            .WithMessage("Key enumeration not allowed on type of PrimitiveConfigOption.");
        
        await Assert.That(() => primitive.Count)
            .Throws<InvalidOperationException>()
            .WithMessage("Count not allowed on type of PrimitiveConfigOption.");
        
        await Assert.That(() => primitive.Items)
            .Throws<InvalidOperationException>()
            .WithMessage("Item enumeration not allowed on type of PrimitiveConfigOption.");
        
        // Indexer operations
        await Assert.That(() => primitive["key"])
            .Throws<InvalidOperationException>()
            .WithMessage("Key indexing operation invalid on type of PrimitiveConfigOption.");
        
        await Assert.That(() => primitive[0])
            .Throws<InvalidOperationException>()
            .WithMessage("List indexing operation invalid on type of PrimitiveConfigOption.");
        
        // Invalid type casting
        var invalidIntPrimitive = new PrimitiveConfigOption("not a number");
        await Assert.That(() => invalidIntPrimitive.AsInt())
            .Throws<InvalidCastException>()
            .WithMessage("Value \"not a number\" is not castable to an Int.");
        
        var invalidDoublePrimitive = new PrimitiveConfigOption("not a number");
        await Assert.That(() => invalidDoublePrimitive.AsDouble())
            .Throws<InvalidCastException>()
            .WithMessage("Value \"not a number\" is not castable to a Double.");
        
        var invalidBoolPrimitive = new PrimitiveConfigOption("not a bool");
        await Assert.That(() => invalidBoolPrimitive.AsBool())
            .Throws<InvalidCastException>()
            .WithMessage("Value \"not a bool\" is not castable to a Boolean.");
    }
}
