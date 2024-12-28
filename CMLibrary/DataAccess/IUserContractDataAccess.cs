using CMLibrary.Models;
namespace CMLibrary.DataAccess;
public interface IUserContractDataAccess
{
    Task<Guid?> CreateContract(UserContractModel model, string firebaseId);
    Task<UserContractModel?> GetContractById(Guid contractId);
    Task<List<UserContractModel>> GetContractsByUser( string firebaseId, int pageNumber = 1, int pageSize = 10);
    Task<bool> UpdateContract(UserContractModel model, string firebaseId);
    Task<bool> SoftDeleteContract(Guid contractId,string firebaseId);
}