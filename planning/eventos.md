# Fase 2: Gestión de Eventos

## 1. Información Básica del Evento

* Nombre
* Descripción
* Organizador
* Teléfono de contacto
* Fecha inicio / fin (validación: fecha fin no puede ser anterior a fecha inicio)
* 2 posters del evento:
  * **Poster vertical**: 1080 x 1920 px (ratio 9:16, ideal para stories)
  * **Poster horizontal**: 1920 x 1080 px (ratio 16:9, ideal para feeds/banners)
  * Máximo 5MB por imagen
  * Se comprimen y redimensionan antes de guardar
* PDF adjunto - reglas (validación de PDF, máximo 5MB, solo 1 archivo)
* Redes sociales: WhatsApp, Facebook, Instagram

## 2. Almacenamiento de Archivos

* Posters y PDF se guardan como blobs en base de datos.

## 3. Módulos del Evento

| Módulo | Descripción |
|--------|-------------|
| **Vista General** | Resumen del evento, estadísticas, número total de participantes. |
| **Torneos** | Listado de torneos del evento. Crear/editar/eliminar torneos. Cada torneo tiene su propia gestión interna. |
| **Establecimientos** | Búsqueda y asociación de establecimientos existentes al evento. Los establecimientos son globales, gestionados por admin de plataforma. |
| **Administradores** | Invitar/retirar usuarios con permisos. |
| **Editar Configuración** | Modificar datos básicos del evento. |

## 4. Visibilidad

* Los eventos son **privados**: solo el organizador y sus administradores pueden verlos.
* No hay visibilidad pública ni búsqueda de eventos por otros usuarios.

## 5. Validaciones

* Fecha de fin del evento no puede ser anterior a fecha de inicio.
* Los datos del evento pueden editarse en cualquier momento.

---

**Anterior:** [autenticacion.md](./autenticacion.md) | **Siguiente:** [torneos.md](./torneos.md)
