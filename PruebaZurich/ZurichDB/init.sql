-- 1. Crear la base de datos
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'ZurichDB')
BEGIN
    ALTER DATABASE ZurichDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ZurichDB;
END
GO

CREATE DATABASE ZurichDB
GO

USE ZurichDB;
GO

-- 2. Se crean tablas con esquema
CREATE SCHEMA seg;
GO

-- Tabla de Tipos de Póliza (catálogo)
CREATE TABLE seg.TiposPoliza
(
    TipoPolizaId INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    CONSTRAINT PK_TiposPoliza PRIMARY KEY (TipoPolizaId)
);
GO

-- Tabla de Usuarios/Roles
CREATE TABLE seg.Usuarios
(
    UsuarioId INT IDENTITY(1,1) NOT NULL,
    NombreUsuario NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash VARBINARY(MAX) NOT NULL,
    PasswordSalt VARBINARY(MAX) NOT NULL,
    Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Administrador', 'Cliente')),
    FechaCreacion DATETIME2 NOT NULL DEFAULT (GETUTCDATE()),
    UltimoLogin DATETIME2 NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioId),
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email)
);
GO

-- Tabla de Clientes
CREATE TABLE seg.Clientes
(
    ClienteId INT IDENTITY(1,1) NOT NULL,
    UsuarioId INT NULL, -- Relación con usuario si tiene acceso al sistema
    Identificacion CHAR(10) NOT NULL UNIQUE CHECK (LEN(Identificacion) = 10 AND Identificacion NOT LIKE '%[^0-9]%'),
    Nombre NVARCHAR(100) NOT NULL CHECK (Nombre NOT LIKE '%[0-9]%' AND Nombre NOT LIKE '%[!@#$%^&*()]%'),
    Email NVARCHAR(100) NOT NULL UNIQUE CHECK (Email LIKE '%_@__%.__%'),
    Telefono NVARCHAR(20) NOT NULL,
    Direccion NVARCHAR(200) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT (GETUTCDATE()),
    FechaActualizacion DATETIME2 NULL,
    CONSTRAINT PK_Clientes PRIMARY KEY (ClienteId),
    CONSTRAINT UQ_Clientes_Identificacion UNIQUE (Identificacion),
    CONSTRAINT FK_Clientes_Usuarios FOREIGN KEY (UsuarioId) REFERENCES seg.Usuarios(UsuarioId)
);
GO

-- Tabla de Pólizas
CREATE TABLE seg.Polizas
(
    PolizaId INT IDENTITY(1,1) NOT NULL,
    ClienteId INT NOT NULL,
    TipoPolizaId INT NOT NULL,
    NumeroPoliza NVARCHAR(20) NOT NULL,
    FechaInicio DATETIME2 NOT NULL,
    FechaExpiracion DATETIME2 NOT NULL,
    MontoAsegurado DECIMAL(18,2) NOT NULL,
    Estado NVARCHAR(20) NOT NULL CHECK (Estado IN ('Activa', 'Cancelada', 'Solicitud Cancelar')) DEFAULT 'Activa',
    FechaEmision DATETIME2 NOT NULL DEFAULT (GETUTCDATE()),
    FechaCancelacion DATETIME2 NULL,
    MotivoCancelacion NVARCHAR(255) NULL,
    CONSTRAINT PK_Polizas PRIMARY KEY (PolizaId),
    CONSTRAINT FK_Polizas_Clientes FOREIGN KEY (ClienteId) REFERENCES seg.Clientes(ClienteId),
    CONSTRAINT FK_Polizas_TiposPoliza FOREIGN KEY (TipoPolizaId) REFERENCES seg.TiposPoliza(TipoPolizaId),
    CONSTRAINT UQ_Polizas_NumeroPoliza UNIQUE (NumeroPoliza),
    CONSTRAINT CHK_Polizas_Fechas CHECK (FechaExpiracion > FechaInicio),
    CONSTRAINT CHK_Polizas_Monto CHECK (MontoAsegurado > 0)
);
GO

-- 3. Se crean índices para mejorar el rendimiento
CREATE INDEX IX_Clientes_Email ON seg.Clientes(Email);
CREATE INDEX IX_Clientes_Nombre ON seg.Clientes(Nombre);
CREATE INDEX IX_Polizas_ClienteId ON seg.Polizas(ClienteId);
CREATE INDEX IX_Polizas_Estado ON seg.Polizas(Estado);
CREATE INDEX IX_Polizas_Fechas ON seg.Polizas(FechaInicio, FechaExpiracion);
GO

-- 4. Poblar datos iniciales
INSERT INTO seg.TiposPoliza (Nombre, Descripcion)
VALUES 
    ('Vida', 'Seguro de vida individual'),
    ('Automóvil', 'Seguro para vehículos automotores'),
    ('Salud', 'Seguro médico y de salud'),
    ('Hogar', 'Seguro para vivienda y contenido');
GO