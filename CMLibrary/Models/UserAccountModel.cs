namespace CMLibrary.Models;

public class UserAccountModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfileImage {  get; set; } = string.Empty;

    public Guid AccountTypeId { get; set; } = Guid.Empty;
    public string AccountType { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }=false;
    public List<UserAccountRolesModel>? Roles { get; set; } = new List<UserAccountRolesModel>();
    public string? RoleString {  get; set; } = string.Empty;
}
