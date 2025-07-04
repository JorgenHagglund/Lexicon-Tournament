namespace Tournament.Core.DTOs;

public record PaginationDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
