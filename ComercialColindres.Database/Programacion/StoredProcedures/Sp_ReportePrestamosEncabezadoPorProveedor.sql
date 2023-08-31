CREATE PROCEDURE [dbo].[Sp_ReportePrestamosEncabezadoPorProveedor]
	@SucursalId INT,
	@FechaInicial DATE,
	@FechaFinal DATE,
	@FiltrarPorFechas BIT,
	@ProveedorId INT
AS

------------****PRESTAMOS ENCABEZADO****----------
SELECT
	p.CodigoPrestamo
	,p.AutorizadoPor
	,p.FechaCreacion
	,p.PorcentajeInteres
	,'EsInteresMensual' =
		CASE
			WHEN p.EsInteresMensual > 0 THEN 'SI' ELSE 'NO'
		END
	,dbo.GetInteresLoan(p.PrestamoId) AS 'Intereses'
	,p.MontoPrestamo
	,dbo.GetTotalChargeLoan(p.PrestamoId) AS 'TotalCobrar'
	,dbo.GetLoanPayments(p.PrestamoId) AS 'TotalAbono'
	,'Estado' =
	CASE p.Estado
		WHEN 'ENPROCESO' THEN 'PENDIENTE'
		ELSE p.Estado
	END
	,proveedor.Nombre AS 'Proveedor'
	,p.Observaciones
FROM Prestamos p
INNER JOIN Proveedores proveedor
	ON proveedor.ProveedorId = p.ProveedorId
INNER JOIN Sucursales s
	ON s.SucursalId = p.SucursalId
WHERE proveedor.ProveedorId = @ProveedorId
AND p.Estado NOT IN ('ANULADO')--, 'ENPROCESO')
AND s.SucursalId = @SucursalId
AND ((CAST(p.FechaCreacion AS DATE) BETWEEN @FechaInicial AND @FechaFinal
AND @FiltrarPorFechas = 1)
OR @FiltrarPorFechas = 0)
ORDER BY p.Estado, p.FechaCreacion DESC
