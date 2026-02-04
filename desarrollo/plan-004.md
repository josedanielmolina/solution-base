# Plan de Implementación - Fase 4: Eventos

## Objetivo

Gestión completa de eventos con sus módulos internos. Esta fase introduce **dos layouts diferenciados**:
1. **Layout Principal** (`/app/events`): Listado de eventos donde el usuario participa
2. **Layout de Administración de Evento** (`/app/events/:publicId`): Interior del evento con módulos

> **SEGURIDAD:** Las URLs usan **GUIDs (PublicId)** en lugar de IDs numéricos para evitar enumeración/scraping. Además, se implementan validaciones de acceso internas.

## Dependencias

- ✅ FASE 1: Autenticación (sistema de permisos e invitaciones)
- ✅ FASE 3: Establecimientos (asociación a eventos)

---

## Entregables

| # | Entregable | Descripción |
|---|------------|-------------|
| 1 | CRUD Eventos | Crear, editar, listar, soft-delete eventos |
| 2 | Información del evento | Nombre, Descripción, Fechas, Posters, PDF reglas, Redes sociales |
| 3 | Carga de archivos | 2 Posters (vertical/horizontal) + PDF de reglas |
| 4 | Vista principal post-login | Listado de eventos del usuario |
| 5 | Layout interno del evento | Navegación por módulos del evento |
| 6 | Asociación de establecimientos | Buscar y asociar establecimientos al evento |
| 7 | CRUD Administradores | Invitar/remover admins de evento |
| 8 | Sistema de invitaciones | Por email con expiración (30 min) |
| 9 | Permisos por evento | Control de acceso configurable |
| 10 | **Seguridad URLs** | GUIDs en URLs + validación de acceso |

---

## Modelo de Datos

### Nuevas Entidades

```
┌───────────────────────────────────────────────────────────────────────────┐
│                               EVENT                                        │
├───────────────────────────────────────────────────────────────────────────┤
│ Id (PK) - interno                                                         │
│ PublicId (GUID, unique) - usado en URLs                                   │
│ Name                                                                      │
│ Description (TEXT)                                                        │
│ OrganizerId (FK → Users) - Organizador designado                          │
│ ContactPhone                                                              │
│ StartDate, EndDate                                                        │
│ PosterVertical (LONGTEXT base64, nullable)                                │
│ PosterHorizontal (LONGTEXT base64, nullable)                              │
│ RulesPdf (LONGTEXT base64, nullable)                                      │
│ WhatsApp, Facebook, Instagram (nullable)                                  │
│ IsActive, CreatedAt, UpdatedAt                                            │
└───────────────────────────────────────────────────────────────────────────┘
                                    │
              1─────────────────────┼─────────────────────*
                                    │
┌──────────────────────────┐        │        ┌──────────────────────────────┐
│    EVENT_ESTABLISHMENT   │        │        │      EVENT_ADMIN             │
├──────────────────────────┤        │        ├──────────────────────────────┤
│ Id (PK)                  │        │        │ Id (PK)                      │
│ EventId (FK)             │        │        │ EventId (FK)                 │
│ EstablishmentId (FK)     │        │        │ UserId (FK)                  │
│ CreatedAt                │        │        │ CreatedAt                    │
└──────────────────────────┘        │        └──────────────────────────────┘
                                    │
                                    │        ┌──────────────────────────────┐
                                    │        │     EVENT_INVITATION         │
                                    │        ├──────────────────────────────┤
                                    │        │ Id (PK)                      │
                                    │        │ EventId (FK)                 │
                                    │        │ Email                        │
                                    │        │ Token (unique)               │
                                    │        │ ExpiresAt                    │
                                    │        │ AcceptedAt (nullable)        │
                                    │        │ CreatedAt                    │
                                    └────────┴──────────────────────────────┘
```

---

## Mapa Completo de Archivos

### ⚠️ Leyenda
- `[NEW]` = Archivo nuevo a crear
- `[MODIFY]` = Archivo existente a modificar

