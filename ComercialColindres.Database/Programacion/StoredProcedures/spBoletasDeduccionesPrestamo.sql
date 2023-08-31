CREATE PROCEDURE [dbo].[spBoletasDeduccionesPrestamo]
	@PrestamoId INT
AS
	SELECT prestamo.CodigoPrestamo, prestamo.MontoPrestamo, prestamo.PorcentajeInteres, prestamo.AutorizadoPor, prestamo.FechaCreacion, 
			   boleta.CodigoBoleta, planta.NombrePlanta, boleta.FechaSalida, boleta.Motorista, boleta.PlacaEquipo, categoriaProducto.Descripcion AS 'TipoProducto', 
			   boleta.PesoProducto, boleta.PrecioProductoCompra,
			   'MontoAbono' =
			   CASE deduccion.Descripcion
				WHEN 'Tasa Seguridad' 
					THEN pagoPrestamo.MontoAbono
					ELSE 0
			   END, 
			   deduccion.Descripcion AS 'DescripcionDeduccion', boletaDetalle.MontoDeduccion, boletaDetalle.NoDocumento, boletaDetalle.Observaciones AS 'ObservacionesDeduccion'
		FROM PagoPrestamos pagoPrestamo
			INNER JOIN Boletas boleta
				ON pagoPrestamo.BoletaId = boleta.BoletaId
			INNER JOIN CategoriaProductos categoriaProducto
				ON categoriaProducto.CategoriaProductoId = boleta.CategoriaProductoId
			INNER JOIN ClientePlantas planta
				ON planta.PlantaId = boleta.PlantaId
			INNER JOIN Prestamos prestamo
				ON prestamo.PrestamoId = pagoPrestamo.PrestamoId
			INNER JOIN BoletaDetalles boletaDetalle
				ON boletaDetalle.CodigoBoleta = boleta.BoletaId
				AND boletaDetalle.DeduccionId NOT IN (5)
			INNER JOIN Deducciones deduccion
				ON deduccion.DeduccionId = boletaDetalle.DeduccionId
		WHERE pagoPrestamo.PrestamoId = @PrestamoId
			AND boleta.Estado = 'CERRADO'