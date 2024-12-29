namespace CMLibrary.Models;

public class CrewShipModel
{
    public Guid CrewId { get; set; } = Guid.NewGuid();
    public string? ProfileImage { get; set; } = string.Empty;
    public string? DisplayName { get; set; } = string.Empty;
    public string Cruiseline { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public int ShipId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RowNum { get; set; } = int.MaxValue;
}
