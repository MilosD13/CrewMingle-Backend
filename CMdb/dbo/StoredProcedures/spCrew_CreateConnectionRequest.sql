CREATE PROCEDURE [dbo].[spCrew_CreateConnectionRequest]
	@requesterId UNIQUEIDENTIFIER,
	@requesteeid UNIQUEIDENTIFIER

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @RequestId UNIQUEIDENTIFIER = NEWID();
	INSERT INTO 

	[dbo].[CrewJoin]([Id], [CreatedByCrewId], [IsDeleted], [IsBlocked])
	VALUES (@RequestId, @requesterId, 0, 0);

	INSERT INTO [dbo].[CrewJoinDetails] ([CrewJoinId], [CrewId], [Status])
	VALUES (@RequestId, @requesterId, 'REQUESTED'),
			(@RequestId, @requesteeid, 'PENDING');
END
