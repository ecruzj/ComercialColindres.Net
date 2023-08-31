CREATE PROCEDURE [dbo].[Sp_ReportePrestamosDetallePorProveedor]
	@SucursalId INT,
	@FechaInicial DATE,
	@FechaFinal DATE,
	@FiltrarPorFechas BIT,
	@ProveedorId INT
AS

DECLARE @TotalBoletas INT
DECLARE @Cursor INT = 1
DECLARE @BoletaId INT
DECLARE @MontoCombustible DECIMAL(18,2)
DECLARE @MontoAbonoPrestamo DECIMAL(18,2)
DECLARE @OtrasDeducciones DECIMAL(18,2)
DECLARE @OtrosIngresos DECIMAL(18,2)
DECLARE @BoletaPayment DECIMAL (18,2)

if OBJECT_ID('tempdb..#DatosFinales') is not null DROP TABLE #DatosFinales

CREATE TABLE #DatosFinales (
	Id INT IDENTITY (1, 1)
   ,BoletaId INT
   ,CodigoBoleta VARCHAR(20)
   ,NumeroEnvio VARCHAR(MAX)
   ,Estado VARCHAR(20)
   ,NombrePlanta VARCHAR(50)
   ,FechaSalida DATE
   ,Motorista VARCHAR(50)
   ,PlacaEquipo VARCHAR(20)
   ,TipoProducto VARCHAR(50)
   ,PesoProducto DECIMAL(18, 2)
   ,CantidadPenalizada DECIMAL(18, 2)
   ,PrecioProductoCompra DECIMAL(18, 2)
   ,TasaSeguridad DECIMAL(18, 2)
   ,MontoCombustible DECIMAL(18, 2)
   ,MontoAbono DECIMAL(18, 2)
   ,PagoCliente DECIMAL(18, 2)
   ,PrecioDescarga DECIMAL(18, 2)
   ,OtrasDeducciones DECIMAL(18, 2)
   ,OtrosIngresos DECIMAL(18, 2)
   ,CodigoPrestamo VARCHAR(10)
)

INSERT INTO #DatosFinales (BoletaId, CodigoBoleta, NumeroEnvio, Estado,
NombrePlanta, FechaSalida, Motorista, PlacaEquipo,
TipoProducto, PesoProducto, CantidadPenalizada, PrecioProductoCompra,
TasaSeguridad, MontoCombustible, MontoAbono, PagoCliente,
PrecioDescarga, OtrasDeducciones, OtrosIngresos, CodigoPrestamo)

	-----------****DETALLE DE PRESTAMOS ABONOS POR BOLETAS****----------
	SELECT
		boleta.BoletaId
	   ,boleta.CodigoBoleta
	   ,boleta.NumeroEnvio
	   ,boleta.Estado
	   ,cp.NombrePlanta
	   ,boleta.FechaSalida
	   ,boleta.Motorista
	   ,boleta.PlacaEquipo
	   ,categoriaProducto.Descripcion --AS 'TipoProducto'
	   ,boleta.PesoProducto
	   ,boleta.CantidadPenalizada
	   ,boleta.PrecioProductoCompra
	   ,ISNULL(bd.MontoDeduccion, 0) --AS 'TasaSeguridad'
	   ,0
	   ,(pp.MontoAbono * -1) --AS 'MontoAbono'
	   ,0 --AS 'PagoCliente'
	   ,ISNULL(descarga.PrecioDescarga * -1, 0) --AS 'PrecioDescarga'
	   ,0
	   ,0
	   ,prestamo.CodigoPrestamo
	FROM Boletas boleta
	INNER JOIN Proveedores p
		ON p.ProveedorId = boleta.ProveedorId
	INNER JOIN ClientePlantas cp
		ON cp.PlantaId = boleta.PlantaId
	INNER JOIN CategoriaProductos categoriaProducto
		ON categoriaProducto.CategoriaProductoId = boleta.CategoriaProductoId
	INNER JOIN PagoPrestamos pp
		ON pp.BoletaId = boleta.BoletaId
	INNER JOIN Prestamos prestamo
		ON prestamo.PrestamoId = pp.PrestamoId
	INNER JOIN Sucursales s
		ON s.SucursalId = prestamo.SucursalId
	LEFT JOIN BoletaDetalles bd
		ON bd.CodigoBoleta = boleta.BoletaId
			AND bd.DeduccionId = 4
	LEFT JOIN Descargadores descarga
		ON descarga.BoletaId = boleta.BoletaId
	WHERE p.ProveedorId = @ProveedorId
	AND prestamo.SucursalId = @SucursalId
	AND prestamo.Estado NOT IN ('ANULADO', 'ENPROCESO')
	AND ((CAST(prestamo.FechaCreacion AS DATE) BETWEEN @FechaInicial AND @FechaFinal
	AND @FiltrarPorFechas = 1)
	OR @FiltrarPorFechas = 0)


