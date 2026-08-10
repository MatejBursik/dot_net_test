using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase {
    private readonly ILibraryRepository _repository;

    public BooksController(ILibraryRepository repository) {
        _repository = repository;
    }

    [HttpPost]
    public IActionResult CreateBook(CreateBookDto dto) {
        var book = new Book {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN
        };

        _repository.AddBook(book);

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBook(Guid id) {
        var book = _repository.GetBook(id);

        if (book == null) {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateBook(Guid id, UpdateBookDto dto) {
        var book = _repository.GetBook(id);

        if (book == null) {
            return NotFound();
        }

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;

        _repository.UpdateBook(book);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBook(Guid id) {
        var book = _repository.GetBook(id);

        if (book == null) {
            return NotFound();
        }

        if (_repository.HasActiveBorrowing(id)) {
            return Conflict("Cannot delete a borrowed book.");
        }

        if (_repository.HasBorrowingHistory(id)) {
            return Conflict("Cannot delete a book that has borrowing history.");
        }

        _repository.DeleteBook(id);

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetBooks() {
        return Ok(_repository.GetBooks());
    }
}