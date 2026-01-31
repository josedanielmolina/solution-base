# Plan de Desarrollo por Fases - App Torneos de Pádel

## Visión General del Plan

El plan sigue una **estrategia de construcción incremental** donde cada fase:
- Es **funcional y probables** de manera independiente
- **Construye las bases** para las fases siguientes
- Termina con un **entregable demostrable**

**Principio rector**: El módulo de **Torneos es el más crítico**, por lo tanto se construye al final cuando todas las piezas fundamentales ya existen y están probadas.

---

## Mapa de Fases

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         FASE 0: FUNDAMENTOS                                 │
│              Base de datos + Infraestructura del proyecto                   │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                       FASE 1: AUTENTICACIÓN                                 │
│                Login + Perfil + Sistema de Permisos                         │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                       FASE 2: MANTENIMIENTOS                                │
│            Panel Admin: Países, Ciudades, Categorías, Jugadores             │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                      FASE 3: ESTABLECIMIENTOS                               │
│              Gestión de clubs y canchas (entidades globales)                │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         FASE 4: EVENTOS                                     │
│           Creación de eventos + Administradores + Asociaciones              │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         FASE 5: TORNEOS                                     │
│          Creación + Participantes + Grupos + Partidos + Posiciones          │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## Detalle por Fase

---

## FASE 0: Fundamentos

**Objetivo**: Establecer la base técnica sobre la cual se construirá todo.

### Entregables

| Entregable | Descripción |
|------------|-------------|
| Modelo de datos | Diseño completo de la base de datos MySQL |
| Migraciones | Scripts de creación de tablas base |
| Proyecto Angular | Estructura inicial del frontend |
| Integración API | Conexión básica frontend ↔ backend funcionando |

### ¿Qué se puede probar al terminar?

- ✅ La aplicación Angular carga correctamente
- ✅ Angular puede comunicarse con el backend .NET
- ✅ La base de datos está configurada y accessible

### Dependencias

- Ninguna (es la primera fase)

---

## FASE 1: Autenticación

**Objetivo**: Tener un sistema de usuarios funcional con login y permisos.

### Entregables

| Entregable | Descripción |
|------------|-------------|
| Login | Pantalla de inicio de sesión (email + contraseña) |
| Registro de usuarios | Desde panel admin (sin auto-registro) |
| Cambio de contraseña | Flujo de contraseña temporal + cambio obligatorio |
| Recuperación de contraseña | Por correo electrónico |
| Perfil de usuario | Edición de datos personales |
| Sistema de roles | Admin Plataforma, Organizador, Admin Evento |
| Sistema de permisos | Permisos configurables por rol |
| Panel de permisos | Configuración de permisos desde UI |

### ¿Qué se puede probar al terminar?

- ✅ Un usuario puede iniciar sesión
- ✅ Un admin puede crear usuarios con contraseña temporal
- ✅ El usuario debe cambiar su contraseña en primer login
- ✅ Los permisos restringen las acciones según el rol
- ✅ Se pueden configurar permisos desde un panel

### Dependencias

- FASE 0: Fundamentos

---

## FASE 2: Mantenimientos

**Objetivo**: Panel de administración con los catálogos globales del sistema.

### Entregables

| Entregable | Descripción |
|------------|-------------|
| CRUD Países | Listado + Crear + Editar + Eliminar países |
| CRUD Ciudades | Listado + Crear + Editar + Eliminar ciudades (por país) |
| CRUD Categorías | Gestión de categorías jerárquicas (País > Género > Categoría) |
| CRUD Jugadores | Gestión global de jugadores (documento único) |
| Asociación Usuario-Jugador | Vincular/desvincular jugadores con usuarios |
| Soft Delete | Implementación de eliminación lógica |
| Validaciones de unicidad | Documento, nombres de país, ciudad, categoría |

### ¿Qué se puede probar al terminar?

- ✅ Admin puede gestionar el catálogo de países (Colombia inicial)
- ✅ Admin puede gestionar ciudades por país
- ✅ Admin puede crear categorías jerárquicas (ej: Colombia > Masculina > Primera)
- ✅ Admin puede gestionar jugadores globales
- ✅ No se permiten duplicados (validaciones funcionan)
- ✅ Los elementos eliminados no se destruyen (soft delete funciona)

### Dependencias

- FASE 1: Autenticación (para acceso al panel admin)

