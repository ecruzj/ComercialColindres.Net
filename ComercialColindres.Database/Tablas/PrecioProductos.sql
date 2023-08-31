CREATE TABLE [dbo].[PrecioProductos]
(
	[PrecioProductoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[CategoriaProductoId] INT NOT NULL,
	[PlantaId] INT NOT NULL,
	[PrecioCompra] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	[PrecioVenta] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	[EsPrecioActual] BIT NOT NULL DEFAULT 1,
	[RequiereOrdenCompra] BIT NOT NULL DEFAULT 0, 
	[FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_PrecioProductos_CategoriaProductos] FOREIGN KEY ([CategoriaProductoId]) REFERENCES CategoriaProductos(CategoriaProductoId),
	CONSTRAINT [FK_PrecioProductos_ClientePlantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId)
)
