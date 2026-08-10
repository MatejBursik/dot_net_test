using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly ILibraryRepository _repository;

    public UsersController(ILibraryRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetUsers() {
        return Ok(_repository.GetUsers());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUser(Guid id) {
        var user = _repository.GetUser(id);

        if (user == null) {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserDto dto) {
        var user = new User {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };

        _repository.AddUser(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, UpdateUserDto dto) {
        var user = _repository.GetUser(id);

        if (user == null) {
            return NotFound();
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;

        _repository.UpdateUser(user);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id) {
        var user = _repository.GetUser(id);

        if (user == null) {
            return NotFound();
        }

        if (_repository.HasActiveBorrowings(id)) {
            return Conflict("Cannot delete a user with borrowed books.");
        }

        _repository.DeleteUser(id);

        return NoContent();
    }
}