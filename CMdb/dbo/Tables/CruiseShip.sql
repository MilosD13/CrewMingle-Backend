﻿CREATE TABLE [dbo].[CruiseShip]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[CruiselineId] INT NOT NULL,
	[Ship] NVARCHAR(255) NOT NULL
)
