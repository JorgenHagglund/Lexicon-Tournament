using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Services.Contracts;

public interface IGameService : IServiceBase<GameDto, GameToCreateDto, GameUpdateDto, Game>
{
    Task<IEnumerable<GameDto>> GetGamesAsync(GameQueryOptionsDto options, PaginationDto pagination, bool trackChanges = false);
    Task<GameDto?> GetGameAsync(int id, bool trackChanges = false);
}
