CREATE TABLE [dbo].[FacturasCategorias]
(
	[FacturaCategoriaId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descripcion] VARCHAR(50) NOT NULL DEFAULT ''
)