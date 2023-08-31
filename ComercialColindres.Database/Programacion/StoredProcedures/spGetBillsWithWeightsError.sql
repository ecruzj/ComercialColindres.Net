CREATE PROCEDURE [dbo].[spGetBillsWithWeightsError]
	@PlantaId INT
AS
-- =============================================
-- Author:		<Josue Cruz>
-- Create date: <8 de Septiembre del 2019>
-- Description:	<Este codigo tiene como objetivo
--				 identificar facturas que sus toneladas 
--				 no cuadran con las toneladas de las boletas 
--				 asociadas a las facturas.>
-- =============================================

DECLARE @TotalFacturas INT,
@FacturaCursor INT = 1,
@FacturaId INT,
@AserrinNuevoId INT,
@SumarPenalidad BIT = 0

if OBJECT_ID('tempdb..#VentasBiomasa') IS NOT NULL DROP TABLE #VentasBiomasa
if OBJECT_ID('tempdb..#ComprasBiomasa') IS NOT NULL DROP TABLE #ComprasBiomasa
if OBJECT_ID('tempdb..#ComprasBiomasaFinal') IS NOT NULL DROP TABLE #ComprasBiomasaFinal
if OBJECT_ID('tempdb..#FinalReport') IS NOT NULL DROP TABLE #FinalReport
if OBJECT_ID('tempdb..#FacturasLog') IS NOT NULL DROP TABLE #FacturasLog

CREATE TABLE #VentasBiomasa (Id INT IDENTITY(1,1),
CategoriaProductoId INT,
Descripcion VARCHAR(50),
Toneladas DECIMAL(18,2))

CREATE TABLE #ComprasBiomasa (Id INT IDENTITY(1,1),
CategoriaProductoId INT,
Descripcion VARCHAR(50),
Toneladas DECIMAL(18,2))

CREATE TABLE #ComprasBiomasaFinal (Id INT IDENTITY(1,1),
CategoriaProductoId INT,
Descripcion VARCHAR(50),
Toneladas DECIMAL(18,2))

CREATE TABLE #FinalReport(Id INT IDENTITY(1,1),
Planta VARCHAR(50),
FacturaId INT,
NumeroFactura VARCHAR(10),
FechaEmision DATE,
Biomasa VARCHAR(50),
VentaTone DECIMAL(18,2),
CompraTone DECIMAL(18,2),
Moneda VARCHAR(3),
PrecioTone DECIMAL(18,2),
DiferenciaTone DECIMAL(18,2),
Saldo DECIMAL(18,2),
MustEvaluate BIT)

CREATE TABLE #FacturasLog(Id INT IDENTITY(1,1),
FacturaId INT)

SET @AserrinNuevoId = (SELECT
		CategoriaProductoId
	FROM CategoriaProductos
	WHERE Descripcion = 'Aserrin Nuevo')

SET @SumarPenalidad = (SELECT
		ISNULL(Valor, 0)
	FROM ConfiguracionesDetalle
	WHERE CodigoConfiguracion = 'SumarPenalidad'
	AND Atributo = @PlantaId)

INSERT INTO #FacturasLog (FacturaId)
	SELECT
		FacturaId
	FROM Facturas
	WHERE PlantaId = @PlantaId
	AND Estado <> 'NUEVO'

