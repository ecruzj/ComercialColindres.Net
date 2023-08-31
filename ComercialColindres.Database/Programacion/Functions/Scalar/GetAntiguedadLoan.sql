CREATE FUNCTION [dbo].[GetAntiguedadLoan]
(@PrestamoId INT)
RETURNS INT
AS
BEGIN

DECLARE @Estado VARCHAR(10),
@FechaPrestamo DATE,
@FechaPago DATE

SELECT
	@Estado = Estado
   ,@FechaPrestamo = FechaCreacion
FROM Prestamos
WHERE PrestamoId = @PrestamoId

IF (@Estado <> 'CERRADO')
BEGIN
RETURN DATEDIFF(DAY, @FechaPrestamo, GETDATE())
END

SET @FechaPago = (SELECT TOP 1
		FechaTransaccion
	FROM PagoPrestamos
	WHERE PrestamoId = @PrestamoId
	ORDER BY FechaTransaccion DESC)

RETURN DATEDIFF(DAY, @FechaPrestamo, @FechaPago)
END