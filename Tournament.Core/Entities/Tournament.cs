namespace Tournament.Core.Entities;

public class Tournament
{
    private DateTime? endDate = null;

    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate
    { 
        get => endDate ?? StartDate.AddMonths(3);
        set => endDate = value; 
    }

    public ICollection<Game> Games { get; set; }
}
