using library_api.Controllers;
using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Tests;

public class BorrowingsControllerTests {
    [Fact]
    public void BorrowBookWithValidData() {
        // Arrange
        var repository = new InMemoryLibraryRepository();

        var user = new User {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test User",
            Email = "test@example.com"
        };

        var book = new Book {
            Id = Guid.NewGuid(),
            Title = "Vagabond",
            Author = "Takehiko Inoue",
            ISBN = "123456789"
        };

        repository.AddUser(user);
        repository.AddBook(book);

        var controller = new BorrowingsController(repository);

        var dto = new BorrowBookDto {
            UserId = user.Id,
            BookId = book.Id,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = controller.BorrowBook(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.NotNull(createdResult.Value);
        
        var borrowing = Assert.IsType<BorrowingResponseDto>(createdResult.Value);
        Assert.Equal(book.Id, borrowing.BookId); Assert.Equal(user.Id, borrowing.UserId);
        Assert.Null(borrowing.ReturnedAt); Assert.True(borrowing.DueDate > DateTime.UtcNow);
    }
}
