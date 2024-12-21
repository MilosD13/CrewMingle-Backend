CREATE PROCEDURE [dbo].[spCrew_CreateConnectionRequest]
	@RequesterId UNIQUEIDENTIFIER,
	@Requesteeid UNIQUEIDENTIFIER

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @RequestId UNIQUEIDENTIFIER = NEWID();
	INSERT INTO 

	[dbo].[CrewJoin]([Id], [CreatedByCrewId], [IsDeleted], [IsBlocked])
	VALUES (@RequestId, @RequesterId, 0, 0);

	INSERT INTO [dbo].[CrewJoinDetails] ([CrewJoinId], [CrewId], [Status])
	VALUES (@RequestId, @RequesterId, 'REQUESTED'),
			(@RequestId, @Requesteeid, 'PENDING');
END
