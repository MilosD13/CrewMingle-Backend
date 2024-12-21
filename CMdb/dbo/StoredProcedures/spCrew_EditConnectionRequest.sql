CREATE PROCEDURE [dbo].[spCrew_EditConnectionRequest]
	@UserId UNIQUEIDENTIFIER,
	@Crewid UNIQUEIDENTIFIER,
	@Status NVARCHAR(255)

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @RequestId UNIQUEIDENTIFIER = (SELECT DISTINCT [CrewJoinId] FROM [dbo].[CrewJoinDetails]
											WHERE [CrewId] IN (@UserId, @Crewid) )
	
	DECLARE @UpdateId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[CrewJoinDetails] WHERE [CrewJoinId] = @RequestId AND [CrewId] = @UserId )



	UPDATE [dbo].[CrewJoinDetails]
	SET [Status] = UPPER(@Status), LastUpdatedDate = GETDATE() --ensure capitalisation
	WHERE [Id] = @UpdateId;
	
	UPDATE [dbo].[CrewJoin]
	SET
		[IsActive] = CASE WHEN @Status = 'ACCEPTED' THEN 1 ELSE [IsActive] END,
		[IsBlocked] = 0, [IsDeleted] = 0,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;

	UPDATE [dbo].[CrewJoin]
	SET
		[IsBlocked] = CASE WHEN @Status = 'BLOCKED' THEN 1 ELSE [IsBlocked] END,
		[IsActive] = 0,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;

	UPDATE [dbo].[CrewJoin]
	SET
		[IsDeleted] = CASE WHEN @Status = 'CANCELLED' THEN 1 ELSE [IsDeleted] END,
		[IsActive]= 0,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;
	
END