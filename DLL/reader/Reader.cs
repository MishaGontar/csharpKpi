namespace DLL2;

public class Reader
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<Book> GetsBooks { get;} = new List<Book>();
}