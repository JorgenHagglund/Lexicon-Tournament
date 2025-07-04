using Services.Contracts;

namespace Tournament.Services;

public class ServiceManager(Lazy<ITournamentService> tournamentService, Lazy<IGameService> gameService) : IServiceManager
{
    public ITournamentService TournamentService => tournamentService.Value;
    public IGameService GameService => gameService.Value;
}
