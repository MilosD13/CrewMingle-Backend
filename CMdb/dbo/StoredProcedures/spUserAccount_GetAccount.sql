CREATE PROCEDURE [dbo].[spUserAccount_GetAccount]
	@UserId VARCHAR(255),
	@Email VARCHAR(255)
AS


BEGIN


	SELECT u.Id, u.CreatedDate, u.UserId, Email, AccountTypeId, t.AccountType, r.Roles [RoleString], u.IsDeleted, u.ProfileImage

    FROM 
    [dbo].[UserAccount] u

    LEFT JOIN UserAccountType t on t.Id = u.AccountTypeId

    LEFT JOIN (SELECT STRING_AGG(ur.Role, ', ') AS Roles, r.UserId FROM dbo.UserAccountRoles r
                LEFT JOIN dbo.UserRoles ur on  ur.Id = r.RoleId
                GROUP BY  r.UserId) r on r.UserId = u.Id

    WHERE u.UserId = @UserId AND Email = @Email
END

