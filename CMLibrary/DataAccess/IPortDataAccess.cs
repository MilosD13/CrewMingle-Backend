using CMLibrary.Models;

namespace CMLibrary.DataAccess
{
    public interface IPortDataAccess
    {
        Task<CrewScheduleResultsModel> GetCrewByPort(string userId, PortScheduleModel port, int pageNumber = 1, int pageSize = 10);
        Task<CrewScheduleResultsModel> GetCrewScheduleByShip(string userId, ShipCrewScheduleModel ship, int pageNumber = 1, int pageSize = 10);
    }
}