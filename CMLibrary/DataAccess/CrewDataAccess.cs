

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

    public async Task<IEnumerable<CrewModel>> GetActiveCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew  = await _sql.LoadData<CrewModel, dynamic>(
            "dbo.spCrew_GetCrewList",
            new
            {
                UserId = userId,
                PageNUmber = pageNumber,
                PageSize = pageSize
            },
            "Default");
        
        return crew;
    }
    public async Task<CrewModel?> GetSingleActiveCrew(string userId)
    {
        var crew = await _sql.LoadData<CrewModel, dynamic>(
            "dbo.spCrew_GetSingleCrew",
            new
            {
                UserId = userId
            },
            "Default");

        return crew.FirstOrDefault();
    }
    public async Task<IEnumerable<CrewModel>> GetPendingCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewModel, dynamic>(
            "dbo.spCrew_GetCrewPendingRequested",
            new
            {
                UserId = userId,
                PageNUmber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        return crew;
    }
    public async Task<IEnumerable<CrewModel>> GetBlockedCrew(string userId, int pageNumber = 1, int pageSize = 10)
    {
        var crew = await _sql.LoadData<CrewModel, dynamic>(
            "dbo.spCrew_GetCrewBlockList",
            new
            {
                UserId = userId,
                PageNUmber = pageNumber,
                PageSize = pageSize
            },
            "Default");

        return crew;
    }
}
