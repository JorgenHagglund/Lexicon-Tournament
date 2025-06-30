using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.DTOs;

public record TournamentDisplayOptionsDto
{
    public bool IncludeGames { get; init; } = false;
    public string? Sort { get; init; }
    public bool Reverse { get; init; } = false;
    public string? Filter { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}
