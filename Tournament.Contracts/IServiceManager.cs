namespace Services.Contracts;

public interface IServiceManager
{
    ITournamentService TournamentService { get; }
    IGameService GameService { get; }
}