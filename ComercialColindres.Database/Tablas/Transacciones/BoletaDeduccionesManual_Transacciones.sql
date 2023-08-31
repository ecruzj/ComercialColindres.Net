CREATE TABLE [dbo].[BoletaDeduccionesManual_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[DeduccionManualId] INT NOT NULL,
	[BoletaId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [MotivoDeduccion] VARCHAR(100) NOT NULL DEFAULT '', 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
