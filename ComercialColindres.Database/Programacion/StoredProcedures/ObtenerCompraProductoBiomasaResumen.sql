CREATE PROCEDURE [dbo].[ObtenerCompraProductoBiomasaResumen]
	@FechaInicio DATE,
	@FechaFinal DATE
AS

---Compras de Productos Biomsa Resumen---
SELECT
	planta.NombrePlanta AS 'Planta'
   ,cp.Descripcion AS 'TipoProducto'
   ,SUM(b.PrecioProductoCompra * b.PesoProducto) AS 'TotalCompra'
FROM dbo.Boletas b
INNER JOIN dbo.CategoriaProductos cp
	ON cp.CategoriaProductoId = b.CategoriaProductoId
INNER JOIN dbo.ClientePlantas planta
	ON planta.PlantaId = b.PlantaId
WHERE b.FechaCreacionBoleta BETWEEN @FechaInicio AND @FechaFinal
AND b.Estado = 'CERRADO'
GROUP BY planta.NombrePlanta
		,cp.Descripcion