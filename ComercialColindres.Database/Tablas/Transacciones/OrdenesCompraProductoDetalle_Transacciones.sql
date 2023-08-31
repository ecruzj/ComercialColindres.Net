CREATE TABLE [dbo].[OrdenesCompraProductoDetalle_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrdenCompraProductoDetalleId] INT NOT NULL, 
    [OrdenCompraProductoId] INT NOT NULL, 
    [BiomasaId] INT NOT NULL, 
    [Toneladas] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[PrecioDollar] DECIMAL(18, 4) NOT NULL DEFAULT 0.00,
	[PrecioLps] DECIMAL(18, 10) NOT NULL DEFAULT 0.00,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
