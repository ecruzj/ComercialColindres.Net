CREATE TABLE [dbo].[BoletasImg]
(
	[BoletaId] INT NOT NULL PRIMARY KEY,
	[Imagen] VARBINARY(MAX) NOT NULL DEFAULT 0x , 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_BoletasImg_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId)
)
