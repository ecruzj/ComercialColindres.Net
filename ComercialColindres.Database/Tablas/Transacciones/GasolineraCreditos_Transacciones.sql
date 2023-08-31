CREATE TABLE [dbo].[GasolineraCreditos_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[GasCreditoId] INT NOT NULL, 
    [CodigoGasCredito] VARCHAR(10) NOT NULL DEFAULT '', 
    [GasolineraId] INT NOT NULL, 
    [Credito] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [Saldo] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [FechaInicio] DATETIME NOT NULL DEFAULT GETDATE(), 
    [FechaFinal] DATETIME NULL, 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO',
	[CreadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[EsCreditoActual] bit NOT NULL DEFAULT 0,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
