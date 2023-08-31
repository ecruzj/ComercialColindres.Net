CREATE TABLE [dbo].[BoletasHumedadPago]
(
	[BoletaHumedadPagoId] INT NOT NULL IDENTITY, 
    [BoletaId] INT NOT NULL,
	[BoletaHumedadId] INT NOT NULL PRIMARY KEY,
	[TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL DEFAULT NEWID(),
	CONSTRAINT [FK_BoletaHumedadPago_Boleta] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_BoletaHumedadPago_BoletaHumedad] FOREIGN KEY ([BoletaHumedadId]) REFERENCES BoletasHumedad(BoletaHumedadId)
)