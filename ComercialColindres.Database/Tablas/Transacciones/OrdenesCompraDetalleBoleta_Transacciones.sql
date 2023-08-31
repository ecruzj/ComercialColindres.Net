CREATE TABLE [dbo].[OrdenesCompraDetalleBoleta_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrdenCompraDetalleBoletaId] INT NOT NULL, 
    [OrdenCompraProductoId] INT NOT NULL, 
    [BoletaId] INT NOT NULL, 
    [CantidadFacturada] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[TransaccionUId] uniqueidentifier NOT NULL,
)
