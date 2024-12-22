CREATE PROCEDURE [dbo].[spCrew_EditConnectionRequest]
	@UserId NVARCHAR(255), -- Firebase user id form token
	@Crewid UNIQUEIDENTIFIER,
	@Status NVARCHAR(255)

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @UserDbId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @UserId )

	DECLARE @RequestId UNIQUEIDENTIFIER = (SELECT  cj.Id  FROM [dbo].[CrewJoin] cj
												LEFT JOIN [dbo].[CrewJoinDetails] cjd ON cjd.CrewJoinId = cj.Id AND cjd.CrewId <> cj.CreatedByCrewId
												LEFT JOIN [dbo].[CrewJoinDetails] cjdc ON cjdc.CrewJoinId = cj.Id AND cjdc.CrewId = cj.CreatedByCrewId
												WHERE cjd.CrewId IN (@UserDbId, @Crewid) AND cjdc.CrewId IN (@UserDbId, @Crewid))
	
	DECLARE @UpdateId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[CrewJoinDetails] WHERE [CrewJoinId] = @RequestId AND [CrewId] = @UserDbId )



	UPDATE [dbo].[CrewJoinDetails]
	SET [Status] = UPPER(@Status), LastUpdatedDate = GETDATE() --ensure capitalisation
	WHERE [Id] = @UpdateId;
	

	UPDATE [dbo].[CrewJoin]
	SET
		[IsBlocked] = CASE 
                    WHEN @Status = 'BLOCKED' THEN 1
                    WHEN @Status = 'ACCEPTED' THEN 0
                    ELSE [IsBlocked]
					  END,
		[IsActive] = CASE 
					   WHEN @Status = 'ACCEPTED' THEN 1
					   WHEN @Status IN ('CANCELLED', 'REJECTED', 'BLOCKED') THEN 0
					   ELSE [IsActive]
					 END,
		[IsDeleted] = CASE 
						WHEN @Status IN ('CANCELLED', 'REJECTED') THEN 1
						ELSE [IsDeleted]
					  END,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;
END