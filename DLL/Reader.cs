namespace DLL2;

public class Reader
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }

    // Зв'язок багато-до-багатьох з книгами
    public ICollection<Book> Books { get; set; } = new List<Book>();
}