CREATE TABLE [dbo].[SubPlantas]
(
	[SubPlantaId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlantaId] INT NOT NULL, 
    [NombreSubPlanta] VARCHAR(50) NOT NULL DEFAULT '', 
    [Rtn] VARCHAR(50) NOT NULL DEFAULT '', 
    [Direccion] VARCHAR(50) NOT NULL DEFAULT '', 
    [EsExonerado] BIT NOT NULL DEFAULT 0, 
    [RegistroExoneracion] VARCHAR(50) NOT NULL DEFAULT ''
	CONSTRAINT [FK_SubPlanta_Planta] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId)
)
