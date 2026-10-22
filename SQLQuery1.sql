-- Crear la base de datos solo si no existe
IF DB_ID('ExamenFinal') IS NULL
BEGIN
    CREATE DATABASE ExamenFinal;
END;
GO

USE ExamenFinal;
GO

-- Crear tb_paises solo si no existe
IF OBJECT_ID('dbo.tb_paises', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tb_paises(
        idpais CHAR(3) PRIMARY KEY,
        nompais VARCHAR(100) NOT NULL
    );
END;
GO

-- Insertar países solo si todavía no existen
IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'PER')
    INSERT INTO dbo.tb_paises VALUES ('PER', 'Perú');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'ARG')
    INSERT INTO dbo.tb_paises VALUES ('ARG', 'Argentina');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'CHL')
    INSERT INTO dbo.tb_paises VALUES ('CHL', 'Chile');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'COL')
    INSERT INTO dbo.tb_paises VALUES ('COL', 'Colombia');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'ECU')
    INSERT INTO dbo.tb_paises VALUES ('ECU', 'Ecuador');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'MEX')
    INSERT INTO dbo.tb_paises VALUES ('MEX', 'México');

IF NOT EXISTS (SELECT 1 FROM dbo.tb_paises WHERE idpais = 'USA')
    INSERT INTO dbo.tb_paises VALUES ('USA', 'Estados Unidos');

GO

-- Crear tb_Consumidor solo si no existe
IF OBJECT_ID('dbo.tb_Consumidor', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tb_Consumidor(
        dnicont CHAR(8) PRIMARY KEY,
        nomcont VARCHAR(40) NOT NULL,
        apecont VARCHAR(40) NOT NULL,
        emailcont VARCHAR(250) UNIQUE,
        idpais CHAR(3) FOREIGN KEY REFERENCES dbo.tb_paises(idpais)
    );
END;
GO

CREATE OR ALTER PROCEDURE usp_agrega_consumidor
    @dnicont   varchar(20),
    @nomcont   varchar(40),
    @apecont   varchar(40),
    @emailcont varchar(250),
    @idpais    varchar(3),
    @mensaje   varchar(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- DNI: numérico y de 8 dígitos
    IF @dnicont IS NULL OR LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%'
    BEGIN
        SET @mensaje = 'ERROR: El DNI debe ser numérico y de 8 dígitos.';
        RETURN;
    END

    -- DNI duplicado
    IF EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'ERROR: Ya existe un consumidor con ese DNI.';
        RETURN;
    END

    -- Nombre y apellido no vacíos
    IF @nomcont IS NULL OR LTRIM(RTRIM(@nomcont)) = ''
    BEGIN
        SET @mensaje = 'ERROR: El nombre no debe estar vacío.';
        RETURN;
    END

    IF @apecont IS NULL OR LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @mensaje = 'ERROR: El apellido no debe estar vacío.';
        RETURN;
    END

    -- País: tipo string y que exista
    IF @idpais IS NULL OR LTRIM(RTRIM(@idpais)) = ''
       OR NOT EXISTS (SELECT 1 FROM tb_paises WHERE idpais = @idpais)
    BEGIN
        SET @mensaje = 'ERROR: El país no es válido.';
        RETURN;
    END

    
    -- Email duplicado
    IF @emailcont IS NOT NULL AND EXISTS (SELECT 1 FROM tb_Consumidor WHERE emailcont = @emailcont)
    BEGIN
        SET @mensaje = 'ERROR: Ya existe un consumidor con ese email.';
        RETURN;
    END

    INSERT INTO tb_Consumidor (dnicont, nomcont, apecont, emailcont, idpais)
    VALUES (@dnicont, LTRIM(RTRIM(@nomcont)), LTRIM(RTRIM(@apecont)), @emailcont, @idpais);

    SET @mensaje = 'OK: Consumidor registrado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE usp_eliminar_consumidor
    @dnicont varchar(20),
    @mensaje varchar(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'ERROR: El DNI ' + ISNULL(@dnicont, '') + ' no existe en la tabla.';
        RETURN;
    END

    DELETE FROM tb_Consumidor WHERE dnicont = @dnicont;
    SET @mensaje = 'OK: Consumidor eliminado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE usp_listar_consumidor
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor
    ORDER BY apecont, nomcont;
END;
GO

CREATE OR ALTER PROCEDURE usp_obtener_consumidor
    @dnicont varchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor
    WHERE dnicont = @dnicont;
END;
GO

CREATE OR ALTER PROCEDURE usp_actualizar_consumidor
    @dnicont   varchar(20),
    @nomcont   varchar(40),
    @apecont   varchar(40),
    @emailcont varchar(250),
    @idpais    varchar(3),
    @mensaje   varchar(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'ERROR: El consumidor no existe.';
        RETURN;
    END

    IF @nomcont IS NULL OR LTRIM(RTRIM(@nomcont)) = ''
    BEGIN
        SET @mensaje = 'ERROR: El nombre no debe estar vacío.';
        RETURN;
    END

    IF @apecont IS NULL OR LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @mensaje = 'ERROR: El apellido no debe estar vacío.';
        RETURN;
    END

    IF @idpais IS NULL OR NOT EXISTS (SELECT 1 FROM tb_paises WHERE idpais = @idpais)
    BEGIN
        SET @mensaje = 'ERROR: El país no es válido.';
        RETURN;
    END

    IF @emailcont IS NOT NULL
       AND EXISTS (SELECT 1 FROM tb_Consumidor WHERE emailcont = @emailcont AND dnicont <> @dnicont)
    BEGIN
        SET @mensaje = 'ERROR: Ese email ya pertenece a otro consumidor.';
        RETURN;
    END

    UPDATE tb_Consumidor
    SET nomcont   = LTRIM(RTRIM(@nomcont)),
        apecont   = LTRIM(RTRIM(@apecont)),
        emailcont = @emailcont,
        idpais    = @idpais
    WHERE dnicont = @dnicont;

    SET @mensaje = 'OK: Consumidor actualizado correctamente.';
END;
GO

CREATE OR ALTER PROCEDURE usp_listar_paises
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idpais, nompais FROM tb_paises ORDER BY nompais;
END;
GO