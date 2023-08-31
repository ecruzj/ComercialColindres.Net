CREATE TABLE [dbo].[PrecioDescargas]
(
	[PrecioDescargaId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PlantaId] INT NOT NULL,
	[EquipoCategoriaId] INT NOT NULL,
	[PrecioDescarga] DECIMAL (18,2) NOT NULL,
	[EsPrecioActual] BIT NOT NULL DEFAULT 1,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_PrecioDescargas_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),
	CONSTRAINT [FK_PrecioDescargas_EquiposCategorias] FOREIGN KEY ([EquipoCategoriaId]) REFERENCES EquiposCategorias(EquipoCategoriaId)
)
