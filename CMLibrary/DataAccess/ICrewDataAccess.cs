using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public interface ICrewDataAccess
{
    Task<bool> AddCrew(string requesterId, Guid requesteeid);
    Task<bool> EditCrew(string userId, CrewEditModel crew);
    Task<CrewResultsModel> GetActiveCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<CrewResultsModel> GetBlockedCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<CrewResultsModel> GetPendingCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<CrewResultsModel?> GetSingleActiveCrew(string userId, Guid crewId);
}
