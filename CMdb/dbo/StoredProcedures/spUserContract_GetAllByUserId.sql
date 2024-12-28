CREATE PROCEDURE [dbo].[spUserContract_GetAllByUserId]
    @FirebaseId VARCHAR(255),
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate total record count (for that user, not deleted)
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM [dbo].[UserContract] uc
    LEFT JOIN [dbo].[UserAccount] u on u.Id = uc.UserAccountId
    WHERE uc.[IsDeleted] <>1
      AND u.UserId = @FirebaseId
      AND [EndDate] >= GETUTCDATE();

    -- Paginated results
    WITH PaginatedResults AS
    (
        SELECT
            uc.[Id],
            uc.[CreatedDate],
            [UserAccountId],
            cl.[Cruiseline],
            cs.[Ship][ShipName],
            [ShipId],
            [StartDate],
            [EndDate],
            uc.[IsDeleted],
            ROW_NUMBER() OVER (ORDER BY uc.[CreatedDate] DESC) AS RowNum
        FROM [dbo].[UserContract] uc 
        left join [dbo].CruiseShip cs on uc.ShipId = cs.Id
        left join [dbo].Cruiseline cl on cl.Id = cs.CruiselineId
        left join [dbo].UserAccount u on u.Id = uc.UserAccountId

        WHERE uc.[IsDeleted] <> 1
          AND u.UserId = @FirebaseId
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
        p.[ShipName],
        p.[Cruiseline],
        P.[StartDate],
        P.[EndDate],
        P.[IsDeleted],
        P.RowNum
    FROM PaginatedResults P
    WHERE P.RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1
                       AND @PageNumber * @PageSize;
END
