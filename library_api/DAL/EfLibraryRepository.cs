using library_api.Data;
using library_api.Models;
using Microsoft.EntityFrameworkCore;

namespace library_api.DAL;

public class EfLibraryRepository : ILibraryRepository {
    private readonly LibraryDbContext _context;

    public EfLibraryRepository(LibraryDbContext context) {
        _context = context;
    }

    // Books
    public List<Book> GetBooks() {
        return _context.Books.AsNoTracking().ToList();
    }

    public Book? GetBook(Guid id) {
        return _context.Books.FirstOrDefault(b => b.Id == id);
    }

    public Book AddBook(Book book) {
        _context.Books.Add(book);
        _context.SaveChanges();

        return book;
    }

    public void UpdateBook(Book book) {
        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void DeleteBook(Guid id) {
        var book = GetBook(id);

        if (book != null) {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
    }

    public bool HasActiveBorrowing(Guid bookId) {
        return _context.Borrowings.Any(b =>
            b.BookId == bookId &&
            b.ReturnedAt == null);
    }

    public bool HasBorrowingHistory(Guid bookId) {
        return _context.Borrowings.Any(b => b.BookId == bookId);
    }

    // Users
    public List<User> GetUsers() {
        return _context.Users.AsNoTracking().ToList();
    }

    public User? GetUser(Guid id) {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User AddUser(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public void UpdateUser(User user) {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void DeleteUser(Guid id) {
        var user = GetUser(id);

        if (user != null) {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public bool HasActiveBorrowings(Guid userId) {
        return _context.Borrowings.Any(b =>
            b.UserId == userId &&
            b.ReturnedAt == null);
    }

    // Borrowings
    public Borrowing AddBorrowing(Borrowing borrowing) {
        _context.Borrowings.Add(borrowing);
        _context.SaveChanges();

        return borrowing;
    }

    public Borrowing? GetActiveBorrowing(Guid bookId) {
        return _context.Borrowings
            .FirstOrDefault(b =>
                b.BookId == bookId &&
                b.ReturnedAt == null);
    }

    public void UpdateBorrowing(Borrowing borrowing) {
        _context.Borrowings.Update(borrowing);
        _context.SaveChanges();
    }
}