using Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLibrary.Models;

public class UserAccountTypeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate = DateTime.UtcNow;
    public string AccountType { get; set; } = string.Empty;

}
