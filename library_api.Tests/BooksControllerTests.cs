using library_api.Controllers;
using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Tests;

public class BooksControllerTests {
    private static Book CreateBook() {
        return new Book {
            Id = Guid.NewGuid(),
            Title = "Vagabond",
            Author = "Takehiko Inoue",
            ISBN = "123456789"
        };
    }

    [Fact]
    public void GetBooks_ReturnsAllBooks() {
        // Arrange
        var repository = new InMemoryLibraryRepository();

        var book1 = CreateBook();
        var book2 = CreateBook();
        
        repository.AddBook(book1);
        repository.AddBook(book2);
        
        var controller = new BooksController(repository);
        
        // Act
        var result = controller.GetBooks();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);

        Assert.Equal(2, books.Count());
        Assert.Contains(books, b => b.Id == book1.Id);
        Assert.Contains(books, b => b.Id == book2.Id);
    }

    [Fact]
    public void GetBookWhenBookExists_ReturnsOk() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var book = CreateBook();

        repository.AddBook(book);

        var controller = new BooksController(repository);
        
        // Act
        var result = controller.GetBook(book.Id);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBook = Assert.IsType<Book>(okResult.Value);

        Assert.Equal(book.Id, returnedBook.Id);
        Assert.Equal(book.Title, returnedBook.Title);
        Assert.Equal(book.Author, returnedBook.Author);
        Assert.Equal(book.ISBN, returnedBook.ISBN);
    }
    
    [Fact]
    public void GetBookWhenBookDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new BooksController(repository);
        var id = Guid.NewGuid();
        
        // Act
        var result = controller.GetBook(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void CreateBookWithValidData_ReturnsCreated() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new BooksController(repository);
        var dto = new CreateBookDto {
            Title = "Vagabond",
            Author = "Takehiko Inoue",
            ISBN = "123456789"
        };

        // Act
        var result = controller.CreateBook(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var book = Assert.IsType<Book>(createdResult.Value);
        
        Assert.NotEqual(Guid.Empty, book.Id);
        Assert.Equal(dto.Title, book.Title);
        Assert.Equal(dto.Author, book.Author);
        Assert.Equal(dto.ISBN, book.ISBN);

        var storedBook = repository.GetBook(book.Id);
        Assert.NotNull(storedBook);
        Assert.Equal(book.Id, storedBook.Id);
    }
    
    [Fact]
    public void UpdateBookWhenBookExists_ReturnsNoContent() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var book = CreateBook();

        repository.AddBook(book);

        var controller = new BooksController(repository);
        var dto = new UpdateBookDto { Title = "Vagabond V2" };
        
        // Act
        var result = controller.UpdateBook(book.Id, dto);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedBook = repository.GetBook(book.Id);

        Assert.NotNull(updatedBook);
        Assert.Equal("Vagabond V2", updatedBook.Title);
    }
    
    [Fact]
    public void UpdateBookWhenBookDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new BooksController(repository);
        var dto = new UpdateBookDto {
            Title = "Vagabond",
            Author = "Takehiko Inoue",
            ISBN = "123456789"
        };
        
        // Act
        var result = controller.UpdateBook(Guid.NewGuid(), dto);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void DeleteBookWhenBookExistsAndHasNoBorrowingHistory_ReturnsNoContent() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var book = CreateBook();

        repository.AddBook(book);

        var controller = new BooksController(repository);
        
        // Act
        var result = controller.DeleteBook(book.Id);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(repository.GetBook(book.Id));
    }
    
    [Fact]
    public void DeleteBookWhenBookDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new BooksController(repository);
        
        // Act
        var result = controller.DeleteBook(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void DeleteBookWhenBookHasBorrowingHistory_ReturnsConflict() { // TEST ERROR
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var book = CreateBook();
        var user = new User {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        repository.AddBook(book);
        repository.AddUser(user);
        
        var borrowing = new Borrowing {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            UserId = user.Id,
            BorrowedAt = DateTime.UtcNow.AddDays(-10),
            DueDate = DateTime.UtcNow.AddDays(-3),
            ReturnedAt = DateTime.UtcNow.AddDays(-2)
        };
        
        repository.AddBorrowing(borrowing);

        var controller = new BooksController(repository);
        
        // Act
        var result = controller.DeleteBook(book.Id);
        
        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.NotNull(conflictResult.Value);
        Assert.NotNull(repository.GetBook(book.Id));
    }
}