using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Exceptions;
using HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;

namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests;

public class AsEnumerablePrimitiveTests {
    [Test]
    public async Task AsStringEnumerable() {
        string json = """
        [ "Lorem", "ipsum" ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<string> p = [.. reader.AsEnumerable<string>()];
        await Assert.That(p[0]).IsEqualTo("Lorem");
        await Assert.That(p[1]).IsEqualTo("ipsum");
    }

    [Test]
    public async Task AsBoolEnumerable() {
        string json = """
        [ "false", "True" ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<bool> p = [.. reader.AsEnumerable<bool>()];
        await Assert.That(p[0]).IsEqualTo(false);
        await Assert.That(p[1]).IsEqualTo(true);
    }

    [Test]
    public async Task AsDoubleEnumerable() {
        string json = """
        [ 2.7, 3.1415 ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<double> p = [.. reader.AsEnumerable<double>()];
        await Assert.That(p[0]).IsBetween(2.69, 2.71); // Attempting to get around rounding errors
        await Assert.That(p[1]).IsBetween(3.1, 3.2);
    }

    [Test]
    public async Task AsIntEnumerable() {
        string json = """
        [ 31, 23, -25, 5 ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<int> p = [.. reader.AsEnumerable<int>()];
        await Assert.That(p[0]).IsEqualTo(31);
        await Assert.That(p[1]).IsEqualTo(23);
        await Assert.That(p[2]).IsEqualTo(-25);
        await Assert.That(p[3]).IsEqualTo(5);
    }

    [Test]
    public async Task AsDateTimeEnumerable() {
        string json = """
        [ "2025-12-04T05:40:30", "2025-12-04T05:50:30" ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<DateTime> p = [.. reader.AsEnumerable<DateTime>()];
        await Assert.That(p.Count).IsEqualTo(2);
        await Assert.That(p[0].Day).IsEqualTo(4);
        await Assert.That(p[1].Minute).IsEqualTo(50);
    }
}
public class AsEnumerableClassTests {
    [Test]
    public async Task AsPersonRecordEnumerable() {
        string json = """
        [
            {
                "name": "Jane",
                "id": 23
            },
            {
                "name": "Adam",
                "id": 26
            }
        ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

        List<PersonRecord> p = [.. reader.AsEnumerable<PersonRecord>()];
        await Assert.That(p[0].Name).IsEqualTo("Jane");
        await Assert.That(p[0].Id).IsEqualTo(23);
        await Assert.That(p[1].Name).IsEqualTo("Adam");
        await Assert.That(p[1].Id).IsEqualTo(26);
    }
}
public class AsStrictEnumerableClassTests {
    [Test]
    public async Task AsPersonStrictEnumerable() {
        string json = """
        [
            {
                "name": "Jane",
                "id": 23
            },
            {
                "name": "Adam",
                "id": 26
            },
        ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        OptionMappingOptions o = new() { StrictMatching = true, UseConstructors = true };

        List<PersonRecord> p = [.. reader.AsEnumerable<PersonRecord>(o)];
        await Assert.That(p[0].Name).IsEqualTo("Jane");
        await Assert.That(p[0].Id).IsEqualTo(23);
        await Assert.That(p[1].Name).IsEqualTo("Adam");
        await Assert.That(p[1].Id).IsEqualTo(26);
    }

    [Test]
    public async Task AsFailingPersonRecordEnumerable1() {
        string json = """
        [
            {
                "name": "Jane",
                "id": 23, 
                "address": "you don't get to know"
            }
        ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        OptionMappingOptions o = new() { StrictMatching = true, UseConstructors = true };

        await Assert.That(() => reader.AsEnumerable<PersonRecord>(o))
            .Throws<StrictMappingException>()
            .WithMessage("""
                        No suitable constructor found for PersonRecord. Consider removing the StrictMatching flag. 
                        Tried to find a constructor that matched the following keys: name, id, address.
                        """);
    }

    [Test]
    public async Task AsFailingPersonRecordEnumerable2() {
        string json = """
        [
            {
                "name": "Jane"
            }
        ]
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        OptionMappingOptions o = new() { StrictMatching = true, UseConstructors = true };

        await Assert.That(() => reader.AsEnumerable<PersonRecord>(o))
            .Throws<StrictMappingException>()
            .WithMessage("""
                        No suitable constructor found for PersonRecord. Consider removing the StrictMatching flag. 
                        Tried to find a constructor that matched the following keys: name.
                        """);
    }
}
public class AsNestedEnumerableClassTests {
    [Test]
    public async Task EnumerableOfStrings() {
        string json = """
        {
            "strings" : [
                "name", "lorem"
            ]
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        SimpleNestedString result = reader.As<SimpleNestedString>();

        await Assert.That(result.GetStrings().Length).IsEqualTo(2);
        await Assert.That(result.GetStrings()[0]).IsEqualTo("name");
        await Assert.That(result.GetStrings()[1]).IsEqualTo("lorem");
    }

    [Test]
    public async Task EnumerableOfInts() {
        string json = """
        {
            "ints" : [
                15, 23
            ]
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        SimpleNestedInt result = reader.As<SimpleNestedInt>();

        await Assert.That(result.Ints.Count()).IsEqualTo(2);
        await Assert.That(result.Ints.First()).IsEqualTo(15);
        await Assert.That(result.Ints.Last()).IsEqualTo(23);
    }

    [Test]
    public async Task EnumerableOfBools() {
        string json = """
        {
            "bools" : [
                true, false
            ]
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        SimpleNestedBool result = reader.As<SimpleNestedBool>();

        await Assert.That(result.Bools.Count).IsEqualTo(2);
        await Assert.That(result.Bools[0]).IsEqualTo(true);
        await Assert.That(result.Bools[1]).IsEqualTo(false);
    }

    [Test]
    public async Task MixedObject() {
        string json = """
        {
            "ints": [
                23, 5
            ], 
            "strings": [
                "Lorem", "Ipsum and more"
            ]
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        ComplexIntsAndStrings result = reader.As<ComplexIntsAndStrings>();

        await Assert.That(result.GetInts().Length).IsEqualTo(2);
        await Assert.That(result.GetInts()[0]).IsEqualTo(23);
        await Assert.That(result.GetInts()[1]).IsEqualTo(5);
        await Assert.That(result.GetStrings().Length).IsEqualTo(2);
        await Assert.That(result.GetStrings()[0]).IsEqualTo("Lorem");
        await Assert.That(result.GetStrings()[1]).IsEqualTo("Ipsum and more");
    }

    [Test]
    public async Task ObjectOfArrayOfObjects() {
        string json = """
        {
            "books": [
                {
                    name: Little Women,
                    weight: 2.3,
                    height: 12.3
                },
                {
                    name: Great Gatsby,
                    weight: 15
                },
            ], 
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        ArrayOfBooks result = reader.As<ArrayOfBooks>();

        await Assert.That(result.GetBooks().Length).IsEqualTo(2);
        await Assert.That(result.GetBooks()[0].Name).IsEqualTo("Little Women");
        await Assert.That(result.GetBooks()[0].Weight).IsEqualTo(2.3);
        await Assert.That(result.GetBooks()[0].Height).IsEqualTo(12.3);
        await Assert.That(result.GetBooks()[1].Name).IsEqualTo("Great Gatsby");
        await Assert.That(result.GetBooks()[1].Weight).IsEqualTo(15);
        await Assert.That(result.GetBooks()[1].Height).IsEqualTo(0);
    }

    [Test]
    public async Task StringOfStringsOfStrings() {
        string json = """
        {
            "upperStrings" : [
                {"strings": ["one", "two"]},
                {"strings": ["three", "four"]},
                {"strings": ["five"]},
            ]
        }
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);
        DoublyNestedStrings result = reader.As<DoublyNestedStrings>();

        await Assert.That(result.GetUpperStrings().Length).IsEqualTo(3);
        await Assert.That(result.GetUpperStrings()[0].GetStrings().Length).IsEqualTo(2);
        await Assert.That(result.GetUpperStrings()[1].GetStrings().Length).IsEqualTo(2);
        await Assert.That(result.GetUpperStrings()[1].GetStrings()[1]).IsEqualTo("four");
        await Assert.That(result.GetUpperStrings()[2].GetStrings().Length).IsEqualTo(1);
    }
}
