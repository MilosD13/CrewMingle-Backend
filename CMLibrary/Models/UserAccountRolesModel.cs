using Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLibrary.Models;

public class UserAccountRolesModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public bool IsDeleted { get; set; } = false;
}
