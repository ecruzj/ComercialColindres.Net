CREATE TABLE [dbo].[OrdenesCompraProductoDetalle]
(
	[OrdenCompraProductoDetalleId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrdenCompraProductoId] INT NOT NULL, 
    [BiomasaId] INT NOT NULL, 
    [Toneladas] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[PrecioDollar] DECIMAL(18, 4) NOT NULL DEFAULT 0.00,
	[PrecioLps] DECIMAL(18, 10) NOT NULL DEFAULT 0.00,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_OrdenesCompraProductoDetalle_OrdenesCompraProducto] FOREIGN KEY ([OrdenCompraProductoId]) REFERENCES OrdenesCompraProducto(OrdenCompraProductoId),
	CONSTRAINT [FK_OrdenesCompraProductoDetalle_MaestroBiomasa] FOREIGN KEY ([BiomasaId]) REFERENCES MaestroBiomasa(BiomasaId)

)
