# Fase 5: Torneos

## 1. Categorías

### Módulo de Mantenimiento

* Módulo de mantenimiento administrado por el admin de plataforma.
* Las categorías son globales y tienen una estructura jerárquica de 3 niveles.
* Se utilizan para crear torneos dentro de los eventos.

### Estructura Jerárquica de Categorías

Las categorías se organizan en tres niveles:

**Nivel 1 - País:**
* Colombia (inicial)
* Otros países (gestionados por admin de plataforma)

**Nivel 2 - Género:**
* Masculina
* Femenina
* Mixta

**Nivel 3 - Categoría Específica:**
* Primera, Segunda, Tercera, Cuarta, Quinta (por nivel)
* Otras categorías definidas por el admin de plataforma

### Ejemplos de Categorías Completas

* Colombia > Masculina > Primera
* Colombia > Masculina > Segunda
* Colombia > Femenina > Primera
* Colombia > Mixta > Única

### Unicidad

* La combinación País + Género + Categoría Específica es única en el sistema.
* Ejemplo: "Colombia > Masculina > Primera" es diferente a "Argentina > Masculina > Primera".

## 2. Creación de Torneos

### Formulario de Registro Inicial

Al crear un torneo se solicitan los siguientes datos:

#### Información General

* Nombre del torneo - obligatorio
* Descripción - opcional
* Categoría - obligatorio (selección jerárquica):
  * País (ej: Colombia)
  * Género (Masculina/Femenina/Mixta)
  * Categoría específica (Primera, Segunda, etc.)
  * Se muestra como: "Colombia - Masculina - Primera"
* Fecha inicio - obligatorio (debe estar dentro del rango del evento)

#### Formato del Torneo

* **Formato**: Fase de grupos + Llave eliminatoria (único formato en MVP)
* **Tipo de inscripción**:
  * Individual (1vs1)
  * En parejas (2vs2)
* **Número de sets por partido** - obligatorio
* **Participantes por grupo** - obligatorio
* **Cuántos avanzan por cada grupo** - obligatorio

#### Configuraciones

* **Cantidad máxima de participantes** - obligatorio
  * Para individual: número de jugadores
  * Para parejas: número de parejas
  * Este límite es la suma total de participantes del torneo

## 3. Estructura de Administración del Torneo

### Header del Torneo

Al entrar a la vista de administración de un torneo, se muestra en el header:

* Nombre del torneo
* Categoría

### Tabs de Gestión

Todas las tabs están visibles desde la creación del torneo, aunque algunas pueden no tener contenido hasta que se completen pasos previos.

| Tab | Descripción | Alcance MVP |
|-----|-------------|-------------|
| **Participantes** | Registro y gestión de jugadores o parejas del torneo | **Ahora** |
| **Grupos** | Generación y gestión de la fase de grupos con sistema round-robin | **Ahora** |
| **Llave Eliminatoria** | Gestión de la fase eliminatoria (eliminación directa) | Posterior |
| **Posiciones** | Tabla de posiciones general del torneo, ranking, estadísticas | Posterior |
| **Ajustes** | Configuración del torneo | Posterior |

## 4. Tab Participantes

### Comportamiento según Tipo de Inscripción

#### Inscripción Individual (1vs1)

* Lista de jugadores individuales del torneo.
* Opciones:
  * Registrar nuevo jugador
  * Buscar jugador existente (por nombre/documento)
  * Editar datos del jugador
  * Eliminar jugador (soft delete, si no ha jugado)
* Validaciones:
  * Verificación por documento (reutiliza datos si existe)
  * No puede estar en otro torneo del mismo evento
  * Límite máximo de participantes definido en configuración

#### Inscripción en Parejas (2vs2)

* Lista de parejas del torneo.
* Opciones:
  * Crear nueva pareja:
    1. Buscar/registrar Jugador 1
    2. Buscar/registrar Jugador 2
    3. Conformar pareja
  * Editar pareja (cambiar integrantes)
  * Eliminar pareja (soft delete, si no ha jugado)
* Validaciones:
  * Ambos jugadores deben ser únicos por documento
  * Ninguno puede estar en otro torneo del evento
  * Límite máximo de parejas definido en configuración

## 5. Tab Grupos

### Generación de Grupos

Los grupos se generan mediante el botón **"Generar Grupos"**, el cual está disponible una vez que hay participantes registrados en el torneo.

#### Distribución Automática de Participantes

El sistema distribuye automáticamente los participantes en grupos según el parámetro **"Participantes por grupo"** definido en la configuración del torneo.

**Ejemplos:**

* **20 participantes, 5 por grupo**: Se crean 4 grupos (A, B, C, D) con 5 participantes cada uno.
* **22 participantes, 5 por grupo**: Se crean 5 grupos donde algunos tendrán 5 y otros 4 participantes.
* **18 participantes, 5 por grupo**: Se crean 4 grupos donde algunos tendrán 5 y otros 4 participantes.

