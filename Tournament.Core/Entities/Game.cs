namespace Tournament.Core.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; }
}
