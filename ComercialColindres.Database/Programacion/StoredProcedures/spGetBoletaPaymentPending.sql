CREATE PROCEDURE [dbo].[spGetBoletaPaymentPending]
	@ProveedorId INT,
	@IsPartialPayment BIT = 0,
	@BoletasId tblBoletas READONLY	
AS
	SELECT
		b.BoletaId
		,b.CodigoBoleta
		,CASE 
		WHEN b.NumeroEnvio = '' THEN 'N/A'
		ELSE b.NumeroEnvio
		END NumeroEnvio
		,biomasa.Descripcion AS 'Biomasa'
		,b.PesoProducto
		,b.CantidadPenalizada
		,b.PrecioProductoCompra AS 'PrecioCompra'
		,cp.NombrePlanta AS 'Planta'
		,(b.PrecioProductoCompra * b.PesoProducto) AS 'TotalFactura'
		,ISNULL(dbo.GetBoletaDeductions(b.BoletaId), 0) AS 'DeduccionTotal'
		,ISNULL(dbo.GetBoletaAmount(b.BoletaId), 0) AS 'TotalPagar'
		,b.FechaSalida
	FROM Boletas b
	INNER JOIN ClientePlantas cp
		ON cp.PlantaId = b.PlantaId
	INNER JOIN CategoriaProductos biomasa
		ON biomasa.CategoriaProductoId = b.CategoriaProductoId
	WHERE ProveedorId = @ProveedorId
	AND b.Estado = 'ACTIVO'
	AND ((b.BoletaId IN (SELECT BoletaId FROM @BoletasId)
	AND @IsPartialPayment = 1)
	OR @IsPartialPayment = 0)