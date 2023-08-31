CREATE TABLE [dbo].[BoletaDetalles]
(
	[PagoBoletaId] INT NOT NULL PRIMARY KEY IDENTITY,
	[CodigoBoleta] INT NOT NULL,
	[DeduccionId] INT NOT NULL ,
	[MontoDeduccion] DECIMAL(18,2) NOT NULL,
	[NoDocumento] VARCHAR(50) NOT NULL DEFAULT '',
	[Observaciones] VARCHAR(80) NOT NULL DEFAULT '',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
	CONSTRAINT [FK_BoletaDetalles_Deducciones] FOREIGN KEY ([DeduccionId]) REFERENCES Deducciones(DeduccionId),
	CONSTRAINT [FK_BoletaDetalles_Boletas] FOREIGN KEY ([CodigoBoleta]) REFERENCES Boletas(BoletaId)
)
