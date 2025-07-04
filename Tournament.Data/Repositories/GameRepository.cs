using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tournament.Core.Contracts;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository(TournamentContext context, IMetaData meta, IConfiguration config) : RepositoryBase<Game>(context), IGameRepository
{
    public async Task<IEnumerable<Game>> GetGamesAsync(GameQueryOptionsDto options, PaginationDto pagination, bool trackChanges = false)
    {
        var query = FindAll(trackChanges);

        var limits = config.GetSection("Limits");
        int maxPageSize = Convert.ToInt32(limits["MaxPageSize"]);

        meta.CurrentPage = pagination.Page;
        meta.PageSize = Math.Min(pagination.PageSize, maxPageSize);
        meta.TotalItems = query.Count();
        meta.TotalPages = (int)Math.Ceiling((double)meta.TotalItems / meta.PageSize);

        // Filtering
        if (!string.IsNullOrEmpty(options.Title))
            if (options.ExactMatch)
                query = query.Where(g => g.Title.Equals(options.Title));
            else
                query = query.Where(g => g.Title.Contains(options.Title));
        // Pagination
        if (pagination != null)
        {
            int skip = (pagination.Page - 1) * meta.PageSize;
            query = query.Skip(skip).Take(meta.PageSize);
        }
        return await query
            .OrderBy(g => g.StartTime)
            .ToListAsync();
    }

    public async Task<Game?> GetGameAsync(int id, bool trackChanges = false) =>
        await FindByCondition(g => g.Id == id, trackChanges).SingleOrDefaultAsync();

    public async Task<bool> ExistsAsync(int id) =>
        await FindByCondition(g => g.Id == id).AnyAsync();

    public int CountGames(int tournamentId) =>
        FindByCondition(g => g.TournamentId == tournamentId).Count();
}
