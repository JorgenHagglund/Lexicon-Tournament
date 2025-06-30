using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.DTOs;

public record TournamentUpdateDto
{
    //private DateTime? endDate;

    [Required(ErrorMessage = "Tournament Title is required.")]
    [MaxLength(35, ErrorMessage = "Tournament Title cannot exceed 35 characters.")]
    public required string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate {  get; set; }
    //{
    //    get => endDate ?? StartTime.AddMonths(3);
    //    set => endDate = value;
    //}
}
