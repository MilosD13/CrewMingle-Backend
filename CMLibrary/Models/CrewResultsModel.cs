namespace CMLibrary.Models;

public class CrewResultsModel
{
    public CrewMetaModel Meta { get; set; } = new CrewMetaModel();
    public List<CrewDataModel> Data { get; set; }= new List<CrewDataModel>();
}
