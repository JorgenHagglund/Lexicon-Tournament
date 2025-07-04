using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.DTOs;

public record GameToCreateDto
{
    [Required(ErrorMessage = "Title is required.")]
    public required string Title { get; init; }
    [Required(ErrorMessage = "Start time is required.")]
    [Range(typeof(DateTime), "2025-01-01", "9999-12-31", ErrorMessage = "{0} must be between {1} and {2}.")]
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    [Required(ErrorMessage = "Tournament ID is required.")]
    public int TournamentId { get; init; }
}
