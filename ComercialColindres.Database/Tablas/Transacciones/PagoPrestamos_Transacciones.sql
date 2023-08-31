CREATE TABLE [dbo].[PagoPrestamos_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PagoPrestamoId] INT NOT NULL,
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
	[TransaccionUId] uniqueidentifier NOT NULL
)
