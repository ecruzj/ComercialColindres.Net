CREATE TABLE [dbo].[BoletaCierres]
(
	[BoletaCierreId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BoletaId] INT NOT NULL,
	[FormaDePago] VARCHAR(50) NOT NULL DEFAULT '', 
    [LineaCreditoId] INT NOT NULL, 
    [NoDocumento] VARCHAR(50) NOT NULL DEFAULT '', 
    [Monto] DECIMAL(18, 2) NOT NULL,
	[FechaPago] DATE NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_BoletaCierres_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_BoletaCierres_LineasCredito] FOREIGN KEY ([LineaCreditoId]) REFERENCES LineasCredito(LineaCreditoId)
)