---

## FASE 3: Establecimientos

**Objetivo**: Gestión completa de clubs y canchas disponibles en la plataforma.

### Entregables

| Entregable | Descripción |
|------------|-------------|
| CRUD Establecimientos | Gestión completa de establecimientos |
| Información de establecimiento | País, Ciudad, Nombre, Logo, Fotos, Teléfonos, Dirección, Google Maps |
| Gestión de horarios | Horario continuo o por bloques |
| CRUD Canchas | Agregar/editar/eliminar canchas por establecimiento |
| Información de cancha | Nombre, Tipo (Indoor/Outdoor), Fotos |
| Carga de imágenes | Upload de logo y fotos (almacenamiento en BD) |
| Validaciones | Nombre único, al menos un teléfono, al menos una cancha |

### ¿Qué se puede probar al terminar?

- ✅ Admin puede crear establecimientos con toda su información
- ✅ Admin puede agregar múltiples canchas a un establecimiento
- ✅ Se pueden subir logos y fotos
- ✅ Los horarios se configuran correctamente
- ✅ Las validaciones funcionan (nombre único, datos requeridos)

### Dependencias

- FASE 2: Mantenimientos (para países y ciudades)

---

## FASE 4: Eventos

**Objetivo**: Gestión completa de eventos con sus módulos asociados.

### Entregables

| Entregable | Descripción |
|------------|-------------|
| CRUD Eventos | Crear, editar, eliminar eventos |
| Información del evento | Nombre, Descripción, Fechas, Posters, PDF reglas, Redes sociales |
| Carga de archivos | Posters (vertical/horizontal) + PDF de reglas |
| Vista principal post-login | Listado de eventos donde el usuario participa |
| Asociación de establecimientos | Buscar y asociar establecimientos existentes al evento |
| CRUD Administradores | Invitar/remover admins de evento |
| Sistema de invitaciones | Invitación por email con expiración (30 min) |
| Notificaciones básicas | Notificación al remover admin |
| Permisos por evento | Aplicación de permisos configurables a nivel evento |

### Módulos del Evento

| Módulo | Descripción |
|--------|-------------|
| Vista General | Resumen del evento (preparación para estadísticas) |
| Establecimientos | Búsqueda y asociación de establecimientos |
| Administradores | Gestión de admins del evento |
| Editar Configuración | Modificar datos del evento |
| **Torneos** | Listado de torneos (vacío hasta Fase 5) |

### ¿Qué se puede probar al terminar?

- ✅ Usuario ve listado de sus eventos al iniciar sesión
- ✅ Admin de plataforma puede crear eventos y asignar organizador
- ✅ Organizador puede editar información del evento
- ✅ Se pueden subir posters y PDF de reglas
- ✅ Se pueden buscar y asociar establecimientos al evento
- ✅ Organizador puede invitar administradores
- ✅ La invitación expira después de 30 minutos
- ✅ Los permisos funcionan según configuración

### Dependencias

- FASE 3: Establecimientos (para asociar al evento)
- FASE 1: Autenticación (para sistema de invitaciones)

---

## FASE 5: Torneos

**Objetivo**: Módulo completo de gestión de torneos dentro de eventos.

### Sub-fases Sugeridas

Dado que es el módulo más complejo, se sugiere dividirlo internamente:

```
FASE 5.1: Estructura Base
    │
    ▼
FASE 5.2: Participantes
    │
    ▼
FASE 5.3: Grupos y Partidos
    │
    ▼
FASE 5.4: Posiciones y Clasificación
```

---

### FASE 5.1: Estructura Base de Torneos

| Entregable | Descripción |
|------------|-------------|
| Vista listado de torneos | Listado de torneos del evento |
| Formulario de creación | Nombre, Descripción, Categoría, Fecha inicio |
| Selección jerárquica de categoría | País > Género > Categoría específica |
| Configuración de formato | Tipo de inscripción, sets, participantes por grupo, clasificados |
| Navegación por tabs | Estructura de tabs (Participantes, Grupos, Llave, Posiciones, Ajustes) |
| CRUD Torneos | Crear, editar, eliminar torneos |

**Probar**: ✅ Se pueden crear torneos con categorías y configuración de formato

---

### FASE 5.2: Participantes

