using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentContext(DbContextOptions<TournamentContext> options) : DbContext(options)
{
    public DbSet<Core.Entities.Tournament> Tournaments { get; set; } = default!;

    public DbSet<Game> Games { get; set; } = default!;
}
