CREATE TABLE [dbo].[BoletasHumedadPago_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BoletaHumedadPagoId] INT NOT NULL, 
    [BoletaId] INT NOT NULL,
	[BoletaHumedadId] INT NOT NULL,
	[TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL DEFAULT NEWID()
)
