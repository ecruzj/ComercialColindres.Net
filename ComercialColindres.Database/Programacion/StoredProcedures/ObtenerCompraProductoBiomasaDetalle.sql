CREATE PROCEDURE [dbo].[ObtenerCompraProductoBiomasaDetalle]
	@FechaInicio DATE,
	@FechaFinal DATE
AS

---Compras de Productos Biomsa Detalle---
SELECT
	p.Nombre AS Proveedor
   ,planta.NombrePlanta AS Planta
   ,b.CodigoBoleta
   ,b.NumeroEnvio
   ,b.PlacaEquipo
   ,cp.Descripcion AS TipoProducto
   ,b.PesoProducto
   ,b.CantidadPenalizada
   ,b.PrecioProductoCompra
   ,(b.PesoProducto * b.PrecioProductoCompra) AS 'TotalCompra'
   ,b.FechaCreacionBoleta AS FechaIngreso
FROM dbo.Boletas b
INNER JOIN dbo.Proveedores p
	ON p.ProveedorId = b.ProveedorId
INNER JOIN dbo.CategoriaProductos cp
	ON cp.CategoriaProductoId = b.CategoriaProductoId
INNER JOIN dbo.ClientePlantas planta
	ON planta.PlantaId = b.PlantaId
WHERE b.FechaCreacionBoleta BETWEEN @FechaInicio AND @FechaFinal
AND b.Estado = 'CERRADO'
ORDER BY b.FechaCreacionBoleta