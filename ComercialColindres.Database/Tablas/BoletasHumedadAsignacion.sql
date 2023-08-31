CREATE TABLE [dbo].[BoletasHumedadAsignacion]
(
	[BoletaHumedadAsignacionId] INT NOT NULL IDENTITY,
	[BoletaHumedadId] INT NOT NULL PRIMARY KEY, 
    [BoletaId] INT NOT NULL,
	[TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL DEFAULT NEWID(),
	UNIQUE ([BoletaHumedadId], [BoletaId]),
	CONSTRAINT [FK_BoletasHumedadAsignacion_BoletaHumedad] FOREIGN KEY ([BoletaHumedadId]) REFERENCES BoletasHumedad(BoletaHumedadId),
	CONSTRAINT [FK_BoletasHumedadAsignacion_Boleta] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
)