using library_api.Models;

namespace library_api.DAL;

public class InMemoryLibraryRepository : ILibraryRepository {
    private readonly List<Book> _books = [];
    private readonly List<User> _users = [];
    private readonly List<Borrowing> _borrowings = [];

    // Books
    public List<Book> GetBooks() => _books;

    public Book? GetBook(Guid id) => _books.FirstOrDefault(b => b.Id == id);

    public Book AddBook(Book book) {
        _books.Add(book);
        return book;
    }

    public void UpdateBook(Book book) {}

    public void DeleteBook(Guid id) {
        var book = GetBook(id);

        if (book != null) {
            _books.Remove(book);
        }
    }

    public bool HasActiveBorrowing(Guid bookId) {
        return _borrowings.Any(b => b.BookId == bookId && b.ReturnedAt == null);
    }

    public bool HasBorrowingHistory(Guid bookId) {
        return _borrowings.Any(b => b.BookId == bookId);
    }

    // Users
    public List<User> GetUsers() => _users;

    public User? GetUser(Guid id) => _users.FirstOrDefault(u => u.Id == id);

    public User AddUser(User user) {
        _users.Add(user);
        return user;
    }

    public void UpdateUser(User user) {}

    public void DeleteUser(Guid id) {
        var user = GetUser(id);

        if (user != null) {
            _users.Remove(user);
        }
    }

    public bool HasActiveBorrowings(Guid userId) {
        return _borrowings.Any(b => b.UserId == userId && b.ReturnedAt == null);
    }

    // Borrowing
    public Borrowing AddBorrowing(Borrowing borrowing) {
        _borrowings.Add(borrowing);
        return borrowing;
    }

    public Borrowing? GetActiveBorrowing(Guid bookId) {
        return _borrowings.FirstOrDefault(x => x.BookId == bookId && x.ReturnedAt == null);
    }

    public void UpdateBorrowing(Borrowing borrowing) {}
}