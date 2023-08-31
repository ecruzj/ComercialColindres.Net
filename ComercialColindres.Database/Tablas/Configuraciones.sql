CREATE TABLE [dbo].[Configuraciones]
(
	[CodigoConfiguracion] VARCHAR(20) NOT NULL PRIMARY KEY, 
    [Descripcion] VARCHAR(50) NOT NULL DEFAULT '', 
    [DefinidaPorUsuario] BIT NOT NULL DEFAULT 0 
)
