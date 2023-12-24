namespace PL;

class Program
{
    private static async Task Main(string[] args)
    {
        await new ConsoleUI().Run();
    }
}