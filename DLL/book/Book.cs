namespace DLL2;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Topic { get; set; }
    public int Quantity { get; set; }

    public ICollection<Reader> Readers { get; set; } = new List<Reader>();
}