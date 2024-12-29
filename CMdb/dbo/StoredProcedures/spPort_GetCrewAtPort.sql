CREATE PROCEDURE [dbo].[spPort_GetCrewAtPort]
    @UserId NVARCHAR(255),
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @PortId INT,
    @ArrivalDate DATE,
    @ArrivalTime TIME,
    @DepartureDate DATE,
    @DepartureTime TIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserDbId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @UserId);

    -- CTE for Ordered Itinerary for PortId
    WITH OrderedItinerary AS (
        SELECT 
            si.ShipId,
            si.PortId,
            si.PortDate,
            si.PortTime,
            si.IsArrival,
            ROW_NUMBER() OVER (PARTITION BY si.ShipId, si.PortId ORDER BY si.PortDate, si.PortTime) AS RowNum
        FROM [dbo].[ShipItinerary] si
        WHERE si.PortId = @PortId
    )
    -- CTE for Paginated Results
    , PaginatedResults AS (
        SELECT 
            ROW_NUMBER() OVER (ORDER BY si1.PortDate, si1.PortTime) AS RowNum,
            cl.Cruiseline,
            cs.Ship [ShipName],
            si1.ShipId,
            p.Id AS PortId,
            p.PortName,
            c.Country,
            p.Latitude,
            p.Longitude,
            si1.PortDate AS ArrivalDate,
            si1.PortTime AS ArrivalTime,
            si2.PortDate AS DepartureDate,
            si2.PortTime AS DepartureTime,
            DATEDIFF(
                MINUTE, 
                CASE 
                    WHEN si1.PortDate = @ArrivalDate AND si1.PortTime < @ArrivalTime 
                        THEN CAST(@ArrivalDate AS DATETIME) + CAST(@ArrivalTime AS DATETIME)
                    ELSE CAST(si1.PortDate AS DATETIME) + CAST(si1.PortTime AS DATETIME)
                END,
                CASE 
                    WHEN si2.PortDate = @DepartureDate AND si2.PortTime > @DepartureTime 
                        THEN CAST(@DepartureDate AS DATETIME) + CAST(@DepartureTime AS DATETIME)
                    ELSE CAST(si2.PortDate AS DATETIME) + CAST(si2.PortTime AS DATETIME)
                END
            ) / 60.0 AS OverlapHours,
            uc.UserAccountId [CrewId],
            uc.DisplayName,
            uc.ProfileImage
        FROM OrderedItinerary si1
        LEFT JOIN OrderedItinerary si2
            ON si1.ShipId = si2.ShipId
            AND si1.PortId = si2.PortId
            AND si1.RowNum + 1 = si2.RowNum
            AND si1.IsArrival = 1
            AND si2.IsArrival = 0
        LEFT JOIN [dbo].[CruiseShip] cs ON cs.Id = si1.ShipId
        LEFT JOIN [dbo].[Cruiseline] cl ON cl.Id = cs.CruiselineId
        LEFT JOIN [dbo].[ShippingPort] p ON p.Id = si1.PortId
        LEFT JOIN [dbo].[Country] c ON c.Id = p.CountryId
        INNER JOIN (
            SELECT 
                [UserAccountId],
                [ShipId],
                [StartDate],
                [EndDate],
                c.DisplayName,
                c.ProfileImage
            FROM [dbo].[UserContract] uc
            INNER JOIN (
                SELECT 
                    ua.Id AS CrewId,
                    ua.DisplayName,
                    ua.ProfileImage
                FROM [dbo].[CrewJoin] cj
                LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
                LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
                LEFT JOIN UserAccount ua ON ua.Id = cjd.CrewId
                WHERE cj.IsBlocked <> 1 AND cj.IsDeleted <> 1 AND cj.IsActive = 1
                    AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId)
            ) c ON c.CrewId = uc.UserAccountId
            WHERE [StartDate] <= @DepartureDate
                AND [EndDate] >= @ArrivalDate
                AND IsDeleted <> 1
        ) uc ON uc.ShipId = si1.ShipId
            AND si1.PortDate <= uc.[EndDate]
            AND si2.PortDate >= uc.[StartDate]
        WHERE si1.IsArrival = 1
            AND (si1.PortDate <= @DepartureDate AND si1.PortTime <= @DepartureTime)
            AND (si2.PortDate >= @ArrivalDate AND si2.PortTime >= @ArrivalTime)
    )
    
    -- Final result with Total Records and Pagination
    SELECT 
        TotalRecords = (SELECT COUNT(*) FROM PaginatedResults),
        TotalPages = CEILING((SELECT COUNT(*) FROM PaginatedResults) * 1.0 / @PageSize),
        @PageNumber AS PageNumber,
        @PageSize AS PageSize,
        Results.*
    FROM PaginatedResults Results
    WHERE Results.RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1 AND @PageNumber * @PageSize
    ORDER BY Results.RowNum;

END
