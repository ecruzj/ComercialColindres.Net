BEGIN TRAN

--COMMIT
--ROLLBACK

DECLARE @TotalBoletas INT,
@BoletaCursor INT = 1,
@BoletaId INT,
@TotalFacturaCompra DECIMAL(18, 2),
@SecurityRate DECIMAL(18, 2),
--@IsInteger BIT = 1,
@DescargaProducto DECIMAL(18, 2),
@OrdenCombustible DECIMAL(18, 2),
@HumidityPayment DECIMAL(18, 2),
@DeduccionesNegativas DECIMAL(18, 2),
@Cuadrilla VARCHAR(50) = '',
@LineaCreditoId INT = 1681,
@TotalDeducciones DECIMAL(18, 2),
@TotalPagoBoleta DECIMAL(18, 2),

---Campos de Control
@FechaTransaccion DATETIME = GETDATE(),
@ModificadoPor VARCHAR(10) = 'ecruzj',
@TipoTransaccion VARCHAR(20) = 'ManualAdjustment',
@DescripcionTransaccion VARCHAR(10) = 'Added',
@TransactionUId UNIQUEIDENTIFIER = NEWID()

if OBJECT_ID('tempdb..#BoletasPendientes') is not null DROP TABLE #BoletasPendientes
CREATE TABLE #BoletasPendientes (Id INT IDENTITY(1,1), BoletaId INT, Actualizado BIT DEFAULT 0)

---deducciones
--1 Prestamo Efectivo
--2 Orden Combustible
--3 Descarga de Producto
--4 Tasa Seguridad
--5 Abono por Boleta
--6 Boleta Otras Deducciones
--7 Boleta Otros Ingresos
--8 Humedades

INSERT INTO #BoletasPendientes (BoletaId)
	SELECT
		b.BoletaId
	FROM Boletas b
	LEFT JOIN PagoPrestamos pg
		ON pg.BoletaId = b.BoletaId
	WHERE b.Estado = 'ACTIVO'
	AND b.BoletaId = 32866
	--AND b.FechaCreacionBoleta <= '2018-08-15' --->> Filtrar por fecha
	AND pg.PagoPrestamoId IS NULL

