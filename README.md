![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![SQL Server](https://img.shields.io/badge/SQL_Server-2021-lightgrey)
![License](https://img.shields.io/badge/License-MIT-green)
![Angular](https://img.shields.io/badge/Angular-20-red)
![License](https://img.shields.io/badge/License-MIT-green)

# PruebaZurich - Backend de Gestión de Seguros
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

## 1. Configuración de Base de datos
La base de datos se encuentra en Azure SQL Database y ya no es necesario modificar la conexión a menos que se requiera trabjara en local

El script de creación de base de datos con sus respectivos objetos se encuentra en la carpeta ZurichDB y se llama init.sql

Para ejecutar la bd en un servidor local ejecutar siguiente comando o el archivo ZurichDB/init.sql en SSMS:

sqlcmd -S [servidor] -U [usuario] -P [contraseña] -d ZurichDB -i ZurichDB/init.sql

## 2. Configurar Connection String
La base de datos se encuentra en Azure SQL Database ya no es necesario modificar la conexión a menos que se requiera trabjara en local

  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:serverpaco.database.windows.net,1433;Initial Catalog=ZurichDB;Persist Security Info=False;User ID=paco;Password=Admin123*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
 }
## 3. Aplicar Migraciones
dotnet ef database update

# 🏗 Estructura del Proyecto
PruebaZurich/
├── bin/                          # Archivos binarios compilados
├── Controllers/                  # Controladores API
│   ├── AuthController.cs         # Autenticación y autorización
│   ├── ClientesController.cs     # Gestión de clientes
│   └── PolizasController.cs      # Gestión de pólizas
├── Data/                         # Capa de acceso a datos
│   ├── Context/
│   │   └── ZurichDBContext.cs    # Contexto de Entity Framework
│   ├── Entities/                 # Entidades de la base de datos
│   ├── Initializers/             # Inicializadores de datos
│   ├── Repositories/             # Patrón Repositorio
│   │   ├── Implementations/      # Implementaciones concretas
│   │   └── Interfaces/           # Interfaces de repositorios
│   └── Migrations/               # Migraciones de Entity Framework
├── Models/                       # Modelos de la aplicación
│   └── DTOs/                     # Objetos de Transferencia de Datos
├── Mapping/                      # Configuraciones de mapeo (AutoMapper)
├── Services/                     # Lógica de negocio
│   ├── Implementations/          # Implementaciones de servicios
│   └── Interfaces/               # Interfaces de servicios
├── Exceptions/                   # Excepciones personalizadas
├── Properties/
│   └── launchSettings.json       # Configuración de lanzamiento
├── ZurichDB/                     # Scripts de base de datos
│   └── init.sql                  # Script de inicialización de BD
└── obj/                          # Objetos temporales de compilación
  
# ▶️ Configuración del Backend (API)
## Iniciar la aplicación con IIS Express
1. Abrir el proyecto en Visual Studio

2. Selecciona PruebaZurich como proyecto de inicio único.

  Esto iniciará automáticamente la aplicación usando IIS Express en el puerto configurado (44359).

  URL de acceso
  Una vez ejecutado, la API estará disponible en:
  https://localhost:44359/swagger/index.html

## Manejo de puertos alternativos
Si el puerto 44359 está ocupado y la aplicación se inicia con un puerto diferente:

1. Actualizar el Frontend
Modifica el archivo environment.ts en tu proyecto Angular para reflejar el nuevo puerto:

apiUrl: 'https://localhost:[nuevo-puerto]/api'

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


# Frontend para sistema de gestión de seguros desarrollado con Angular 20

## 📋 Tabla de Contenidos
- [Prerrequisitos](#-prerrequisitos)
- [Instalación](#-instalación)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Desarrollo](#-desarrollo)
- [Construcción](#-construcción)
- [Testing](#-testing)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

## ⚙️ Prerrequisitos

| Tecnología | Versión | Instalación |
|------------|---------|-------------|
| Node.js | ^20.19.0 \|\| ^22.12.0 \|\| ^24.0.0 | [Descargar](https://nodejs.org/) |
| Angular CLI | 20.x+ | `npm install -g @angular/cli@20.x` |

## 🚀 Instalación

# Clonar repositorio
git clone https://github.com/Frank95G/PruebaZurich.git
cd PruebaZurich/ZurichUI

# Instalar dependencias
npm install

# Actualizar paquetes
npm update

## 🏗 Estructura del Proyecto
zurich-frontend/
├── src/                         
│   ├── app/                     
│   │   ├── core/                # Funcionalidades centrales
│   │   │   ├── guards/          # Protección de rutas
│   │   │   ├── interceptors/    # Interceptores HTTP
│   │   │   └── services/        # Servicios globales
│   │   ├── modules/             # Módulos de funcionalidad
│   │   │   ├── clientes/        # Gestión de clientes
│   │   │   └── polizas/         # Gestión de pólizas
│   │   ├── shared/              # Componentes/compartidos
│   │   └── layout/              # Estructura visual
│   ├── assets/                  # Recursos estáticos
│   ├── environments/            # Configuraciones por entorno
│   └── styles/                  # Estilos globales
├── angular.json                 # Configuración Angular CLI
└── package.json                 # Dependencias

##💻 Desarrollo
# Iniciar servidor de desarrollo
ng serve

# Acceder en navegador
http://localhost:4200
El servidor recargará automáticamente al modificar archivos.

##🔨 Construcción
# Build de desarrollo
ng build

# Build de producción (optimizado)
ng build --configuration production
Los artefactos se generan en dist/.

##🧪 Testing
# Ejecutar pruebas unitarias
ng test

# Ejecutar pruebas e2e (requiere servidor corriendo)
ng e2e
##🤝 Contribución
Crear issue describiendo los cambios propuestos

Hacer fork del proyecto

Crear feature branch (git checkout -b feature/nueva-funcionalidad)

Hacer commit de los cambios (git commit -am 'Agrega nueva funcionalidad')

Hacer push al branch (git push origin feature/nueva-funcionalidad)

Abrir Pull Request

##📜 Licencia
Distribuido bajo licencia MIT. Ver LICENSE para más información.