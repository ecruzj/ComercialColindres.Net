CREATE FUNCTION [dbo].[GetBoletaDeductions]
( @BoletaId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @TotalFacturaCompra DECIMAL(18, 2),
@DescargaProducto DECIMAL(18, 2),
@OrdenCombustible DECIMAL(18, 2),
@AbonoPrestamos DECIMAL(18, 2),
@DeduccionesNegativas DECIMAL(18, 2),
@HumidityPayments DECIMAL(18, 2),
@AjustePayments DECIMAL(18, 2)


SET @TotalFacturaCompra = (SELECT
		ROUND(PrecioProductoCompra * PesoProducto, 2)
	FROM dbo.Boletas
	WHERE BoletaId = @BoletaId)	
SET @DescargaProducto = (SELECT
		ISNULL(SUM(PrecioDescarga), 0)
	FROM dbo.Descargadores
	WHERE BoletaId = @BoletaId AND EsIngresoManual = 0)
SET @OrdenCombustible = (SELECT
		ISNULL(SUM(Monto), 0)
	FROM dbo.OrdenesCombustible
	WHERE BoletaId = @BoletaId)
SET @AbonoPrestamos = (SELECT
		ISNULL(SUM(MontoAbono), 0)
	FROM dbo.PagoPrestamos
	WHERE BoletaId = @BoletaId)
SET @DeduccionesNegativas = (SELECT
		ISNULL(SUM(ABS(Monto)), 0)
	FROM dbo.BoletaOtrasDeducciones
	WHERE BoletaId = @BoletaId
	AND Monto < 0)
SET @HumidityPayments = (SELECT dbo.GetHumidityPaymentByBoletaId(@BoletaId))
SET @AjustePayments = (SELECT ISNULL(SUM(Monto), 0) FROM AjusteBoletaPagos WHERE BoletaId = @BoletaId)

	RETURN ROUND((SELECT dbo.GetSecurityRate(@BoletaId)) + @DescargaProducto + @OrdenCombustible + @AbonoPrestamos + @DeduccionesNegativas + @HumidityPayments + @AjustePayments, 2)
END