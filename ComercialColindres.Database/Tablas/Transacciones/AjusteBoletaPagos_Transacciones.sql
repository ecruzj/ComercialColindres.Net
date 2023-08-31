CREATE TABLE [dbo].[AjusteBoletaPagos_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[AjusteBoletaPagoId] INT NOT NULL, 
	[AjusteBoletaDetalleId] INT NOT NULL, 
    [BoletaId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [FechaAbono] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
