CREATE FUNCTION [dbo].[GetSecurityRate]
(@boletaId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN

DECLARE @TotalInvoice DECIMAL(18, 2),
@SecurityRate DECIMAL(18, 2)

IF (( SELECT
		p.IsExempt
	FROM Boletas b
	INNER JOIN Proveedores p
		ON p.ProveedorId = b.ProveedorId
	WHERE b.BoletaId = @boletaId) = 1)
SET @SecurityRate = 0.10
ELSE
	BEGIN
	SET @TotalInvoice = (SELECT
			ROUND(PrecioProductoCompra * PesoProducto, 2)
		FROM dbo.Boletas
		WHERE BoletaId = @boletaId)
	SET @SecurityRate = ROUND((@TotalInvoice / 1000), 2);

		IF (( SELECT
			dbo.IsInt(@SecurityRate)) = 0)
	SET @SecurityRate = @SecurityRate + 1

	SET @SecurityRate = ROUND(@SecurityRate * 2, 0)
	END

RETURN ISNULL(@SecurityRate, 0)

END