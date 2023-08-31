CREATE TABLE [dbo].[Prestamos]
(
	[PrestamoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[CodigoPrestamo] VARCHAR(10) NOT NULL DEFAULT '',
	[ProveedorId] INT NOT NULL,
	[SucursalId] INT NOT NULL,
	[FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
	[AutorizadoPor] VARCHAR(50) NOT NULL DEFAULT '',
	[PorcentajeInteres] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	[EsInteresMensual] BIT NOT NULL DEFAULT 0, 
	[MontoPrestamo] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO',
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,	
    CONSTRAINT [FK_Prestamos_Proveedores] FOREIGN KEY ([ProveedorId]) REFERENCES Proveedores(ProveedorId),
	CONSTRAINT [FK_Prestamos_Sucursales] FOREIGN KEY ([SucursalId]) REFERENCES Sucursales(SucursalId)
)
