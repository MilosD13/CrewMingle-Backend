CREATE TABLE [dbo].[UserContract]
(
    [Id] UNIQUEIDENTIFIER  DEFAULT NEWID()      PRIMARY KEY,
    [CreatedDate] DATETIME2(7)    NOT NULL      DEFAULT GETUTCDATE(),
    [UserAccountId] UNIQUEIDENTIFIER NOT NULL,
    [ShipId] INT NOT NULL,
    [StartDate] DATETIME2(7) NOT NULL,
    [EndDate] DATETIME2(7) NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_UserContract_UserAccount
        FOREIGN KEY ([UserAccountId]) 
        REFERENCES [dbo].[UserAccount]([Id]),

    CONSTRAINT FK_UserContract_CruiseShip
        FOREIGN KEY ([ShipId]) 
        REFERENCES [dbo].[CruiseShip]([Id])
);