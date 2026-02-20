namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;

public class SimpleNestedString {
    public string[] Strings { get; set; } = [];
}
public class SimpleNestedInt {
    public IEnumerable<int> Ints { get; set; } = [];
}
public class SimpleNestedBool {
    public List<bool> Bools { get; set; } = [];
}
public class ComplexIntsAndStrings {
    public int[] Ints { get; set; } = [];
    public string[] Strings { get; set; } = [];
}
public class DoublyNestedStrings {
    public SimpleNestedString[] UpperStrings { get; set; } = [];
}
