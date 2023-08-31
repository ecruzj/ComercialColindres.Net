CREATE PROCEDURE [dbo].[spGetHumidityPendingPayment]

AS
SELECT
	cp.NombrePlanta AS 'Planta'
	,'Estado' =
	CASE ISNULL(bha.BoletaHumedadAsignacionId, 0)
		WHEN 0 THEN 'Pendiente'
		ELSE 'Activo'
	END
	,bh.NumeroEnvio
	,ISNULL(p.Nombre, 'Pendiente') AS 'Proveedor'
	,bh.HumedadPromedio
	,bh.PorcentajeTolerancia AS 'Tolerancia'
	,ISNULL(b.PesoProducto, 0) AS 'Toneladas'
	,ISNULL(b.PrecioProductoCompra, 0) AS 'PrecioCompra'
	,ISNULL(dbo.GetHumidityFactor(bh.BoletaHumedadId) * b.PrecioProductoCompra / 100, 0) AS 'Total'
	,bh.FechaHumedad	
FROM BoletasHumedad bh
INNER JOIN ClientePlantas cp
	ON cp.PlantaId = bh.PlantaId
LEFT JOIN BoletasHumedadAsignacion bha
	ON bha.BoletaHumedadId = bh.BoletaHumedadId
LEFT JOIN BoletasHumedadPago bhp
	ON bhp.BoletaHumedadId = bh.BoletaHumedadId
LEFT JOIN Boletas b
	ON b.BoletaId = bha.BoletaId
LEFT JOIN Proveedores p
	ON p.ProveedorId = b.ProveedorId
WHERE bh.Estado = 'ACTIVO'
AND bhp.BoletaHumedadPagoId IS NULL
ORDER BY bh.FechaHumedad