---

## 1. Domain Layer

**Ubicación:** `src/Core/Core.Domain/`

| Archivo | Ruta Completa | Descripción |
|---------|---------------|-------------|
| `[NEW]` Event.cs | `Entities/Event.cs` | Entidad principal con organizador y relaciones |
| `[NEW]` EventEstablishment.cs | `Entities/EventEstablishment.cs` | Relación muchos-a-muchos |
| `[NEW]` EventAdmin.cs | `Entities/EventAdmin.cs` | Admins del evento |
| `[NEW]` EventInvitation.cs | `Entities/EventInvitation.cs` | Invitaciones pendientes |
| `[NEW]` IEventRepository.cs | `Interfaces/Repositories/IEventRepository.cs` | Interfaz del repositorio |

### Contenido de las Entidades

```csharp
// Event.cs
public class Event : BaseEntity
{
    public Guid PublicId { get; set; } = Guid.NewGuid();  // Para URLs seguras
    public string Name { get; set; }
    public string? Description { get; set; }
    public int OrganizerId { get; set; }
    public string? ContactPhone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? PosterVertical { get; set; }    // Base64 (1080x1920)
    public string? PosterHorizontal { get; set; }  // Base64 (1920x1080)
    public string? RulesPdf { get; set; }           // Base64
    public string? WhatsApp { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public virtual User Organizer { get; set; }
    public virtual ICollection<EventEstablishment> Establishments { get; set; }
    public virtual ICollection<EventAdmin> Admins { get; set; }
    public virtual ICollection<EventInvitation> Invitations { get; set; }
    
    // Helper: verificar si usuario tiene acceso (Organizer o Admin)
    public bool HasAccess(int userId) => 
        OrganizerId == userId || Admins?.Any(a => a.UserId == userId) == true;
}

// EventAdmin.cs
public class EventAdmin
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Event Event { get; set; }
    public virtual User User { get; set; }
}

// EventInvitation.cs
public class EventInvitation
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Event Event { get; set; }
}
```

---

## 2. Persistence Layer

**Ubicación:** `src/Infrastructure/Infrastructure.Persistence/`

| Archivo | Ruta Completa | Descripción |
|---------|---------------|-------------|
| `[NEW]` EventConfiguration.cs | `Configurations/EventConfiguration.cs` | Configuración EF Core |
| `[NEW]` EventEstablishmentConfiguration.cs | `Configurations/EventEstablishmentConfiguration.cs` | Relación M:N |
| `[NEW]` EventAdminConfiguration.cs | `Configurations/EventAdminConfiguration.cs` | Admins |
| `[NEW]` EventInvitationConfiguration.cs | `Configurations/EventInvitationConfiguration.cs` | Invitaciones |
| `[NEW]` EventRepository.cs | `Repositories/EventRepository.cs` | Implementación repositorio |
| `[MODIFY]` ApplicationDbContext.cs | `Context/ApplicationDbContext.cs` | Agregar DbSets |
| `[MODIFY]` DependencyInjection.cs | `DependencyInjection.cs` | Registrar repositorios |

---

## 3. Application Layer

**Ubicación:** `src/Core/Core.Application/`

### DTOs

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EventDtos.cs | `DTOs/Events/EventDtos.cs` |
| `[NEW]` EventAdminDtos.cs | `DTOs/Events/EventAdminDtos.cs` |
| `[NEW]` EventInvitationDtos.cs | `DTOs/Events/EventInvitationDtos.cs` |

**DTOs incluidos:**
```
EventDto, EventListDto, EventDetailDto, CreateEventDto, UpdateEventDto
EventEstablishmentDto, AddEventEstablishmentDto
EventAdminDto, InviteAdminDto
EventInvitationDto
```

### Validators

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EventValidators.cs | `Validators/Events/EventValidators.cs` |

**Validaciones:**
- Nombre obligatorio
- Fecha fin >= Fecha inicio
- Organizador válido
- Posters: máximo 5MB, formatos imagen
- PDF: máximo 5MB, formato PDF

