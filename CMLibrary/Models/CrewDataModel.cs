

namespace CMLibrary.Models;

public class CrewDataModel
{
    public DateTime LastUpdatedDate { get; set; }
    public Guid CrewId { get; set; } = Guid.NewGuid();
    public string? Email { get; set; } = string.Empty;
    public string? ProfileImage { get; set; } = string.Empty;
    public string? DisplayName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public int RowNum { get; set; } = 1;
}