SET @TotalBoletas = (SELECT
		COUNT(*)
	FROM #BoletasPendientes)

WHILE (@BoletaCursor <= @TotalBoletas)
BEGIN
	SET @BoletaId = (SELECT BoletaId FROM #BoletasPendientes WHERE Id = @BoletaCursor)
	SET @TotalFacturaCompra = (SELECT ROUND(PrecioProductoCompra * PesoProducto, 2)	FROM dbo.Boletas WHERE BoletaId = @BoletaId)
	SET @SecurityRate = dbo.GetSecurityRate(@BoletaId)
	SET @HumidityPayment = dbo.GetHumidityPaymentByBoletaId(@BoletaId)
	SET @DescargaProducto = (SELECT	ISNULL(SUM(PrecioDescarga), 0) FROM dbo.Descargadores WHERE BoletaId = @BoletaId)
	SET @OrdenCombustible = (SELECT ISNULL(SUM(Monto), 0) FROM dbo.OrdenesCombustible WHERE BoletaId = @BoletaId)
	SET @DeduccionesNegativas = (SELECT ISNULL(SUM(ABS(Monto)), 0) FROM dbo.BoletaOtrasDeducciones WHERE BoletaId = @BoletaId AND Monto < 0)
	SET @TotalDeducciones = (SELECT dbo.GetBoletaDeductions(@BoletaId))
	SET @TotalPagoBoleta = (SELECT dbo.GetBoletaAmount(@BoletaId))

	IF (@TotalFacturaCompra > @TotalDeducciones)
	BEGIN

		--**Creating BoletaDetalles**--

		--Tasa Seguridad
		IF (@SecurityRate > 0)
		BEGIN
			INSERT INTO BoletaDetalles (CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			VALUES (@BoletaId, 4, @SecurityRate * -1, 'N/A', '', @TipoTransaccion, @DescripcionTransaccion, @ModificadoPor, @FechaTransaccion, @TransactionUId)
		END

		--Pago de Humedad
		IF (@HumidityPayment > 0)
		BEGIN
			INSERT INTO BoletaDetalles (CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			SELECT @BoletaId, 8, ((SELECT dbo.GetHumidityFactor(bhp.BoletaHumedadId) * b.PrecioProductoCompra) / 100) * -1, '', '#Envío ' + bh.NumeroEnvio + ' con Humedad del ' + CAST(bh.HumedadPromedio AS VARCHAR) + '% y Tolerancia del ' + CAST(bh.PorcentajeTolerancia AS VARCHAR) + '%',
				   @TipoTransaccion, @DescripcionTransaccion, @ModificadoPor, @FechaTransaccion, @TransactionUId
			FROM BoletasHumedad bh
				INNER JOIN BoletasHumedadPago bhp
					ON bhp.BoletaHumedadId = bh.BoletaHumedadId
				INNER JOIN Boletas b
					ON b.BoletaId = bhp.BoletaId
			WHERE bhp.BoletaId = @BoletaId

			UPDATE bh
				SET bh.Estado = 'CERRADO',
				bh.ModificadoPor = @ModificadoPor,
				bh.DescripcionTransaccion = @DescripcionTransaccion,
				bh.TipoTransaccion = @TipoTransaccion,
				bh.FechaTransaccion = @FechaTransaccion,
				bh.TransaccionUId = @TransactionUId
			FROM Boletas b
				INNER JOIN BoletasHumedadPago bhp
					ON bhp.BoletaId = b.BoletaId
				INNER JOIN BoletasHumedad bh
					ON bh.BoletaHumedadId = bhp.BoletaHumedadId
			WHERE b.BoletaId = @BoletaId

			---Transacciones de Boletas Humedad
			INSERT INTO BoletasHumedad_Transacciones
			(BoletaHumedadId, NumeroEnvio, PlantaId, BoletaIngresada, HumedadPromedio, PorcentajeTolerancia, FechaHumedad, Estado, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			SELECT bh.BoletaHumedadId, bh.NumeroEnvio, bh.PlantaId, bh.BoletaIngresada, bh.HumedadPromedio, bh.PorcentajeTolerancia, bh.FechaHumedad, bh.Estado, bh.TipoTransaccion, bh.DescripcionTransaccion, bh.ModificadoPor, bh.FechaTransaccion, bh.TransaccionUId
			FROM Boletas b
				INNER JOIN BoletasHumedadPago bhp
					ON bhp.BoletaId = b.BoletaId
				INNER JOIN BoletasHumedad bh
					ON bh.BoletaHumedadId = bhp.BoletaHumedadId
			WHERE b.BoletaId = @BoletaId
		END

		--Descarga Producto
		IF (@DescargaProducto > 0)
		BEGIN
			SET @Cuadrilla = (SELECT c.NombreEncargado FROM Cuadrillas c
				INNER JOIN Descargadores d
					ON d.CuadrillaId = c.CuadrillaId
				INNER JOIN Boletas b
					ON b.BoletaId = d.BoletaId
				WHERE b.BoletaId = @BoletaId)
			INSERT INTO BoletaDetalles (CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			VALUES (@BoletaId, 3, @DescargaProducto * -1, 'Descargado por ' + @Cuadrilla, '', @TipoTransaccion, @DescripcionTransaccion, @ModificadoPor, @FechaTransaccion, @TransactionUId)
		END

		--Ordenes Combustible
		IF (@OrdenCombustible > 0)
		BEGIN
			INSERT INTO BoletaDetalles (CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			SELECT @BoletaId, 2, oc.Monto * -1, '#Fact ' + oc.CodigoFactura, oc.Observaciones, @TipoTransaccion, @DescripcionTransaccion, @ModificadoPor, @FechaTransaccion, @TransactionUId
			FROM OrdenesCombustible oc
				INNER JOIN Boletas b
					ON b.BoletaId = oc.BoletaId
			WHERE b.BoletaId = @BoletaId
		END

		--Deducciones Negativas
		IF (@DeduccionesNegativas > 0)
		BEGIN
			INSERT INTO BoletaDetalles(CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
			SELECT @BoletaId, 6, bod.Monto, bod.MotivoDeduccion, bod.NoDocumento, @TipoTransaccion, @DescripcionTransaccion, @ModificadoPor, @FechaTransaccion, @TransactionUId
			FROM BoletaOtrasDeducciones bod
				INNER JOIN Boletas b
					ON b.BoletaId = bod.BoletaId
			WHERE b.BoletaId = @BoletaId
			AND Monto < 0
		END

		--BoletaDetalles_Transacciones
		INSERT INTO BoletaDetalles_Transacciones
			(PagoBoletaId, CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
		SELECT
			PagoBoletaId, CodigoBoleta, DeduccionId, MontoDeduccion, NoDocumento, Observaciones, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId
		FROM BoletaDetalles
		WHERE CodigoBoleta = @BoletaId

		--Actualizar Boleta
		UPDATE Boletas
			SET Estado = 'CERRADO',
			ModificadoPor = @ModificadoPor,
			TipoTransaccion = @TipoTransaccion,
			DescripcionTransaccion = @DescripcionTransaccion,
			FechaTransaccion = @FechaTransaccion,
			TransaccionUId = @TransactionUId
		WHERE BoletaId = @BoletaId

		--Boleta_Transacciones
		INSERT INTO Boletas_Transacciones 
			(BoletaId, CodigoBoleta, NumeroEnvio, ProveedorId, PlacaEquipo, Motorista, CategoriaProductoId, PlantaId, PesoEntrada, PesoSalida, PesoProducto, CantidadPenalizada, PrecioProductoCompra, 
			 PrecioProductoVenta, FechaSalida, FechaCreacionBoleta, Estado, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
		 SELECT 
			 BoletaId, CodigoBoleta, NumeroEnvio, ProveedorId, PlacaEquipo, Motorista, CategoriaProductoId, PlantaId, PesoEntrada, PesoSalida, PesoProducto, CantidadPenalizada, PrecioProductoCompra, 
			 PrecioProductoVenta, FechaSalida, FechaCreacionBoleta, Estado, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId
		 FROM Boletas
		 WHERE BoletaId = @BoletaId

		 --Creating BoletaCierres
		 INSERT INTO BoletaCierres
			(BoletaId, FormaDePago, LineaCreditoId, NoDocumento, Monto, TipoTransaccion, DescripcionTransaccion, ModificadoPor, FechaTransaccion, TransaccionUId)
		 VALUES
			(@BoletaId, 'Manual Adjustment', @LineaCreditoId, 'Manual Adjustment', @TotalPagoBoleta, @TipoTransaccion, @DescargaProducto, @ModificadoPor, @FechaTransaccion, @TransactionUId)

		UPDATE #BoletasPendientes
		SET Actualizado = 1
		WHERE BoletaId = @BoletaId

	END

	SET @BoletaCursor = @BoletaCursor + 1
END

SELECT * FROM #BoletasPendientes
WHERE Actualizado = 0

DROP TABLE #BoletasPendientes