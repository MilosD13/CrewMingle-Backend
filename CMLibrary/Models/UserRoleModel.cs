

namespace CMLibrary.Models;

public class UserRoleModel
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate = DateTime.UtcNow;
    public string Role { get; set; } = string.Empty;
}
