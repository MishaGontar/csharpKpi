using System.Text;
using DLL2;
using Newtonsoft.Json;

namespace PL;

// ReSharper disable once InvalidXmlDocComment
public static class HttpMethods
{
    private static readonly string BookApiUrl = "http://localhost:5097/api/BookApi";
    private static readonly string ReaderApiUrl = "http://localhost:5097/api/ReaderApi";

    /// /// /// ///  Start Books http /// /// /// /// 
    public static async Task DisplayAllBooks()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(BookApiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return;
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

        Console.WriteLine("\nВсі книжки:");
        if (books != null)
            foreach (var book in books)
                PrintBook(book);
    }

    public static async Task GetBookByIdAsync()
    {
        Console.Write("\nВведіть id книги: ");
        var id = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();
        var apiUrl = $"{BookApiUrl}/{id}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return;
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var book = JsonConvert.DeserializeObject<Book>(jsonContent);

        if (book == null)
        {
            Console.WriteLine(book + "is null ");
            return;
        }

        PrintBook(book);
    }

    public static async Task CreateBookAsync()
    {
        Console.Write("\nВведіть назву книги: ");
        var title = Console.ReadLine();

        Console.Write("Введіть автора книги: ");
        var author = Console.ReadLine();

        Console.Write("Введіть тему книги: ");
        var topic = Console.ReadLine();

        Console.Write("Введіть кількість книг: ");
        var quantity = Convert.ToInt32(Console.ReadLine());

        var book = new Book { Title = title, Author = author, Topic = topic, Quantity = quantity };

        using var client = new HttpClient();
        var jsonData = JsonConvert.SerializeObject(book);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(BookApiUrl, content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Книгу успішно додано!: {response.Headers.Location}"
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    public static async Task DeleteBookAsync()
    {
        Console.Write("\nВведіть id книги, яку потрібно видалити: ");
        int id = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();
        var apiUrl = $"{BookApiUrl}/{id}";
        var response = await client.DeleteAsync(apiUrl);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Книга з ID {id} видалена"
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    public static async Task SearchByTitleAsync()
    {
        Console.Write("\nВведіть назву для пошуку: ");
        var title = Console.ReadLine();

        using var client = new HttpClient();
        var apiUrl = $"{BookApiUrl}/SearchByTitle?title={title}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

        Console.WriteLine($"Книги з назвою '{title}':");
        if (books != null)
            foreach (var book in books)
                PrintBook(book);
    }

    public static async Task SearchByAuthorAsync()
    {
        Console.Write("\nВведіть автора для пошуку: ");
        var author = Console.ReadLine();

        using var client = new HttpClient();
        var apiUrl = $"{BookApiUrl}/SearchByAuthor?author={author}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

        Console.WriteLine($"Книги автора: '{author}':");
        if (books != null)
            foreach (var book in books)
                PrintBook(book);
    }

    public static async Task SearchByTopicAsync()
    {
        Console.Write("\nВведіть тематику для пошуку: ");
        var topic = Console.ReadLine();

        using var client = new HttpClient();
        var apiUrl = $"{BookApiUrl}/SearchByTopic?topic={topic}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<Book>>(jsonContent);

        Console.WriteLine($"Книги з тематикою: '{topic}':");
        if (books != null)
            foreach (var book in books)
                PrintBook(book);
    }

    /// /// /// ///  End Books http /// /// /// ///
    /// /// /// ///  Start Reader http /// /// /// ///
    public static async Task DisplayAllReaders()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(ReaderApiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return;
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var readers = JsonConvert.DeserializeObject<List<Reader>>(jsonContent);

        Console.WriteLine("\nВсі читачі:");
        if (readers != null)
            foreach (var reader in readers)
                PrintReader(reader);
    }

    public static async Task CreateReaderAsync()
    {
        Console.Write("\nВведіть ім'я читача: ");
        var name = Console.ReadLine();

        Console.Write("Введіть адресу електронної пошти читача: ");
        var email = Console.ReadLine();

        var reader = new Reader { Name = name, Email = email };

        using var client = new HttpClient();
        var jsonData = JsonConvert.SerializeObject(reader);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(ReaderApiUrl, content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Читача успішно додано!: {response.Headers.Location}"
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    public static async Task GetReaderByIdAsync()
    {
        Console.Write("\nВведіть id читача: ");
        var id = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();
        var apiUrl = $"{ReaderApiUrl}/{id}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return;
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var reader = JsonConvert.DeserializeObject<Reader>(jsonContent);

        if (reader == null)
        {
            Console.WriteLine(reader + "is null ");
            return;
        }

        PrintReader(reader);
        Console.WriteLine("Всі книги які взяв читач:");
        foreach (var book in reader.GetsBooks)
            PrintBook(book);
    }

    public static async Task SearchByNameAsync()
    {
        Console.Write("\nВведіть ім'я для пошуку читача: ");
        var name = Console.ReadLine();

        using var client = new HttpClient();
        var apiUrl = $"{ReaderApiUrl}/searchByName?name={name}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var readers = JsonConvert.DeserializeObject<List<Reader>>(jsonContent);

        Console.WriteLine($"Читачі з ім'ям '{name}':");
        if (readers != null)
            foreach (var reader in readers)
                PrintReader(reader);
    }

    public static async Task SearchByEmailAsync()
    {
        Console.Write("\nВведіть email для пошуку читача: ");
        var email = Console.ReadLine();

        using var client = new HttpClient();
        var apiUrl = $"{ReaderApiUrl}/searchByEmail?email={email}";
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var readers = JsonConvert.DeserializeObject<List<Reader>>(jsonContent);

        Console.WriteLine($"Читачі з email '{email}':");
        if (readers != null)
            foreach (var reader in readers)
                PrintReader(reader);
    }

    public static async Task DeleteReaderAsync()
    {
        Console.Write("\nВведіть id читача, яку потрібно видалити: ");
        int id = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();
        var apiUrl = $"{ReaderApiUrl}/{id}";
        var response = await client.DeleteAsync(apiUrl);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Читач з ID {id} видалений"
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    public static async Task GetBookByReaderAsync()
    {
        Console.Write("\nВведіть id читача: ");
        var idReader = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введіть id книги: ");
        var idBook = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();

        var url = $"{ReaderApiUrl}/getBook?readerId={idReader}&bookId={idBook}";
        var response = await client.PostAsync(url, null);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Книгу з ідентифікатором {idBook} успішно отримано для читача з ідентифікатором {idReader}."
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    public static async Task GiveBackBookByReaderAsync()
    {
        Console.Write("\nВведіть id читача: ");
        var idReader = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введіть id книги: ");
        var idBook = Convert.ToInt32(Console.ReadLine());

        using var client = new HttpClient();

        var url = $"{ReaderApiUrl}/giveBackBook?readerId={idReader}&bookId={idBook}";
        var response = await client.PostAsync(url, null);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Книгу з ідентифікатором {idBook} успішно повернено користувачем {idReader}."
            : $"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    /// /// /// ///  End Reader http /// /// /// ///
    private static void PrintBook(Book book)
    {
        Console.WriteLine(
            $"Id: {book.Id} Назва: {book.Title}, Автор: {book.Author}, Тематика: {book.Topic} Кількість: {book.Quantity}");
    }

    private static void PrintReader(Reader reader)
    {
        Console.WriteLine(
            $"id: {reader.Id} Ім'я: {reader.Name}, Пошта: {reader.Email} , Кількість книжок взято : {reader.GetsBooks.Count}");
    }
}