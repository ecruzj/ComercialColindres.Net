CREATE TABLE [dbo].[CuentasFinancieraTipos]
(
	[CuentaFinancieraTipoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descripcion] VARCHAR(50) NOT NULL DEFAULT '', 
	[RequiereBanco] BIT NOT NULL DEFAULT 1,
    [ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE()
)
