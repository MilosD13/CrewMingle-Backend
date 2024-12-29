
namespace CMLibrary.Models;

public class CrewScheduleModel
{
    public Guid CrewId { get; set; } = Guid.NewGuid();
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
    public int RowNum { get; set; } = int.MaxValue;
}
