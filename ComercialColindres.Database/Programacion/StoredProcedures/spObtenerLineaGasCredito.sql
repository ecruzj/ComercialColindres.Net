CREATE PROCEDURE [dbo].[spObtenerLineaGasCredito]
	@GasCreditoId INT
AS
	--CODIGO
	SELECT 
		   g.Descripcion AS 'Gasolinera', 
		   u.Nombre AS 'CreadoPor', 
		   glc.EsCreditoActual,
		   glc.Estado,
		   CodigoGasCredito, 
		   glc.Credito, 
		   glc.Saldo, 
		   glc.FechaInicio, 
		   glc.FechaFinal
	FROM GasolineraCreditos glc
		INNER JOIN Gasolineras g
			ON g.GasolineraId = glc.GasolineraId
		INNER JOIN Usuarios u
			ON u.Usuario = glc.CreadoPor
	WHERE GasCreditoId = @GasCreditoId
