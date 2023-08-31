CREATE TABLE [dbo].[OrdenesCombustible]
(
	[OrdenCombustibleId] INT NOT NULL PRIMARY KEY IDENTITY,
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
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_OrdenesCombustible_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_OrdenesCombustible_GasolineraCreditos] FOREIGN KEY ([GasCreditoId]) REFERENCES GasolineraCreditos(GasCreditoId),
	CONSTRAINT [FK_OrdenesCombustible_Proveedores] FOREIGN KEY ([GasCreditoId]) REFERENCES GasolineraCreditos(GasCreditoId)
)
