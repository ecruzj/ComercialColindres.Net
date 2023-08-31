CREATE TABLE [dbo].[FuelOrderManualPayment_Transacciones]
(
	[UId] INT NOT NULL PRIMARY KEY IDENTITY,
	[FuelOrderManualPaymentId] INT NOT NULL, 
    [FuelOrderId] INT NOT NULL, 
	[WayToPay] VARCHAR(50) NOT NULL DEFAULT '',
    [BankId] INT NULL, 
	[PaymentDate] DATE NOT NULL DEFAULT GETDATE(), 
    [BankReference] VARCHAR(50) NOT NULL DEFAULT '', 
    [Amount] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
	[Observations] VARCHAR(100) NOT NULL DEFAULT '', 
	[TipoTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL DEFAULT '',
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),
	[TransaccionUId] uniqueidentifier NOT NULL
)
