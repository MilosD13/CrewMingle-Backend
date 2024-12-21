

using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class CrewDataAccess
{
    private readonly ISqlDataAccess _sql;
    public CrewDataAccess(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    private async Task AddCrew(Guid requesterId, Guid requesteeid)
    {
        await _sql.SaveData<dynamic>(
            "dbo.spCrew_CreateConnectionRequest",
            new
            {
                RequesterId = requesterId,
                Requesteeid = requesteeid
            },
            "Default");

        return;
    }

    private async Task EditCrew(Guid requesterId, Guid requesteeid, string status)
    {
        /*
         * Can Accept, Reject, Block, Cancel crew connections / requests
         * */
        
        await _sql.SaveData<dynamic>(
            "dbo.spCrew_EditConnectionRequest",
            new
            {
                RequesterId = requesterId,
                Requesteeid = requesteeid,
                Status = status
            },
            "Default");

        return;
    }
}
