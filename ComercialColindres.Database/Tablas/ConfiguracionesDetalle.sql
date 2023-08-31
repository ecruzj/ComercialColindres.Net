CREATE TABLE [dbo].[ConfiguracionesDetalle]
(
	[ConfiguracionDetalleId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CodigoConfiguracion] VARCHAR(20) NOT NULL, 
    [Atributo] VARCHAR(50) NOT NULL DEFAULT '', 
    [Valor] VARCHAR(50) NOT NULL DEFAULT '', 
    [EsRequerido] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ConfiguracionesDetalle_Configuraciones] FOREIGN KEY (CodigoConfiguracion) REFERENCES Configuraciones(CodigoConfiguracion)
)