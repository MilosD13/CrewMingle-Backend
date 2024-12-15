CREATE PROCEDURE [dbo].[spUserAccount_CreateUserAccount]
	@UserId VARCHAR(255),
	@Email VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	
   
    DECLARE @NewUserId UNIQUEIDENTIFIER
    DECLARE @RoleId UNIQUEIDENTIFIER
    -- Declare or set account type 'User'
	DECLARE @AccountTypeId UNIQUEIDENTIFIER

	IF NOT EXISTS (SELECT 1 FROM UserAccountType WHERE AccountType = 'User')
	BEGIN
		INSERT INTO UserAccountType (AccountType)
		VALUES ('User')
	END

	set @AccountTypeId = (SELECT Id FROM UserAccountType WHERE AccountType = 'User')

	-------------------

	IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE [Role] = 'User')
	BEGIN
		INSERT INTO UserRoles ([Role])
		VALUES ('User')
	END

	set @RoleId = (SELECT Id FROM UserRoles WHERE [Role] = 'User')

	------------------

	INSERT INTO UserAccount (Email, UserId, AccountTypeId)
	VALUES (@Email, @UserId, @AccountTypeId)

    SET @NewUserId = (SELECT Id FROM UserAccount WHERE UserId = @UserId);

    INSERT INTO UserAccountRoles(UserId, RoleId)
    VALUES (@NewUserId, @RoleId);

END