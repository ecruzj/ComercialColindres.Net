CREATE FUNCTION [dbo].[GetTotalChargeLoan]
(@PrestamoId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @MontoPrestamo DECIMAL(18, 2)

SELECT
    @MontoPrestamo = MontoPrestamo
FROM Prestamos
WHERE PrestamoId = @PrestamoId

RETURN ROUND((SELECT dbo.GetInteresLoan(@PrestamoId) + @MontoPrestamo), 2)
END