CREATE PROCEDURE [dbo].[spUserContract_SoftDelete]
    @ContractId UNIQUEIDENTIFIER,
    @FirebaseId NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserAccountId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @FirebaseId )

    UPDATE [dbo].[UserContract] 
    SET [IsDeleted] = 1
    WHERE [Id] = @ContractId
    AND UserAccountId = @UserAccountId;
END