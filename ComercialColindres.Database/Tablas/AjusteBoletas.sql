CREATE TABLE [dbo].[AjusteBoletas]
(
    [AjusteBoletaId] INT NOT NULL PRIMARY KEY IDENTITY,
    [BoletaId] INT NOT NULL,
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'NUEVO', 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
    CONSTRAINT [FK_AjusteBoletas_Boletas] FOREIGN KEY ([BoletaId]) REFERENCES Boletas(BoletaId),
	UNIQUE ([BoletaId])
)
