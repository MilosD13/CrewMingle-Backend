CREATE TABLE [dbo].[UserRoles]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[Role] nvarchar(255) NOT NULL DEFAULT 'User' -- Admin, Beta etc..
)
