using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournament.Core.Contracts;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetGamesAsync(GameQueryOptionsDto options, PaginationDto pagination, bool trackChanges = false);
    Task<Game?> GetGameAsync(int id, bool trackChanges = false);
    void Create(Game game);
    void Update(Game game);
    void Delete(Game game);
    Task<bool> ExistsAsync(int id);
    int CountGames(int tournamentId);
}
