CREATE PROCEDURE [dbo].[spGetBoletasWithOutInvoice]
	@FechaInicial DATE,
	@FechaFinal DATE,
	@FiltroBusqueda VARCHAR(MAX),
	@FiltrarPorFechas BIT = 0,
	@PlantaId INT = 0
AS

----**Boletas Pendientes de Facturar**---
SELECT
	cp.NombrePlanta AS 'Planta'
   ,b.CodigoBoleta
   ,'NumeroEnvio' =
	CASE
		WHEN b.NumeroEnvio = '' THEN 'N/A'
		ELSE b.NumeroEnvio
	END
   ,b.PesoProducto
   ,p.Nombre AS 'Proveedor'
   ,b.PlacaEquipo
   ,Producto.Descripcion AS 'Producto'
   ,b.FechaSalida
   ,b.FechaCreacionBoleta
FROM Boletas b
INNER JOIN ClientePlantas cp
	ON cp.PlantaId = b.PlantaId
INNER JOIN Proveedores p
	ON p.ProveedorId = b.ProveedorId
INNER JOIN CategoriaProductos Producto
	ON Producto.CategoriaProductoId = b.CategoriaProductoId
LEFT JOIN FacturaDetalleBoletas fdb
	ON fdb.BoletaId = b.BoletaId
WHERE fdb.BoletaId IS NULL
AND ((@FiltroBusqueda = 'Por Planta'
AND b.PlantaId = @PlantaId)
OR @FiltroBusqueda = 'Mostrar Todas')
AND ((b.FechaSalida BETWEEN @FechaInicial AND @FechaFinal
AND @FiltrarPorFechas = 1)
OR @FiltrarPorFechas = 0)
ORDER BY cp.NombrePlanta, b.FechaCreacionBoleta