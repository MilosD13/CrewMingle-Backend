namespace CMLibrary.Models;
public class ShipModel
{
    public int ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public int CruiselineId { get; set; }
    public string CruiselineName { get; set; } = string.Empty;

    // The itinerary or schedule
    public List<ShipScheduleModel> ShipSchedules { get; set; } = new List<ShipScheduleModel>();
}
