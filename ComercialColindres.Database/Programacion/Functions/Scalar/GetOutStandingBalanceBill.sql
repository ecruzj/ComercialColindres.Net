CREATE FUNCTION [dbo].[GetOutStandingBalanceBill]
(@FacturaId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @OutStandingBill DECIMAL(18, 2)
DECLARE @TotalBill DECIMAL(18, 2)

SET @TotalBill = (dbo.GetTotalBill(@FacturaId))

SET @OutStandingBill = (SELECT
		@TotalBill - ISNULL(SUM(fp.Monto), 0)
	FROM Facturas f
		LEFT JOIN dbo.FacturaPagos fp
			ON fp.FacturaId = f.FacturaId
	WHERE f.FacturaId = @FacturaId)
	
	RETURN ROUND(@OutStandingBill, 2)
END