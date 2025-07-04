using Tournament.Core.DTOs;
using E= Tournament.Core.Entities;

namespace Services.Contracts;

public interface ITournamentService : IServiceBase<TournamentDto, TournamentDto, TournamentUpdateDto, E.Tournament>
{
    // Define methods for the tournament service here
    // For example:
    Task<IEnumerable<TournamentDto>> GetTournamentsAsync(TournamentDisplayOptionsDto options, PaginationDto pagination, bool trackChanges = false);
    Task<TournamentDto?> GetTournamentAsync(int id, bool includeGames = false, bool trackChanges = false);
}
