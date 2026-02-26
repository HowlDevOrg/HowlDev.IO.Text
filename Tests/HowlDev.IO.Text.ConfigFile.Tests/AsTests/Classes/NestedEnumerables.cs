namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;

public class SimpleNestedString(string[] strings) {
    private string[] strings = strings;

    public string[] GetStrings() {
        return strings;
    }
}
public class SimpleNestedInt {
    public IEnumerable<int> Ints { get; set; } = [];
}
public class SimpleNestedBool {
    public List<bool> Bools { get; set; } = [];
}
public class ComplexIntsAndStrings(int[] ints, string[] strings) {
    private int[] ints = ints;

    public int[] GetInts() {
        return ints;
    }
    private string[] strings = strings;

    public string[] GetStrings() {
        return strings;
    }
}
public class DoublyNestedStrings(SimpleNestedString[] upperStrings) {
    private SimpleNestedString[] upperStrings = upperStrings;

    public SimpleNestedString[] GetUpperStrings() {
        return upperStrings;
    }
}
