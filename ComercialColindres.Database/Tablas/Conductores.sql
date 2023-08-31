CREATE TABLE [dbo].[Conductores]
(
	[ConductorId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Nombre] VARCHAR(50) NOT NULL DEFAULT '',
	[ProveedorId] INT NOT NULL,
	[Telefonos] VARCHAR(50) NOT NULL DEFAULT '',
	CONSTRAINT [FK_Conductores_Proveedores] FOREIGN KEY (ProveedorId) REFERENCES Proveedores(ProveedorId),
)
