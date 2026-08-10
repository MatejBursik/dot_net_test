using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase {
    private readonly ILibraryRepository _repository;

    public BorrowingsController(ILibraryRepository repository) {
        _repository = repository;
    }

    [HttpPost]
    public IActionResult BorrowBook(BorrowBookDto dto) {
        var book = _repository.GetBook(dto.BookId);

        if (book == null) {
            return NotFound("Book not found.");   
        }

        if (_repository.HasActiveBorrowing(dto.BookId)) {
            return Conflict("Book is already borrowed.");
        }

        var user = _repository.GetUser(dto.UserId);

        if (user == null) {
            return NotFound("User not found.");   
        }

        var borrowing = new Borrowing {
            Id = Guid.NewGuid(),
            BookId = dto.BookId,
            UserId = dto.UserId,
            BorrowedAt = DateTime.UtcNow,
            DueDate = dto.DueDate
        };

        _repository.AddBorrowing(borrowing);

        var response = new BorrowingResponseDto {
            Id = borrowing.Id,
            BookId = borrowing.BookId,
            UserId = borrowing.UserId,
            BorrowedAt = borrowing.BorrowedAt,
            DueDate = borrowing.DueDate,
            ReturnedAt = borrowing.ReturnedAt
        };

        return Created(string.Empty, response);
    }

    [HttpPost("{bookId:guid}/return")]
    public IActionResult ReturnBook(Guid bookId) {
        var book = _repository.GetBook(bookId);

        if (book == null) {
            return NotFound("Book not found.");   
        }

        var borrowing = _repository.GetActiveBorrowing(bookId);

        if (borrowing == null) {
            return Conflict("Book is not currently borrowed.");
        }

        borrowing.ReturnedAt = DateTime.UtcNow;

        _repository.UpdateBorrowing(borrowing);

        return NoContent();
    }
}