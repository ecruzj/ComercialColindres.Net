CREATE TABLE [dbo].[LogTransacciones]
(
	[TransaccionUId] uniqueidentifier NOT NULL PRIMARY KEY, 
    [TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '', 
    [FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(), 
    [FechaTransaccionUtc] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ModificadoPor] VARCHAR(15) NOT NULL DEFAULT ''
)
