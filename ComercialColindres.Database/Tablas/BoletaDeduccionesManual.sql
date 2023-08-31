CREATE TABLE [dbo].[BoletaDeduccionesManual]
(
	[DeduccionManualId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BoletaId] INT NOT NULL, 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [MotivoDeduccion] VARCHAR(100) NOT NULL DEFAULT '', 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_BoletaDeduccionesManual_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId)
)
