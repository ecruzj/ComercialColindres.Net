CREATE TABLE [dbo].[Descargadores]
(
	[DescargadaId] INT NOT NULL IDENTITY,
	[BoletaId] INT NOT NULL PRIMARY KEY,
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
	[TransaccionUId] uniqueidentifier NOT NULL,    
    CONSTRAINT [FK_Descargadores_Cuadrillas] FOREIGN KEY ([CuadrillaId]) REFERENCES Cuadrillas(CuadrillaId),
	CONSTRAINT [FK_Descargadores_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_Descargadores_PagoDescargadores] FOREIGN KEY ([PagoDescargaId]) REFERENCES PagoDescargadores(PagoDescargaId)
)
