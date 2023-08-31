CREATE TABLE [dbo].[OrdenesCombustibleImg]
(
	[OrdenCombustibleId] INT NOT NULL PRIMARY KEY,
	[Imagen] VARBINARY(MAX) NOT NULL DEFAULT 0x , 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_OrdenesCombustibleImg_Boletas] FOREIGN KEY ([OrdenCombustibleId]) REFERENCES OrdenesCombustible(OrdenCombustibleId)
)
