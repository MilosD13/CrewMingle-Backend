CREATE TABLE [dbo].[ScheduleImport]
(
	[CruiselineId] INT NOT NULL PRIMARY KEY,
	[Cruiseline] NVARCHAR(255) NOT NULL,
	[ShipId] INT NOT NULL,
	[Ship] NVARCHAR(255) NOT NULL,
	[JourneyId] INT NOT NULL,
	[StartDate] DATE NOT NULL,
	[Description] NVARCHAR(255) NOT NULL,
	[Date] DATE NOT NULL,
	[ArrivalTime] NVARCHAR(16) NULL,
	[DepartureTime] NVARCHAR(16) NULL,
	[Port] NVARCHAR(255) NOT NULL

)
