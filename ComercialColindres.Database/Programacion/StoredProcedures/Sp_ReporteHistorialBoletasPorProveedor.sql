CREATE PROCEDURE [dbo].[Sp_ReporteHistorialBoletasPorProveedor]
	@FechaInicial DATE,
	@FechaFinal DATE,
	@FiltrarPorFechas BIT,
	@ProveedorId INT
AS
	----------****HISTORIAL DE BOLETAS POR PROVEEDOR****----------
DECLARE @TotalBoletas INT
DECLARE @Cursor INT = 1
DECLARE @BoletaId INT
DECLARE @MontoCombustible DECIMAL(18,2), 
@MontoAbonoPrestamo DECIMAL(18,2),
@OtrasDeducciones DECIMAL(18,2),
@OtrosIngresos DECIMAL(18,2)

if OBJECT_ID('tempdb..#DatosIniciales') is not null DROP TABLE #DatosIniciales
if OBJECT_ID('tempdb..#DatosFinales') is not null DROP TABLE #DatosFinales

CREATE TABLE #DatosFinales (Id INT IDENTITY(1,1),
							NombreProveedor VARCHAR(50),
							BoletaId INT,
							CodigoBoleta VARCHAR(20),
							NumeroEnvio VARCHAR(20),
							Estado VARCHAR(20),	
							NombrePlanta VARCHAR(50),
							FechaSalida DATE,	
							Motorista VARCHAR(50),	
							PlacaEquipo VARCHAR(20),
							TipoProducto VARCHAR(50),
							PesoProducto DECIMAL(18,2),
							CantidadPenalizada DECIMAL(18,2),
							PrecioProductoCompra DECIMAL(18,2),
							TasaSeguridad DECIMAL(18,2),
							MontoCombustible DECIMAL(18,2),	
							PrecioDescarga DECIMAL(18,2),
							MontoAbonoPrestamo DECIMAL(18,2),
							OtrasDeducciones DECIMAL(18,2),
							OtrosIngresos DECIMAL(18,2),
							PagoCliente DECIMAL(18,2))
							
SELECT * INTO #DatosIniciales
FROM(
SELECT  p.Nombre AS 'NombreProveedor', boleta.BoletaId, boleta.CodigoBoleta, boleta.NumeroEnvio, 
		'Estado' =
		CASE (boleta.Estado)
			WHEN 'ACTIVO' THEN 'Pendiente'
			WHEN 'CERRADO' THEN 'Pagado'
			WHEN 'ENPROCESO' THEN 'EnProceso'
		END,
		cp.NombrePlanta, boleta.FechaSalida, 
		boleta.Motorista, boleta.PlacaEquipo, categoriaProducto.Descripcion AS 'TipoProducto', 
		boleta.PesoProducto, boleta.CantidadPenalizada, boleta.PrecioProductoCompra, 
		ISNULL(bd.MontoDeduccion, 0) AS 'TasaSeguridad',
		ISNULL(descarga.PrecioDescarga, 0) AS 'PrecioDescarga',
		ISNULL(bc.Monto,0) AS 'PagoCliente'
		FROM Boletas boleta
			INNER JOIN ClientePlantas cp
				ON cp.PlantaId = boleta.PlantaId
			INNER JOIN Proveedores p
				ON p.ProveedorId = boleta.ProveedorId
			INNER JOIN CategoriaProductos categoriaProducto
				ON categoriaProducto.CategoriaProductoId = boleta.CategoriaProductoId			
			LEFT JOIN BoletaDetalles bd
				ON bd.CodigoBoleta = boleta.BoletaId
					AND bd.DeduccionId = 4
			LEFT JOIN Descargadores descarga
				ON descarga.BoletaId = boleta.BoletaId
			LEFT JOIN BoletaCierres bc
				ON bc.BoletaId = boleta.BoletaId
		WHERE p.ProveedorId = @ProveedorId		
		AND ((boleta.FechaCreacionBoleta BETWEEN @FechaInicial AND @FechaFinal AND @FiltrarPorFechas = 1) OR @FiltrarPorFechas = 0)) datosIniciales
		
INSERT INTO #DatosFinales
SELECT
NombreProveedor,
BoletaId,
CodigoBoleta,
NumeroEnvio,
Estado,	
NombrePlanta,
FechaSalida,
Motorista,	
PlacaEquipo,
TipoProducto,
PesoProducto,
CantidadPenalizada,
PrecioProductoCompra,
TasaSeguridad,
0, --MontoCombustible
PrecioDescarga, 
0, --MontoAbonoPrestamo
0, --OtrasDeducciones
0, --OtrosIngresos
PagoCliente
FROM #DatosIniciales
GROUP BY NombreProveedor, BoletaId, CodigoBoleta, NumeroEnvio, Estado,	NombrePlanta, FechaSalida, Motorista, PlacaEquipo, TipoProducto,
PesoProducto, CantidadPenalizada, PrecioProductoCompra, TasaSeguridad, PrecioDescarga, PagoCliente

SET @TotalBoletas = (SELECT COUNT(*) FROM #DatosFinales)

WHILE (@Cursor <= @TotalBoletas)
BEGIN
	SET @BoletaId = (SELECT BoletaId FROM #DatosFinales WHERE Id = @Cursor)

	SET @MontoCombustible = (SELECT ISNULL(SUM(Monto) * -1, 0) FROM OrdenesCombustible WHERE BoletaId = @BoletaId)
	SET @MontoAbonoPrestamo = (SELECT ISNULL(SUM(MontoAbono) * -1, 0) FROM PagoPrestamos WHERE BoletaId = @BoletaId)
	SET @OtrasDeducciones = (SELECT ISNULL(SUM(Monto), 0) FROM BoletaOtrasDeducciones WHERE BoletaId = @BoletaId AND Monto < 0)
	SET @OtrosIngresos = (SELECT ISNULL(SUM(Monto), 0) FROM BoletaOtrasDeducciones WHERE BoletaId = @BoletaId AND Monto > 0)

	UPDATE #DatosFinales
	SET MontoCombustible = @MontoCombustible,
		MontoAbonoPrestamo = @MontoAbonoPrestamo,
		OtrasDeducciones = @OtrasDeducciones,
		OtrosIngresos = @OtrosIngresos
	WHERE BoletaId = @BoletaId

	SET @Cursor = @Cursor + 1
	
END

---Consulta Final
SELECT * FROM #DatosFinales

DROP TABLE #DatosIniciales
DROP TABLE #DatosFinales