using Tournament.Core.DTOs;
using E = Tournament.Core.Entities;

namespace Tournament.Core.Contracts;

public interface ITournamentRepository
{
    Task<IEnumerable<E.Tournament>> GetTournamentsAsync(TournamentDisplayOptionsDto options, PaginationDto pagination, bool trackChanges = false);
    Task<E.Tournament?> GetTournamentAsync(int id, bool includeGames, bool trackChanges = false);
    void Create(E.Tournament tournament);
    void Update(E.Tournament tournament);
    void Delete(E.Tournament tournament);
    Task<bool> ExistsAsync(int id);
}
