using Microsoft.EntityFrameworkCore;
using MemoApp.Data.Entities;

namespace MemoApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<UserBook> UserBooks => Set<UserBook>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Isbn13)
            .IsUnique();

        modelBuilder.Entity<UserBook>()
            .HasIndex(ub => new { ub.UserId, ub.BookId })
            .IsUnique();
    }
}
