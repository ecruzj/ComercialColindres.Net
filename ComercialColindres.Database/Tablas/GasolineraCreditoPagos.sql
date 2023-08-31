CREATE TABLE [dbo].[GasolineraCreditoPagos]
(
	[GasCreditoPagoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[GasCreditoId] INT NOT NULL,
	[FormaDePago] VARCHAR(50) NOT NULL DEFAULT '', 
    [LineaCreditoId] INT NOT NULL, 
    [NoDocumento] VARCHAR(50) NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_GasolineraCreditoPagos_GasolineraCreditos] FOREIGN KEY ([GasCreditoId]) REFERENCES GasolineraCreditos(GasCreditoId),
	CONSTRAINT [FK_GasolineraCreditoPagos_LineasCredito] FOREIGN KEY ([LineaCreditoId]) REFERENCES LineasCredito(LineaCreditoId)
)
