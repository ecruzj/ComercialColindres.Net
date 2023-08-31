CREATE TABLE [dbo].[Cuadrillas]
(
	[CuadrillaId] INT NOT NULL PRIMARY KEY IDENTITY,
	[NombreEncargado] VARCHAR(50) NOT NULL,
	[PlantaId] INT NOT NULL,
	[AplicaPago] BIT NOT NULL DEFAULT 0,
	[Estado] VARCHAR(20) NOT NULL DEFAULT 'ACTIVO'
	CONSTRAINT [FK_Cuadrillas_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),    
)
