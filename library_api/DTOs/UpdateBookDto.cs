using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs;

public class UpdateBookDto {
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = "";
}