CREATE PROCEDURE [dbo].[spUserContract_Update]
    @ContractId      UNIQUEIDENTIFIER,
    @UserAccountId   UNIQUEIDENTIFIER,
    @ShipId          INT = NULL,
    @StartDate       DATETIME2(7) = NULL,
    @EndDate         DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[UserContract]
    SET
        [UserAccountId] = @UserAccountId,  -- always update userAccountId if you need to
        [ShipId]        = COALESCE(@ShipId,  [ShipId]),
        [StartDate]     = COALESCE(@StartDate, [StartDate]),
        [EndDate]       = COALESCE(@EndDate,   [EndDate])
    WHERE [Id] = @ContractId
      AND [IsDeleted] = 0;
END