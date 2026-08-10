using library_api.Models;

namespace library_api.DAL;

public interface ILibraryRepository {
    // Books
    List<Book> GetBooks();
    Book? GetBook(Guid id);
    Book AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Guid id);
    bool HasActiveBorrowing(Guid bookId);
    bool HasBorrowingHistory(Guid bookId);

    // Users
    List<User> GetUsers();
    User? GetUser(Guid id);
    User AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(Guid id);
    bool HasActiveBorrowings(Guid userId);

    // Borrowing
    Borrowing AddBorrowing(Borrowing borrowing);
    Borrowing? GetActiveBorrowing(Guid bookId);
    void UpdateBorrowing(Borrowing borrowing);
}