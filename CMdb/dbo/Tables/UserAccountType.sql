CREATE TABLE [dbo].[UserAccountType]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[AccountType] nvarchar(255) NOT NULL DEFAULT 'User' -- User, business, premium, recruiter etc.
)
