
/*

CREATE TABLE Usuarios(
IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL,
Apellido NVARCHAR(100) NOT NULL,
NombreUsuario NVARCHAR(50) UNIQUE,
Contraseña NVARCHAR(255) NOT NULL,  -- Almacena la contraseña cifrada
Email NVARCHAR(100) UNIQUE NOT NULL,
FechaRegistro DATE DEFAULT GETDATE(),
Estado INT CHECK(Estado IN (0, 1)) DEFAULT 1,
Rol NVARCHAR(50) CHECK(Rol IN ('Admin', 'Profesor', 'Estudiante')) NOT NULL
);


CREATE TABLE Estudiantes (
IdEstudiantes INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL,
Apellido NVARCHAR(100) NOT NULL,
FechaNacimiento DATE NOT NULL,
Telefono NVARCHAR(15),
Estado INT CHECK (Estado IN (0, 1)),
IdUsuario INT,  -- Relación con la tabla Usuarios
FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);


CREATE TABLE Materias (
IdMaterias INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL,
Descripcion NVARCHAR(MAX),
Creditos INT NOT NULL,
IdUsuario int
);

CREATE TABLE Inscripciones (
IdInscripciones INT IDENTITY(1,1) PRIMARY KEY,
IdEstudiantes INT NOT NULL,  -- Relación con la tabla Estudiantes
IdMaterias INT NOT NULL,  -- Relación con la tabla Materias
FechaInscripcion DATE NOT NULL,
Estado NVARCHAR(50) CHECK (Estado IN ('Pendiente', 'Confirmada', 'Cancelada')) DEFAULT 'Pendiente',
FOREIGN KEY (IdEstudiantes) REFERENCES Estudiantes(IdEstudiantes),
FOREIGN KEY (IdMaterias) REFERENCES Materias(IdMaterias)
);
GO


CREATE TABLE Pagos (
IdPago INT IDENTITY(1,1) PRIMARY KEY,
IdInscripciones INT NOT NULL,  -- Relación con la tabla Inscripciones
Monto DECIMAL(10, 2) NOT NULL,
FechaPago DATE NOT NULL,
MetodoPago NVARCHAR(50) CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'Transferencia')) NOT NULL,
FOREIGN KEY (IdInscripciones) REFERENCES Inscripciones(IdInscripciones)
);



//Procedimientos almacenados para estudiantes
CREATE PROCEDURE listar_estudiantes
AS
BEGIN
SELECT 
    e.IdEstudiantes,
    e.Nombre,
    e.Apellido,
    e.FechaNacimiento,
    e.Telefono,
    e.Estado,
    STRING_AGG(m.Nombre, ', ') AS Materias
FROM 
    Estudiantes e
LEFT JOIN 
    EstudiantesMaterias em ON e.IdEstudiantes = em.IdEstudiante
LEFT JOIN 
    Materias m ON em.IdMateria = m.IdMaterias
GROUP BY 
    e.IdEstudiantes, e.Nombre, e.Apellido, e.FechaNacimiento, e.Telefono, e.Estado
ORDER BY 
    e.IdEstudiantes ASC;
END

//Insertar
CREATE PROCEDURE insertar_estudiante
@Nombre NVARCHAR(100),
@Apellido NVARCHAR(100),
@FechaNacimiento DATE,
@Telefono NVARCHAR(15),
@Estado INT,
@IdMateria INT  
AS
BEGIN
BEGIN TRANSACTION;

BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM Materias WHERE IdMaterias = @IdMateria)
    BEGIN
        -- Si la materia no existe, lanzar un error
        THROW 50000, 'La materia especificada no existe.', 1;
END

    INSERT INTO Estudiantes (Nombre, Apellido, FechaNacimiento, Telefono, Estado, IdMateria)
    VALUES (@Nombre, @Apellido, @FechaNacimiento, @Telefono, @Estado, @IdMateria);

    DECLARE @IdEstudiante INT;
SET @IdEstudiante = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT 
        e.IdEstudiantes,
e.Nombre,
e.Apellido,
e.FechaNacimiento,
e.Telefono,
e.Estado,
m.Nombre AS MateriaNombre,
m.Descripcion AS MateriaDescripcion,
m.Creditos AS MateriaCreditos
    FROM Estudiantes e
    INNER JOIN Materias m ON e.IdMateria = m.IdMaterias
    WHERE e.IdEstudiantes = @IdEstudiante;
END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION;

    THROW;
END CATCH
END
GO

//Actualizar
CREATE PROCEDURE Actualizar_estudiante
@IdEstudiantes INT,
@Nombre VARCHAR(100),
@Apellido VARCHAR(100),
@FechaNacimiento DATE,
@Telefono VARCHAR(15),
@Estado INT,
@IdMateria INT -- Parámetro para actualizar la materia
AS
BEGIN
UPDATE Estudiantes
SET Nombre = @Nombre,
    Apellido = @Apellido,
    FechaNacimiento = @FechaNacimiento,
    Telefono = @Telefono,
    Estado = @Estado,
    IdMateria = @IdMateria
WHERE IdEstudiantes = @IdEstudiantes;
END

//Inicio de sesión
create proc iniciosesion
@Nombre NVARCHAR(100),
@Contraseña NVARCHAR(255),
@mensaje NVARCHAR(30) OUTPUT
AS
BEGIN
IF NOT EXISTS (
    SELECT 1 
    FROM Usuarios 
    WHERE NombreUsuario = @Nombre AND Contraseña = @Contraseña
)
BEGIN
    SET @mensaje = 'Datos incorrectos';
END
ELSE
BEGIN
    SET @mensaje = 'Bienvenid@: ' + @Nombre;
END
END;



////Usuarios
CREATE PROC listar_usuarios
AS
SELECT IdUsuario, Nombre, Apellido, NombreUsuario, Email, Estado, Rol 
FROM Usuarios
ORDER BY IdUsuario ASC;


//Buscar
CREATE PROC buscar_usuarios
@valor NVARCHAR(50)
AS
SELECT IdUsuario, Nombre, Apellido, NombreUsuario, Email, Estado, Rol 
FROM Usuarios
WHERE Nombre LIKE '%' + @valor + '%' 
OR Apellido LIKE '%' + @valor + '%' 
OR NombreUsuario LIKE '%' + @valor + '%' 
ORDER BY Nombre ASC;

//Insertar
CREATE PROC insertar_usuario
@Nombre NVARCHAR(100),
@Apellido NVARCHAR(100),
@NombreUsuario NVARCHAR(50),
@Contraseña NVARCHAR(255),
@Email NVARCHAR(100),
@Estado INT,
@Rol NVARCHAR(50)
AS
INSERT INTO Usuarios (Nombre, Apellido, NombreUsuario, Contraseña, Email, Estado, Rol)
VALUES (@Nombre, @Apellido, @NombreUsuario, @Contraseña, @Email, @Estado, @Rol);


//Actualizar
CREATE PROC actualizar_usuario
@IdUsuario INT,
@Nombre NVARCHAR(100),
@Apellido NVARCHAR(100),
@NombreUsuario NVARCHAR(50),
@Contraseña NVARCHAR(255),
@Email NVARCHAR(100),
@Estado INT,
@Rol NVARCHAR(50)
AS
UPDATE Usuarios
SET Nombre = @Nombre,
Apellido = @Apellido,
NombreUsuario = @NombreUsuario,
Contraseña = @Contraseña,
Email = @Email,
Estado = @Estado,
Rol = @Rol
WHERE IdUsuario = @IdUsuario;


//Eliminar  - Activar y Desactivar
CREATE PROC eliminar_usuario
@IdUsuario INT
AS
DELETE FROM Usuarios 
WHERE IdUsuario = @IdUsuario;

CREATE PROC desactivar_usuario
@IdUsuario INT
AS
UPDATE Usuarios 
SET Estado = 0
WHERE IdUsuario = @IdUsuario;

CREATE PROC activar_usuario
@IdUsuario INT
AS
UPDATE Usuarios 
SET Estado = 1
WHERE IdUsuario = @IdUsuario;



//Listar materias
CREATE PROCEDURE listar_materias
AS
BEGIN
SELECT 
    IdMaterias,
    Nombre,
    Descripcion,
    Creditos,
    IdUsuario
FROM 
    Materias
ORDER BY 
    IdMaterias ASC;
END;

//Buscar
CREATE PROCEDURE buscar_materias
@valor NVARCHAR(100)
AS
BEGIN
SELECT IdMaterias, Nombre, Descripcion, Creditos, IdUsuario
FROM Materias
WHERE Nombre LIKE '%' + @valor + '%' 
ORDER BY Nombre ASC;
END;
GO

//Insertar

CREATE PROCEDURE insertar_materia
@Nombre NVARCHAR(100),
@Descripcion NVARCHAR(MAX),
@Creditos INT 
AS
BEGIN

BEGIN TRANSACTION;

BEGIN TRY

    INSERT INTO Materias (Nombre, Descripcion, Creditos)
    VALUES (@Nombre, @Descripcion, @Creditos);

COMMIT TRANSACTION;
END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION;

THROW;
END CATCH
END
GO

//Actualizar
CREATE PROCEDURE Actualizar_materia
@IdMaterias INT,
@Nombre NVARCHAR(100),
@Descripcion NVARCHAR(MAX),
@Creditos INT
AS
BEGIN
-- Actualizar el registro en la tabla Materias
UPDATE Materias
SET Nombre = @Nombre,
    Descripcion = @Descripcion,
    Creditos = @Creditos
WHERE IdMaterias = @IdMaterias;
END
GO


//Eliminar
CREATE PROCEDURE eliminar_materia
@IdMaterias INT
AS
BEGIN
DELETE FROM Materias
WHERE IdMaterias = @IdMaterias;
END
GO


*/



