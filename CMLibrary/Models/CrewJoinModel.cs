using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLibrary.Models;

public class CrewJoinModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public Guid CrewJoinId { get; set; }
    public Guid CrewId { get; set; }
    public string Status { get; set; } = string.Empty;


}
