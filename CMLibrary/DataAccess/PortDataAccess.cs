
using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class PortDataAccess : IPortDataAccess
{
    private readonly ISqlDataAccess _sql;
    public PortDataAccess(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<CrewScheduleResultsModel> GetCrewByPort(string userId,
                                                      PortScheduleModel port,
                                                      int pageNumber = 1,
                                                      int pageSize = 10)
    {
        var schedule = await _sql.LoadData<CrewItineraryDataModel, dynamic>(
            "dbo.spPort_GetCrewAtPort",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                port.ArrivalDate,
                port.ArrivalTime,
                port.DepartureDate,
                port.DepartureTime,
                port.PortId
            },
            "Default");

        CrewScheduleResultsModel results = GetReturnResults(schedule.ToList());

        return results;
    }

    public async Task<CrewScheduleResultsModel> GetCrewScheduleByShip(string userId,
                                                      ShipCrewScheduleModel ship,
                                                      int pageNumber = 1,
                                                      int pageSize = 10)
    {
        var schedule = await _sql.LoadData<CrewItineraryDataModel, dynamic>(
            "dbo.spPort_GetCrewAcrossScheduleByShip",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                ship.ArrivalDate,                
                ship.DepartureDate,
                ship.ShipId                
            },
            "Default");

        CrewScheduleResultsModel results = GetReturnResults(schedule.ToList());

        return results;
    }

    private CrewScheduleResultsModel GetReturnResults(List<CrewItineraryDataModel> dataResults)
    {
        List<CrewScheduleModel> dataModel = new List<CrewScheduleModel>();

        foreach (var crew_data in dataResults)
        {
            CrewScheduleModel crewDataModel = new CrewScheduleModel
            {
                CrewId = crew_data.CrewId,
                DisplayName = crew_data.DisplayName,
                ProfileImage = crew_data.ProfileImage,
                RowNum = crew_data.RowNum,
                Cruiseline = crew_data.Cruiseline,
                ShipName = crew_data.ShipName,
                PortId = crew_data.PortId,
                PortName = crew_data.PortName,
                Country = crew_data.Country,
                Latitude = crew_data.Latitude,
                Longitude = crew_data.Longitude,
                ArrivalDate = crew_data.ArrivalDate,
                ArrivalTime = crew_data.ArrivalTime,
                DepartureDate = crew_data.DepartureDate,
                DepartureTime = crew_data.DepartureTime,
                OverlapHours = crew_data.OverlapHours
            };
            dataModel.Add(crewDataModel);
        }
        var firstRecord = dataResults.FirstOrDefault();
        CrewMetaModel meta = new CrewMetaModel
        {
            PageNumber = firstRecord.PageNumber,
            TotalRecords = firstRecord.TotalRecords,
            TotalPages = firstRecord.TotalPages,
            PageSize = firstRecord.PageSize
        };

        CrewScheduleResultsModel results = new CrewScheduleResultsModel
        {
            Meta = meta,
            Data = dataModel
        };

        return results;

    }
}
