namespace Tournament.Core.DTOs;

public record GameDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public int TournamentId { get; init; }
}
