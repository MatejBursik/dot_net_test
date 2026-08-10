using library_api.Models;
using Microsoft.EntityFrameworkCore;

namespace library_api.Data;

public class LibraryDbContext : DbContext {
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {}

    public DbSet<Book> Books => Set<Book>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<Borrowing>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany(book => book.Borrowings)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.User)
            .WithMany(user => user.Borrowings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}