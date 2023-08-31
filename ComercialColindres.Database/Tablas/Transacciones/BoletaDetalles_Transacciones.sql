CREATE TABLE [dbo].[BoletaDetalles_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PagoBoletaId] INT NOT NULL,
	[CodigoBoleta] INT NOT NULL,
	[DeduccionId] INT NOT NULL ,
	[MontoDeduccion] DECIMAL(18,2) NOT NULL,
	[NoDocumento] VARCHAR(50) NOT NULL DEFAULT '',
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
)
