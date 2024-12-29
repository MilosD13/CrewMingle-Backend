

namespace CMLibrary.Models;

public class CrewItineraryDataModel
{
    public Guid CrewId { get; set; } = Guid.Empty;
    public string? ProfileImage { get; set; } = string.Empty;
    public string? DisplayName { get; set; } = string.Empty;
    public string Cruiseline { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public int PortId { get; set; }
    public string PortName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime ArrivalDate { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public DateTime DepartureDate { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public int OverlapHours { get; set; }

    // ** PAGINATION ** //
    public int TotalRecords { get; set; } = 0;
    public int TotalPages { get; set; } = int.MaxValue;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
    public int RowNum { get; set; } = int.MaxValue;
}
