CREATE TABLE [dbo].[AjusteBoletaPagos]
(
	[AjusteBoletaPagoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AjusteBoletaDetalleId] INT NOT NULL, 
	[BoletaId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [FechaAbono] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_AjusteBoletaPagos_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_AjusteBoletaPagos_AjusteBoletaDetalles] FOREIGN KEY ([AjusteBoletaDetalleId]) REFERENCES AjusteBoletaDetalles(AjusteBoletaDetalleId)
)
