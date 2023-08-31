CREATE TABLE [dbo].[FacturaDetalleItems_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[FacturaDetalleItemId] INT NOT NULL, 
    [FacturaId] INT NOT NULL, 
	[Cantidad] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [CategoriaProductoId] INT NOT NULL, 
    [Precio] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
