# App de Eventos de Pádel — Visión General

## 1. Objetivo

Crear una aplicación para **gestionar eventos de pádel**, permitiendo a los usuarios crear y administrar múltiples eventos, con inscripción de jugadores, conformación de parejas, gestión de establecimientos/canchas y administradores. Dentro de cada evento se pueden crear **torneos** por categorías. **El manejo competitivo de torneos (llaves, rondas, resultados)** queda fuera del alcance inicial.

## 2. Alcance (MVP)

**Incluye:**

* Autenticación y perfil de usuario.
* Creación y listado de eventos por usuario (vista inicial).
* Gestión básica del evento (datos, medios, redes).
* Creación de torneos por categoría jerárquica (País > Género > Categoría específica).
* Gestión de participantes a nivel torneo (individual o parejas).
* Formato de torneo: Fase de grupos + Llave eliminatoria.
* Inscripción individual (1vs1) o en parejas (2vs2).
* **Generación automática de grupos** con distribución equitativa de participantes.
* **Gestión de fase de grupos** con formato round-robin (todos contra todos).
* **Asignación de canchas y horarios** a partidos de la fase de grupos.
* **Sistema de puntuación** (Victoria 3pts, Empate 1pt, Derrota 0pts).
* **Tabla de posiciones por grupo** con clasificación automática.
* Gestión de establecimientos globales (País, Ciudad, canchas) y asociación a eventos.
* Gestión de administradores del evento.
* Módulos de mantenimiento: Países, Ciudades, Categorías, Jugadores y Establecimientos.
* Soft delete para entidades principales (torneos, participantes, eventos, establecimientos).

**Excluye (fase posterior):**

* Llave eliminatoria y gestión de brackets.
* Registro de resultados de partidos.
* Otros formatos de torneo (solo eliminatoria, round robin puro).
* Inscripción por equipos.
* Pagos y notificaciones avanzadas.
* Calendario y reserva de canchas.
* Asignación automática de canchas a partidos.
* Criterios de desempate avanzados en tablas de posiciones.

## 3. Consideraciones No‑Funcionales

* App responsive (web first).
* Control de permisos basado en acciones flexibles (configurables por panel).
* Almacenamiento de archivos en base de datos (imágenes y PDF).
* Validaciones de unicidad:
  * Documento de jugador (global)
  * Nombre de país (global)
  * Nombre de ciudad (por país)
  * Categoría: País + Género + Nombre específico (global)
  * Nombre de establecimiento (global)
* Sincronización automática de datos de jugador entre eventos.
* Soft delete para mantener integridad de datos e historial.
* Catálogos globales centralizados: Países, Ciudades, Categorías, Establecimientos.
* Preparada para escalar a lógica competitiva en fases futuras.

## 4. Flujos Clave

### Flujo de Admin de Plataforma

1. Admin de plataforma gestiona catálogos globales:
   - Países (Colombia inicial)
   - Ciudades por país
   - Categorías jerárquicas (País > Género > Categoría específica)
   - Establecimientos (con País, Ciudad y canchas)
   - Jugadores globales
2. Admin de plataforma registra usuarios en plataforma.
3. Admin de plataforma crea evento → designa organizador.

### Flujo de Usuario/Organizador

1. Usuario inicia sesión (si tiene contraseña temporal, debe cambiarla).
2. Usuario puede crear/completar perfil de jugador (opcional).
3. Organizador completa info básica del evento.
4. Organizador crea torneos:
   - Selecciona categoría jerárquica (País > Género > Categoría)
   - Define formato, límites y tipo de inscripción
5. Dentro de cada torneo, registra participantes:
   - Individual: registra jugadores directamente
   - Parejas: registra parejas (busca jugadores por documento, crea si no existe)
6. Genera grupos con el botón "Generar Grupos" (distribución automática).
7. Asigna canchas y horarios a los partidos de cada grupo.
8. Define llave eliminatoria (posterior al MVP).
9. Busca y asocia establecimientos existentes al evento (por nombre).
10. Invita administradores al evento.

---

## Documentos Relacionados

| Documento | Descripción |
|-----------|-------------|
| [autenticacion.md](./autenticacion.md) | Autenticación, perfil, roles y permisos |
| [eventos.md](./eventos.md) | Gestión de eventos y estados |
| [torneos.md](./torneos.md) | Torneos, categorías y participantes |
| [establecimientos.md](./establecimientos.md) | Clubs y canchas - entidades globales |
| [administracion.md](./administracion.md) | Administradores y permisos configurables |
| [mantenimientos.md](./mantenimientos.md) | Gestión global de jugadores, países, ciudades, categorías y establecimientos |
| [backlog.md](./backlog.md) | Deuda técnica y futuras implementaciones |
