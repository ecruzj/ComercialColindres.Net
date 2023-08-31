CREATE TABLE [dbo].[Equipos]
(
	[EquipoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EquipoCategoriaId] INT NOT NULL,
	[ProveedorId] INT NOT NULL,
	[PlacaCabezal] VARCHAR(10) NOT NULL DEFAULT '',
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT [FK_Equipos_Proveedores] FOREIGN KEY ([ProveedorId]) REFERENCES Proveedores(ProveedorId),
	CONSTRAINT [FK_Equipos_EquiposCategoria] FOREIGN KEY (EquipoCategoriaId) REFERENCES EquiposCategorias(EquipoCategoriaId),
)
