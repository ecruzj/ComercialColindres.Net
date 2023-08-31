CREATE FUNCTION [dbo].[GetHumidityPaymentByBoletaId]
(@boletaId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN

RETURN (SELECT ISNULL(SUM((dbo.GetHumidityFactor(bhp.BoletaHumedadId) * b.PrecioProductoCompra) / 100), 0)
FROM dbo.BoletasHumedadPago bhp
	INNER JOIN BoletasHumedadAsignacion bha
		ON bha.BoletaHumedadId = bhp.BoletaHumedadId
	INNER JOIN dbo.Boletas b
		ON b.BoletaId = bha.BoletaId
WHERE bhp.BoletaId = @boletaId)

END