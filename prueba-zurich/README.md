
## Índice

* [Instalación](#instalación)
* [Uso básico](#uso-basico)
* [Qué incluye](#qué-incluye)
* [Versionamiento](#Versionamiento)
* [Servidor de desarrollo](#servidor-de-desarrollo)
* [Andamiaje de código](#andamiaje-de-código)
* [Copyright y licencia](#copyright-y-licencia)
* [Building](#building)
* [Ejecución de pruebas unitarias](#Ejecución-de-pruebas-unitarias)
* [Copyright y licencia](#copyright-y-licencia)

#### <i>Prerrequisitos</i>

Antes de comenzar, asegúrese de que su entorno de desarrollo incluya `Node.js®` y un administrador de paquetes `npm`.

###### Node.js

[**Angular 20**](https://angular.io/guide/what-is-angular) requires `Node.js` LTS version `^20.19.0 || ^22.12.0 || ^24.0.0`.

- Para comprobar tu versión, run `node -v` en una ventana terminal/console.
- Para descargar `Node.js`, ir a [nodejs.org](https://nodejs.org/).

###### Angular CLI

Instale Angular CLI globalmente usando una ventana terminal/console.

```bash
npm install -g @angular/cli
```

### Instalación

``` bash
$ npm install
$ npm update
```

### Uso básico

``` bash
# dev server with hot reload at http://localhost:4200
$ npm start
```

Navega a [http://localhost:4200](http://localhost:4200). La aplicación se recargará automáticamente si cambia alguno de los archivos de origen.

#### Build

Ejecuta `build` para construir el proyecto. Los artefactos de construcción se almacenarán en el directorio `dist/`.

```bash
# Construir para producción con minificación
$ npm run build
```

## Qué incluye
Verás algo como esto:

```
prueba-zurich
├── src/                         # project root
│   ├── app/                     # directorio principal de la aplicación
|   │   ├── guards/              # guards para restringir páginas
|   │   ├── icons/               # conjunto de iconos para la aplicación
|   │   ├── layout/              # layout 
|   |   │   └── default-layout/  # layout components
|   |   |       └── _nav.js      # configuración de navegación de la barra lateral
|   │   ├── models/              # modelos de la aplicación
|   │   ├── services/            # servicios de la aplicación
|   │   ├── shared/              # objetos compartidos
|   │   ├── store/               # stores para manejo de estado
|   │   ├── views/               # vistas de la aplicación
|   │   └── models/              # application models
│   ├── assets/                  # imagenes, iconos, etc.
│   ├── scss/                    # estilos scss
│   └── index.html               # html template
│
├── angular.json
├── README.md
└── package.json
```

## Versionamiento

Este proyecto se generó utilizando [Angular CLI](https://github.com/angular/angular-cli) versión 20.0.2.

## Servidor de desarrollo

Para iniciar un servidor de desarrollo local, ejecute:

```bash
ng serve
```

Una vez que el servidor esté en funcionamiento, abra su navegador y navegue a `http://localhost:4200/`. La aplicación se recargará automáticamente al modificar cualquier archivo fuente.

## Andamiaje de código

Angular CLI incluye potentes herramientas de andamiaje de código. Para generar un nuevo componente, ejecute:

```bash
ng generate component component-name
```

Para obtener una lista completa de los esquemas disponibles (como `components`, `directives` o `pipes`), ejecute:

```bash
ng generate --help
```

## Building

Para construir el proyecto ejecute:

```bash
ng build
```

Esto compilará tu proyecto y almacenará los artefactos de compilación en el directorio `dist/`. De forma predeterminada, la compilación de producción optimiza el rendimiento y la velocidad de tu aplicación.

## Ejecución de pruebas unitarias

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Copyright and License

copyright 2025.
