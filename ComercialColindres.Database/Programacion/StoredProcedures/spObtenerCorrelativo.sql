CREATE PROCEDURE [dbo].[spObtenerCorrelativo]
	 @SucursalId int,
	 @CodigoCorrelativo VARCHAR(max),
	 @ActualizarCorrelativo bit,
	 @PrefijoControl varchar(max)=''
AS
	BEGIN 
		BEGIN TRANSACTION     
		--LETRAS VALIDAS PARA EL CORRELATIVO
        DECLARE @LetrasValidas CHAR(36) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ' ;

		--OBTENIENDO ULTIMOS DATOS ALMACENADOS
        DECLARE @Prefijo VARCHAR(MAX)
        DECLARE @Letra VARCHAR(MAX)
        DECLARE @Tamaño INT
        DECLARE @CorrelativoActual INT
		DECLARE @ControlPorRango INT
		DECLARE @CorrelativoInicialPermitido INT
		DECLARE @CorrelativoFinalPermitido INT

        SELECT
            @Prefijo = Prefijo,
            @Letra = Letra,
            @Tamaño = Tamaño,
            @CorrelativoActual = CorrelativoActual,
			@CorrelativoInicialPermitido = CorrelativoInicialPermitido,
			@CorrelativoFinalPermitido = CorrelativoFinalPermitido,
			@ControlPorRango = ControlarPorRango
        FROM
            dbo.Correlativos
        WHERE
            CodigoCorrelativo = @CodigoCorrelativo AND SucursalId = @SucursalId AND (Prefijo = @PrefijoControl OR ISNULL(@PrefijoControl, '') = '')

		IF (@ControlPorRango = 1 AND (@CorrelativoActual+1) NOT BETWEEN @CorrelativoInicialPermitido AND @CorrelativoFinalPermitido)
		BEGIN
			SELECT '' AS SiguienteCorrelativo
			ROLLBACK
			RETURN
		END
		
		--VERIFICANDO QUE SI NO EXISTE, ENTONCES SE AGREGA EL CORRELATIVO SOLO SI ES CONTROLADO CON PREFIJO
		IF (ISNULL(@CorrelativoActual, 0) = 0 AND
			ISNULL(@PrefijoControl, '') <> '' AND
			NOT EXISTS(	SELECT CorrelativoActual
						FROM  dbo.Correlativos 
						WHERE  CodigoCorrelativo = @CodigoCorrelativo AND SucursalId = @SucursalId AND (Prefijo = @PrefijoControl OR ISNULL(@PrefijoControl, '') = '')))
		BEGIN
			INSERT INTO Correlativos (CodigoCorrelativo, SucursalId, Prefijo, Letra, Tamaño, CorrelativoActual, ControlarPorPrefijo, ControlarPorRango)
				SELECT TOP 1 CodigoCorrelativo, SucursalId, @PrefijoControl, Letra, Tamaño, 0, ControlarPorPrefijo, ControlarPorRango
				FROM Correlativos
				WHERE CodigoCorrelativo = @CodigoCorrelativo AND SucursalId = @SucursalId
				ORDER BY CorrelativoId DESC

			SELECT
				@Prefijo = Prefijo,
				@Letra = Letra,
				@Tamaño = Tamaño,
				@CorrelativoActual = CorrelativoActual
			FROM
				dbo.Correlativos
			WHERE
				CodigoCorrelativo = @CodigoCorrelativo AND SucursalId = @SucursalId AND (Prefijo = @PrefijoControl OR ISNULL(@PrefijoControl, '') = '')
		END

		--DEFINIENDO VARIABLES GENERALES
		DECLARE @tamañoCorrelativo INT
		DECLARE @tamañoCorrelativoActual INT
		DECLARE @tamañoLetra INT
		DECLARE @relleno VARCHAR(MAX)
		DECLARE @correlativoFinal VARCHAR(MAX)
		
		SET @tamañoCorrelativo = @Tamaño - LEN(@Prefijo) 
		SET @tamañoLetra = LEN(@Letra)

		DECLARE @ultimoCorrelativoPermitido NUMERIC
		SET @ultimoCorrelativoPermitido = CAST(REPLICATE('9',
															@tamañoCorrelativo
															- @tamañoLetra) AS NUMERIC)
		--VERIFICANDO SI EL ID DEL CORRELATIVO YA LLEGO AL ULTIMO PERMITIDO
		IF @ultimoCorrelativoPermitido = @CorrelativoActual 
			BEGIN
				--SI YA LLEGO AL ULTIMO CORRELATIVO PERMITIDO ENTONCES OBTENER LA ULTIMA LETRA UTILIZADA PARA VER SI
				--SE CAMBIA LA LETRA O SE AGREGA UNA NUEVA
				SET @CorrelativoActual = 1	
		
				DECLARE @ultimaLetra CHAR(1)
				SET @ultimaLetra = SUBSTRING(@Letra, LEN(@Letra), 1)
		
				IF @ultimaLetra = 'Z' 
					BEGIN
						SET @Letra = @Letra + 'A'
					END
				ELSE 
					BEGIN		
						DECLARE @posicionLetra INT	
						SET @posicionLetra = CHARINDEX(@ultimaLetra,
														@LetrasValidas, 0)

						SET @Letra = SUBSTRING(@Letra, 1, LEN(@Letra) - 1)
							+ SUBSTRING(@LetrasValidas, @posicionLetra + 1, 1)	                
					END                
			END
		ELSE 
			BEGIN
				SET @CorrelativoActual = @CorrelativoActual + 1		
			END
		
		--CALCULANDO EL CORRELATIVO SIGUIENTE
		SET @tamañoCorrelativoActual = LEN(@CorrelativoActual)
		SET @tamañoLetra = LEN(@Letra)
		SET @relleno = REPLICATE('0',
									@tamañoCorrelativo - @tamañoCorrelativoActual
									- @tamañoLetra)
		SET @correlativoFinal = @Prefijo + @Letra + @relleno
			+ CAST(( @CorrelativoActual ) AS VARCHAR)

		--ACTUALIZANDO EL ULTIMO CORRELATIVO
		if @ActualizarCorrelativo=1
		begin
			UPDATE
				dbo.Correlativos
			SET 
				Letra = @Letra,
				CorrelativoActual = @CorrelativoActual
			WHERE
				CodigoCorrelativo = @CodigoCorrelativo AND SucursalId = @SucursalId AND (Prefijo = @PrefijoControl OR ISNULL(@PrefijoControl, '') = '')
		end
		COMMIT

		--RESULTADO DEL CORRELATIVO
		SELECT
			@correlativoFinal AS SiguienteCorrelativo    		
    END