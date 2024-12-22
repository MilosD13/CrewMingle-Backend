

namespace CMLibrary.Models;

public class CrewMetaModel
{
    public int TotalRecords { get; set; } = 0;
    public int TotalPages { get; set; } = int.MaxValue;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
    
}
