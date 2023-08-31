CREATE TABLE [dbo].[Descargadores_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[DescargadaId] INT NOT NULL,
	[BoletaId] INT NOT NULL,
	[CuadrillaId] INT NOT NULL,
	[PrecioDescarga] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[PagoDescarga] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[PagoDescargaId] INT NULL,
	[EsDescargaPorAdelanto] BIT NOT NULL DEFAULT 0, 
	[EsIngresoManual] BIT NOT NULL DEFAULT 0, 
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO',
	[FechaDescarga] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
