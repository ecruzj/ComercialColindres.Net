CREATE TABLE [dbo].[Sucursales]
(
	[SucursalId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[CodigoSucursal] VARCHAR(10) NOT NULL DEFAULT '',
    [Nombre] VARCHAR(50) NOT NULL DEFAULT '', 
    [Direccion] TEXT NOT NULL DEFAULT '', 
    [Telefonos] VARCHAR(50) NOT NULL DEFAULT '', 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO'
)
