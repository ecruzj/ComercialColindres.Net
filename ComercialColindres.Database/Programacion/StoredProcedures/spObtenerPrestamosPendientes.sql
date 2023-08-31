CREATE PROCEDURE [dbo].[spObtenerPrestamosPendientes]
AS
	if OBJECT_ID('tempdb..#PrestamosPendientes') is not null DROP TABLE #PrestamosPendientes

	SELECT p.PrestamoId,
		   p.ProveedorId, 
		   proveedor.Nombre AS 'NombreProveedor',	   
		   ((p.MontoPrestamo * (ISNULL(p.PorcentajeInteres, 0) / 100)) + p.MontoPrestamo) AS 'TotalPrestamo',
		   ISNULL(SUM(pp.MontoAbono),0) AS 'TotalAbono'
		   INTO #PrestamosPendientes
	FROM Prestamos p
		INNER JOIN Proveedores proveedor
			ON proveedor.ProveedorId = p.ProveedorId
		LEFT JOIN PagoPrestamos pp
			ON pp.PrestamoId = p.PrestamoId
	WHERE p.Estado NOT IN ('ANULADO')
	GROUP BY p.PrestamoId, p.ProveedorId, proveedor.Nombre, p.MontoPrestamo, p.PorcentajeInteres
	
	SELECT ProveedorId, NombreProveedor, SUM(TotalPrestamo) AS 'TotalPrestamo', SUM(TotalAbono) AS 'TotalAbono'
	FROM #PrestamosPendientes
	GROUP BY ProveedorId, NombreProveedor

	DROP TABLE #PrestamosPendientes