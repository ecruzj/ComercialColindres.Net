CREATE TABLE [dbo].[OrdenesCompraProducto_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrdenCompraProductoId] INT NOT NULL, 
    [OrdenCompraNo] VARCHAR(50) NOT NULL DEFAULT '', 
    [NoExoneracionDEI] VARCHAR(50) NOT NULL DEFAULT '', 
    [PlantaId] INT NOT NULL, 
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(), 
    [FechaActivacion] DATETIME NULL , 
	[EsOrdenCompraActual] BIT NOT NULL DEFAULT 0,
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'NUEVO', 
    [FechaCierre] DATETIME NULL , 
    [CreadoPor] VARCHAR(10) NOT NULL DEFAULT '', 
    [MontoDollar] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [ConversionDollarToLps] DECIMAL(18, 10) NOT NULL DEFAULT 0.00,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
