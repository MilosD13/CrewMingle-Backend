namespace CMLibrary.Models;

public class ShipScheduleModel
{
    public DateTime? ArrivalDateTime { get; set; }
    public DateTime? DepartureDateTime { get; set; }
    public int PortId { get; set; }
    public string PortName { get; set; } = string.Empty;
    public string PortCountry { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
