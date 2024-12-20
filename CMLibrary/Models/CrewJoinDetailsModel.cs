using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLibrary.Models;

public class CrewJoinDetailsModel
{
	public Guid  Id { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime LastUpdatedDate { get; set;}
	public Guid CreatedByCrewId { get; set; }
	public bool IsDeleted { get; set; } = false;
	public bool IsBlocked { get; set; } = false;

	public List<CrewJoinDetailsModel> Crew {  get; set; } = new List<CrewJoinDetailsModel>();

}
