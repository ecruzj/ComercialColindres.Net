CREATE TABLE [dbo].[FacturaDetalleItems]
(
	[FacturaDetalleItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacturaId] INT NOT NULL, 
	[Cantidad] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [CategoriaProductoId] INT NOT NULL, 
    [Precio] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,    
	CONSTRAINT [FK_FacturaDetalleItems_Facturas] FOREIGN KEY ([FacturaId]) REFERENCES [Facturas](FacturaId), 
	CONSTRAINT [FK_FacturaDetalleItems_CategoriaPRoductos] FOREIGN KEY ([CategoriaProductoId]) REFERENCES [CategoriaProductos](CategoriaProductoId)
)
