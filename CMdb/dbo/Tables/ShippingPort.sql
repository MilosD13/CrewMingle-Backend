CREATE TABLE [dbo].[ShippingPort]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[PortName] NVARCHAR(255) NOT NULL,
	[CountryId] INT NULL,
	[Latitude] DECIMAL(8,6) NULL,
	[Longitude] DECIMAL(9,6) NULL,
	[Code] NVARCHAR(16) NULL,
	[OldPortName]  NVARCHAR(255) NULL
)
