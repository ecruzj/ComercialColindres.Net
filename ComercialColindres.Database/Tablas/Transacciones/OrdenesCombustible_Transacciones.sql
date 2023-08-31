CREATE TABLE [dbo].[OrdenesCombustible_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrdenCombustibleId] INT NOT NULL,
	[CodigoFactura] VARCHAR(10) NOT NULL DEFAULT '',
	[GasCreditoId] INT NOT NULL,
	[AutorizadoPor] VARCHAR(50) NOT NULL DEFAULT '',
	[BoletaId] INT NULL,
	[ProveedorId] INT NULL, 
	[PlacaEquipo] VARCHAR(10) NOT NULL DEFAULT '',
	[Monto] DECIMAL(18,2) NOT NULL,
	[FechaCreacion] DATE NOT NULL DEFAULT GETDATE(),
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO',
	[EsOrdenPersonal] BIT NOT NULL DEFAULT 0,
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',	
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
