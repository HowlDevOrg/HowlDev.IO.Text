namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;

public record PersonRecord(string Name, int Id);
public class PersonClass {
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }

    public PersonClass() { }

    public PersonClass(int id) {
        Id = id;
    }

    public PersonClass(string name) {
        Name = name;
    }

    public PersonClass(string name, int id) {
        Name = name;
        Id = id;
    }
}
public class StrictPersonClass {
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }

    public StrictPersonClass() { }
    public StrictPersonClass(string name, int id) {
        Name = name;
        Id = id;
    }
}