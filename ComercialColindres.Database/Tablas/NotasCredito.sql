﻿CREATE TABLE [dbo].[NotasCredito]
(
	[NotaCreditoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacturaId] INT NOT NULL, 
	[NotaCreditoNo] VARCHAR(10) NOT NULL DEFAULT '', 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[Observaciones] VARCHAR(50) NOT NULL DEFAULT '', 
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,	    
    CONSTRAINT [FK_NotasCredito_Facturas] FOREIGN KEY ([FacturaId]) REFERENCES Facturas(FacturaId)
)
