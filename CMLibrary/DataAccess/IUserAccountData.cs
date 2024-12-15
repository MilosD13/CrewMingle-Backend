using CMLibrary.Models;

namespace CMLibrary.DataAccess
{
    public interface IUserAccountData
    {
       Task<UserAccountModel>? VerifyUserAccount(UserFirebaseModel user);
    }
}