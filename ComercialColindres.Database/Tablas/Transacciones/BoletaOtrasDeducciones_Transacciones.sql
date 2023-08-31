CREATE TABLE [dbo].[BoletaOtrasDeducciones_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BoletaOtraDeduccionId] INT NOT NULL, 
    [BoletaId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [MotivoDeduccion] VARCHAR(100) NOT NULL DEFAULT '', 
    [FormaDePago] VARCHAR(50) NOT NULL DEFAULT '', 
    [LineaCreditoId] INT NULL, 
    [NoDocumento] VARCHAR(50) NOT NULL DEFAULT '', 
	[EsDeduccionManual] BIT NOT NULL DEFAULT 0, 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
)
