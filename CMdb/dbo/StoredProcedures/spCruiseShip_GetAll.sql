CREATE PROCEDURE [dbo].[spCruiseShip_GetAll]
    @CruiselineId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
      cs.[Id]          AS ShipId,
      cs.[Ship]        AS ShipName,
      cs.[CruiselineId],
      cl.[Cruiseline]  AS CruiselineName
    FROM [dbo].[CruiseShip] cs
    JOIN [dbo].[Cruiseline] cl ON cs.CruiselineId = cl.Id
    WHERE (@CruiselineId IS NULL OR cs.CruiselineId = @CruiselineId)
    ORDER BY cl.[Cruiseline], cs.[Ship];
END
