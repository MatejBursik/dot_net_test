using library_api.Controllers;
using library_api.DTOs;
using library_api.Models;
using library_api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Tests;

public class UsersControllerTests {
    private static User CreateUser() {
        return new User {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };
    }

    [Fact]
    public void GetUsers_ReturnsAllUsers() { // TEST ERROR
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var user1 = CreateUser();
        var user2 = CreateUser();

        repository.AddUser(user1);
        repository.AddUser(user2);

        var controller = new UsersController(repository);

        // Act
        var result = controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);

        Assert.Equal(2, users.Count());
        Assert.Contains(users, u => u.Id == user1.Id);
        Assert.Contains(users, u => u.Id == user2.Id);
    }

    [Fact]
    public void GetUserWhenUserExists_ReturnsOk() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var user = CreateUser();

        repository.AddUser(user);

        var controller = new UsersController(repository);

        // Act
        var result = controller.GetUser(user.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);

        Assert.Equal(user.Id, returnedUser.Id);
        Assert.Equal(user.FirstName, returnedUser.FirstName);
        Assert.Equal(user.LastName, returnedUser.LastName);
        Assert.Equal(user.Email, returnedUser.Email);
    }

    [Fact]
    public void GetUserWhenUserDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new UsersController(repository);

        // Act
        var result = controller.GetUser(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateUserWithValidData_ReturnsCreated() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new UsersController(repository);

        var dto = new CreateUserDto {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        // Act
        var result = controller.CreateUser(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var user = Assert.IsType<User>(createdResult.Value);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
        Assert.Equal(dto.Email, user.Email);

        var storedUser = repository.GetUser(user.Id);

        Assert.NotNull(storedUser);
        Assert.Equal(user.Id, storedUser.Id);
        Assert.Equal(user.FirstName, storedUser.FirstName);
        Assert.Equal(user.LastName, storedUser.LastName);
        Assert.Equal(user.Email, storedUser.Email);
    }

    [Fact]
    public void UpdateUserWhenUserExists_ReturnsNoContent() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var user = CreateUser();

        repository.AddUser(user);

        var controller = new UsersController(repository);

        var dto = new UpdateUserDto {
            Email = "updated@example.com"
        };

        // Act
        var result = controller.UpdateUser(user.Id, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var updatedUser = repository.GetUser(user.Id);

        Assert.NotNull(updatedUser);
        Assert.Equal("updated@example.com", updatedUser.Email);
    }

    [Fact]
    public void UpdateUserWhenUserDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new UsersController(repository);

        var dto = new UpdateUserDto {
            Email = "updated@example.com"
        };

        // Act
        var result = controller.UpdateUser(Guid.NewGuid(), dto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteUserWhenUserExists_ReturnsNoContent() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var user = CreateUser();

        repository.AddUser(user);

        var controller = new UsersController(repository);

        // Act
        var result = controller.DeleteUser(user.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(repository.GetUser(user.Id));
    }

    [Fact]
    public void DeleteUserWhenUserDoesNotExist_ReturnsNotFound() {
        // Arrange
        var repository = new InMemoryLibraryRepository();
        var controller = new UsersController(repository);

        // Act
        var result = controller.DeleteUser(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
