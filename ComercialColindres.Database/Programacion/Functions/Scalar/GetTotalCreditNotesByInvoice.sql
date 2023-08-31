-- =============================================
-- Author:		<Josue Cruz>
-- Create date: <23-06-2022>
-- Description:	<displays the total sum of the credit notes that belong to an invoice>
-- =============================================
CREATE FUNCTION [dbo].[GetTotalCreditNotesByInvoice]
(@FacturaId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

DECLARE @TotalCreditNotes DECIMAL(18, 2)

SET @TotalCreditNotes = (SELECT
		ISNULL(SUM(Monto), 0)
	FROM NotasCredito
	WHERE FacturaId = @FacturaId)

	
	RETURN ROUND(@TotalCreditNotes, 2)
END