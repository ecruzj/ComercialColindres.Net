CREATE TABLE [dbo].[ClientePlantas]
(
	[PlantaId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RTN] VARCHAR(20) NOT NULL DEFAULT '',
	[NombrePlanta] VARCHAR(50) NOT NULL,
	[Telefonos] VARCHAR(50) NOT NULL DEFAULT '',
	[Direccion] VARCHAR(50) NOT NULL, 
    [RequiresPurchaseOrder] BIT NOT NULL DEFAULT 0, 
	[RequiresProForm] BIT NOT NULL DEFAULT 0, 
	[RequiresWeekNo] BIT NOT NULL DEFAULT 0, 
    [IsExempt] BIT NOT NULL DEFAULT 0, 
    [HasSubPlanta] BIT NOT NULL DEFAULT 0, 
    [HasExonerationNo] BIT NOT NULL DEFAULT 0, 
    [ImgHorizontalFormat] BIT NOT NULL DEFAULT 0 
)