### Features

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EventFeatures.cs | `Features/Events/EventFeatures.cs` |
| `[NEW]` EventAdminFeatures.cs | `Features/Events/EventAdminFeatures.cs` |
| `[NEW]` EventEstablishmentFeatures.cs | `Features/Events/EventEstablishmentFeatures.cs` |

**Features incluidos:**
```
// Events
GetMyEvents, GetEventById, CreateEvent, UpdateEvent, DeleteEvent,
UploadPoster, UploadRulesPdf, RemovePoster, RemoveRulesPdf

// Admins
GetEventAdmins, InviteAdmin, RemoveAdmin, AcceptInvitation,
GetPendingInvitations

// Establishments
GetEventEstablishments, AddEstablishment, RemoveEstablishment,
SearchAvailableEstablishments
```

---

## 4. API Layer

**Ubicación:** `src/Presentation/API/`

| Archivo | Ruta Completa | Descripción |
|---------|---------------|-------------|
| `[NEW]` EventsController.cs | `Controllers/EventsController.cs` | Endpoints de eventos |
| `[NEW]` EventAdminsController.cs | `Controllers/EventAdminsController.cs` | Gestión de admins |
| `[NEW]` EventEstablishmentsController.cs | `Controllers/EventEstablishmentsController.cs` | Gestión establecimientos |

### Endpoints

> ⚠️ **IMPORTANTE:** Los endpoints usan `{publicId}` (GUID) en lugar de `{id}` numérico.

**EventsController:**
```
GET    /api/events                              - Mis eventos (usuario actual)
GET    /api/events/{publicId}                   - Detalle del evento
POST   /api/events                              - Crear evento (Admin Plataforma)
PUT    /api/events/{publicId}                   - Actualizar evento
DELETE /api/events/{publicId}                   - Soft delete
POST   /api/events/{publicId}/poster-vertical   - Subir poster vertical
POST   /api/events/{publicId}/poster-horizontal - Subir poster horizontal
DELETE /api/events/{publicId}/poster/{type}     - Eliminar poster
POST   /api/events/{publicId}/rules-pdf         - Subir PDF reglas
DELETE /api/events/{publicId}/rules-pdf         - Eliminar PDF
```

**EventAdminsController:**
```
GET    /api/events/{publicId}/admins                - Lista de admins
POST   /api/events/{publicId}/admins/invite         - Enviar invitación
DELETE /api/events/{publicId}/admins/{userId}       - Remover admin
GET    /api/events/{publicId}/invitations           - Invitaciones pendientes
POST   /api/invitations/{token}/accept              - Aceptar invitación (token independiente)
```

**EventEstablishmentsController:**
```
GET    /api/events/{publicId}/establishments        - Lista asociados
POST   /api/events/{publicId}/establishments        - Agregar
DELETE /api/events/{publicId}/establishments/{id}   - Remover
GET    /api/events/{publicId}/establishments/search - Buscar disponibles
```

### Validación de Acceso (Authorization)

Cada endpoint que requiere `{publicId}` implementa:

```csharp
// En cada Feature/Handler
var event = await _eventRepository.GetByPublicIdAsync(publicId);
if (event == null) return NotFound();

// Validar que el usuario tenga acceso al evento
var currentUserId = _currentUserService.UserId;
if (!event.HasAccess(currentUserId) && !_currentUserService.HasRole("PlatformAdmin"))
    return Forbid(); // 403 Forbidden
```

**Reglas de acceso:**
- El usuario debe ser **Organizador** del evento, O
- El usuario debe ser **Admin** del evento (estar en EventAdmins), O
- El usuario debe tener rol **PlatformAdmin**

### Permisos

| Permiso | Descripción |
|---------|-------------|
| `events.view` | Ver eventos donde participa |
| `events.create` | Crear eventos (Admin Plataforma) |
| `events.edit` | Editar información del evento |
| `events.delete` | Eliminar eventos |
| `events.manage-admins` | Invitar/remover administradores |
| `events.manage-establishments` | Asociar establecimientos |

