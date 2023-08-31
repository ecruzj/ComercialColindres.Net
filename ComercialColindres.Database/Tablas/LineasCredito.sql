CREATE TABLE [dbo].[LineasCredito]
(
	[LineaCreditoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CodigoLineaCredito] VARCHAR(10) NOT NULL DEFAULT '', 
	[SucursalId] INT NOT NULL,
    [CuentaFinancieraId] INT NOT NULL, 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO', 
    [NoDocumento] VARCHAR(50) NOT NULL DEFAULT '', 
    [Observaciones] VARCHAR(100) NOT NULL DEFAULT '', 
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(), 
    [FechaCierre] DATETIME NULL , 
    [MontoInicial] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [Saldo] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [CreadoPor] VARCHAR(10) NOT NULL DEFAULT '', 
    [EsLineaCreditoActual] BIT NOT NULL DEFAULT 1,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_LineasCredito_CuentasFinancieras] FOREIGN KEY ([CuentaFinancieraId]) REFERENCES CuentasFinancieras(CuentaFinancieraId),
	CONSTRAINT [FK_LineasCredito_Sucursales] FOREIGN KEY ([SucursalId]) REFERENCES Sucursales(SucursalId)
)
