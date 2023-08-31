CREATE PROCEDURE [dbo].[spObtenerOrdenesCombustibleOperativo]
	@GasCreditoId INT
AS
	--CODIGO
	SELECT 
		   b.CodigoBoleta, 
		   b.NumeroEnvio,
		   oc.CodigoFactura, 
		   oc.AutorizadoPor, 
		   oc.PlacaEquipo, 
		   oc.Monto,
		   oc.FechaCreacion
	FROM OrdenesCombustible oc
			INNER JOIN Boletas b
				ON b.BoletaId = oc.BoletaId
	WHERE oc.GasCreditoId = @GasCreditoId
	AND oc.EsOrdenPersonal = 0
	ORDER BY oc.FechaCreacion DESC
