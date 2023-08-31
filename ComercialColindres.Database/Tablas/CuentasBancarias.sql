CREATE TABLE [dbo].[CuentasBancarias]
(
	[CuentaId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProveedorId] INT NOT NULL,
	[BancoId] INT NOT NULL,
	[CuentaNo] VARCHAR(20) NOT NULL,
	[NombreAbonado] VARCHAR(50) NOT NULL,
	[CedulaNo] VARCHAR(20) NOT NULL DEFAULT '',	
	[Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO'
	CONSTRAINT [FK_CuentasBancarias_Proveedor] FOREIGN KEY ([ProveedorId]) REFERENCES Proveedores(ProveedorId)
	CONSTRAINT [FK_CuentasBancarias_Bancos] FOREIGN KEY (BancoId) REFERENCES Bancos(BancoId)
)