SET @TotalFacturas = (SELECT
		COUNT(*)
	FROM #FacturasLog)

WHILE (@FacturaCursor <= @TotalFacturas)
BEGIN
	SET @FacturaId = (SELECT
			FacturaId
		FROM #FacturasLog
		WHERE Id = @FacturaCursor)

	----**Ventas**----
	INSERT INTO #VentasBiomasa (CategoriaProductoId, Descripcion, Toneladas)
		SELECT
			cp.CategoriaProductoId
		   ,cp.Descripcion AS 'Producto'
		   ,fdi.Cantidad
		FROM dbo.FacturaDetalleItems fdi
		INNER JOIN dbo.CategoriaProductos cp
			ON cp.CategoriaProductoId = fdi.CategoriaProductoId
		WHERE fdi.FacturaId = @FacturaId
		ORDER BY cp.CategoriaProductoId, cp.Descripcion

	----**Compras**----
	INSERT INTO #ComprasBiomasa (CategoriaProductoId, Descripcion, Toneladas)
		SELECT
			cp.CategoriaProductoId
		   ,'Producto' = ---Se asocia los derivados del aserrín
			CASE (cp.Descripcion)
				WHEN 'Chip' THEN 'Aserrin Nuevo'
				WHEN 'Aserrin/Chip' THEN 'Aserrin Nuevo'
				WHEN 'Piller' THEN 'Aserrin Nuevo'
				ELSE cp.Descripcion
			END
		   ,'Toneladas' =
			CASE (@SumarPenalidad)
				WHEN 1 THEN SUM(b.PesoProducto + b.CantidadPenalizada)
				ELSE SUM(b.PesoProducto)
			END
		FROM dbo.FacturaDetalleBoletas fdb
		INNER JOIN dbo.Boletas b
			ON b.BoletaId = fdb.BoletaId
		INNER JOIN dbo.CategoriaProductos cp
			ON cp.CategoriaProductoId = b.CategoriaProductoId
		WHERE FacturaId = @FacturaId
		GROUP BY cp.CategoriaProductoId
				,cp.Descripcion
		ORDER BY cp.CategoriaProductoId, cp.Descripcion

	UPDATE #ComprasBiomasa
	SET CategoriaProductoId = @AserrinNuevoId
	WHERE Descripcion = 'Aserrin Nuevo'

	----**Compras Final**----
	INSERT INTO #ComprasBiomasaFinal (CategoriaProductoId, Descripcion, Toneladas)
		SELECT
			CategoriaProductoId
		   ,Descripcion
		   ,SUM(Toneladas)
		FROM #ComprasBiomasa cb
		GROUP BY CategoriaProductoId
				,Descripcion
		ORDER BY CategoriaProductoId, Descripcion

	----***Reporte Final***-----
	INSERT INTO #FinalReport (Planta, FacturaId, NumeroFactura, FechaEmision, Biomasa, VentaTone, CompraTone, Moneda, PrecioTone, DiferenciaTone, Saldo, MustEvaluate)
		SELECT
			cp.NombrePlanta AS 'Planta'
		   ,f.FacturaId
		   ,f.NumeroFactura
		   ,f.Fecha AS 'FechaEmision'
		   ,vb.Descripcion
		   ,vb.Toneladas AS 'Venta'
		   ,ISNULL(cbf.Toneladas, 0) AS 'Compra'
		   ,'Moneda' =
			CASE (f.IsForeignCurrency)
				WHEN 1 THEN '$'
				ELSE 'Lps'
			END
		   ,fdi.Precio
		   ,ISNULL((vb.Toneladas - cbf.Toneladas), 0) AS 'DiferenciaTone'
		   ,'Saldo' =
			CASE
				WHEN ISNULL((vb.Toneladas - cbf.Toneladas), 0) >= 0 THEN 0
				WHEN ISNULL((vb.Toneladas - cbf.Toneladas), 0) < 0 THEN ROUND(ABS(vb.Toneladas - cbf.Toneladas) * fdi.Precio, 2)
			END
		   ,'MustEvaluate' =
			CASE
				WHEN ISNULL(cbf.Toneladas, 0) = 0 THEN 1
				WHEN ISNULL((vb.Toneladas - cbf.Toneladas), 0) >= 0 THEN 0
				ELSE 1
			END
		FROM #VentasBiomasa vb
		INNER JOIN dbo.FacturaDetalleItems fdi
			ON fdi.CategoriaProductoId = vb.CategoriaProductoId
		INNER JOIN dbo.Facturas f
			ON f.FacturaId = fdi.FacturaId
		INNER JOIN ClientePlantas cp
			ON cp.PlantaId = f.PlantaId
		LEFT JOIN #ComprasBiomasaFinal cbf
			ON cbf.CategoriaProductoId = vb.CategoriaProductoId
		WHERE f.FacturaId = @FacturaId

	TRUNCATE TABLE #VentasBiomasa
	TRUNCATE TABLE #ComprasBiomasa
	TRUNCATE TABLE #ComprasBiomasaFinal

	SET @FacturaCursor = @FacturaCursor + 1;
END

SELECT
	*
FROM #FinalReport
WHERE MustEvaluate = 1
ORDER BY NumeroFactura

DROP TABLE #VentasBiomasa
DROP TABLE #ComprasBiomasa
DROP TABLE #ComprasBiomasaFinal
DROP TABLE #FinalReport
DROP TABLE #FacturasLog