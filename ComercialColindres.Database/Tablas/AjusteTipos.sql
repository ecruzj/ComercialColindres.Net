CREATE TABLE [dbo].[AjusteTipos]
(
	[AjusteTipoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descripcion] VARCHAR(100) NOT NULL, 
    [UseCreditLine] BIT NOT NULL DEFAULT 0
)
