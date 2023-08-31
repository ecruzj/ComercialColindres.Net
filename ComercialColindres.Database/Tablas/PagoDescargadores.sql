CREATE TABLE [dbo].[PagoDescargadores]
(
	[PagoDescargaId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CodigoPagoDescarga] VARCHAR(50) NOT NULL DEFAULT '', 
	[CuadrillaId] INT NOT NULL,
    [TotalPago] DECIMAL(18, 2) NOT NULL, 
	[FechaPago] DATE NOT NULL DEFAULT GETDATE(),
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO', 
    [CreadoPor] VARCHAR(10) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_PagoDescargadores_Cuadrillas] FOREIGN KEY ([CuadrillaId]) REFERENCES Cuadrillas(CuadrillaId)
)
