CREATE TABLE [dbo].[UserAccount]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[UserId] nvarchar(255) NOT NULL,
	[Email] nvarchar(255) NOT NULL,
	[DisplayName] nvarchar(255) NULL,
	[AccountTypeId] UNIQUEIDENTIFIER NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
)
