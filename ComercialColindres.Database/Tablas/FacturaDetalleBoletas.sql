CREATE TABLE [dbo].[FacturaDetalleBoletas]
(
	[FacturaDetalleBoletaId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacturaId] INT NOT NULL, 
	[PlantaId] INT NOT NULL , 
    [BoletaId] INT NULL,
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
	[TransaccionUId] uniqueidentifier NOT NULL,        
    CONSTRAINT [FK_FacturaDetalle_Facturas] FOREIGN KEY ([FacturaId]) REFERENCES Facturas(FacturaId),
	CONSTRAINT [FK_FacturaDetalle_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_FacturaDetalle_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),
	UNIQUE ([BoletaId], [PlantaId], [NumeroEnvio], [CodigoBoleta])
)
