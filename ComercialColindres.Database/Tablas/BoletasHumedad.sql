CREATE TABLE [dbo].[BoletasHumedad]
(
	[BoletaHumedadId] INT NOT NULL PRIMARY KEY IDENTITY,
	[NumeroEnvio] VARCHAR(10) NOT NULL DEFAULT '', 
    [PlantaId] INT NOT NULL, 
    [BoletaIngresada] BIT NOT NULL DEFAULT 0, 
    [HumedadPromedio] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [PorcentajeTolerancia] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[FechaHumedad] DATE NOT NULL DEFAULT GETDATE(),  
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO', 
	[TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL DEFAULT NEWID(),
    CONSTRAINT [FK_BoletasHumedad_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),
	UNIQUE ([NumeroEnvio], PlantaId)
)