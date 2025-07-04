using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tournament.Core.Contracts;
using Tournament.Core.DTOs;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentRepository(TournamentContext context, IMetaData meta, IConfiguration config) : RepositoryBase<Core.Entities.Tournament>(context: context), ITournamentRepository
{
    // Detta krävs för att "släcka" en varning om tvetydighet
    private readonly TournamentContext context = context;
    private readonly IMetaData meta = meta;
    private readonly IConfiguration config = config;

    public async Task<IEnumerable<Core.Entities.Tournament>> GetTournamentsAsync(TournamentDisplayOptionsDto options, PaginationDto pagination, bool trackChanges = false)
    {
        IQueryable<Core.Entities.Tournament> query = FindAll(trackChanges);

        var limits = config.GetSection("Limits");
        int maxPageSize = Convert.ToInt32(limits["MaxPageSize"]);

        meta.TotalItems = query.Count();
        meta.CurrentPage = pagination.Page;
        meta.PageSize = Math.Min(pagination.PageSize, maxPageSize);
        meta.TotalPages = (int)Math.Ceiling((double)meta.TotalItems / meta.PageSize);

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
//        if (pagination.Page.HasValue && options.PageSize.HasValue)
//        {
            int skip = (pagination.Page - 1) * meta.PageSize;
            query = query.Skip(skip).Take(meta.PageSize);
//        }

        return await query.ToListAsync();
    }

    public async Task<Core.Entities.Tournament?> GetTournamentAsync(int id, bool includeGames, bool trackChanges = false) =>
        includeGames
        ? await FindByCondition(t => t.Id == id, trackChanges).Include(t => t.Games).FirstOrDefaultAsync()
        : await FindByCondition(t => t.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<bool> ExistsAsync(int id) =>
        await context.Tournaments.AnyAsync(t => t.Id == id);
}
