namespace CMLibrary.Models;
public class UserContractModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid UserAccountId { get; set; }
    public int? ShipId { get; set; }          // optional
    public DateTime? StartDate { get; set; }  // optional
    public DateTime? EndDate { get; set; }    // optional
    public bool IsDeleted { get; set; }
}