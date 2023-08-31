CREATE PROCEDURE [dbo].[spObtenerPagoDescargadores]
	@PagoDescargasId INT
AS
	--CODIGO	
	SELECT pdd.FormaDePago, 
		   b.Descripcion AS 'NombreBanco', 
		   lc.CodigoLineaCredito, 
		   pdd.NoDocumento, 
		   pdd.Monto, 
		   pdd.FechaTransaccion
	FROM PagoDescargaDetalles pdd
		INNER JOIN PagoDescargadores pd
			ON pd.PagoDescargaId = pdd.PagoDescargaId
		INNER JOIN LineasCredito lc
			ON lc.LineaCreditoId = pdd.LineaCreditoId
		INNER JOIN CuentasFinancieras cf
			ON cf.CuentaFinancieraId = lc.CuentaFinancieraId
		LEFT JOIN Bancos b
			ON b.BancoId = cf.BancoId
	WHERE pd.PagoDescargaId = @PagoDescargasId
