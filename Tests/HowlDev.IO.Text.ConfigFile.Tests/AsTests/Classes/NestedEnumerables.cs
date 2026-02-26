namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;

public class SimpleNestedString {
    private string[] strings = [];

    public string[] GetStrings() {
        return strings;
    }

    public void SetStrings(string[] value) {
        strings = value;
    }
}
public class SimpleNestedInt {
    public IEnumerable<int> Ints { get; set; } = [];
}
public class SimpleNestedBool {
    public List<bool> Bools { get; set; } = [];
}
public class ComplexIntsAndStrings {
    private int[] ints = [];

    public int[] GetInts() {
        return ints;
    }
    
    public void SetInts(int[] value) {
        ints = value;
    }

    private string[] strings = [];

    public string[] GetStrings() {
        return strings;
    }

    public void SetStrings(string[] value) {
        strings = value;
    }
}
public class DoublyNestedStrings {
    private SimpleNestedString[] upperStrings = [];

    public SimpleNestedString[] GetUpperStrings() {
        return upperStrings;
    }

    public void SetUpperStrings(SimpleNestedString[] value) {
        upperStrings = value;
    }
}
