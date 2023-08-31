CREATE TABLE [dbo].[FacturaPagos_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[FacturaPagoId] INT NOT NULL, 
    [FacturaId] INT NOT NULL, 
	[FormaDePago] VARCHAR(50) NOT NULL DEFAULT '',
    [BancoId] INT NOT NULL, 
	[FechaDePago] DATE NOT NULL DEFAULT GETDATE(), 
    [ReferenciaBancaria] VARCHAR(50) NOT NULL DEFAULT '', 
    [Monto] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[TipoTransaccion] VARCHAR(50) NOT NULL,
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL,
)
