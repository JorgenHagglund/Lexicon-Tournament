using Tournament.Core.Contracts;

namespace Tournament.Core.Entities;

public class MetaData : IMetaData
{
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
}
