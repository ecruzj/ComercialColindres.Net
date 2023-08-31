CREATE TABLE [dbo].[Correlativos]
(
	[CorrelativoId]     INT           IDENTITY (1, 1) NOT NULL,
    [CodigoCorrelativo] VARCHAR (10)  NOT NULL,
    [SucursalId]         INT           NOT NULL,
    [Prefijo]           VARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Letra]             VARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Tamaño]            INT           DEFAULT ((0)) NOT NULL,
    [CorrelativoActual] INT           DEFAULT ((0)) NOT NULL,
	[ControlarPorPrefijo] BIT NOT NULL DEFAULT 0,
	[ControlarPorRango] BIT NOT NULL DEFAULT 1,
    [CorrelativoInicialPermitido] INT NOT NULL DEFAULT 0, 
    [CorrelativoFinalPermitido] INT NOT NULL DEFAULT 0, 
    [DefinidoPorUsuario] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Correlativos_Sucursales] FOREIGN KEY ([SucursalId]) REFERENCES Sucursales ([SucursalId]), 
    CONSTRAINT [PK_Correlativos] PRIMARY KEY ([CorrelativoId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Este campo se utiliza para que la busqueda del correlativo deba incluir el prefijo',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Correlativos',
    @level2type = N'COLUMN',
    @level2name = N'ControlarPorPrefijo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Se utiliza para que el correlativo que se genere debe de estar dentro del rango permitodo',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Correlativos',
    @level2type = N'COLUMN',
    @level2name = N'ControlarPorRango'