using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetGamesAsync(bool trackChanges = false);
    Task<Game?> GetGameAsync(int id, bool trackChanges = false);
    Task<IEnumerable<Game?>> GetByTitleAsync(string title, bool exactMatch = false, bool trackChanges = false);
    void Create(Game game);
    void Update(Game game);
    void Delete(Game game);
    Task<bool> ExistsAsync(int id);
}
