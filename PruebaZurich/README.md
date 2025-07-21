# PruebaZurich - Sistema de Gestión de Seguros

![.NET Core](https://img.shields.io/badge/.NET-8.0)
![Angular](https://img.shields.io/badge/Angular-20)
![SQL Server](https://img.shields.io/badge/SQL_Server-2021)

Sistema integral para la gestión de clientes, pólizas de seguros y usuarios, desarrollado con .NET 8 y Angular 20.

## 📋 Tabla de Contenidos
- [Requisitos](#-requisitos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Buenas Prácticas](#-buenas-prácticas)
- [Uso](#-uso)
- [API Endpoints](#-api-endpoints)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

## ⚙️ Requisitos

### 📦 Dependencias Principales
| Tecnología       | Versión  |
|------------------|----------|
| .NET SDK         | 8.0+     |
| Node.js          | 22.x+    |
| SQL Server       | 2021+    |
| Angular CLI      | 20.x+    |

### 🛠 Herramientas Recomendadas
- Visual Studio 2022 o VS Code
- SQL Server Management Studio
- Postman (para pruebas API)

## 🚀 Instalación

### Backend (.NET 8)
```bash
# Clonar repositorio
git clone https://github.com/Frank95G/PruebaZurich

# Navegar al directorio del proyecto
cd PruebaZurich

# Restaurar paquetes NuGet
dotnet restore

# Estrcutura del Proyecto
PruebaZurich/
├── Controllers/          # Endpoints API
│   ├── AuthController.cs # Autenticación JWT
│   ├── ClientesController.cs
│   └── PolizasController.cs
├── Data/
│   ├── Context/          # Configuración EF Core
│   ├── Entities/         # Modelos de BD
│   ├── Repositories/     # Implementación Repository
│   └── Initializers/     # Datos iniciales
├── Models/
│   └── DTOs/             # Objetos de transferencia
├── Services/             # Lógica de negocio
├── Exceptions/           # Excepciones personalizadas
└── Mapping/              # Perfiles AutoMapper

# Buenas Prácticas

## Implementadas
 - Clean Architecture: Separación clara de responsabilidades
 - Repository Pattern: IRepository<T> con implementación genérica
 - CQRS: Segregación de consultas y comandos
 - DTOs: Transferencia segura de datos
 - Validaciones:
 - DataAnnotations en DTOs
 - FluentValidation para reglas complejas
 - JWT: Autenticación stateless
 - Logging Estructurado: Serilog con enriquecimiento de contexto
 - Migraciones: Control de versiones de esquema