---

## 5. Frontend (Angular)

**IMPORTANTE:** Esta fase requiere dos layouts/estructuras diferenciadas.

### Estructura de Carpetas

```
src/app/features/
├── events/                                    [NEW] Módulo de eventos
│   ├── models/
│   │   └── event.model.ts                    [NEW]
│   ├── services/
│   │   └── event.service.ts                  [NEW]
│   ├── pages/
│   │   ├── event-list/                       [NEW] Vista principal (listado)
│   │   │   ├── event-list.page.ts
│   │   │   ├── event-list.page.html
│   │   │   └── event-list.page.css
│   │   └── event-form/                       [NEW] Crear/editar evento
│   │       ├── event-form.page.ts
│   │       ├── event-form.page.html
│   │       └── event-form.page.css
│   └── events.routes.ts                      [NEW]
│
└── event-admin/                              [NEW] Layout interno del evento
    ├── layouts/
    │   └── event-layout/                     [NEW] Layout con sidebar del evento
    │       ├── event-layout.component.ts
    │       ├── event-layout.component.html
    │       └── event-layout.component.css
    ├── pages/
    │   ├── overview/                         [NEW] Vista general
    │   │   ├── overview.page.ts
    │   │   ├── overview.page.html
    │   │   └── overview.page.css
    │   ├── establishments/                   [NEW] Gestión establecimientos
    │   │   ├── establishments.page.ts
    │   │   ├── establishments.page.html
    │   │   └── establishments.page.css
    │   ├── admins/                           [NEW] Gestión administradores
    │   │   ├── admins.page.ts
    │   │   ├── admins.page.html
    │   │   └── admins.page.css
    │   ├── settings/                         [NEW] Configuración evento
    │   │   ├── settings.page.ts
    │   │   ├── settings.page.html
    │   │   └── settings.page.css
    │   └── tournaments/                      [NEW] Lista torneos (vacío, Fase 5)
    │       ├── tournaments.page.ts
    │       ├── tournaments.page.html
    │       └── tournaments.page.css
    ├── components/
    │   ├── invite-admin-modal/               [NEW]
    │   └── establishment-search/             [NEW]
    └── event-admin.routes.ts                 [NEW]
```

### Archivos a Modificar

| Archivo | Cambio |
|---------|--------|
| `app.routes.ts` | Agregar rutas `events` y `events/:publicId` |
| `main-layout.component.ts` | Cambiar vista principal a eventos, agregar link a admin |

### Layouts

1. **Layout Principal** (`MainLayoutComponent`): 
   - Usará `/app/events` como página principal post-login
   - Mostrará listado de eventos del usuario

2. **Layout de Evento** (`EventLayoutComponent`):
   - Usado en `/app/events/:publicId/*`
   - Sidebar con módulos: Vista General, Torneos, Establecimientos, Admins, Configuración
   - Header con nombre del evento
   - Guard que valida acceso al evento antes de cargar

---

## 6. Migraciones SQL

**Ubicación:** `migraciones/`

| Archivo | Descripción |
|---------|-------------|
| `[NEW]` migracion04.sql | Script completo de Fase 4 |

### Contenido

