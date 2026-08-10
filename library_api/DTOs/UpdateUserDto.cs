using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs;

public class UpdateUserDto {
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}