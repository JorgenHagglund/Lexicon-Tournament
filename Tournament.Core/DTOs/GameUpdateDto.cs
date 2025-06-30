using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.DTOs;

public record GameUpdateDto
{
    [Required(ErrorMessage = "Game Title is required.")]
    [MaxLength(35, ErrorMessage = "Game Title cannot exceed 35 characters.")]
    public required string Title { get; set; }
    public DateTime Time { get; set; }
}