```sql
-- 1. Tabla Events
CREATE TABLE Events (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PublicId CHAR(36) NOT NULL COMMENT 'GUID para URLs seguras',
    Name VARCHAR(200) NOT NULL,
    Description TEXT NULL,
    OrganizerId INT NOT NULL,
    ContactPhone VARCHAR(20) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    PosterVertical LONGTEXT NULL COMMENT 'Base64 1080x1920',
    PosterHorizontal LONGTEXT NULL COMMENT 'Base64 1920x1080',
    RulesPdf LONGTEXT NULL COMMENT 'Base64 PDF',
    WhatsApp VARCHAR(50) NULL,
    Facebook VARCHAR(200) NULL,
    Instagram VARCHAR(200) NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Events_Users FOREIGN KEY (OrganizerId) REFERENCES Users(Id),
    CONSTRAINT CK_Events_Dates CHECK (EndDate >= StartDate),
    CONSTRAINT UQ_Events_PublicId UNIQUE (PublicId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Events_PublicId ON Events(PublicId);
CREATE INDEX IX_Events_OrganizerId ON Events(OrganizerId);
CREATE INDEX IX_Events_IsActive ON Events(IsActive);
CREATE INDEX IX_Events_StartDate ON Events(StartDate);

-- 2. Tabla EventEstablishments (M:N)
CREATE TABLE EventEstablishments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EventId INT NOT NULL,
    EstablishmentId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_EventEstablishments_Events FOREIGN KEY (EventId) 
        REFERENCES Events(Id) ON DELETE CASCADE,
    CONSTRAINT FK_EventEstablishments_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_EventEstablishments UNIQUE (EventId, EstablishmentId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 3. Tabla EventAdmins
CREATE TABLE EventAdmins (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EventId INT NOT NULL,
    UserId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_EventAdmins_Events FOREIGN KEY (EventId) 
        REFERENCES Events(Id) ON DELETE CASCADE,
    CONSTRAINT FK_EventAdmins_Users FOREIGN KEY (UserId) 
        REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_EventAdmins UNIQUE (EventId, UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 4. Tabla EventInvitations
CREATE TABLE EventInvitations (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EventId INT NOT NULL,
    Email VARCHAR(256) NOT NULL,
    Token VARCHAR(100) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    AcceptedAt DATETIME NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_EventInvitations_Events FOREIGN KEY (EventId) 
        REFERENCES Events(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_EventInvitations_Token UNIQUE (Token)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_EventInvitations_Token ON EventInvitations(Token);
CREATE INDEX IX_EventInvitations_Email ON EventInvitations(Email);

-- 5. Permisos
INSERT INTO Permissions (Code, Name, Description, Module) VALUES
('events.view', 'Ver eventos', 'Permite ver eventos donde participa', 'Events'),
('events.create', 'Crear eventos', 'Permite crear nuevos eventos (Admin Plataforma)', 'Events'),
('events.edit', 'Editar eventos', 'Permite editar información del evento', 'Events'),
('events.delete', 'Eliminar eventos', 'Permite eliminar/desactivar eventos', 'Events'),
('events.manage-admins', 'Gestionar admins', 'Permite invitar y remover administradores', 'Events'),
('events.manage-establishments', 'Gestionar establecimientos', 'Permite asociar establecimientos al evento', 'Events');

-- 6. Asignar permisos a PlatformAdmin
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions WHERE Code LIKE 'events.%';

-- 7. Permisos parciales a Organizer (RoleId = 2)
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 2, Id FROM Permissions 
WHERE Code IN ('events.view', 'events.edit', 'events.manage-admins', 'events.manage-establishments');
```

---

## Resumen de Archivos

### Por Capa

| Capa | Nuevos | Modificados | Total |
|------|--------|-------------|-------|
| Domain | 5 | 0 | 5 |
| Persistence | 5 | 2 | 7 |
| Application | 6 | 1 | 7 |
| API | 3 | 0 | 3 |
| Frontend | 35 | 2 | 37 |
| Migraciones | 1 | 0 | 1 |
| **TOTAL** | **55** | **5** | **60** |

---

## Plan de Verificación

### Tests Manuales

