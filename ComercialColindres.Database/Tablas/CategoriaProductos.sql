CREATE TABLE [dbo].[CategoriaProductos]
(
	[CategoriaProductoId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BiomasaId] INT NOT NULL  ,
	[Descripcion] VARCHAR(50) NOT NULL, 
	CONSTRAINT [FK_CategoriaProductos_MaestroBiomasa] FOREIGN KEY ([BiomasaId]) REFERENCES MaestroBiomasa(BiomasaId)
    
)
