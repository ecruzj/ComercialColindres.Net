CREATE TABLE [dbo].[OrdenesCombustibleImg_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrdenCombustibleId] INT NOT NULL,
	[Imagen] VARBINARY(MAX) NOT NULL DEFAULT 0x , 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
