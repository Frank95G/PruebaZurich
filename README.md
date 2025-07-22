https://img.shields.io/badge/.NET-8.0-blue
https://img.shields.io/badge/SQL_Server-2021-lightgrey
https://img.shields.io/badge/Angular-20-red
https://img.shields.io/badge/License-MIT-green

Sistema integral para la gestión de clientes y pólizas de seguros, con backend en .NET 8 y frontend en Angular 20.

📋 Tabla de Contenidos
Requisitos Técnicos

Instalación

Configuración

Estructura del Proyecto

Buenas Prácticas

Despliegue

API Reference

Contribución

Licencia

⚙️ Requisitos Técnicos
Backend (.NET 8)
Componente	Versión
.NET SDK	8.0+
SQL Server	2021+
Entity Framework	8.0+
Frontend (Angular 20)
Componente	Versión
Node.js	20.x+
Angular CLI	20.x+
TypeScript	5.0+
🚀 Instalación
1. Clonar repositorio
bash
git clone https://github.com/Frank95G/PruebaZurich.git
cd PruebaZurich
2. Configurar Backend
bash
cd Backend
dotnet restore
dotnet build
3. Configurar Frontend
bash
cd Frontend
npm install
npm start
🔧 Configuración
Base de Datos
Ejecutar script SQL inicial:

bash
sqlcmd -S [server] -U [user] -P [password] -d ZurichDB -i Database/init.sql
Configurar connection string en Backend/appsettings.json:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=[server];Database=ZurichDB;User Id=[user];Password=[password];TrustServerCertificate=true;"
}
Aplicar migraciones:

bash
dotnet ef database update
🏗 Estructura del Proyecto
text
PruebaZurich/
├── Backend/
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   ├── Data/
│   └── appsettings.json
├── Frontend/
│   ├── src/
│   │   ├── app/
│   │   ├── assets/
│   │   └── environments/
│   └── angular.json
└── Database/
    ├── init.sql
    └── schema.sql
✅ Buenas Prácticas Implementadas
Backend
Arquitectura limpia (Clean Architecture)

Patrón Repository + Unit of Work

Segregación CQRS

Validaciones con FluentValidation

Autenticación JWT

Logging estructurado con Serilog

Frontend
Lazy loading de módulos

State management con NgRx

Interceptores HTTP

Guards de ruta

Componentes reutilizables

🌐 API Reference
Método	Endpoint	Descripción
POST	/api/auth/login	Autenticación de usuarios
GET	/api/clientes	Obtener todos los clientes
POST	/api/polizas	Crear nueva póliza
🤝 Cómo Contribuir
Haz fork del proyecto

Crea tu rama (git checkout -b feature/nueva-funcionalidad)

Haz commit de tus cambios (git commit -am 'Agrega nueva funcionalidad')

Haz push a la rama (git push origin feature/nueva-funcionalidad)

Abre un Pull Request

📜 Licencia
Este proyecto está licenciado bajo MIT License - ver el archivo LICENSE para más detalles.