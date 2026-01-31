# Mantenimientos de Plataforma

## 1. Acceso

* **Exclusivo para Admin de Plataforma**
* Panel administrativo separado de la aplicación principal
* Gestión centralizada de entidades globales del sistema

## 2. Módulos de Mantenimiento

### 2.1 Gestión de Jugadores

#### Listado de Jugadores

* Visualización de todos los jugadores registrados en el sistema
* Búsqueda por:
  * Nombre
  * Documento
  * Correo electrónico
  * Teléfono
* Filtros:
  * Jugadores con usuario asociado
  * Jugadores sin usuario
  * Por evento/torneo donde participa

#### Operaciones CRUD

**Crear Jugador:**
* Nombres (nombres y apellidos) - obligatorio
* Documento (DNI, cédula o pasaporte) - obligatorio, único en todo el sistema
* Correo electrónico - opcional
* Teléfono - obligatorio
* Foto - opcional

**Editar Jugador:**
* Modificar cualquier dato del jugador
* Los cambios se sincronizan automáticamente en todos los torneos donde participa

**Eliminar Jugador (Soft Delete):**
* Validación: solo si no está participando en torneos activos
* Mantiene historial

#### Asociación Usuario-Jugador

* Ver si el jugador tiene usuario asociado
* Vincular jugador existente con usuario
* Desvincular asociación

### 2.2 Gestión de Países

#### Listado de Países

* Visualización de todos los países registrados en el sistema
* País inicial: Colombia

#### Operaciones CRUD

**Crear País:**
* Nombre - obligatorio, único
* Código ISO (opcional) - ej: CO, AR, VE

**Editar País:**
* Modificar nombre o código
* Los registros asociados mantienen la referencia

**Eliminar País (Soft Delete):**
* Validación: solo si no tiene ciudades, categorías o establecimientos asociados
* Mantiene historial

### 2.3 Gestión de Ciudades

#### Listado de Ciudades

* Visualización de todas las ciudades registradas
* Filtro por país

#### Operaciones CRUD

**Crear Ciudad:**
* País - obligatorio (selección de catálogo)
* Nombre - obligatorio, único dentro del país

**Editar Ciudad:**
* Modificar nombre
* No se puede cambiar de país si tiene establecimientos asociados

**Eliminar Ciudad (Soft Delete):**
* Validación: solo si no tiene establecimientos asociados
* Mantiene historial

### 2.4 Gestión de Categorías

#### Estructura Jerárquica

Las categorías tienen 3 niveles:
1. **País** (selección de catálogo)
2. **Género** (Masculina, Femenina, Mixta)
3. **Categoría Específica** (Primera, Segunda, etc.)

#### Listado de Categorías

* Visualización jerárquica de todas las categorías
* Filtros:
  * Por país
  * Por género
* Visualización: "Colombia - Masculina - Primera"

#### Operaciones CRUD

**Crear Categoría:**
* País - obligatorio (selección de catálogo)
* Género - obligatorio (Masculina/Femenina/Mixta)
* Nombre de categoría específica - obligatorio
* Validación: la combinación País + Género + Nombre debe ser única

**Editar Categoría:**
* Modificar el nombre de la categoría específica
* No se puede cambiar País ni Género si tiene torneos asociados
* Los torneos existentes mantienen la referencia

**Eliminar Categoría (Soft Delete):**
* Validación: solo si no está siendo usada por torneos activos
* Mantiene historial

#### Ejemplos de Categorías

* Colombia > Masculina > Primera
* Colombia > Masculina > Segunda
* Colombia > Femenina > Primera
* Colombia > Mixta > Única

### 2.5 Gestión de Establecimientos

#### Listado de Establecimientos

* Visualización de todos los establecimientos registrados en el sistema
* Búsqueda por nombre
* Filtros:
  * Por país
  * Por ciudad
  * Con/sin canchas registradas
  * Por cantidad de eventos asociados
  * Activos/Inactivos

#### Operaciones CRUD

**Crear Establecimiento:**

*Información del Establecimiento:*
* País - obligatorio (selección de catálogo)
* Ciudad - obligatorio (selección de catálogo, filtrado por país)
* Nombre - obligatorio, único en el sistema
* Logo - imagen (1 archivo, máx 5MB)
* Fotos adicionales - ilimitadas (máx 5MB c/u)
* Teléfono fijo - opcional
* Teléfono celular - opcional (al menos uno obligatorio)
* Dirección - texto descriptivo, obligatorio
* Dirección Google Maps - enlace, opcional
* Horarios - obligatorio:
  * Continuo: horario único (HH:MM - HH:MM)
  * Por bloques: múltiples rangos horarios

*Gestión de Canchas:*
* Agregar canchas al establecimiento
* Cada cancha requiere:
  * Nombre/Número - obligatorio
  * Tipo - Indoor/Outdoor, obligatorio
  * Fotos - ilimitadas (máx 5MB c/u)

**Editar Establecimiento:**
* Modificar información del establecimiento
* Agregar/editar/eliminar canchas asociadas
* Los cambios se reflejan automáticamente en todos los eventos que usan el establecimiento
* Validación: debe tener al menos una cancha

**Eliminar Establecimiento (Soft Delete):**
* Validación: solo si no está asociado a eventos activos
* Eliminar establecimiento también marca como eliminadas sus canchas
* Mantiene historial

#### Gestión de Canchas

**Crear Cancha:**
* Asociada a un establecimiento específico
* Nombre/Número único dentro del establecimiento
* Tipo (Indoor/Outdoor)
* Fotos opcionales

**Editar Cancha:**
* Modificar nombre, tipo o fotos
* No se puede cambiar de establecimiento

**Eliminar Cancha (Soft Delete):**
* Validación: solo si no está siendo usada en partidos/asignaciones
* El establecimiento debe mantener al menos una cancha activa

#### Uso en Eventos

* Los organizadores pueden **buscar y asociar** establecimientos a sus eventos
* Los organizadores tienen acceso de **solo lectura** a los datos
* Un establecimiento puede estar asociado a múltiples eventos simultáneamente
* Las canchas del establecimiento quedan disponibles para asignación de partidos (fase futura)

---

## 3. Consideraciones Técnicas

### Sincronización de Datos

* **Jugadores**: Los cambios en datos de un jugador se reflejan automáticamente en todos los torneos donde participa
* **Categorías**: Los cambios en el nombre no afectan torneos existentes (mantienen snapshot)
* **Establecimientos**: A definir según nueva lógica

### Validaciones Globales

* Documento de jugador es único en todo el sistema
* Nombre de país es único en el sistema
* Nombre de ciudad es único dentro de cada país
* Combinación País + Género + Categoría específica es única
* Nombre de establecimiento es único en el sistema
* No se pueden eliminar entidades vinculadas a torneos o eventos activos

### Auditoría

* Registro de todas las operaciones realizadas por el admin
* Historial de cambios en entidades críticas
* Soft delete para mantener integridad referencial

---

**Anterior:** [administracion.md](./administracion.md) | **Backlog:** [backlog.md](./backlog.md)
