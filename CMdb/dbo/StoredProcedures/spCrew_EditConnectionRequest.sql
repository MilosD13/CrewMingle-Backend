CREATE PROCEDURE [dbo].[spCrew_EditConnectionRequest]
	@requesterId UNIQUEIDENTIFIER,
	@requesteeid UNIQUEIDENTIFIER,
	@Status NVARCHAR(255)

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @RequestId UNIQUEIDENTIFIER = (SELECT DISTINCT [CrewJoinId] FROM [dbo].[CrewJoinDetails]
											WHERE [CrewId] IN (@requesterId, @requesteeid) )
	
	DECLARE @UpdateId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[CrewJoinDetails] WHERE [CrewId] = @RequestId AND [CrewId] = @requesterId )



	UPDATE [dbo].[CrewJoinDetails]
	SET [Status] = @Status, LastUpdatedDate = GETDATE()
	WHERE [Id] = @UpdateId;
	
	
	UPDATE [dbo].[CrewJoin]
	SET
		[IsBlocked] = CASE WHEN @Status = 'BLOCKED' THEN 1 ELSE [IsBlocked] END,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;

	UPDATE [dbo].[CrewJoin]
	SET
		[IsDeleted] = CASE WHEN @Status = 'CANCELLED' THEN 1 ELSE [IsDeleted] END,
		LastUpdatedDate = GETDATE()
	WHERE [Id] = @RequestId;
	
END