| # | Escenario | Pasos | Resultado Esperado |
|---|-----------|-------|-------------------|
| 1 | Ver mis eventos | Login → Página principal | Listado de eventos del usuario |
| 2 | Crear evento | Admin → Crear evento → Llenar datos + poster | Evento creado |
| 3 | Validar fechas | Crear evento con fecha fin < inicio | Error de validación |
| 4 | Subir posters | Editar evento → Subir poster vertical y horizontal | Imágenes visibles |
| 5 | Subir PDF reglas | Editar → Subir PDF | PDF guardado y descargable |
| 6 | Asociar establecimiento | Interior evento → Establecimientos → Buscar → Agregar | Establecimiento asociado |
| 7 | Invitar admin | Interior evento → Admins → Invitar por email | Invitación enviada |
| 8 | Expiración invitación | Esperar 30+ min → Intentar aceptar | Error: invitación expirada |
| 9 | Aceptar invitación | Usuario recibe email → Clic en link → Aceptar | Usuario aparece como admin |
| 10 | Remover admin | Interior evento → Admins → Remover | Admin eliminado |
| 11 | Navegación módulos | Interior evento → Clic en cada módulo | Navegación funcional |

### Verificación API

```bash
# Listar mis eventos
curl -X GET http://localhost:5242/api/events -H "Authorization: Bearer {token}"

# Crear evento (Admin)
curl -X POST http://localhost:5242/api/events \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Torneo ABC","organizerId":2,"startDate":"2026-03-01","endDate":"2026-03-15"}'

# Invitar admin
curl -X POST http://localhost:5242/api/events/1/admins/invite \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com"}'

# Asociar establecimiento
curl -X POST http://localhost:5242/api/events/1/establishments \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"establishmentId":1}'
```

---

## Orden de Implementación

1. **Migraciones SQL** - Crear tablas y permisos
2. **Domain Layer** - Entidades e interfaces
3. **Persistence Layer** - Configuraciones y repositorios
4. **Application Layer** - DTOs, Validators, Features
5. **API Layer** - Controllers
6. **Frontend - Events Module** - Listado y formulario de eventos
7. **Frontend - Event Admin Layout** - Layout interno con sidebar
8. **Frontend - Módulos internos** - Overview, Establishments, Admins, Settings, Tournaments (vacío)
9. **Testing manual**

---

## Estimación

| Capa | Tiempo Estimado |
|------|-----------------|
| Migraciones SQL | 30 min |
| Domain + Persistence | 2 horas |
| Application Layer | 3 horas |
| API Layer | 1.5 horas |
| Frontend - Events Module | 3 horas |
| Frontend - Event Admin Layout | 3 horas |
| Frontend - Módulos internos | 4 horas |
| Testing | 1 hora |
| **Total** | **18-20 horas** |

---

## Notas de Arquitectura

### Patrones Seguidos

1. **Estructura de carpetas:** Igual a fases anteriores
2. **DTOs separados:** Record types para inmutabilidad
3. **Features como Use Cases:** Una clase por operación
4. **Validators con FluentValidation:** En subcarpeta por módulo
5. **EF Configurations:** Fluent API separado del DbContext
6. **Frontend standalone components:** Sin NgModules

### Decisiones de Diseño

| Decisión | Justificación |
|----------|---------------|
| Dos layouts diferenciados | Mejor UX, separación de contextos |
| **PublicId (GUID) en URLs** | Evita enumeración/scraping de IDs numéricos |
| **Validación de acceso interna** | Usuario solo ve eventos donde participa |
| Invitaciones con token | Seguridad, expiración controlable |
| Posters como base64 | Simplicidad, sin file server |
| Módulo Torneos vacío | Preparación para Fase 5 |
| OrganizerId fijo | Un único organizador designado por admin |

### Flujo de Navegación

```
/login
    ↓
/app/events                          ← Vista principal (listado de eventos)
    ↓ clic en evento
/app/events/:publicId                ← Redirección a overview (GUID en URL)
    ↓
/app/events/:publicId/overview       ← Layout interno, módulo activo
/app/events/:publicId/tournaments
/app/events/:publicId/establishments
/app/events/:publicId/admins
/app/events/:publicId/settings
```

### Ejemplo de URL

```
/app/events/a1b2c3d4-e5f6-7890-abcd-ef1234567890/overview
```

> El PublicId es un GUID generado automáticamente al crear el evento. No se puede adivinar ni enumerar.
