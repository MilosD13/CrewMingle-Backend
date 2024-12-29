namespace CMLibrary.Models;

public class ShipItineraryFlatModel
{
    public string Cruiseline { get; set; } = string.Empty;
    public string Ship { get; set; } = string.Empty;
    public int ShipId { get; set; }
    public int PortId { get; set; }
    public string PortName { get; set; } = string.Empty;
    public string PortCountry { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public DateTime? ArrivalDate { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public DateTime? DepartureDate { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public int? RowNum { get; set; }
}
