CREATE FUNCTION [dbo].[GetBoletaAmount]
( @BoletaId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @TotalFacturaCompra DECIMAL(18, 2),
@OtrosIngresos DECIMAL(18, 2)

SET @TotalFacturaCompra = (SELECT
		ROUND(PrecioProductoCompra * PesoProducto, 2)
	FROM dbo.Boletas
	WHERE BoletaId = @BoletaId)
SET @OtrosIngresos = (SELECT
		ISNULL(SUM(Monto), 0)
	FROM dbo.BoletaOtrasDeducciones
	WHERE BoletaId = @BoletaId
	AND Monto > 0 AND EsDeduccionManual = 0)

	RETURN ROUND((@TotalFacturaCompra + @OtrosIngresos) - dbo.GetBoletaDeductions(@BoletaId), 2)
END