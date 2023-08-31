CREATE TABLE [dbo].[BoletaOtrasDeducciones]
(
	[BoletaOtraDeduccionId] INT NOT NULL PRIMARY KEY IDENTITY, 
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
    CONSTRAINT [FK_BoletaOtrasDeducciones_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	CONSTRAINT [FK_BoletaOtrasDeducciones_LineasCredito] FOREIGN KEY ([LineaCreditoId]) REFERENCES LineasCredito(LineaCreditoId)
)
