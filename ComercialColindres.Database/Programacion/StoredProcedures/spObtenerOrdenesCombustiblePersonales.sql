CREATE PROCEDURE [dbo].[spObtenerOrdenesCombustiblePersonales]
	@GasCreditoId INT
AS
	--CODIGO
	SELECT 
		   oc.CodigoFactura, 
		   oc.AutorizadoPor, 
		   oc.PlacaEquipo AS 'PlacaCarro',
		   oc.Monto,
		   oc.FechaCreacion
	FROM OrdenesCombustible oc
	WHERE oc.GasCreditoId = @GasCreditoId
	AND oc.EsOrdenPersonal = 1
	ORDER BY oc.FechaCreacion DESC
