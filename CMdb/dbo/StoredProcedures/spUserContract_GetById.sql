CREATE PROCEDURE [dbo].[spUserContract_GetById]
    @ContractId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [Id],
        [CreatedDate],
        [UserAccountId],
        [ShipId],
        [StartDate],
        [EndDate],
        [IsDeleted]
    FROM [dbo].[UserContract]
    WHERE [Id] = @ContractId 
      AND [EndDate] >= GETUTCDATE()
      AND [IsDeleted] <>1;   -- return only if not deleted (optional check)

END