using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.State)
                .HasConversion<string>();

            entity.Property(e => e.Category)
                .HasConversion<string>();
        });
    }
}