using CMLibrary.Models;

namespace CMLibrary.DataAccess;

public class UserAccountData : IUserAccountData
{
    private readonly ISqlDataAccess _sql;
    public UserAccountData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<UserAccountModel>? VerifyUserAccount(UserFirebaseModel user)
    {
        // check if user exists in db
        var userAccount = await _sql.LoadData<UserAccountModel, dynamic>("dbo.spUserAccount_GetAccount", new {user.UserId, user.Email }, "Default")?? null;

        if (userAccount.Count() == 0)
        {
            // register
            await CreateUserAccount(user);
            userAccount = await _sql.LoadData<UserAccountModel, dynamic>("dbo.spUserAccount_GetAccount", new { user.UserId, user.Email  }, "Default");
        }
        if (userAccount.FirstOrDefault().IsDeleted)
        {
            // deleted account
            return null;
        }

        var account = userAccount.FirstOrDefault();
        //return data
        return account;

    }

    private async Task CreateUserAccount(UserFirebaseModel user)
    {
        await _sql.SaveData<dynamic>(
            "dbo.spUserAccount_CreateUserAccount",
            new
            {
                user.UserId,
                user.Email
            },
            "Default");

        return;
    }
}
