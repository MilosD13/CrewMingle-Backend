

using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class CrewDataAccess : ICrewDataAccess
{
    private readonly ISqlDataAccess _sql;
    public CrewDataAccess(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<bool> AddCrew(string requesterId, Guid requesteeid)
    {
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spCrew_CreateConnectionRequest",
                new
                {
                    RequesterId = requesterId,
                    Requesteeid = requesteeid
                },
                "Default");

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    public async Task<bool> EditCrew(string userId, CrewEditModel crew)
    {
        /*
         * Can Accept, Reject, Block, Cancel crew connections / requests
         * 
         * */
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spCrew_EditConnectionRequest",
                new
                {
                    UserId = userId,
                    crew.CrewId,
                    crew.Status 
                },
                "Default");

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<CrewResultsModel> GetActiveCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew  = await _sql.LoadData<CrewDatabaseModel, dynamic>(
            "dbo.spCrew_GetCrewList",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        CrewResultsModel results = GetReturnResults(crew.ToList());

        return results;
    }
    public async Task<CrewResultsModel> GetSingleActiveCrew(string userId, Guid crewId)
    {
        var crew = await _sql.LoadData<CrewDatabaseModel, dynamic>(
            "dbo.spCrew_GetSingleCrew",
            new
            {
                UserId = userId,
                CrewId = crewId
            },
            "Default");

        CrewResultsModel results = GetReturnResults(crew.ToList());

        return results;
    }
    public async Task<CrewResultsModel> GetPendingCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewDatabaseModel, dynamic>(
            "dbo.spCrew_GetCrewPendingRequested",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        CrewResultsModel results = GetReturnResults(crew.ToList());

        return results;
    }
    public async Task<CrewResultsModel> GetBlockedCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewDatabaseModel, dynamic>(
            "dbo.spCrew_GetCrewBlockList",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        CrewResultsModel results = GetReturnResults(crew.ToList());

        return results;
    }


    public async Task<CrewShipResultsModel> GetCrewOnShip(string userId, int shipId, DateTime startDate , DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewShipDataModel, dynamic>(
            "dbo.spCrew_GetCrewByShip",
            new
            {
                UserId = userId,
                ShipId = shipId,
                StartDate = startDate,
                EndDate = endDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        CrewShipResultsModel results = GetShipReturnResults(crew.ToList());

        return results;
    }

    public async Task<CrewShipResultsModel> GetCrewShips(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewShipDataModel, dynamic>(
            "dbo.spCrew_GetCrewShips",
            new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        CrewShipResultsModel results = GetShipReturnResults(crew.ToList());

        return results;
    }

    private CrewResultsModel GetReturnResults(List<CrewDatabaseModel> dataResults)
    {
        List<CrewDataModel> dataModel = new List<CrewDataModel>();

        foreach (var crew_data in dataResults)
        {
            CrewDataModel crewDataModel = new CrewDataModel
            {
                LastUpdatedDate = crew_data.LastUpdatedDate,
                CrewId = crew_data.CrewId,
                Email = crew_data.Email,
                DisplayName = crew_data.DisplayName,
                ProfileImage = crew_data.ProfileImage,
                Status = crew_data.Status,
                RowNum = crew_data.RowNum
            };
            dataModel.Add(crewDataModel);
        }

        if (dataResults.Count() > 0)
        {
            var firstRecord = dataResults.FirstOrDefault();

            CrewMetaModel meta = new CrewMetaModel
            {
                PageNumber = firstRecord.PageNumber,
                TotalRecords = firstRecord.TotalRecords,
                TotalPages = firstRecord.TotalPages,
                PageSize  = firstRecord.PageSize
            };

            CrewResultsModel results = new CrewResultsModel
            {
                Meta = meta,
                Data = dataModel
            };

            return results;
        }
        else
        {
            CrewMetaModel meta = new CrewMetaModel
            {
                PageNumber = 1,
                TotalRecords = 0,
                TotalPages = 0,
                PageSize = 0
            };
            CrewResultsModel results = new CrewResultsModel
            {
                Meta = meta,
                Data = dataModel
            };

            return results;
        }
    }

    private CrewShipResultsModel GetShipReturnResults(List<CrewShipDataModel> dataResults)
    {
        List<CrewShipModel> dataModel = new List<CrewShipModel>();

        foreach (var crew_data in dataResults)
        {
            CrewShipModel crewDataModel = new CrewShipModel
            {
                CrewId = crew_data.CrewId,
                DisplayName = crew_data.DisplayName,
                ProfileImage = crew_data.ProfileImage,
                RowNum = crew_data.RowNum,
                Cruiseline = crew_data.Cruiseline,
                ShipName = crew_data.ShipName,
                StartDate = crew_data.StartDate,
                EndDate = crew_data.EndDate
            };
            dataModel.Add(crewDataModel);

}

        if (dataResults.Count() > 0)
        {
            var firstRecord = dataResults.FirstOrDefault();

            CrewMetaModel meta = new CrewMetaModel
            {
                PageNumber = firstRecord.PageNumber,
                TotalRecords = firstRecord.TotalRecords,
                TotalPages = firstRecord.TotalPages,
                PageSize = firstRecord.PageSize
            };

            CrewShipResultsModel results = new CrewShipResultsModel
            {
                Meta = meta,
                Data = dataModel
            };

            return results;
        }
        else
        {
            CrewMetaModel meta = new CrewMetaModel
            {
                PageNumber = 1,
                TotalRecords = 0,
                TotalPages = 0,
                PageSize = 0
            };
            CrewShipResultsModel results = new CrewShipResultsModel
            {
                Meta = meta,
                Data = dataModel
            };

            return results;
        }
    }
}