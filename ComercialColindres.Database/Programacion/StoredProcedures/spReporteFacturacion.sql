CREATE PROCEDURE [dbo].[spReporteFacturacion]
	@FacturaId INT = 0
AS
	--CODIGO
	IF OBJECT_ID('tempdb..#FACTURACIONES') IS NOT NULL DROP TABLE #FACTURACIONES
	CREATE TABLE #FACTURACIONES (Planta VARCHAR(50),
								 Empresa VARCHAR(50),
								 TipoFactura VARCHAR(10),
								 NumeroFactura VARCHAR(50),
								 FechaFacturacion Date,
								 TotalFactura Decimal(18,2),
								 Observaciones VARCHAR(max),
								 Estado VARCHAR(10),
								 CodigoBoleta VARCHAR(10),
								 NumeroEnvio VARCHAR(10),
								 Proveedor VARCHAR(50),
								 PlacaEquipo VARCHAR (10),
								 Motorista VARCHAR(50),
								 TipoProducto VARCHAR(50),
								 PesoProducto DECIMAL(18,2),
								 PrecioCompra DECIMAL(18,2),
								 PrecioVenta DECIMAL(18,2),
								 TotalBoleta DECIMAL(18,2),
								 FechaSalida DATE)
							 
	INSERT INTO #FACTURACIONES
	SELECT Planta.NombrePlanta,
		   Sucursal.Nombre,
		   FACT.TipoFactura,
		   FACT.NumeroFactura,
		   FACT.Fecha,
		   FACT.Observaciones,
		   FACT.Estado,
		   Boleta.CodigoBoleta,
		   Boleta.NumeroEnvio,
		   Proveedor.Nombre,
		   Boleta.PlacaEquipo,
		   Boleta.Motorista,
		   CP.Descripcion,
		   Boleta.PesoProducto,
		   Boleta.PrecioProductoCompra,
		   Boleta.PrecioProductoVenta,
		   Boleta.PesoProducto * Boleta.PrecioProductoVenta AS TotalBoleta,
		   Boleta.FechaSalida
	FROM Facturas FACT
		INNER JOIN Sucursales Sucursal ON
			Sucursal.SucursalId = FACT.SucursalId
		INNER JOIN FacturaDetalleBoletas FDB ON
			FDB.FacturaId = FACT.FacturaId
		INNER JOIN Boletas Boleta ON
			Boleta.BoletaId = FDB.BoletaId
		INNER JOIN ClientePlantas Planta ON
			Boleta.PlantaId = Planta.PlantaId
		INNER JOIN CategoriaProductos CP ON
			CP.CategoriaProductoId = Boleta.CategoriaProductoId
		INNER JOIN Proveedores Proveedor ON
			Proveedor.ProveedorId = Boleta.ProveedorId
	WHERE FACT.FacturaId = @FacturaId

	SELECT * FROM #FACTURACIONES

	DROP TABLE #FACTURACIONES
	
