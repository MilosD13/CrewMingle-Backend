CREATE PROCEDURE [dbo].[spCrew_CreateConnectionRequest]
	@RequesterId NVARCHAR(255), -- Firebase user id form token
	@Requesteeid UNIQUEIDENTIFIER

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @UserDbId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[UserAccount] WHERE [UserId] = @RequesterId )

	-- Check if the relationship already exists
    IF EXISTS (
        SELECT 1
        FROM [dbo].[CrewJoinDetails] cd
        INNER JOIN [dbo].[CrewJoin] cj ON cj.Id = cd.CrewJoinId
        WHERE (cd.CrewId = @UserDbId AND cd.CrewId = @Requesteeid)
           OR (cd.CrewId = @Requesteeid AND cd.CrewId = @UserDbId)
    )
    BEGIN
        -- Return a failure code if the relationship already exists
        RAISERROR ('Relationship already exists.', 16, 1);
        RETURN -1; -- return code to indicate failure
    END


	DECLARE @RequestId UNIQUEIDENTIFIER = NEWID();
	INSERT INTO 

	[dbo].[CrewJoin]([Id], [CreatedByCrewId], [IsDeleted], [IsBlocked])
	VALUES (@RequestId, @UserDbId, 0, 0);

	INSERT INTO [dbo].[CrewJoinDetails] ([CrewJoinId], [CrewId], [Status])
	VALUES (@RequestId, @UserDbId, 'REQUESTED'),
			(@RequestId, @Requesteeid, 'PENDING');

	-- Return a success code
    RETURN 0;

END
