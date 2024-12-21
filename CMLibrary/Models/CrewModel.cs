

namespace CMLibrary.Models;

public class CrewModel
{
    public DateTime LastUpdatedDate { get; set; }
    public Guid CrewId { get; set; } = Guid.NewGuid();
    public string? Email { get; set; } = string.Empty;
    public string? ProfileImage { get; set; } = string.Empty;
    public string? DisplayName { get; set; } = string.Empty;

    public string Status {  get; set; } = string.Empty;

    // ** PAGINATION ** //
    public int TotalRecords { get; set; } = 0;
    public int TotalPages { get; set; } = int.MaxValue;
    public int CurrentPage { get; set; } = 1;
    public int PageSize {  get; set; } = int.MaxValue;
    public int RowNum { get; set; } = int.MaxValue;
}
