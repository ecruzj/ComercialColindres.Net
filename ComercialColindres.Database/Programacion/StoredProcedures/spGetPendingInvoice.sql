CREATE PROCEDURE [dbo].[spGetPendingInvoice]
	@FiltroBusqueda VARCHAR(MAX),
	@PlantaId INT = 0
AS

----**Facturas Pendientes de Pago**---
SELECT
	f.NumeroFactura
   ,cp.NombrePlanta AS 'Planta'
   ,'Moneda' =
   CASE
		WHEN f.IsForeignCurrency = 1 THEN '$'
		ELSE 'Lps'
	END
   ,dbo.GetTotalBill(f.FacturaId) AS 'Total'
   ,dbo.GetOutStandingBalanceBill(f.FacturaId) AS 'SaldoPendiente'
   ,f.Fecha AS 'FechaEmision'
   ,DATEDIFF(DAY, f.Fecha, GETDATE()) AS 'Antiguedad'
FROM Facturas f
INNER JOIN ClientePlantas cp
	ON cp.PlantaId = f.PlantaId
WHERE (Estado <> 'CERRADO'
AND Estado NOT IN ('ANULADO'))
AND ((@FiltroBusqueda = 'Por Planta'
AND f.PlantaId = @PlantaId)
OR @FiltroBusqueda = 'Mostrar Todas')
ORDER BY cp.NombrePlanta, f.Fecha