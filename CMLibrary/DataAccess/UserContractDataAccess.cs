using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class UserContractDataAccess : IUserContractDataAccess
{
    private readonly ISqlDataAccess _sql;
    public UserContractDataAccess(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<Guid?> CreateContract(UserContractModel model, string firebaseId)
    {
        try
        {
            var result = await _sql.LoadData<Guid, dynamic>(
                "dbo.spUserContract_Insert",
                new
                {
                    FirebaseId = firebaseId,
                    ShipId = model.ShipId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                },
                "Default"
            );
            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            // Log or handle error if we need it
            return null;
        }
    }

    public async Task<UserContractModel?> GetContractById(Guid contractId)
    {
        try
        {
            var result = await _sql.LoadData<UserContractModel, dynamic>(
                "dbo.spUserContract_GetById",
                new { ContractId = contractId },
                "Default"
            );

            return result.FirstOrDefault();
        }
        catch
        {
            // Log error
            return null;
        }
    }

    public async Task<List<UserContractModel>> GetContractsByUser(string firebaseId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var result = await _sql.LoadData<UserContractModel, dynamic>(
                "dbo.spUserContract_GetAllByUserId",
                new
                {
                    FirebaseId = firebaseId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                "Default"
            );

            return result.ToList();
        }
        catch
        {
            // Log error
            return new List<UserContractModel>();
        }
    }

    public async Task<bool> UpdateContract(UserContractModel model, string firebaseId)
    {
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spUserContract_Update",
                new
                {
                    ContractId = model.Id,
                    FirebaseId = firebaseId,
                    ShipId = model.ShipId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                },
                "Default"
            );

            return true;
        }
        catch
        {
            // Log error
            return false;
        }
    }

    public async Task<bool> SoftDeleteContract(Guid contractId, string firebaseId)
    {
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spUserContract_SoftDelete",
                new { ContractId = contractId, FirebaseId = firebaseId},
                "Default"
            );

            return true;
        }
        catch
        {
            // Log error
            return false;
        }
    }
}
