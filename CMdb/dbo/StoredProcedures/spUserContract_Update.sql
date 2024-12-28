CREATE PROCEDURE [dbo].[spUserContract_Update]
    @ContractId      UNIQUEIDENTIFIER,
     @FirebaseId NVARCHAR(255),
    @ShipId          INT = NULL,
    @StartDate       DATETIME2(7) = NULL,
    @EndDate         DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserAccountId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @FirebaseId )

    UPDATE [dbo].[UserContract]
    SET
        [ShipId]        = COALESCE(@ShipId,  [ShipId]),
        [StartDate]     = COALESCE(@StartDate, [StartDate]),
        [EndDate]       = COALESCE(@EndDate,   [EndDate])
    WHERE [Id] = @ContractId
      AND [IsDeleted] = 0
      AND [UserAccountId] = @UserAccountId;
END