CREATE TABLE [dbo].[PagoPrestamos]
(
	[PagoPrestamoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PrestamoId] INT NOT NULL,
	[FormaDePago] VARCHAR(50) NOT NULL DEFAULT '',
	[BancoId] INT NULL, 
    [NoDocumento] VARCHAR(50) NULL, 
	[BoletaId] INT NULL,
	[MontoAbono] DECIMAL(18,2) NOT NULL,
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_PagoPrestamos_Prestamos] FOREIGN KEY ([PrestamoId]) REFERENCES Prestamos(PrestamoId),
	CONSTRAINT [FK_PagoPrestamos_Bancos] FOREIGN KEY ([BancoId]) REFERENCES Bancos(BancoId),
	CONSTRAINT [FK_PagoPrestamos_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
)
