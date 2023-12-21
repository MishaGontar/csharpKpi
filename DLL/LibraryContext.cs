using Microsoft.EntityFrameworkCore;

namespace DLL2;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book>? Books { get; set; }
    public DbSet<Reader>? Readers { get; set; }
}