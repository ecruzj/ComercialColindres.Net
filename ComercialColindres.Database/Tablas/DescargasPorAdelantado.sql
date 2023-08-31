CREATE TABLE [dbo].[DescargasPorAdelantado]
(
	[DescargaPorAdelantadoId] INT NOT NULL IDENTITY PRIMARY KEY, 
	[PlantaId] INT NOT NULL,
	[BoletaId] INT NULL,
	[NumeroEnvio] VARCHAR(10) NOT NULL DEFAULT '', 
    [CodigoBoleta] VARCHAR(10) NOT NULL DEFAULT '',      
	[PagoDescargaId] INT NOT NULL, 
    [CreadoPor] VARCHAR(10) NOT NULL, 
    [PrecioDescarga] DECIMAL(18, 2) NOT NULL DEFAULT 0.00, 
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'PENDIENTE',
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,	
	CONSTRAINT [FK_DescargasPorAdelantado_Plantas] FOREIGN KEY ([PlantaId]) REFERENCES ClientePlantas(PlantaId),
	CONSTRAINT [FK_DescargasPorAdelantado_PagoDescargadores] FOREIGN KEY ([PagoDescargaId]) REFERENCES PagoDescargadores(PagoDescargaId),
	CONSTRAINT [FK_DescargasPorAdelanto_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	UNIQUE ([PlantaId], [NumeroEnvio], [CodigoBoleta])
)
