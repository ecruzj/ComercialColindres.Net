CREATE TABLE [dbo].[Recibos]
(
[ReciboId] INT NOT NULL PRIMARY KEY IDENTITY,
	[SucursalId] INT NOT NULL,	
	[EsAnticipo] BIT DEFAULT 0 NOT NULL,
	[AplicaA] VARCHAR(15) NOT NULL DEFAULT '', 
	[PrestamoId] INT NULL,
    [FacturaId] INT NULL,
	[NumeroRecibo] VARCHAR(50) NOT NULL DEFAULT '', 
    [Fecha] DATE NOT NULL DEFAULT GETDATE(), 
    [Monto] NUMERIC(18, 2) NOT NULL DEFAULT 0.00, 
    [Observaciones] VARCHAR(MAX) NOT NULL DEFAULT '', 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO', 
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Recibos_Prestamos] FOREIGN KEY (PrestamoId) REFERENCES Prestamos(PrestamoId),
    CONSTRAINT [FK_Recibos_Facturas] FOREIGN KEY (FacturaId) REFERENCES Facturas(FacturaId),
    CONSTRAINT [FK_Recibos_Sucursales] FOREIGN KEY (SucursalId) REFERENCES Sucursales(SucursalId)
)
