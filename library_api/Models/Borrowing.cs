namespace library_api.Models;

public class Borrowing {
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public Guid UserId { get; set; }

    public DateTime BorrowedAt { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public Book Book { get; set; } = null!;

    public User User { get; set; } = null!;
}