| Entregable | Descripción |
|------------|-------------|
| Tab Participantes | Vista de gestión de participantes |
| Modo Individual | Registro de jugadores individuales |
| Modo Parejas | Registro de parejas (buscar/crear jugadores) |
| Búsqueda de jugadores | Por nombre/documento (reutiliza si existe) |
| Validaciones | Documento único, no duplicado en evento, límite máximo |
| Edición/Eliminación | Editar datos, eliminar participante (soft delete) |

**Probar**: ✅ Se pueden registrar participantes individuales y parejas en un torneo

---

### FASE 5.3: Grupos y Partidos

| Entregable | Descripción |
|------------|-------------|
| Tab Grupos | Vista de gestión de grupos |
| Botón "Generar Grupos" | Generación automática con distribución equitativa |
| Nomenclatura automática | Grupos A, B, C, D... Z |
| Generación de partidos | Cruces automáticos round-robin |
| Mover participantes | Reasignación manual entre grupos |
| Sub-tab Partidos | Vista de partidos por ronda |
| Asignación de cancha | Seleccionar cancha para cada partido |
| Asignación de horario | Fecha y hora para cada partido |
| Agregar nuevos participantes | Asignación automática a grupos existentes |

**Probar**: ✅ Se generan grupos automáticamente con todos los partidos, se pueden asignar canchas y horarios

---

### FASE 5.4: Posiciones y Clasificación

| Entregable | Descripción |
|------------|-------------|
| Sub-tab Posiciones (por grupo) | Tabla de posiciones del grupo |
| Registro de resultados | Ingresar resultado de cada partido |
| Cálculo de puntos | Victoria 3, Empate 1, Derrota 0 |
| Ordenamiento automático | Ranking por puntos |
| Indicador de clasificados | Marcar quiénes avanzan según configuración |
| Tab Posiciones General | Tabla general del torneo (preparación para llave) |

**Probar**: ✅ Se pueden registrar resultados, la tabla se actualiza automáticamente, se identifican clasificados

---

## Resumen del Plan

| Fase | Nombre | Dependencias | Complejidad |
|------|--------|--------------|-------------|
| 0 | Fundamentos | - | ⭐⭐ |
| 1 | Autenticación | Fase 0 | ⭐⭐⭐ |
| 2 | Mantenimientos | Fase 1 | ⭐⭐ |
| 3 | Establecimientos | Fase 2 | ⭐⭐⭐ |
| 4 | Eventos | Fases 1, 3 | ⭐⭐⭐ |
| 5.1 | Torneos - Estructura | Fase 4 | ⭐⭐ |
| 5.2 | Torneos - Participantes | Fase 5.1 | ⭐⭐⭐ |
| 5.3 | Torneos - Grupos | Fase 5.2 | ⭐⭐⭐⭐ |
| 5.4 | Torneos - Posiciones | Fase 5.3 | ⭐⭐⭐ |

---

## Diagrama de Dependencias

```
                    ┌──────────────┐
                    │   FASE 0     │
                    │ Fundamentos  │
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   FASE 1     │
                    │Autenticación │
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   FASE 2     │
                    │Mantenimientos│
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   FASE 3     │
                    │Establecimien.│
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   FASE 4     │
                    │   Eventos    │
                    └──────┬───────┘
                           │
              ┌────────────┼────────────┐
              ▼            ▼            ▼
        ┌──────────┐ ┌──────────┐ ┌──────────┐
        │ FASE 5.1 │→│ FASE 5.2 │→│ FASE 5.3 │
        │Estructura│ │Participan│ │ Grupos   │
        └──────────┘ └──────────┘ └────┬─────┘
                                       │
                                       ▼
                                 ┌──────────┐
                                 │ FASE 5.4 │
                                 │Posiciones│
                                 └──────────┘
```

---

## Notas Finales

### Principios del Plan

1. **Vertical primero**: Cada fase entrega funcionalidad completa (backend + frontend)
2. **Probables**: Cada fase termina con algo demostrable
3. **Dependencias claras**: No iniciar una fase sin completar sus dependencias
4. **Lo crítico al final**: Torneos se construye sobre bases sólidas

### Fuera del Alcance Inicial

- Llave eliminatoria (post-MVP)
- Criterios de desempate avanzados
- Notificaciones push
- Pagos y suscripciones

---

**Documentos relacionados**: Ver carpeta `planning/` para especificaciones funcionales detalladas.
