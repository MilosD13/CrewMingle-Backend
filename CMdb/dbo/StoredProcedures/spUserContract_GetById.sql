CREATE PROCEDURE [dbo].[spUserContract_GetById]
    @ContractId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        uc.[Id],
        [CreatedDate],
        [UserAccountId],
        cl.[Cruiseline],
        cs.[Ship][ShipName],
        [ShipId],
        [StartDate],
        [EndDate],
        [IsDeleted]
    FROM [dbo].[UserContract] uc 
    left join [dbo].CruiseShip cs on uc.ShipId = cs.Id
    left join [dbo].Cruiseline cl on cl.Id = cs.CruiselineId

    WHERE uc.[Id] = @ContractId 
      AND [EndDate] >= GETUTCDATE()
      AND [IsDeleted] <>1;   -- return only if not deleted (optional check)

END