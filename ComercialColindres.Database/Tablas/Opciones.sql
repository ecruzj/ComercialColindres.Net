﻿CREATE TABLE [dbo].[Opciones]
(
	[OpcionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nombre] VARCHAR(50) NOT NULL DEFAULT '', 
    [TipoAcceso] VARCHAR(20) NOT NULL DEFAULT '', 
    [TipoPropiedad] VARCHAR(10) NOT NULL DEFAULT ''
)