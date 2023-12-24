namespace PL;

public class ConsoleUI()
{
    public async Task Run()
    {
        Console.WriteLine("Ласкаво просимо до Системи управління бібліотекою!");

        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine() ?? throw new InvalidOperationException();

            switch (choice)
            {
                case "1.1":
                    await HttpMethods.DisplayAllBooks();
                    break;
                case "1.2":
                    await HttpMethods.CreateBookAsync();
                    break;
                case "1.3":
                    await HttpMethods.GetBookByIdAsync();
                    break;
                case "1.4":
                    await HttpMethods.SearchByTitleAsync();
                    break;
                case "1.5":
                    await HttpMethods.SearchByAuthorAsync();
                    break;
                case "1.6":
                    await HttpMethods.SearchByTopicAsync();
                    break;
                case "1.7":
                    await HttpMethods.DeleteBookAsync();
                    break;
                case "2.1":
                    await HttpMethods.DisplayAllReaders();
                    break;
                case "2.2":
                    await HttpMethods.CreateReaderAsync();
                    break;
                case "2.3":
                    await HttpMethods.GetReaderByIdAsync();
                    break;
                case "2.4":
                    await HttpMethods.SearchByNameAsync();
                    break;
                case "2.5":
                    await HttpMethods.SearchByEmailAsync();
                    break;
                case "2.6":
                    await HttpMethods.GetBookByReaderAsync();
                    break;
                case "2.7":
                    await HttpMethods.GiveBackBookByReaderAsync();
                    break;
                case "2.8":
                    await HttpMethods.DeleteReaderAsync();
                    break;
                case "Q":
                case "q":
                    Console.WriteLine("Вихід з програми. До побачення!");
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Будь ласка спробуйте ще раз.");
                    break;
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nМеню:");
        Console.WriteLine("-----| Book |------ ");
        Console.WriteLine("1.1 Вивести всіх книжок");
        Console.WriteLine("1.2 Додати книжку");
        Console.WriteLine("1.3 Знайти книгу по ІД");
        Console.WriteLine("1.4 Пошук по назві");
        Console.WriteLine("1.5 Пошук по автору");
        Console.WriteLine("1.6 Пошук по тематиці");
        Console.WriteLine("1.7 Видалити книжку по ІД");

        Console.WriteLine("");

        Console.WriteLine("-----| Reader |------ ");
        Console.WriteLine("2.1 Вивести всіх читачів");
        Console.WriteLine("2.2 Додати читача");
        Console.WriteLine("2.3 Знайти читача по ІД");
        Console.WriteLine("2.4 Пошук по ім'ямі");
        Console.WriteLine("2.5 Пошук по email");
        Console.WriteLine("2.6 Додати книжку читачу");
        Console.WriteLine("2.7 Видалити книжку читачу");
        Console.WriteLine("2.8 Видалити читача по ІД");

        Console.WriteLine("");
        Console.WriteLine("Q. Вийти");
        Console.Write("Введіть число : ");
    }
}