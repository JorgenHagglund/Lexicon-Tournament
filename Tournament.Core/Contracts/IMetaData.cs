namespace Tournament.Core.Contracts;

public interface IMetaData
{
    int CurrentPage { get; set; }
    int PageSize { get; set; }
    int TotalItems { get; set; }
    int TotalPages { get; set; }
}