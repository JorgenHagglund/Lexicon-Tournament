using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository(TournamentContext context) : RepositoryBase<Game>(context), IGameRepository
{
    public async Task<IEnumerable<Game>> GetGamesAsync(bool trackChanges = false) =>
        await FindAll(trackChanges)
            .OrderBy(g => g.StartTime)
            .ToListAsync();

    public async Task<Game?> GetGameAsync(int id, bool trackChanges = false) =>
        await FindByCondition(g => g.Id == id, trackChanges).SingleOrDefaultAsync();

    public async Task<IEnumerable<Game?>> GetByTitleAsync(string title, bool exactMatch = false, bool trackChanges = false) =>
        await FindByCondition(g => 
            exactMatch ? g.Title.Equals(title) : g.Title.Contains(title),
            trackChanges)
        .ToListAsync();
    public async Task<bool> ExistsAsync(int id) =>
        await FindByCondition(g => g.Id == id).AnyAsync();
}
