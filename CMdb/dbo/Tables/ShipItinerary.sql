CREATE TABLE [dbo].[ShipItinerary]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ShipId] INT NOT NULL,
	[PortId] INT NOT NULL,
	[PortDate]  Date NOT NULL,
	[PortTime] TIME NOT NULL,
	[IsArrival] BIT NOT NULL 
)
