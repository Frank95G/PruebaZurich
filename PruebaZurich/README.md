# PruebaZurich - Backend de Gestión de Seguros

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![SQL Server](https://img.shields.io/badge/SQL_Server-2021-lightgrey)
![License](https://img.shields.io/badge/License-MIT-green)

Backend para sistema de gestión de seguros desarrollado con .NET 8 y SQL Server.

## 📋 Tabla de Contenidos
- [Requisitos](#-requisitos)
- [Instalación](#-instalación)
- [Configuración de Base de Datos](#-configuración-de-base-de-datos)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Buenas Prácticas](#-buenas-prácticas)
- [API Endpoints](#-api-endpoints)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

## ⚙️ Requisitos

### 📦 Dependencias Principales
| Tecnología       | Versión  | Instalación |
|------------------|----------|-------------|
| .NET SDK         | 8.0+     | [Descargar](https://dotnet.microsoft.com/download) |
| SQL Server       | 2021+    | [Descargar](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |

### 🛠 Herramientas Recomendadas
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Postman](https://www.postman.com/downloads/) (para pruebas API)

## 🚀 Instalación

# Clonar repositorio
git clone https://github.com/Frank95G/PruebaZurich.git
cd PruebaZurich

# Restaurar paquetes NuGet
dotnet restore

# Compilar solución
dotnet build

# 🗃 Configuración de Base de Datos

## 1. Ejecutar Script de Inicialización
Ejecute el siguiente comando o el archivo ZurichDB/init.sql en SSMS:

sqlcmd -S [servidor] -U [usuario] -P [contraseña] -d ZurichDB -i ZurichDB/init.sql

## 2. Configurar Connection String
Modificar en appsettings.json:

  json
  "ConnectionStrings": {
    "DefaultConnection": "Server=[servidor];Database=ZurichDB;User Id=[usuario];Password=[contraseña];TrustServerCertificate=true;"
  }

## 3. Aplicar Migraciones
dotnet ef database update

# 🏗 Estructura del Proyecto
  PruebaZurich/
  ├── Controllers/
  │   ├── AuthController.cs
  │   ├── ClientesController.cs
  │   └── PolizasController.cs
  ├── Data/
  │   ├── Context/
  │   ├── Entities/
  │   ├── Repositories/
  │   └── Migrations/
  ├── Models/
  │   └── DTOs/
  ├── Services/
  ├── Exceptions/
  └── Mapping/
  
# ✅ Buenas Prácticas
Clean Architecture: Separación de capas

Repository Pattern: IRepository<T> genérico

CQRS: Segregación de consultas/comandos

DTOs: Mapeo seguro con AutoMapper

Validaciones: FluentValidation + DataAnnotations

JWT Authentication: Seguridad implementada

Logging: Serilog con contexto

Migrations: Control de esquema EF Core

## 📡 API Endpoints
Método	Endpoint	Descripción
POST	/api/auth/login	Autenticación JWT
GET	/api/clientes	Listar clientes
POST	/api/clientes	Crear cliente
GET	/api/polizas	Listar pólizas
POST	/api/polizas	Crear póliza
## 🤝 Contribución
Crear issue describiendo los cambios

Hacer fork del proyecto

Crear feature branch (git checkout -b feature/nueva-funcionalidad)

Hacer commit de los cambios (git commit -am 'Agrega nueva funcionalidad')

Hacer push al branch (git push origin feature/nueva-funcionalidad)

Abrir Pull Request

## 📜 Licencia
Distribuido bajo licencia MIT. Ver LICENSE para más información.