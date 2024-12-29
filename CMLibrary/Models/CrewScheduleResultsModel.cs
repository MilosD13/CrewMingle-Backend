
namespace CMLibrary.Models;

public class CrewScheduleResultsModel
{
    public CrewMetaModel Meta { get; set; } = new CrewMetaModel();
    public List<CrewScheduleModel> Data { get; set; } = new List<CrewScheduleModel>();
}
