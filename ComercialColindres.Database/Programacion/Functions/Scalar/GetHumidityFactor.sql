CREATE FUNCTION [dbo].[GetHumidityFactor]
(
@HumidityId INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @Value DECIMAL (18, 2) = 0
DECLARE @DecimalValues DECIMAL(18, 2) = 0
DECLARE @ValueFinal DECIMAL(18, 2) = 0
DECLARE @AverageHumidity DECIMAL(18, 2) = 0
DECLARE @TolerancePercent DECIMAL(18, 2) = 0

SET @AverageHumidity = (SELECT
		HumedadPromedio
	FROM BoletasHumedad
	WHERE BoletaHumedadId = @HumidityId)
SET @TolerancePercent = (SELECT
		PorcentajeTolerancia
	FROM BoletasHumedad
	WHERE BoletaHumedadId = @HumidityId)

IF (@AverageHumidity > @TolerancePercent)
BEGIN

SET @Value = ISNULL((SELECT
		ROUND((bh.HumedadPromedio - bh.PorcentajeTolerancia) * b.PesoProducto, 2)
	FROM BoletasHumedad bh
	INNER JOIN BoletasHumedadAsignacion bha
		ON bha.BoletaHumedadId = bh.BoletaHumedadId
	INNER JOIN Boletas b
		ON b.BoletaId = bha.BoletaId
	WHERE bh.BoletaHumedadId = @HumidityId)
, 0)

SET @DecimalValues = @Value % 1

IF (@DecimalValues >= 0.50)
	BEGIN
SET @ValueFinal = (@Value - @DecimalValues) + 1
	END
ELSE
SET @ValueFinal = (@Value - @DecimalValues)
END

RETURN	ISNULL(@ValueFinal, 0)

END