
namespace CMLibrary.Models;

public class PortScheduleModel
{
    public int PortId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public DateTime DepartureDate { get; set; }
    public TimeSpan DepartureTime { get; set; }
}
