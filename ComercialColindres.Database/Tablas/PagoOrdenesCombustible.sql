CREATE TABLE [dbo].[PagoOrdenesCombustible]
(
	[PagoOrdenCombustibleId] INT NOT NULL PRIMARY KEY IDENTITY, 
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
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_PagoOrdenesCombustible_LineasCredito] FOREIGN KEY ([LineaCreditoId]) REFERENCES LineasCredito(LineaCreditoId),
	CONSTRAINT [FK_PagoOrdenesCombustible_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId)
)
