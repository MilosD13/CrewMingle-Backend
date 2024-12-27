using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class UserContractDataAccess : IUserContractDataAccess
{
    private readonly ISqlDataAccess _sql;
    public UserContractDataAccess(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<Guid?> CreateContract(UserContractModel model)
    {
        try
        {
            var result = await _sql.LoadData<Guid, dynamic>(
                "dbo.spUserContract_Insert",
                new
                {
                    UserAccountId = model.UserAccountId,
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

    public async Task<List<UserContractModel>> GetAllContracts()
    {
        try
        {
            var result = await _sql.LoadData<UserContractModel, dynamic>(
                "dbo.spUserContract_GetAll",
                new { },
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

    public async Task<List<UserContractModel>> GetContractsByUser(Guid userAccountId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var result = await _sql.LoadData<UserContractModel, dynamic>(
                "dbo.spUserContract_GetAllByUserId",
                new
                {
                    UserAccountId = userAccountId,
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

    public async Task<bool> UpdateContract(UserContractModel model)
    {
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spUserContract_Update",
                new
                {
                    ContractId = model.Id,
                    UserAccountId = model.UserAccountId,
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

    public async Task<bool> SoftDeleteContract(Guid contractId)
    {
        try
        {
            await _sql.SaveData<dynamic>(
                "dbo.spUserContract_SoftDelete",
                new { ContractId = contractId },
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
