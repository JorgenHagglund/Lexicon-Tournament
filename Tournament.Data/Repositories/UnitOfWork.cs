using Microsoft.Extensions.Configuration;
using Tournament.Core.Contracts;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class UnitOfWork(TournamentContext context, IMetaData meta, IConfiguration config) : IUnitOfWork
{
    private ITournamentRepository? _TournamentRepository;
    private IGameRepository? _GameRepository;
    public ITournamentRepository TournamentRepository
    {
        get
        {
            _TournamentRepository ??= new TournamentRepository(context, meta, config);
            return _TournamentRepository;
        }
    }

    public IGameRepository GameRepository
    {
        get
        {
            _GameRepository ??= new GameRepository(context, meta, config);
            return _GameRepository;
        }
    }

    public async Task<int> CompleteAsync() =>
        await context.SaveChangesAsync();
}
