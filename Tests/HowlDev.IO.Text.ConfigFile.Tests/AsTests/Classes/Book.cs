namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests.Classes;


public class BookClass {
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Height { get; set; }
}

public class BrokenBookClass {
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Height { get; set; }

    public BrokenBookClass() { }

    public BrokenBookClass(string innerName, double weight, double height) {
        Name = innerName;
        Weight = weight;
        Height = height;
    }

}

public class ArrayOfBooks(BookClass[] books) {
    private BookClass[] books = books;

    public BookClass[] GetBooks() {
        return books;
    }
}
