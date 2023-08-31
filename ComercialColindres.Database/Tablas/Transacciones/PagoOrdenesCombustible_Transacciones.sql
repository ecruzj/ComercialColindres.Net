CREATE TABLE [dbo].[PagoOrdenesCombustible_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PagoOrdenCombustibleId] INT NOT NULL, 
    [OrdenCombustibleId] INT NOT NULL, 
    [FormaDePago] VARCHAR(50) NOT NULL DEFAULT '',
	[LineaCreditoId] INT NOT NULL, 
    [NoDocumento] VARCHAR(50) NULL, 
	[BoletaId] INT NULL,
	[MontoAbono] DECIMAL(18,2) NOT NULL,
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
