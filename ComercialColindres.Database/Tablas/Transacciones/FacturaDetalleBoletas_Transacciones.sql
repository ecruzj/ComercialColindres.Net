CREATE TABLE [dbo].[FacturaDetalleBoletas_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[FacturaDetalleBoletaId] INT NOT NULL, 
    [FacturaId] INT NOT NULL, 
    [BoletaId] INT NULL,
	[PlantaId] INT NOT NULL , 
	[NumeroEnvio] VARCHAR(10) NOT NULL DEFAULT '', 
    [CodigoBoleta] VARCHAR(10) NOT NULL DEFAULT '', 
	[EstaIngresada] BIT NOT NULL DEFAULT 0, 
	[PesoProducto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
	[UnitPrice] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
	[FechaIngreso] DATE NOT NULL DEFAULT GETDATE(), 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
