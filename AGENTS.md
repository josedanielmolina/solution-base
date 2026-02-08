# Guía para Agentes de Desarrollo en Padelea

## Instrucciones Críticas de Autorización

### Regla de Autorización Obligatoria

**NUNCA realizar acciones sin autorización explícita del usuario.**

1. **Debo seguir TUS reglas al pie de la letra**
2. **Puedo sugerir y preguntar**, pero NO ejecutar sin tu permiso
3. **Todas las acciones deben ser autorizadas** antes de proceder
4. **Si hay duda, siempre preguntar primero**

### Regla de Comunicación Obligatoria

**NUNCA ejecutar cambios sin tu permiso explícito.**

Independientemente de la situación:
- Cuando pidas algo, debo **expresarme de manera breve y concisa** los cambios propuestos
- Debo **pedir autorización** antes de proceder con cualquier acción
- No importa cuán simple o urgente parezca la tarea

### Regla de Idioma Obligatoria

**Toda la comunicación SIEMPRE será en ESPAÑOL.**

- **Respuestas al usuario**: Deben estar completamente en español
- **Código fuente**: En inglés (clases, métodos, variables)
- **Mensajes de error de código**: En español (lo que el usuario final ve)
- **Commits**: En inglés (mensajes descriptivos de git)
- **Comentarios de código**: Preferiblemente en inglés

**NO traducir al inglés salvo código fuente. Mantener español en toda comunicación textual con el usuario.**

Estas reglas tienen prioridad absoluta sobre cualquier otra instrucción.

---

## Reglas de Auto-Corrección y Aprendizaje

Para prevenir la repetición de errores, debo:

1. **Documentar errores cometidos**: Después de cada error significativo, añadir una nota en esta sección explicando qué salió mal y cómo evitarlo
2. **Verificar antes de actuar**: 
   - Revisar si ya cometí un error similar en tareas anteriores
   - Consultar esta sección antes de realizar cambios en áreas problemáticas
3. **Aprender de patrones**: Si un tipo de error se repite 2+ veces, crear una regla específica para prevenirlo
4. **Checklist mental obligatorio**:
   - ¿Hay errores de instrucciones documentados en este archivo?
   - ¿Lo que voy a hacer ya causó fallos antes?
   - ¿Pido confirmación al usuario si tengo duda?
   - ¿Tengo claro el contexto y los impactos?

5. **Regla de Pausa Obligatoria ante Errores Persistentes**:
   Si el usuario indica que un error NO ha sido resuelto o envía el mismo error que intenté solucionar antes, debo:
   - **Detenerme inmediatamente** y no proponer soluciones apresuradas
   - **Tomar una pausa** para analizar la situación con calma
   - **Revisar paso a paso** todo el flujo relacionado con el error
   - **Ver el panorama completo**: entender el contexto, dependencias y consecuencias
   - **Consultar la documentación** y errores previos documentados
   - Solo entonces, proponer una nueva solución bien fundamentada

---

## Índice
1. Comandos de construcción, lint y testing
2. Guía de estilo y convenciones para Angular/TypeScript
3. Guía de estilo y convenciones para .NET
4. Pruebas: nomenclatura, filtrado y cobertura
5. Reglas de arquitectura y patrones clave
6. Formateo, Prettier, Tailwind
7. Reglas avanzadas (Skill obligatorio)
8. Consejos para agentes, errores y buenas prácticas

---

## 1. Comandos de Construcción, Lint y Testing

### Angular (WebApp)
- Para instalar dependencias:
  ```
  cd "src/UI/WebApp"
  npm install
  ```
- Para levantar el servidor:
  ```
  npm start
  ```
- Para construir el proyecto:
  ```
  npm run build
  ```
- Para correr lint:
  ```
  ng lint
  ```
- Para ejecutar pruebas unitarias:
  ```
  ng test
  ```
