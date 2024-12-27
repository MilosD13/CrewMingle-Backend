CREATE PROCEDURE [dbo].[spUserContract_Insert]
    @UserAccountId UNIQUEIDENTIFIER,
    @ShipId INT,
    @StartDate DATETIME2(7),
    @EndDate DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [dbo].[UserContract]
    (
        [Id],
        [CreatedDate],
        [UserAccountId],
        [ShipId],
        [StartDate],
        [EndDate],
        [IsDeleted]
    )
    VALUES
    (
        @NewId,
        GETUTCDATE(),
        @UserAccountId,
        @ShipId,
        @StartDate,
        @EndDate,
        0
    );

    SELECT @NewId AS NewContractId;
END