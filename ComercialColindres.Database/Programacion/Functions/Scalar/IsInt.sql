CREATE FUNCTION [dbo].[IsInt]
(
@number VARCHAR(20)
)
RETURNS BIT
AS
BEGIN

   RETURN 
		   CASE
			WHEN ROUND(@number, 2) = ROUND(@number, 0) THEN 1 ELSE 0
		   END
END