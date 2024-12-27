CREATE PROCEDURE [dbo].[spUserContract_SoftDelete]
    @ContractId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[UserContract]
    SET [IsDeleted] = 1
    WHERE [Id] = @ContractId;
END
