CREATE PROCEDURE [dbo].[spShipItinerary_GetFlat]
    @ShipId INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH OrderedItinerary AS
    (
        SELECT
            si.[ShipId],
            si.[PortId],
            si.[PortDate],
            si.[PortTime],
            si.[IsArrival],
            ROW_NUMBER() OVER (PARTITION BY si.ShipId, si.PortId 
                               ORDER BY si.PortDate, si.PortTime) AS RowNum
        FROM [dbo].[ShipItinerary] si
        WHERE si.ShipId = @ShipId
    )
    SELECT
        cl.[Cruiseline],
        cs.[Ship]  AS Ship,
        cs.[Id]    AS ShipId,
        p.[Id]     AS PortId,
        p.[PortName],
        c.[Country] AS PortCountry,
        p.[Latitude],
        p.[Longitude],

        si1.[PortDate] AS ArrivalDate,
        si1.[PortTime] AS ArrivalTime,
        si2.[PortDate] AS DepartureDate,
        si2.[PortTime] AS DepartureTime,
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
    WHERE si1.IsArrival = 1
    ORDER BY si1.PortDate, si1.PortTime;
END
