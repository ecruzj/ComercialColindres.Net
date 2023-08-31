﻿CREATE TABLE [dbo].[LineasCreditoDeducciones_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[LineaCreditoDeduccionId] INT NOT NULL, 
    [LineaCreditoId] INT NOT NULL, 
    [Descripcion] VARCHAR(200) NOT NULL DEFAULT '', 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [EsGastoOperativo] BIT NOT NULL DEFAULT 1,
	[NoDocumento] VARCHAR(50) NULL,
	[FechaCreacion] DATE NOT NULL DEFAULT GETDATE(),
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
