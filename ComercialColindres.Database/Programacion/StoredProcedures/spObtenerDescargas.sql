CREATE PROCEDURE [dbo].[spObtenerDescargas]
	@PagoDescargasId INT
AS
--CODIGO
	SELECT
	pDescargas.CodigoPagoDescarga, pDescargas.FechaPago, c.NombreEncargado AS 'Cuadrilla', ISNULL(planta.NombrePlanta, 'PENDIENTE') AS 'Planta',
	pDescargas.Estado, dpa.CodigoBoleta, ISNULL(b.NumeroEnvio, '') AS 'NumeroEnvio', ISNULL(p.Nombre, 'PENDIENTE') AS 'Proveedor', 
	ISNULL(b.PlacaEquipo, 'PENDIENTE') AS 'PlacaEquipo', ISNULL(b.Motorista, 'PENDIENTE') AS 'Motorista', 
	ISNULL(cp.Descripcion, 'PENDIENTE') AS 'TipoProducto', 'PENDIENTE' AS 'TipoEquipo', dpa.PrecioDescarga
	FROM DescargasPorAdelantado dpa
		INNER JOIN PagoDescargadores pDescargas
			ON pDescargas.PagoDescargaId = dpa.PagoDescargaId
		INNER JOIN Cuadrillas c
			ON c.CuadrillaId = pDescargas.CuadrillaId
		LEFT JOIN Boletas b
			ON b.CodigoBoleta = dpa.CodigoBoleta
			AND b.PlantaId = dpa.PlantaId
		LEFT JOIN ClientePlantas planta
			ON planta.PlantaId = c.PlantaId
		LEFT JOIN Proveedores p
			ON p.ProveedorId = b.ProveedorId
		LEFT JOIN CategoriaProductos cp
			ON cp.CategoriaProductoId = b.CategoriaProductoId
	WHERE dpa.PagoDescargaId = @PagoDescargasId
	UNION ALL
	SELECT	PD.CodigoPagoDescarga,
			PD.FechaPago,
			Cuadrilla.NombreEncargado AS 'Cuadrilla',
			CP.NombrePlanta AS 'Planta',
			PD.Estado,
			Boleta.CodigoBoleta,
			Boleta.NumeroEnvio,
			P.Nombre AS 'Proveedor',
			Boleta.PlacaEquipo,
			Boleta.Motorista,
			CProducto.Descripcion AS 'TipoProducto',
			ISNULL(EC.Descripcion, 'N/A') AS 'TipoEquipo',
			Descarga.PrecioDescarga
	FROM PagoDescargadores PD
		INNER JOIN Descargadores Descarga ON
			Descarga.PagoDescargaId = PD.PagoDescargaId
			AND Descarga.EsDescargaPorAdelanto = 0
		INNER JOIN Cuadrillas Cuadrilla ON
			Cuadrilla.CuadrillaId = Descarga.CuadrillaId
		INNER JOIN Boletas Boleta ON
			Boleta.BoletaId = Descarga.BoletaId		
		INNER JOIN ClientePlantas CP ON
			CP.PlantaId = Boleta.PlantaId
		INNER JOIN Proveedores P ON
			P.ProveedorId = Boleta.ProveedorId
		INNER JOIN CategoriaProductos CProducto ON
			CProducto.CategoriaProductoId = Boleta.CategoriaProductoId
		LEFT JOIN Equipos EQ
			ON EQ.PlacaCabezal = Boleta.PlacaEquipo
		LEFT JOIN EquiposCategorias EC ON
			EC.EquipoCategoriaId = EQ.EquipoCategoriaId
	WHERE PD.PagoDescargaId = @PagoDescargasId
	GROUP BY PD.CodigoPagoDescarga,
			PD.FechaPago,
			Cuadrilla.NombreEncargado,
			CP.NombrePlanta,
			PD.Estado,
			Boleta.CodigoBoleta,
			Boleta.NumeroEnvio,
			P.Nombre,
			Boleta.PlacaEquipo,
			Boleta.Motorista,
			CProducto.Descripcion,
			EC.Descripcion,
			Descarga.PrecioDescarga