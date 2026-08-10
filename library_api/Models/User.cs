namespace library_api.Models;

public class User {
    public Guid Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";

    public ICollection<Borrowing> Borrowings { get; set; } = [];
}