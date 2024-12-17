

namespace CMLibrary.Models;

public class UserAccountRolesModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public bool IsDeleted { get; set; } = false;
}
