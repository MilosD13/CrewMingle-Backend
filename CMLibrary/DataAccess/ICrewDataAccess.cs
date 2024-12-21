using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public interface ICrewDataAccess
{
    Task<bool> AddCrew(string requesterId, Guid requesteeid);
    Task<bool> EditCrew(string userId, CrewEditModel crew);
    Task<IEnumerable<CrewModel>> GetActiveCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<IEnumerable<CrewModel>> GetBlockedCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<IEnumerable<CrewModel>> GetPendingCrew(string userId, int pageNumber = 1, int pageSize = 10);
    Task<CrewModel?> GetSingleActiveCrew(string userId);
}
