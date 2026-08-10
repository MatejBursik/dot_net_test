using System.ComponentModel.DataAnnotations;
using library_api.Validation;

namespace library_api.DTOs;

public class BorrowBookDto {
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [Required]
    [FutureDate]
    public DateTime DueDate { get; set; }
}