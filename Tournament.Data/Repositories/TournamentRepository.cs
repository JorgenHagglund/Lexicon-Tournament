using Microsoft.EntityFrameworkCore;
using Tournament.Core.DTOs;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentRepository(TournamentContext context) : RepositoryBase<Core.Entities.Tournament>(context: context), ITournamentRepository
{
    public async Task<IEnumerable<Core.Entities.Tournament>> GetTournamentsAsync(TournamentDisplayOptionsDto options, bool trackChanges = false)
    {
        IQueryable<Core.Entities.Tournament> query = FindAll(trackChanges);
        query = options.IncludeGames 
            ? query.Include(t => t.Games)
            : query;
        if (!string.IsNullOrEmpty(options.Sort))
        {
            query = options.Sort.ToLowerInvariant() switch
            {
                "title" => options.Reverse
                                        ? query.OrderByDescending(t => t.Title)
                                        : query.OrderBy(t => t.Title),
                "date" => options.Reverse
                                        ? query.OrderByDescending(t => t.StartDate)
                                        : query.OrderBy(t => t.StartDate),
                _ => throw new ArgumentException("Invalid sort option"),
            };
        }
        // Filtering
        if (!string.IsNullOrEmpty(options.Filter))
            query = query.Where(t => t.Title.Contains(options.Filter));

        // Pagination
        if (options.Page.HasValue && options.PageSize.HasValue)
        {
            int skip = (options.Page.Value - 1) * options.PageSize.Value;
            query = query.Skip(skip).Take(options.PageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Core.Entities.Tournament?> GetTournamentAsync(int id, bool includeGames, bool trackChanges = false) =>
        includeGames
        ? await FindByCondition(t => t.Id == id, trackChanges).Include(t => t.Games).FirstOrDefaultAsync()
        : await FindByCondition(t => t.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<bool> ExistsAsync(int id) =>
        await context.Tournaments.AnyAsync(t => t.Id == id);
}