SET @TotalBoletas = (SELECT
		COUNT(*)
	FROM #DatosFinales)

WHILE (@Cursor <= @TotalBoletas)
BEGIN
SET @BoletaId = (SELECT
		BoletaId
	FROM #DatosFinales
	WHERE Id = @Cursor)

SET @MontoCombustible = (SELECT
		ISNULL(SUM(Monto) * -1, 0)
	FROM OrdenesCombustible
	WHERE BoletaId = @BoletaId)
SET @OtrasDeducciones = (SELECT
		ISNULL(SUM(Monto), 0)
	FROM BoletaOtrasDeducciones
	WHERE BoletaId = @BoletaId
	AND Monto < 0)
SET @OtrosIngresos = (SELECT
		ISNULL(SUM(Monto), 0)
	FROM BoletaOtrasDeducciones
	WHERE BoletaId = @BoletaId
	AND Monto > 0)
SET @BoletaPayment = (SELECT ISNULL(SUM(Monto) *-1, 0) 
	FROM BoletaCierres WHERE BoletaId = @BoletaId)

UPDATE #DatosFinales
SET MontoCombustible = @MontoCombustible
   ,OtrasDeducciones = @OtrasDeducciones
   ,OtrosIngresos = @OtrosIngresos
   ,PagoCliente = @BoletaPayment
WHERE BoletaId = @BoletaId

SET @Cursor = @Cursor + 1
	
END


-----Obteniendo los Abonos realizados sin boletas----
INSERT INTO #DatosFinales (BoletaId, CodigoBoleta, NumeroEnvio, Estado,
NombrePlanta, FechaSalida, Motorista, PlacaEquipo,
TipoProducto, PesoProducto, CantidadPenalizada, PrecioProductoCompra,
TasaSeguridad, MontoCombustible, MontoAbono, PagoCliente,
PrecioDescarga, OtrasDeducciones, OtrosIngresos, CodigoPrestamo)
	SELECT
		0
	   ,'Depósito'
	   ,pp.Observaciones
	   ,pp.FormaDePago
	   ,'N/A'
	   ,pp.FechaTransaccion
	   ,'N/A'
	   ,'N/A'
	   ,'N/A'
	   ,0
	   ,0
	   ,0
	   ,0
	   ,0
	   ,pp.MontoAbono * -1
	   ,0
	   ,0
	   ,0
	   ,0
	   ,p.CodigoPrestamo
	FROM dbo.Prestamos p
	INNER JOIN dbo.PagoPrestamos pp
		ON pp.PrestamoId = p.PrestamoId
	LEFT JOIN dbo.Boletas b
		ON b.BoletaId = pp.BoletaId
	WHERE p.ProveedorId = @ProveedorId
	AND p.SucursalId = @SucursalId
	AND p.Estado NOT IN ('ANULADO', 'ENPROCESO')
	AND ((CAST(p.FechaCreacion AS DATE) BETWEEN @FechaInicial AND @FechaFinal
	AND @FiltrarPorFechas = 1)
	OR @FiltrarPorFechas = 0)
	AND pp.BoletaId IS NULL

----Consulta Final--------
SELECT
	*
FROM #DatosFinales
ORDER BY id DESC

DROP TABLE #DatosFinales