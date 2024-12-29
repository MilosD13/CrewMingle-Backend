CREATE PROCEDURE [dbo].[spCruiseline_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
      [Id] AS CruiselineId,
      [Cruiseline] AS CruiselineName
    FROM [dbo].[Cruiseline]
    ORDER BY [Cruiseline];
END
