using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class UnitOfWork(TournamentContext context) : IUnitOfWork
{
    private ITournamentRepository? _TournamentRepository;
    private IGameRepository? _GameRepository;
    public ITournamentRepository TournamentRepository
    {
        get
        {
            _TournamentRepository ??= new TournamentRepository(context);
            return _TournamentRepository;
        }
    }

    public IGameRepository GameRepository
    {
        get
        {
            _GameRepository ??= new GameRepository(context);
            return _GameRepository;
        }
    }

    public async Task<int> CompleteAsync() =>
        await context.SaveChangesAsync();
}
