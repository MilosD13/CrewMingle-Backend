CREATE PROCEDURE [dbo].[spCrew_GetCrewByShip]
	@UserId NVARCHAR(255),
	@ShipId INT,
	@StartDate Date,
	@EndDate Date,
	@PageNumber INT = 1,
	@PageSize INT = 10

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @UserDbId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @UserId );

	-- Calculate total record count
	DECLARE @TotalRecords INT;
	SELECT 
		@TotalRecords = COUNT(*)
	FROM [dbo].[UserContract] uc 
	 left join [dbo].CruiseShip cs on uc.ShipId = cs.Id
	 left join [dbo].Cruiseline cl on cl.Id = cs.CruiselineId

	INNER JOIN (SELECT ua.Id [CrewId], ua.DisplayName, ua.ProfileImage
			FROM [dbo].[CrewJoin] cj
				LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
				LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
				LEFT JOIN UserAccount ua on ua.Id = cjd.CrewId --crew profile
			WHERE cj.IsBlocked <> 1 AND cj.IsDeleted <> 1 AND cj.IsActive = 1
				AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId) ) c on c.CrewId = uc.UserAccountId

	WHERE uc.ShipId = @ShipId
	AND StartDate <= @StartDate AND EndDate > = @EndDate -- current
	AND uc.[IsDeleted] <>1;;

	-- Retrieve paginated data
	WITH PaginatedResults AS (

		SELECT 
		[UserAccountId]  [CrewId],
		 cl.[Cruiseline],
		 cs.[Ship][ShipName],
		 [ShipId],
		 [StartDate],
		 [EndDate],
		 c.DisplayName, c.ProfileImage
		 ,ROW_NUMBER() OVER (ORDER BY uc.CreatedDate DESC) AS RowNum -- sorting of results
	 FROM [dbo].[UserContract] uc 
	 left join [dbo].CruiseShip cs on uc.ShipId = cs.Id
	 left join [dbo].Cruiseline cl on cl.Id = cs.CruiselineId

	INNER JOIN (SELECT ua.Id [CrewId], ua.DisplayName, ua.ProfileImage
			FROM [dbo].[CrewJoin] cj
				LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
				LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
				LEFT JOIN UserAccount ua on ua.Id = cjd.CrewId --crew profile
			WHERE cj.IsBlocked <> 1 AND cj.IsDeleted <> 1 AND cj.IsActive = 1
				AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId) ) c on c.CrewId = uc.UserAccountId

	WHERE uc.ShipId = @ShipId
	AND StartDate <= @StartDate AND EndDate > = @EndDate -- current
	AND uc.[IsDeleted] <>1

		)

	SELECT 
		@TotalRecords AS TotalRecords,
		CEILING(@TotalRecords * 1.0 / @PageSize) AS TotalPages,
		@PageNumber AS PageNumber,
		@PageSize AS PageSize,
		Results.*
	FROM PaginatedResults Results
	WHERE Results.RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1 AND @PageNumber * @PageSize;

END