![Angular](https://img.shields.io/badge/Angular-20-red)
![License](https://img.shields.io/badge/License-MIT-green)

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

cd prueba-zurich

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