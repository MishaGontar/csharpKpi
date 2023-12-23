using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;

namespace Lab4;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        LibraryContext context = new LibraryContext(options);
        context.Database.EnsureDeleted();

        services.AddScoped<IRepository<Book>>(provider => new BookRepository(context));
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IRepository<Reader>>(provider => new ReaderRepository(context));
        services.AddScoped<IReaderService, ReaderService>();

        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}