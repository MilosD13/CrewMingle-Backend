CREATE PROCEDURE [dbo].[spUserContract_Insert]
    @FirebaseId NVARCHAR(255),
    @ShipId INT,
    @StartDate DATETIME2(7),
    @EndDate DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserAccountId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @FirebaseId )
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