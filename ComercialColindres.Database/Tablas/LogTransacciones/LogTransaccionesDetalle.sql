CREATE TABLE [dbo].[LogTransaccionesDetalle]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[TransaccionUId] uniqueidentifier NOT NULL, 
    [TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '', 
    [EntidadDominio] VARCHAR(100) NOT NULL DEFAULT '', 
    [DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	CONSTRAINT [FK_LogTransaccionesDetalle_LogTransacciones] FOREIGN KEY ([TransaccionUId]) REFERENCES LogTransacciones(TransaccionUId)
)
