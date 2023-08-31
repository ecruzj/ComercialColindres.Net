CREATE TABLE [dbo].[OrdenesCompraDetalleBoleta]
(
	[OrdenCompraDetalleBoletaId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrdenCompraProductoId] INT NOT NULL, 
    [BoletaId] INT NOT NULL, 
    [CantidadFacturada] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_OrdenesCompraDetalleBoletas_OrdenesCompraProducto] FOREIGN KEY ([OrdenCompraProductoId]) REFERENCES OrdenesCompraProducto(OrdenCompraProductoId),
	CONSTRAINT [FK_OrdenesCompraDetalleBoletas_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId)
)
