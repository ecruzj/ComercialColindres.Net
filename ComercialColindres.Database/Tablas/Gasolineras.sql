CREATE TABLE [dbo].[Gasolineras]
(
	[GasolineraId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Descripcion] VARCHAR(50) NOT NULL,
	[NombreContacto] VARCHAR(50) NOT NULL,
	[TelefonoContacto] VARCHAR(20) NOT NULL DEFAULT ''
)
