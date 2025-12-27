using HowlDev.IO.Text.ConfigFile.Primitives;

namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

// Tests for all IConvertible conversion types supported by PrimitiveConfigOption
// This includes all conversion methods from the IConvertible interface
internal class ConversionTypesTests {
    [Test]
    public async Task AllSupportedConversionTypes() {
        // Test all conversion types that work without throwing errors

        // String conversion
        var stringPrimitive = new PrimitiveConfigOption("Hello World");
        await Assert.That(stringPrimitive.ToString(null)).IsEqualTo("Hello World");
        await Assert.That(stringPrimitive.As<string>()).IsEqualTo("Hello World");
        await Assert.That(stringPrimitive.GetTypeCode()).IsEqualTo(TypeCode.String);

        // Datetime conversion
        var datetimePrimitive = new PrimitiveConfigOption("2025-12-15T06:00:23Z");
        await Assert.That(datetimePrimitive.ToDateTime(null).Second).IsEqualTo(23);
        await Assert.That(datetimePrimitive.As<DateTime>().Second).IsEqualTo(23);

        // Integer conversions
        var intPrimitive = new PrimitiveConfigOption("42");
        await Assert.That(intPrimitive.ToInt32(null)).IsEqualTo(42);
        await Assert.That(intPrimitive.ToInt16(null)).IsEqualTo((short)42);
        await Assert.That(intPrimitive.ToInt64(null)).IsEqualTo(42);
        await Assert.That(intPrimitive.ToUInt16(null)).IsEqualTo((ushort)42);
        await Assert.That(intPrimitive.ToUInt32(null)).IsEqualTo((uint)42);
        await Assert.That(intPrimitive.ToUInt64(null)).IsEqualTo((ulong)42);
        await Assert.That(intPrimitive.As<int>()).IsEqualTo(42);
        await Assert.That(intPrimitive.As<short>()).IsEqualTo((short)42);
        await Assert.That(intPrimitive.As<long>()).IsEqualTo(42);
        await Assert.That(intPrimitive.As<ushort>()).IsEqualTo((ushort)42);
        await Assert.That(intPrimitive.As<uint>()).IsEqualTo((uint)42);
        await Assert.That(intPrimitive.As<ulong>()).IsEqualTo((ulong)42);

        // Double/Decimal conversions
        var doublePrimitive = new PrimitiveConfigOption("3.14159");
        await Assert.That(doublePrimitive.ToDouble(null)).IsEqualTo(3.14159);
        await Assert.That(doublePrimitive.ToDecimal(null)).IsEqualTo((decimal)3.14159);
        await Assert.That(doublePrimitive.As<double>()).IsEqualTo(3.14159);
        await Assert.That(doublePrimitive.As<decimal>()).IsEqualTo((decimal)3.14159);

        // Boolean conversions
        var boolPrimitive = new PrimitiveConfigOption("true");
        await Assert.That(boolPrimitive.ToBoolean(null)).IsEqualTo(true);
        await Assert.That(boolPrimitive.As<bool>()).IsEqualTo(true);

        var falseBoolPrimitive = new PrimitiveConfigOption("false");
        await Assert.That(falseBoolPrimitive.ToBoolean(null)).IsEqualTo(false);
        await Assert.That(falseBoolPrimitive.As<bool>()).IsEqualTo(false);
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

        await Assert.That(() => primitive.ToSByte(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToSByte not supported on type PrimitiveConfigOption");

        await Assert.That(() => primitive.ToSingle(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToSingle not supported on type PrimitiveConfigOption");

        await Assert.That(() => primitive.ToType(typeof(string), null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToType not supported on type PrimitiveConfigOption");
    }
}