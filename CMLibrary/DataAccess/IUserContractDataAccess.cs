using CMLibrary.Models;
namespace CMLibrary.DataAccess;
public interface IUserContractDataAccess
{
    Task<Guid?> CreateContract(UserContractModel model);
    Task<UserContractModel?> GetContractById(Guid contractId);
    Task<List<UserContractModel>> GetAllContracts();
    Task<List<UserContractModel>> GetContractsByUser(Guid userAccountId, int pageNumber = 1, int pageSize = 10);
    Task<bool> UpdateContract(UserContractModel model);
    Task<bool> SoftDeleteContract(Guid contractId);
}