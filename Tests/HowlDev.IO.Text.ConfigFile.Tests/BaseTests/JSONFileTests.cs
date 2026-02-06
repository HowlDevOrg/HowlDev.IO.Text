using HowlDev.IO.Text.ConfigFile.Enums;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderJSONFileTests {
    [Test]
    public async Task ArrayTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/SimpleArray.json");

        await Assert.That(reader[0].ToInt32(null)).IsEqualTo(14);
        await Assert.That(reader[1].ToString(null)).IsEqualTo("lorem string");
        await Assert.That(reader[2].ToBoolean(null)).IsEqualTo(true);
    }

    [Test]
    public async Task ObjectTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/SimpleObject.json");

        await Assert.That(reader["lorem"].ToBoolean(null)).IsEqualTo(true);
        await Assert.That(reader["works"].ToDouble(null)).IsEqualTo(2.5);
        await Assert.That(reader["string"].ToString(null)).IsEqualTo("this is a string");
    }

    [Test]
    public async Task IntArrayTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/IntArray.json");

        List<int> ints = [.. reader.AsEnumerable<int>()];

        await Assert.That(ints.Count).IsEqualTo(4);
        await Assert.That(ints[0]).IsEqualTo(1);
        await Assert.That(ints[1]).IsEqualTo(3);
        await Assert.That(ints[2]).IsEqualTo(6);
        await Assert.That(ints[3]).IsEqualTo(7);
    }
}
public class SecondOrderJSONFileTests {
    [Test]
    public async Task ArrayWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ArrayWithArray.json");

        await Assert.That(reader[0][0].ToInt32(null)).IsEqualTo(1);
        await Assert.That(reader[0][1].ToBoolean(null)).IsEqualTo(true);
        await Assert.That(reader[0][2].ToString(null)).IsEqualTo("string");

        await Assert.That(reader[1][0].ToString(null)).IsEqualTo("second array");
        await Assert.That(reader[1][1].ToDouble(null)).IsEqualTo(5.3);
        await Assert.That(reader[1][2].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ArrayWithObject.json");

        await Assert.That(reader[0]["num"].ToInt32(null)).IsEqualTo(1);
        await Assert.That(reader[0]["happy"].ToString(null)).IsEqualTo("maybe");
        await Assert.That(reader[0]["number"].ToDouble(null)).IsEqualTo(5.3);

        await Assert.That(reader[1]["num"].ToInt32(null)).IsEqualTo(2);
        await Assert.That(reader[1]["happy"].ToString(null)).IsEqualTo("yes");
        await Assert.That(reader[1]["number"].ToBoolean(null)).IsEqualTo(false); // Because they don't have to be consistent
    }

    [Test]
    public async Task ObjectWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ObjectWithObject.json");

        await Assert.That(reader["first"]["type"].ToString(null)).IsEqualTo("object");
        await Assert.That(reader["first"]["number"].ToInt32(null)).IsEqualTo(15);

        await Assert.That(reader["second"]["type"].ToString(null)).IsEqualTo("still an object");
        await Assert.That(reader["second"]["boolean"].ToBoolean(null)).IsEqualTo(true);
    }


    [Test]
    public async Task ObjectWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ObjectWithArray.json");

        await Assert.That(reader["first"][0].ToInt32(null)).IsEqualTo(15);
        await Assert.That(reader["first"][1].ToString(null)).IsEqualTo("open string");
        await Assert.That(reader["first"][2].ToBoolean(null)).IsEqualTo(false);

        await Assert.That(reader["second"][0].ToDouble(null)).IsEqualTo(2.3);
        await Assert.That(reader["second"][1].ToString(null)).IsEqualTo("closed string");
    }
}
public class RealisticJSONFileTests {
    [Test]
    public async Task RealisticTest() {
        // Copied from the YAML system because I was lazy. But they're interoperable!
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/Realistic/ComplexObject.json");

        await Assert.That(reader["first"]["simple Array"][0].ToInt32(null)).IsEqualTo(1);
        await Assert.That(reader["first"]["simple Array"][1].ToInt32(null)).IsEqualTo(2);
        await Assert.That(reader["first"]["simple Array"][2].ToInt32(null)).IsEqualTo(3);
        await Assert.That(reader["first"]["brother"].ToString(null)).IsEqualTo("sample String");
        await Assert.That(reader["first"]["other sibling"]["sibKey"].ToString(null)).IsEqualTo("sibValue");

        await Assert.That(reader["second"]["arrayOfObjects"][0]["lorem"].ToString(null)).IsEqualTo("ipsum");
        await Assert.That(reader["second"]["arrayOfObjects"][0]["something"].ToDouble(null)).IsEqualTo(1.2);
        await Assert.That(reader["second"]["arrayOfObjects"][1]["lorem2"].ToString(null)).IsEqualTo("ipsum2");
        await Assert.That(reader["second"]["arrayOfObjects"][1]["something2"].ToBoolean(null)).IsEqualTo(false);
        await Assert.That(reader["second"]["otherThing"].ToString(null)).IsEqualTo("hopefully");
    }

    [Test]
    public async Task TestDTO() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/Realistic/DtoObject.json");

        await Assert.That(reader["namespace"].ToString(null)).IsEqualTo("ProjectTracker.Classes");
        await Assert.That(reader["name"].ToString(null)).IsEqualTo("IdAndTitleDTO");
        await Assert.That(reader["type"].ToString(null)).IsEqualTo("Class");

        await Assert.That(reader["properties"][0]["name"].ToString(null)).IsEqualTo("Id");
        await Assert.That(reader["properties"][0]["type"].ToString(null)).IsEqualTo("int[]");
        await Assert.That(reader["properties"][0]["default"].ToString(null)).IsEqualTo("[]");
    }
}

public class TextReadingJSONTests {
    [Test]
    public async Task ReadingTest1() {
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, "[ 14,  \"lorem string\",  true  ]");
        await Assert.That(reader[0].ToInt32(null)).IsEqualTo(14);
        await Assert.That(reader[1].ToString(null)).IsEqualTo("lorem string");
        await Assert.That(reader[2].ToBoolean(null)).IsEqualTo(true);
    }

    [Test]
    public async Task CanReadArrayBracketsInQuotesCorrectly() {
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, "{ \"value\": \"[]\" }");
        await Assert.That(reader["value"].ToString()).IsEqualTo("[]");
    }

    [Test]
    public async Task CanReadObjectBracketsInQuotesCorrectly() {
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, "{ \"value\": \"{}\" }");
        await Assert.That(reader["value"].ToString()).IsEqualTo("{}");
    }
}