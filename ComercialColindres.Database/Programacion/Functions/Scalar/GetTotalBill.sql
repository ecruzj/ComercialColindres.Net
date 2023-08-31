CREATE FUNCTION [dbo].[GetTotalBill]
(@FacturaId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @TotalBill DECIMAL(18, 2)
DECLARE @Tax DECIMAL(18, 2)

SET @Tax = (SELECT
		Total * (TaxPercent / 100)
	FROM dbo.Facturas
	WHERE FacturaId = @FacturaId)

SET @TotalBill = (SELECT
		(f.Total + @Tax) - dbo.GetTotalCreditNotesByInvoice(@FacturaId)
	FROM Facturas f
	WHERE f.FacturaId = @FacturaId)
	
	RETURN ROUND(@TotalBill, 2)
END