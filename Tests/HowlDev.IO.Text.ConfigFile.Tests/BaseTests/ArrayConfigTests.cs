using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderArrayConfigTests {
    [Test]
    public async Task ArrayOfStrings() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("lorem"),
            new PrimitiveConfigOption("huh"),
            ]);
        await Assert.That(array[0].ToString(null)).IsEqualTo("True");
        await Assert.That(array[1].ToString(null)).IsEqualTo("lorem");
        await Assert.That(array[2].ToString(null)).IsEqualTo("huh");
    }

    [Test]
    public async Task ArrayOfStringsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("lorem"),
            new PrimitiveConfigOption("huh"),
            ]);
        List<string> testList = [.. array.AsEnumerable<string>()];
        await Assert.That(testList.Count).IsEqualTo(3);
        await Assert.That(testList[0]).IsEqualTo("True");
        await Assert.That(testList[1]).IsEqualTo("lorem");
        await Assert.That(testList[2]).IsEqualTo("huh");
    }

    [Test]
    public async Task ArrayOfInts() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1"),
            new PrimitiveConfigOption("3"),
            new PrimitiveConfigOption("4"),
            new PrimitiveConfigOption("-5"),
            ]);
        await Assert.That(array[0].ToInt32(null)).IsEqualTo(1);
        await Assert.That(array[1].ToInt32(null)).IsEqualTo(3);
        await Assert.That(array[2].ToInt32(null)).IsEqualTo(4);
        await Assert.That(array[3].ToInt32(null)).IsEqualTo(-5);
    }

    [Test]
    public async Task ArrayOfIntsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1"),
            new PrimitiveConfigOption("3"),
            new PrimitiveConfigOption("4"),
            new PrimitiveConfigOption("-5"),
            ]);
        List<int> testList = [.. array.AsEnumerable<int>()];
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(1);
        await Assert.That(testList[1]).IsEqualTo(3);
        await Assert.That(testList[2]).IsEqualTo(4);
        await Assert.That(testList[3]).IsEqualTo(-5);
    }

    [Test]
    public async Task ArrayOfDoubles() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1.234"),
            new PrimitiveConfigOption("3.74"),
            new PrimitiveConfigOption("4.1"),
            new PrimitiveConfigOption("-5.463"),
            ]);
        await Assert.That(array[0].ToDouble(null)).IsEqualTo(1.234);
        await Assert.That(array[1].ToDouble(null)).IsEqualTo(3.74);
        await Assert.That(array[2].ToDouble(null)).IsEqualTo(4.1);
        await Assert.That(array[3].ToDouble(null)).IsEqualTo(-5.463);
    }

    [Test]
    public async Task ArrayOfDoublesAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1.234"),
            new PrimitiveConfigOption("3.74"),
            new PrimitiveConfigOption("4.1"),
            new PrimitiveConfigOption("-5.463"),
            ]);
        List<double> testList = [.. array.AsEnumerable<double>()];
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(1.234);
        await Assert.That(testList[1]).IsEqualTo(3.74);
        await Assert.That(testList[2]).IsEqualTo(4.1);
        await Assert.That(testList[3]).IsEqualTo(-5.463);
    }

    [Test]
    public async Task ArrayOfBools() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("true"),
            new PrimitiveConfigOption("False"),
            new PrimitiveConfigOption("false"),
            ]);
        await Assert.That(array[0].ToBoolean(null)).IsEqualTo(true);
        await Assert.That(array[1].ToBoolean(null)).IsEqualTo(true);
        await Assert.That(array[2].ToBoolean(null)).IsEqualTo(false);
        await Assert.That(array[3].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayOfBoolsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("true"),
            new PrimitiveConfigOption("False"),
            new PrimitiveConfigOption("false"),
            ]);
        List<bool> testList = [.. array.AsEnumerable<bool>()];
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(true);
        await Assert.That(testList[1]).IsEqualTo(true);
        await Assert.That(testList[2]).IsEqualTo(false);
        await Assert.That(testList[3]).IsEqualTo(false);
    }

    [Test]
    public async Task TypeCastingThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array.ToString(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToString not allowed on type of ArrayConfigOption.");
        await Assert.That(() => array.ToInt32(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToInt32 not allowed on type of ArrayConfigOption.");
        await Assert.That(() => array.ToDouble(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToDouble not allowed on type of ArrayConfigOption.");
        await Assert.That(() => array.ToBoolean(null))
            .Throws<InvalidOperationException>()
            .WithMessage("ToBoolean not allowed on type of ArrayConfigOption.");
    }

    [Test]
    public async Task StringIndexingThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array["lorem"])
            .Throws<InvalidOperationException>()
            .WithMessage("Key indexing operation invalid on type of ArrayConfigOption.");
    }

    [Test]
    public async Task OutOfBoundsThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array[1])
            .Throws<ArgumentException>()
            .WithMessage("Index 1 is out of range. This array has 1 items.");
    }

    [Test]
    public async Task NonArrayTypesThrowError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array.ToType("".GetType(), null))
            .Throws<InvalidDataException>()
            .WithMessage("Type conversion is not an enumerable.");
    }

    [Test]
    public async Task MixedArray() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            new PrimitiveConfigOption("15"),
            new PrimitiveConfigOption("2.34"),
            new PrimitiveConfigOption("true")
            ]);
        await Assert.That(array[0].ToString(null)).IsEqualTo("Lorem");
        await Assert.That(array[1].ToInt32(null)).IsEqualTo(15);
        await Assert.That(array[2].ToDouble(null)).IsEqualTo(2.34);
        await Assert.That(array[3].ToBoolean(null)).IsEqualTo(true);
    }
}
public class SecondOrderArrayConfigTests {
    [Test]
    public async Task ArrayOfArrays() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("15"),
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ]),
            new PrimitiveConfigOption("false")
            ]);
        await Assert.That(array[0].ToInt32(null)).IsEqualTo(15);
        await Assert.That(array[1][0].ToString(null)).IsEqualTo("Lorem");
        await Assert.That(array[1][1].ToDouble(null)).IsEqualTo(2.34);
        await Assert.That(array[2].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task NestedArraysThrowHelpfulErrors() {
        ArrayConfigOption array = new ArrayConfigOption([
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ], "test", "0"),
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ], "test", "1")
            ], "test");
        await Assert.That(() => array[0][2])
            .Throws<ArgumentException>()
            .WithMessage("Index 2 is out of range. This array has 2 items.\n\tPath: test[0]");
    }
}
