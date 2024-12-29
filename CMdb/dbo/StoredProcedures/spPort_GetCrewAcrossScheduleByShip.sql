CREATE PROCEDURE [dbo].[spPort_GetCrewAcrossScheduleByShip]
    @UserId NVARCHAR(255),
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @ShipId INT,
    @ArrivalDate DATE,
    @DepartureDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserDbId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @UserId);

    -- CTE for Ordered Itinerary for ShipId
    WITH OrderedItinerary AS (
        SELECT 
            si.ShipId,
            si.PortId,
            si.PortDate,
            si.PortTime,
            si.IsArrival,
            ROW_NUMBER() OVER (PARTITION BY si.ShipId, si.PortId ORDER BY si.PortDate, si.PortTime) AS RowNum
        FROM [dbo].[ShipItinerary] si
        WHERE si.ShipId = @ShipId
    ),
    -- CTE for Ordered Itinerary for Date Range
    OrderedItineraryOther AS (
        SELECT 
            si.ShipId,
            si.PortId,
            si.PortDate,
            si.PortTime,
            si.IsArrival,
            ROW_NUMBER() OVER (PARTITION BY si.ShipId, si.PortId ORDER BY si.PortDate, si.PortTime) AS RowNum
        FROM [dbo].[ShipItinerary] si
        WHERE si.PortDate >= @ArrivalDate AND si.PortDate <= @DepartureDate
    ),
    -- CTE for Paginated Results
    PaginatedResults AS (
        SELECT 
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
                    WHEN osi.ArrivalDate <= si1.PortDate AND osi.DepartureDate >= si1.PortDate
                        THEN CAST(si1.PortDate AS DATETIME) + CAST(si1.PortTime AS DATETIME)
                    ELSE CAST(osi.ArrivalDate AS DATETIME) + CAST(osi.ArrivalTime AS DATETIME)
                END,
                CASE 
                    WHEN osi.DepartureDate >= si2.PortDate AND osi.ArrivalDate <= si2.PortDate 
                        THEN CAST(si2.PortDate AS DATETIME) + CAST(si2.PortTime AS DATETIME)
                    ELSE CAST(osi.DepartureDate AS DATETIME) + CAST(osi.DepartureTime AS DATETIME)
                END
            ) / 60.0 AS OverlapHours,
            osi.UserAccountId [CrewId],
            osi.DisplayName, osi.ProfileImage,
            ROW_NUMBER() OVER (ORDER BY si1.PortDate, si1.PortTime) AS RowNum
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
        LEFT JOIN (SELECT 
                    cl.Cruiseline,
                    cs.Ship,
                    si1.ShipId,	
                    p.Id AS PortId,
                    si1.PortDate AS ArrivalDate,
                    si1.PortTime AS ArrivalTime,
                    si2.PortDate AS DepartureDate,
                    si2.PortTime AS DepartureTime,
                    uc.UserAccountId,
                    uc.DisplayName,
                    uc.ProfileImage
                FROM OrderedItineraryOther si1
                LEFT JOIN OrderedItineraryOther si2
                    ON si1.ShipId = si2.ShipId
                    AND si1.PortId = si2.PortId
                    AND si1.RowNum + 1 = si2.RowNum
                    AND si1.IsArrival = 1
                    AND si2.IsArrival = 0
                LEFT JOIN [dbo].[CruiseShip] cs ON cs.Id = si1.ShipId
                LEFT JOIN [dbo].[Cruiseline] cl ON cl.Id = cs.CruiselineId
                LEFT JOIN [dbo].[ShippingPort] p ON p.Id = si1.PortId
                LEFT JOIN [dbo].[Country] c ON c.Id = p.CountryId
                INNER JOIN (SELECT 
                                [UserAccountId],
                                [ShipId],
                                [StartDate],
                                [EndDate],
                                c.DisplayName, 
                                c.ProfileImage
                            FROM [dbo].[UserContract] uc
                            INNER JOIN (SELECT ua.Id [CrewId], ua.DisplayName, ua.ProfileImage
                                FROM [dbo].[CrewJoin] cj
                                    LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
                                    LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
                                    LEFT JOIN UserAccount ua on ua.Id = cjd.CrewId
                                WHERE cj.IsBlocked <> 1 
                                    AND cj.IsDeleted <> 1 
                                    AND cj.IsActive = 1
                                    AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId)) c 
                            ON c.CrewId = uc.UserAccountId
                            WHERE [StartDate] <= @DepartureDate
                            AND [EndDate] >= @ArrivalDate
                            AND IsDeleted <> 1
                ) uc 
                ON uc.ShipId = si1.ShipId
                    AND si1.PortDate <= uc.[EndDate] 
                    AND si2.PortDate >= uc.[StartDate]
                WHERE si1.IsArrival = 1
                AND si1.PortDate <= @DepartureDate
                AND si2.PortDate >= @ArrivalDate
        ) osi 
            ON osi.PortId = si1.PortId 
            AND osi.ArrivalDate <= si2.PortDate 
            AND osi.DepartureDate >= si1.PortDate
        WHERE si1.IsArrival = 1
        AND (si1.PortDate <= @DepartureDate)
        AND (si2.PortDate >= @ArrivalDate)
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
