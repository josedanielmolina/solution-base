# Documentación de la Aplicación de Gestión de Torneos de Pádel

## Descripción General

Aplicación para la gestión integral de torneos de pádel.

---

## Funcionalidades

### 1. Autenticación
- Sistema de usuarios y permisos

### 2. Torneos
- **2.1** Administración de torneos
- **2.2** Administración de canchas
- **2.3** Administración de jugadores
- **2.4** Administración de categorías

### 3. Dashboard de Administración
- **3.1** Administración de tablas maestras
- **3.2** Administración de usuarios

---

## Estructura de Carpetas

```
docs/
├── main.md (este archivo)
├── auth/
│   └── index.md
├── users/
│   └── index.md
├── torneos/
│   ├── administracion-torneos/
│   │   └── index.md
│   ├── administracion-canchas/
│   │   └── index.md
│   ├── administracion-jugadores/
│   │   └── index.md
│   └── administracion-categorias/
│       └── index.md
└── dashboard-admin/
    └── index.md
```

---

## Páginas de la Aplicación

### 1. Login
Página de autenticación de usuarios.

### 2. Torneos
- **2.1** Componente que lista los torneos visibles para el usuario.

### 3. Dashboard de Administración
- **3.1 Sidebar**
  - **3.1.1 Menús:**
    - **Maestros** _(título)_
      - Categorías _(link)_
    - **Usuarios** _(link)_

---

## Reglas para los Archivos `index.md`

El documento `index.md` dentro de cada carpeta de feature debe describir tres elementos principales:

### 1. Descripción del Feature
Explicación general del propósito y alcance del feature.

### 2. Historias de Usuario
Descripción de los casos de uso desde la perspectiva del usuario.

### 3. Documentación Técnica
Documentación completa del feature orientada a IA para soporte y mantenimiento, que debe incluir:

- **Consideraciones:** Justificación de las decisiones técnicas tomadas.
- **Relaciones de base de datos y modelos:** Esquema de las entidades y sus relaciones.
- **Entidades DTO:** Definición de los objetos de transferencia de datos.
- **Validaciones:** Reglas de validación implementadas.
- **Relaciones:** Conexiones con otros features si existen.
- **Mapa de endpoints:** Descripción de alto nivel que incluya:
  - Endpoints disponibles
  - DTOs que reciben
  - Respuestas que devuelven
  - Servicios que invocan
- **Dependencias:** Librerías y servicios de terceros utilizados.
- **Test:** Listado de todos los tests del feature.
- **Deuda técnica:** Mejoras pendientes y consideraciones de performance para implementación futura.

> **⚠️ Importante:** No debe incluirse código fuera de lo estrictamente necesario para ejemplificar conceptos básicos. El código extenso está prohibido en esta documentación.

---

## Notas sobre la Estructura

- **Features pequeños:** Contendrán únicamente un archivo `index.md`.
- **Features grandes:** Contendrán subcarpetas con sus propios archivos `index.md`, siguiendo las mismas reglas documentadas anteriormente.