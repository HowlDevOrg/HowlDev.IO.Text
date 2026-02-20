namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderYAMLFileTests {
    [Test]
    public async Task PrimitiveTest1() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/FirstOrder/Primitive1.yaml");
        await Assert.That(reader.ToString(null)).IsEqualTo("45");
        await Assert.That(reader.ToInt32(null)).IsEqualTo(45);
    }

    [Test]
    public async Task PrimitiveTest2() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/FirstOrder/Primitive2.yaml");
        await Assert.That(reader.ToString(null)).IsEqualTo("This is a sample multiline string that may become useful");
    }

    [Test]
    public async Task ObjectTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/FirstOrder/Object.yaml");
        await Assert.That(reader["Lorem"].ToString(null)).IsEqualTo("Test String");
        await Assert.That(reader["Lorem2"].ToInt32(null)).IsEqualTo(15);
        await Assert.That(reader["Lorem3"].ToDouble(null)).IsEqualTo(3.15);
        await Assert.That(reader["Lorem4"].ToBoolean(null)).IsEqualTo(true);
    }

    [Test]
    public async Task BrokenObjectThrowsError() {
        await Assert.That(() => new TextConfigFile("../../../data/YAML/FirstOrder/BrokenObject.yaml"))
            .Throws<FormatException>()
            .WithMessage("Don't include multiple (:) on the same line. I read key: \"Broken\"");
    }

    [Test]
    public async Task ArrayTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/FirstOrder/Array.yaml");
        await Assert.That(reader[0].ToString(null)).IsEqualTo("Test String");
        await Assert.That(reader[1].ToInt32(null)).IsEqualTo(15);
        await Assert.That(reader[2].ToDouble(null)).IsEqualTo(3.15);
        await Assert.That(reader[3].ToBoolean(null)).IsEqualTo(true);
    }
}
public class SecondOrderYAMLFileTests {
    [Test]
    public async Task ObjectWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/SecondOrder/ObjectWithObject.yaml");

        await Assert.That(reader["first"]["lorem"].ToString(null)).IsEqualTo("test");
        await Assert.That(reader["first"]["num"].ToInt32(null)).IsEqualTo(1);
        await Assert.That(reader["first"]["double"].ToDouble(null)).IsEqualTo(2.0);
        await Assert.That(reader["first"]["bool"].ToBoolean(null)).IsEqualTo(true);
        await Assert.That(reader["second"]["ipsum"].ToString(null)).IsEqualTo("something");
        await Assert.That(reader["second"]["num"].ToInt32(null)).IsEqualTo(2);
        await Assert.That(reader["second"]["double"].ToDouble(null)).IsEqualTo(3.2);
        await Assert.That(reader["second"]["bool"].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task ObjectWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/SecondOrder/ObjectWithArray.yaml");

        await Assert.That(reader["first"][0].ToInt32(null)).IsEqualTo(1);
        await Assert.That(reader["first"][1].ToInt32(null)).IsEqualTo(5);
        await Assert.That(reader["first"][2].ToString(null)).IsEqualTo("string");
        await Assert.That(reader["second"][0].ToString(null)).IsEqualTo("this should have 4 spaces before it");
        await Assert.That(reader["second"][1].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/SecondOrder/ArrayWithObject.yaml");

        await Assert.That(reader[0]["lorem"].ToString(null)).IsEqualTo("inside");
        await Assert.That(reader[0]["whatsthis"].ToDouble(null)).IsEqualTo(2.5);
        await Assert.That(reader[1]["arrayCount"].ToString(null)).IsEqualTo("second");
        await Assert.That(reader[1]["bool"].ToBoolean(null)).IsEqualTo(false);
        await Assert.That(reader[1]["int"].ToInt32(null)).IsEqualTo(5);
    }

    [Test]
    public async Task ArrayWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/SecondOrder/ArrayWithArray.yaml");

        await Assert.That(reader[0][0].ToString(null)).IsEqualTo("lorem");
        await Assert.That(reader[0][1].ToDouble(null)).IsEqualTo(2.5);
        await Assert.That(reader[0][2].ToString(null)).IsEqualTo("allowed");
        await Assert.That(reader[1][0].ToString(null)).IsEqualTo("second");
        await Assert.That(reader[1][1].ToInt32(null)).IsEqualTo(3);
        await Assert.That(reader[1][2].ToBoolean(null)).IsEqualTo(false);
    }

    [Test]
    public async Task MixedArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/SecondOrder/MixedArray.yml");

        await Assert.That(reader[0]["object"].ToString(null)).IsEqualTo("this is");
        await Assert.That(reader[0]["part2"].ToString(null)).IsEqualTo("still part of this object");
        await Assert.That(reader[0]["part3"].ToDouble(null)).IsEqualTo(45.3);
        await Assert.That(reader[1].ToInt32(null)).IsEqualTo(15);
        await Assert.That(reader[2].ToString(null)).IsEqualTo("test string");
    }
}
public class RealisticYAMLFileTests {
    [Test]
    public async Task RealisticTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/YAML/Realistic/ComplexObject.yaml");

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
}
