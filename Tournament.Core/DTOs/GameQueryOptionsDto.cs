namespace Tournament.Core.DTOs;

public record GameQueryOptionsDto
{
    public string? Title { get; init; }
    public bool ExactMatch { get; init; } = false;
}