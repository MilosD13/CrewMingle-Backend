

namespace CMLibrary.Models;

public class CrewShipDataModel
{
    public Guid CrewId { get; set; } = Guid.NewGuid();
    public string? ProfileImage { get; set; } = string.Empty;
    public string? DisplayName { get; set; } = string.Empty;
    public string Cruiseline { get; set; } = string.Empty;
    public string ShipName {  get; set; } = string.Empty;
    public int ShipId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // ** PAGINATION ** //
    public int TotalRecords { get; set; } = 0;
    public int TotalPages { get; set; } = int.MaxValue;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
    public int RowNum { get; set; } = int.MaxValue;
}
