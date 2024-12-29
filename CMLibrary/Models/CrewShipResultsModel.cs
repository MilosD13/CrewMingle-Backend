namespace CMLibrary.Models;

public class CrewShipResultsModel
{
    public CrewMetaModel Meta { get; set; } = new CrewMetaModel();
    public List<CrewShipModel> Data { get; set; } = new List<CrewShipModel>();
}
