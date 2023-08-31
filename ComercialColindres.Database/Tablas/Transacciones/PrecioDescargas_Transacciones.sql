CREATE TABLE [dbo].[PrecioDescargas_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PrecioDescargaId] INT NOT NULL,
	[PlantaId] INT NOT NULL,
	[EquipoCategoriaId] INT NOT NULL,
	[PrecioDescarga] DECIMAL (18,2) NOT NULL,
	[EsPrecioActual] BIT NOT NULL DEFAULT 1,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