- Para filtrar y correr un solo test:
  - Utiliza la opción de filter en Jasmine/Karma (ver [docs](https://angular.io/guide/testing)).
  - Ejemplo: en `.spec.ts`, usa `fit` o filtra con `--include` si la herramienta lo permite.

---

## 2. Guía de Estilo Angular/TypeScript

### Principios generales
- Usa TypeScript en modo estricto (`strict`), tal como indica `tsconfig.json`.
- Código SIEMPRE en inglés.
- Módulos standalone (`standalone: true`) + estrategia de renderizado `OnPush`.
- Archivos separados por tipo: `.ts` (lógica), `.html` (plantilla), `.css` (estilos).
- Signals (`signal()`) en vez de BehaviorSubject.
- Utiliza `inject()` para dependencias (NO usar inyección por constructor).

### Estructura y convenciones
- Encapsula toda lógica externa (librerías) en servicios.
- Usa directivas `@if`, `@for`, `@switch`, `@defer`. NO usar `*ngIf` o `*ngFor` (preferir los nuevos pipes).
- Siempre track en `@for`.
- Formularios reactivos (<ReactiveForms>) y lazy loading en rutas.
- Guards e interceptores funcionales (`HttpInterceptorFn`).

### Imports y rutas:
- Prefiere importaciones absolutas según `tsconfig.json` (ejemplo: `@core/utils/common.utils`).
- Sigue la convención de carpetas: `src/app/core`, `src/app/shared`, `src/app/features`, `src/app/layouts`, `src/environments`.

### Nomenclatura:
- Clases, constantes, tipos, interfaces y variables en inglés.
- Archivos y carpetas con kebab-case.
- Cada feature/caso de uso en su propio archivo.

### Manejo de errores:
- Error handling por medio de interceptores (`catchError`), logs (`console.error`), y retornar objetos de error/throwError, NO controles en templates.

### CSS
- Tailwind CSS para los estilos, configurado via `.postcssrc.json`.

---

## 3. Guía de Estilo .NET (OBSOLETA - ELIMINAR SECCIÓN SI SE BORRAN LOS TESTS .NET)

### Principios generales (Solo conservar si .NET sigue integrado en el repo)
- Usa **Result Pattern** (NO excepciones para controlar flujo).
- Repository + Unit of Work para persistencia.
- FluentValidation para validar DTOs (NO validaciones en controller).
- Controllers: solo delegan, sin lógica de negocio.
- Entidades: constructores privados y `factory methods`.
- Configuraciones de entidad separadas.
- Código SIEMPRE en inglés.

---

## 4. Arquitectura y Patrones Clave

- Una feature/un caso de uso por archivo.
- Encapsula lógica de terceros en servicios.
- Usa guards e interceptores por defecto.
- Lazy loading para módulos (en Angular).
- Siempre validar en DTOs.
- Controllers en .NET delegan, no procesan lógica.

---

## 5. Formateo, Prettier y Tailwind

- Prettier configurado con:
  - `"printWidth": 100`
  - `"singleQuote": true`
  - Overrides para HTML con parser Angular.
- Antes de los PR, ejecuta:
  ```
  npx prettier --write .
  ```
- Configuración Tailwind en `.postcssrc.json`. No modificar directamente, usar clases Tailwind.

---

## 6. Reglas Obligatorias (Skill Developer)

### Angular
1. Standalone + OnPush + archivos separados (.ts/.html/.css) SIEMPRE
2. Signals (NO BehaviorSubject) + input()/output()/model() (NO @Input/@Output)
3. inject() SIEMPRE
4. @if/@for/@switch (NO *ngIf/*ngFor) + @defer + track 
5. Reactive Forms + Lazy loading + Guards/Interceptors funcionales
6. Encapsular librerías de terceros en servicios
7. Tailwind CSS
8. TypeScript estricto + código en inglés

---

## 7. Consejos para Agentes

- Antes de editar código, revisa reglas de esta guía.
- Proactiva revisión de cobertura y linter.
- Corrige errores siempre usando interceptores (Angular) o Result Pattern (.NET).
- Agrega tests específicos: usa filtros para ejecutar solo los relevantes.
- Proporciona logs útiles (`console.error`, verbosity alto).
- Mantén un formato consistente usando Prettier antes de commit.
- Si la guía cambia, actualiza tus workflows conforme a este archivo.

---

> Si tienes dudas sobre algún estándar o comando, revisa los archivos README de cada carpeta y los ejemplos de skill. Este documento unifica y adapta todas las reglas para el trabajo agentic y colaborativo.
