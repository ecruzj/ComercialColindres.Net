CREATE FUNCTION [dbo].[GetInteresLoan]
(@PrestamoId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @EsInteresMensual BIT,
@InteresPercent DECIMAL(18, 2),
@MontoPrestamo DECIMAL(18, 2),
@InteresMensual DECIMAL(18, 2),
@InteresDiario DECIMAL(18, 2),
@DiasTranscurridos INT

SELECT
	@EsInteresMensual = EsInteresMensual
   ,@InteresPercent = PorcentajeInteres
   ,@MontoPrestamo = MontoPrestamo
FROM Prestamos
WHERE PrestamoId = @PrestamoId

--Interes Unico
IF (@InteresPercent > 0 AND @EsInteresMensual = 0)
BEGIN
	RETURN ROUND((@MontoPrestamo * (ISNULL(@InteresPercent, 0) / 100)), 2)
END

--Interes Mensual
IF (@InteresPercent > 0 AND @EsInteresMensual = 1)
BEGIN
SET @InteresMensual = ROUND((@MontoPrestamo * (@InteresPercent / 100)), 2)
SET @InteresDiario = ROUND((@InteresMensual / 30), 2)
SET @DiasTranscurridos = (SELECT
		dbo.GetAntiguedadLoan(@PrestamoId))

RETURN ROUND((@DiasTranscurridos * @InteresDiario), 2)
END

RETURN 0
END
