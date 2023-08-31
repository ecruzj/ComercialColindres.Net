CREATE FUNCTION [dbo].[GetLoanPayments]
(@PrestamoId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @Payments DECIMAL(18,2)

SET @Payments = (SELECT
		SUM(pp.MontoAbono)
	FROM Prestamos p
	INNER JOIN PagoPrestamos pp
		ON pp.PrestamoId = p.PrestamoId
	WHERE p.PrestamoId = @PrestamoId)

RETURN ROUND(ISNULL(@Payments, 0), 2)
END