CREATE PROCEDURE [dbo].[spUserContract_GetAllByUserId]
    @UserAccountId UNIQUEIDENTIFIER,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate total record count (for that user, not deleted)
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM [dbo].[UserContract]
    WHERE [IsDeleted] <>1
      AND [UserAccountId] = @UserAccountId
      AND [EndDate] >= GETUTCDATE();

    -- Paginated results
    WITH PaginatedResults AS
    (
        SELECT
            [Id],
            [CreatedDate],
            [UserAccountId],
            [ShipId],
            [StartDate],
            [EndDate],
            [IsDeleted],
            ROW_NUMBER() OVER (ORDER BY [CreatedDate] DESC) AS RowNum
        FROM [dbo].[UserContract]
        WHERE [IsDeleted] = 0
          AND [UserAccountId] = @UserAccountId
    )
    SELECT
        @TotalRecords AS TotalRecords,
        CEILING(@TotalRecords * 1.0 / @PageSize) AS TotalPages,
        @PageNumber AS PageNumber,
        @PageSize AS PageSize,
        P.[Id],
        P.[CreatedDate],
        P.[UserAccountId],
        P.[ShipId],
        P.[StartDate],
        P.[EndDate],
        P.[IsDeleted],
        P.RowNum
    FROM PaginatedResults P
    WHERE P.RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1
                       AND @PageNumber * @PageSize;
END