**Regla de distribución:**
* Si el número total de participantes no es divisible exactamente, algunos grupos tendrán un participante más o menos para equilibrar.
* La distribución se hace automáticamente de forma equitativa.

#### Nomenclatura de Grupos

* Los grupos se nombran automáticamente con letras: **A, B, C, D... Z**
* El orden es alfabético según el número de grupos generados.

### Formato Round-Robin (Todos contra Todos)

Cada grupo funciona con formato **round-robin**, donde:

* Cada participante juega exactamente **una vez** contra cada otro participante del mismo grupo.
* El número de **rondas** por grupo se calcula como: `n - 1` donde `n` es el número de participantes del grupo.
  * Ejemplo: Grupo con 5 participantes = 4 rondas
  * Ejemplo: Grupo con 4 participantes = 3 rondas

### Generación Automática de Partidos

Al presionar **"Generar Grupos"**, el sistema:

1. Distribuye automáticamente los participantes en grupos.
2. Genera todos los cruces (partidos) de cada grupo según el formato round-robin.
3. Organiza los partidos en rondas.

### Edición de Grupos

Una vez generados los grupos, el organizador puede:

* **Mover participantes entre grupos** manualmente.
* **Reasignar participantes** a diferentes grupos.
* **Regenerar grupos** si se agregan nuevos participantes (el sistema los asigna automáticamente).

### Estructura de Cada Grupo

Cada grupo tiene **dos sub-tabs**:

#### Sub-tab: Partidos

Muestra todas las rondas y partidos del grupo.

**Funcionalidades:**
* Visualización de todos los partidos organizados por ronda.
* **Asignación de cancha** para cada partido.
* **Asignación de fecha y hora** para cada partido.
* **Registro de resultados** (a desarrollar posteriormente).

**Vista de partidos:**
* Participante 1 vs Participante 2
* Cancha asignada
* Fecha y hora
* Resultado (cuando esté implementado)

#### Sub-tab: Posiciones

Muestra la tabla de posiciones del grupo.

**Contenido:**
* Lista de participantes del grupo.
* Puntos acumulados.
* Partidos jugados, ganados, empatados, perdidos.
* Ordenamiento por puntos (mayor a menor).

### Sistema de Puntuación

Los puntos se otorgan según el resultado de cada partido:

| Resultado | Puntos |
|-----------|--------|
| **Victoria** | 3 puntos |
| **Empate** | 1 punto |
| **Derrota** | 0 puntos |

### Clasificación a Llave Eliminatoria

* El número de participantes que avanzan por grupo es **configurable** en la creación del torneo (parámetro "Cuántos avanzan por cada grupo").
* Los participantes que **más puntos acumulen** en su grupo avanzan a la llave eliminatoria.
* El sistema ordena automáticamente la tabla de posiciones según puntos.
* En caso de empate en puntos, se aplicarán criterios de desempate (a definir posteriormente).

### Gestión de Nuevos Participantes

* Si se agregan nuevos participantes después de generar los grupos, **el sistema los asigna automáticamente** a los grupos existentes.
* La distribución busca mantener el equilibrio entre grupos.
* Se pueden regenerar completamente los grupos si es necesario.

## 6. Validaciones Generales

* Un jugador no puede participar en múltiples torneos del mismo evento.
* La fecha inicio del torneo debe estar dentro del rango de fechas del evento.
* El límite de participantes es estrictamente por torneo.
* Los datos del jugador se sincronizan automáticamente en todo el sistema.

## 7. Navegación (Sidebar)

### Vista de Listado de Torneos

Al hacer clic en el sidebar de **"Torneos"**, se carga una vista que muestra:

* **Listado de torneos creados** en el evento actual.
* **Botón "Crear Nuevo Torneo"** para iniciar el formulario de creación.

**Acciones disponibles en cada torneo:**
* Ver detalles
* Editar
* Eliminar (soft delete)

### Creación de Torneo

Al hacer clic en **"Crear Nuevo Torneo"**, se abre el formulario de creación con los campos descritos en la sección 2.

Una vez creado el torneo, se redirige automáticamente a la vista de administración del torneo con todas las tabs disponibles.

### Estructura de Navegación

La estructura de navegación dentro de un evento es:

```
📁 Evento
   └─ Vista General
   └─ Establecimientos (búsqueda y asociación)
   └─ Editar Configuración del Evento
📁 Torneos ← Vista de listado
   └─ Listado de torneos (ver/crear/editar/eliminar)
   └─ [Al entrar a un torneo específico]
      └─ Participantes (registro de jugadores/parejas)
      └─ Grupos (generación, partidos, posiciones por grupo)
      └─ Llave Eliminatoria
      └─ Posiciones (tabla general del torneo)
      └─ Ajustes
📁 Administradores
   └─ Gestión de administradores del evento
```

---

**Anterior:** [eventos.md](./eventos.md) | **Siguiente:** [establecimientos.md](./establecimientos.md)
