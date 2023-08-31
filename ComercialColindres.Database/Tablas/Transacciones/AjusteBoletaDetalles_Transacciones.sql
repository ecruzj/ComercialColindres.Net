CREATE TABLE [dbo].[AjusteBoletaDetalles_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AjusteBoletaDetalleId] INT NOT NULL, 
    [AjusteBoletaId] INT NOT NULL, 
    [AjusteTipoId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[LineaCreditoId] INT NULL, 
	[NoDocumento] VARCHAR(50) NOT NULL DEFAULT '',
    [Observaciones] VARCHAR(100) NOT NULL DEFAULT '', 
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
