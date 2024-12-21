using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public interface ICrewDataAccess
{
    Task<bool> AddCrew(string requesterId, Guid requesteeid);
    Task<bool> EditCrew(string userId, CrewEditModel crew);
}
