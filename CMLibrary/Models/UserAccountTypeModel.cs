namespace CMLibrary.Models;

public class UserAccountTypeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate = DateTime.UtcNow;
    public string AccountType { get; set; } = string.Empty;
}
