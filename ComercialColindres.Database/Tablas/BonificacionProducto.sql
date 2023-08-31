CREATE TABLE [dbo].[BonificacionProducto]
(
	[BonificacionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlantaId] INT NOT NULL, 
    [CategoriaProductoId] INT NOT NULL, 
    [IsEnable] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [FK_Bonificaciones_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),
	CONSTRAINT [FK_Bonificaciones_CategoriaProductos] FOREIGN KEY ([CategoriaProductoId]) REFERENCES CategoriaProductos(CategoriaProductoId),
	UNIQUE ([PlantaId], [CategoriaProductoId])
)
