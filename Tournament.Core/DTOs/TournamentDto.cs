using Tournament.Core.Entities;

namespace Tournament.Core.DTOs;

public record TournamentDto
{
    //private DateTime? endDate;
    public int Id { get; init; }
    public required string Title { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate {  get; init; }
    //{ 
    //    get => endDate ?? StartTime.AddMonths(3); 
    //    init => endDate = value; 
    //}

    public ICollection<GameDto> Games { get; init; }
}
