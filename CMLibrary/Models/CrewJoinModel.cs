namespace CMLibrary.Models;

public class CrewJoinModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public Guid? CreatedByCrewId { get; set; }
    public string Status { get; set; } = string.Empty;
}
