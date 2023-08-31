CREATE TABLE [dbo].[CuentasFinancieras]
(
	[CuentaFinancieraId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BancoId] INT NULL DEFAULT NULL, 
    [CuentaFinancieraTipoId] INT NOT NULL, 
    [CuentaNo] VARCHAR(50) NOT NULL DEFAULT '', 
	[EsCuentaAdministrativa] BIT NOT NULL DEFAULT 0,
    [NombreAbonado] VARCHAR(50) NOT NULL DEFAULT '', 
    [Cedula] VARCHAR(20) NOT NULL DEFAULT '', 
    [Estado] VARCHAR(10) NOT NULL DEFAULT 'ACTIVO' ,
	[ModificadoPor] VARCHAR(15) NOT NULL DEFAULT '',
	[DescripcionTransaccion] VARCHAR(50) NOT NULL,
	[FechaTransaccion] DATETIME NOT NULL DEFAULT GETDATE(),	
    CONSTRAINT [FK_CuentasFinancieras_Bancos] FOREIGN KEY ([BancoId]) REFERENCES Bancos(BancoId),
	CONSTRAINT [FK_CuentasFinancieras_CuentasFinancieraTipos] FOREIGN KEY ([CuentaFinancieraTipoId]) REFERENCES CuentasFinancieraTipos(CuentaFinancieraTipoId)
)
