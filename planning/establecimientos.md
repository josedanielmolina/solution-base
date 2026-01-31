# Establecimientos y Canchas

## 1. Concepto General

* Los establecimientos son **entidades globales** de la plataforma.
* Gestionados exclusivamente por el **Admin de Plataforma** en el módulo de Mantenimientos.
* Los organizadores **buscan y asocian** establecimientos existentes a sus eventos.
* Un establecimiento puede ser usado por múltiples eventos simultáneamente.

## 2. Gestión de Establecimientos

### 2.1 Creación y Mantenimiento

**Exclusivo para Admin de Plataforma** en el panel de Mantenimientos.

#### Información del Establecimiento

* **País** - obligatorio (selección de catálogo global)
* **Ciudad** - obligatorio (selección de catálogo global, filtrado por país)
* **Nombre** - obligatorio, único en el sistema
* **Logo** - imagen del establecimiento (1 archivo)
* **Fotos adicionales** - imágenes del lugar para ayudar a los jugadores a ubicarse (ilimitadas)
* **Teléfono fijo** - opcional
* **Teléfono celular** - opcional
* **Dirección** - texto descriptivo, obligatorio
* **Dirección Google Maps** - enlace de Google Maps, opcional
* **Horarios** - obligatorio, dos modalidades:
  * **Continuo**: horario único (ej: 08:00 - 22:00)
  * **Por bloques**: múltiples rangos horarios (ej: 08:00-12:00, 16:00-22:00)

#### Información de Canchas

Cada establecimiento puede tener múltiples canchas asociadas:

* **Nombre/Número** - obligatorio (ej: "Cancha 1", "Cancha Central")
* **Tipo** - obligatorio:
  * Indoor (techada/cubierta)
  * Outdoor (descubierta)
* **Fotos** - ilimitadas, ayudan a identificar la cancha

### 2.2 Operaciones CRUD

**Crear Establecimiento:**
1. Registrar información del establecimiento
2. Agregar canchas asociadas
3. El establecimiento queda disponible para todos los organizadores

**Editar Establecimiento:**
* Modificar datos del establecimiento
* Agregar/editar/eliminar canchas
* Los cambios se reflejan en todos los eventos que lo usan

**Eliminar Establecimiento (Soft Delete):**
* Validación: solo si no está asociado a eventos activos
* Mantiene historial

## 3. Uso en Eventos

### 3.1 Asociación al Evento

* El organizador **busca establecimientos por nombre** en el sistema.
* La búsqueda muestra: Nombre, País y Ciudad del establecimiento.
* Selecciona y **asocia** el establecimiento a su evento.
* El establecimiento y todas sus canchas quedan disponibles para el evento.
* **Solo lectura**: el organizador no puede editar datos del establecimiento.

### 3.2 Selección de Canchas

* El organizador puede ver todas las canchas del establecimiento asociado.
* En fases posteriores, podrá asignar canchas específicas a partidos.
* Los horarios del establecimiento se usarán para validar asignaciones futuras.

### 3.3 Múltiples Establecimientos

* Un evento puede asociar múltiples establecimientos.
* Útil cuando el evento se desarrolla en varios lugares.

## 4. Consideraciones

### Almacenamiento de Archivos

* Logo y fotos se guardan como blobs en base de datos.
* Validación: máximo 5MB por imagen.
* Se comprimen y redimensionan antes de guardar.

### Horarios

* **Informativos** en esta fase.
* Servirán para validaciones futuras cuando se implemente asignación de partidos con fecha/hora.
* Formato 24 horas.

### Validaciones

* Nombre del establecimiento único en el sistema.
* Al menos un teléfono (fijo o celular) debe estar registrado.
* Un establecimiento debe tener al menos una cancha.
* No se puede eliminar un establecimiento asociado a eventos activos.

---

**Anterior:** [torneos.md](./torneos.md) | **Siguiente:** [administracion.md](./administracion.md)
