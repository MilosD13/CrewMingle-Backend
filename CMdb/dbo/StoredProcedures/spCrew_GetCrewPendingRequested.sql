CREATE PROCEDURE [dbo].[spCrew_GetCrewPendingRequested]
	@UserId NVARCHAR(255),
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
	FROM [dbo].[CrewJoin] cj
	LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
	LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
	WHERE cj.IsBlocked <> 1 AND cj.IsDeleted <> 1 AND cj.IsActive = 1
	AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId);

	-- Retrieve paginated data
	WITH PaginatedResults AS (

		SELECT 
			cj.[LastUpdatedDate]
		  ,cjdc.[Status]  [Status]
		  ,ua.Id [CrewId]
		  , ua.Email
		  , ua.DisplayName
		  , ua.ProfileImage
		  ,ROW_NUMBER() OVER (ORDER BY cj.CreatedDate DESC) AS RowNum -- sorting of results
		FROM [dbo].[CrewJoin] cj

			LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> @UserDbId
			LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = @UserDbId
			LEFT JOIN UserAccount ua on ua.Id = cjd.CrewId --crew profile

		WHERE cj.IsBlocked <> 1 -- NOT blocked 
			AND cj.IsDeleted <> 1 -- NOT cancelled (deleted) / rejected
			AND cj.IsActive = 0 -- pending / requested

			AND (cj.CreatedByCrewId = @UserDbId OR cjdc.CrewId = @UserDbId)

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
