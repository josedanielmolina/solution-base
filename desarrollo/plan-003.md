# Plan de Implementación - Fase 3: Establecimientos

## Objetivo

Gestión completa de establecimientos (clubs/centros deportivos) y canchas de la plataforma. Los establecimientos son entidades **globales** gestionadas por el Admin de Plataforma.

## Dependencias

- ✅ FASE 1: Autenticación (sistema de permisos)
- ✅ FASE 2: Mantenimientos (Países y Ciudades como catálogos)

---

## Entregables

| # | Entregable | Descripción |
|---|------------|-------------|
| 1 | CRUD Establecimientos | Crear, editar, listar, soft-delete establecimientos |
| 2 | Información completa | País, Ciudad, Nombre, Logo, Fotos, Teléfonos, Dirección, Maps, Horarios |
| 3 | Gestión de Horarios | Horario continuo o por bloques |
| 4 | CRUD Canchas | Agregar/editar/eliminar canchas por establecimiento |
| 5 | Carga de imágenes | Upload de logo y fotos (almacenamiento en BD como base64) |
| 6 | Validaciones | Nombre único, al menos un teléfono, al menos una cancha |

---

## Modelo de Datos

### Nuevas Entidades

```
┌───────────────────────────────────────────────────────────────────────────┐
│                            ESTABLISHMENT                                   │
├───────────────────────────────────────────────────────────────────────────┤
│ Id (PK)                                                                   │
│ Name (unique)                                                             │
│ CountryId (FK → Countries)                                                │
│ CityId (FK → Cities)                                                      │
│ Address                                                                   │
│ GoogleMapsUrl (nullable)                                                  │
│ PhoneLandline (nullable)                                                  │
│ PhoneMobile (nullable)                                                    │
│ Logo (LONGTEXT base64, nullable)                                          │
│ ScheduleType (enum: Continuous=1, Blocks=2)                               │
│ IsActive, CreatedAt, UpdatedAt                                            │
└───────────────────────────────────────────────────────────────────────────┘
                                    │
                          1─────────┼─────────*
                                    │
┌──────────────────────────┐        │        ┌──────────────────────────────┐
│  ESTABLISHMENT_SCHEDULE  │        │        │   ESTABLISHMENT_PHOTO        │
├──────────────────────────┤        │        ├──────────────────────────────┤
│ Id (PK)                  │        │        │ Id (PK)                      │
│ EstablishmentId (FK)     │        │        │ EstablishmentId (FK)         │
│ DayOfWeek (1-7)          │        │        │ ImageData (LONGTEXT base64)  │
│ OpenTime (TIME)          │        │        │ DisplayOrder                 │
│ CloseTime (TIME)         │        │        │ CreatedAt                    │
│ BlockNumber (1,2,3...)   │        │        └──────────────────────────────┘
└──────────────────────────┘        │
                                    │
                          1─────────┼─────────*
                                    │
                        ┌───────────────────────────┐
                        │          COURT            │
                        ├───────────────────────────┤
                        │ Id (PK)                   │
                        │ EstablishmentId (FK)      │
                        │ Name                      │
                        │ CourtType (Indoor=1,      │
                        │           Outdoor=2)      │
                        │ IsActive, CreatedAt       │
                        └───────────────────────────┘
                                    │
                          1─────────┼─────────*
                                    │
                        ┌───────────────────────────┐
                        │       COURT_PHOTO         │
                        ├───────────────────────────┤
                        │ Id (PK)                   │
                        │ CourtId (FK)              │
                        │ ImageData (LONGTEXT)      │
                        │ DisplayOrder              │
                        │ CreatedAt                 │
                        └───────────────────────────┘
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
| `[NEW]` Establishment.cs | `Entities/Establishment.cs` | Entidad principal con propiedades y navegaciones |
| `[NEW]` EstablishmentPhoto.cs | `Entities/EstablishmentPhoto.cs` | Fotos del establecimiento |
| `[NEW]` EstablishmentSchedule.cs | `Entities/EstablishmentSchedule.cs` | Horarios (continuo o bloques) |
| `[NEW]` Court.cs | `Entities/Court.cs` | Cancha con tipo Indoor/Outdoor |
| `[NEW]` CourtPhoto.cs | `Entities/CourtPhoto.cs` | Fotos de cancha |
| `[NEW]` IEstablishmentRepository.cs | `Interfaces/Repositories/IEstablishmentRepository.cs` | Interfaz del repositorio |
| `[NEW]` ICourtRepository.cs | `Interfaces/Repositories/ICourtRepository.cs` | Interfaz del repositorio de canchas |

**Justificación:**
- Las entidades siguen el patrón de Domain-Driven Design del proyecto
- Todas heredan de `BaseEntity` que provee `Id`, `CreatedAt`, `UpdatedAt`
- Los repositorios se definen en Domain para inversión de dependencias

### Contenido de las Entidades

```csharp
// Establishment.cs
public enum ScheduleType { Continuous = 1, Blocks = 2 }

public class Establishment : BaseEntity
{
    public string Name { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string Address { get; set; }
    public string? GoogleMapsUrl { get; set; }
    public string? PhoneLandline { get; set; }
    public string? PhoneMobile { get; set; }
    public string? Logo { get; set; }  // Base64
    public ScheduleType ScheduleType { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public virtual Country Country { get; set; }
    public virtual City City { get; set; }
    public virtual ICollection<Court> Courts { get; set; }
    public virtual ICollection<EstablishmentPhoto> Photos { get; set; }
    public virtual ICollection<EstablishmentSchedule> Schedules { get; set; }
}

// Court.cs
public enum CourtType { Indoor = 1, Outdoor = 2 }

public class Court : BaseEntity
{
    public int EstablishmentId { get; set; }
    public string Name { get; set; }
    public CourtType CourtType { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public virtual Establishment Establishment { get; set; }
    public virtual ICollection<CourtPhoto> Photos { get; set; }
}
```

---

## 2. Persistence Layer

**Ubicación:** `src/Infrastructure/Infrastructure.Persistence/`

| Archivo | Ruta Completa | Descripción |
|---------|---------------|-------------|
| `[NEW]` EstablishmentConfiguration.cs | `Configurations/EstablishmentConfiguration.cs` | Configuración EF Core |
| `[NEW]` EstablishmentPhotoConfiguration.cs | `Configurations/EstablishmentPhotoConfiguration.cs` | Configuración fotos |
| `[NEW]` EstablishmentScheduleConfiguration.cs | `Configurations/EstablishmentScheduleConfiguration.cs` | Configuración horarios |
| `[NEW]` CourtConfiguration.cs | `Configurations/CourtConfiguration.cs` | Configuración canchas |
| `[NEW]` CourtPhotoConfiguration.cs | `Configurations/CourtPhotoConfiguration.cs` | Configuración fotos cancha |
| `[NEW]` EstablishmentRepository.cs | `Repositories/EstablishmentRepository.cs` | Implementación repositorio |
| `[NEW]` CourtRepository.cs | `Repositories/CourtRepository.cs` | Implementación repositorio |
| `[MODIFY]` ApplicationDbContext.cs | `Context/ApplicationDbContext.cs` | Agregar DbSets |
| `[MODIFY]` DependencyInjection.cs | `DependencyInjection.cs` | Registrar repositorios |

**Justificación:**
- Sigue el patrón de configuración fluent de EF Core usado en el proyecto
- Los repositorios implementan las interfaces del Domain
- El registro de DI sigue el patrón existente

---

## 3. Application Layer

**Ubicación:** `src/Core/Core.Application/`

### DTOs

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EstablishmentDtos.cs | `DTOs/Establishments/EstablishmentDtos.cs` |
| `[NEW]` CourtDtos.cs | `DTOs/Establishments/CourtDtos.cs` |
| `[NEW]` ScheduleDtos.cs | `DTOs/Establishments/ScheduleDtos.cs` |

**DTOs incluidos:**
```
EstablishmentDto, EstablishmentListDto, CreateEstablishmentDto, UpdateEstablishmentDto
CourtDto, CreateCourtDto, UpdateCourtDto
ScheduleDto, CreateScheduleDto
PhotoDto, CreatePhotoDto
```

### Validators

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EstablishmentValidators.cs | `Validators/Establishments/EstablishmentValidators.cs` |
| `[NEW]` CourtValidators.cs | `Validators/Establishments/CourtValidators.cs` |

**Validaciones:**
- Nombre único del establecimiento
- Al menos un teléfono (fijo o celular)
- Dirección obligatoria
- País y Ciudad obligatorios
- Cancha: nombre obligatorio, tipo válido

### Features

| Archivo | Ruta Completa |
|---------|---------------|
| `[NEW]` EstablishmentFeatures.cs | `Features/Establishments/EstablishmentFeatures.cs` |
| `[NEW]` CourtFeatures.cs | `Features/Establishments/CourtFeatures.cs` |

**Features incluidos:**
```
// Establishments
GetEstablishments, GetEstablishmentById, CreateEstablishment, 
UpdateEstablishment, DeleteEstablishment, SearchEstablishments,
AddEstablishmentPhoto, RemoveEstablishmentPhoto,
SetEstablishmentSchedule

// Courts
GetCourtsByEstablishment, GetCourtById, CreateCourt, 
UpdateCourt, DeleteCourt, AddCourtPhoto, RemoveCourtPhoto
```

### Registro DI

| Archivo | Ruta Completa |
|---------|---------------|
| `[MODIFY]` DependencyInjection.cs | `DependencyInjection.cs` | Registrar Features y Validators |

---

## 4. API Layer

**Ubicación:** `src/Presentation/API/`

| Archivo | Ruta Completa | Descripción |
|---------|---------------|-------------|
| `[NEW]` EstablishmentsController.cs | `Controllers/EstablishmentsController.cs` | Endpoints de establecimiento |
| `[NEW]` CourtsController.cs | `Controllers/CourtsController.cs` | Endpoints de canchas |

### Endpoints

**EstablishmentsController:**
```
GET    /api/establishments              - Lista paginada
GET    /api/establishments/{id}         - Detalle con canchas
GET    /api/establishments/search       - Búsqueda por nombre
POST   /api/establishments              - Crear
PUT    /api/establishments/{id}         - Actualizar
DELETE /api/establishments/{id}         - Soft delete
POST   /api/establishments/{id}/photos  - Agregar foto
DELETE /api/establishments/{id}/photos/{photoId} - Eliminar foto
PUT    /api/establishments/{id}/schedule - Configurar horarios
```

**CourtsController:**
```
GET    /api/establishments/{establishmentId}/courts       - Lista
GET    /api/establishments/{establishmentId}/courts/{id}  - Detalle
POST   /api/establishments/{establishmentId}/courts       - Crear
PUT    /api/establishments/{establishmentId}/courts/{id}  - Actualizar
DELETE /api/establishments/{establishmentId}/courts/{id}  - Soft delete
POST   /api/establishments/{establishmentId}/courts/{id}/photos  - Agregar foto
DELETE /api/establishments/{establishmentId}/courts/{id}/photos/{photoId} - Eliminar foto
```

### Permisos

| Permiso | Descripción |
|---------|-------------|
| `establishments.view` | Ver listado y detalle |
| `establishments.create` | Crear establecimientos |
| `establishments.edit` | Editar establecimientos y canchas |
| `establishments.delete` | Eliminar (soft delete) |

---

## 5. Frontend (Angular)

**Ubicación:** `src/UI/WebApp/src/app/features/admin/`

### Estructura de Carpetas

```
src/app/features/admin/establishments/
├── models/
│   ├── establishment.model.ts        [NEW]
│   └── court.model.ts                [NEW]
├── services/
│   └── establishment.service.ts      [NEW]
├── pages/
│   ├── establishment-list/
│   │   ├── establishment-list.page.ts      [NEW]
│   │   ├── establishment-list.page.html    [NEW]
│   │   └── establishment-list.page.css     [NEW]
│   ├── establishment-form/
│   │   ├── establishment-form.page.ts      [NEW]
│   │   ├── establishment-form.page.html    [NEW]
│   │   └── establishment-form.page.css     [NEW]
│   └── establishment-detail/
│       ├── establishment-detail.page.ts    [NEW]
│       ├── establishment-detail.page.html  [NEW]
│       └── establishment-detail.page.css   [NEW]
├── components/
│   ├── court-form/
│   │   ├── court-form.component.ts         [NEW]
│   │   ├── court-form.component.html       [NEW]
│   │   └── court-form.component.css        [NEW]
│   ├── schedule-form/
│   │   ├── schedule-form.component.ts      [NEW]
│   │   ├── schedule-form.component.html    [NEW]
│   │   └── schedule-form.component.css     [NEW]
│   └── photo-uploader/
│       ├── photo-uploader.component.ts     [NEW]
│       ├── photo-uploader.component.html   [NEW]
│       └── photo-uploader.component.css    [NEW]
└── establishments.routes.ts          [NEW]
```

### Archivos a Modificar

| Archivo | Cambio |
|---------|--------|
| `admin.routes.ts` | Agregar ruta `establishments` |
| `main-layout.component.ts` | Agregar item menú "Establecimientos" |

---

## 6. Migraciones SQL

**Ubicación:** `migraciones/`

| Archivo | Descripción |
|---------|-------------|
| `[NEW]` migracion03.sql | Script completo de Fase 3 |

### Contenido

```sql
-- 1. Tabla Establishments
CREATE TABLE Establishments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    CountryId INT NOT NULL,
    CityId INT NOT NULL,
    Address VARCHAR(500) NOT NULL,
    GoogleMapsUrl VARCHAR(500) NULL,
    PhoneLandline VARCHAR(20) NULL,
    PhoneMobile VARCHAR(20) NULL,
    Logo LONGTEXT NULL,
    ScheduleType TINYINT NOT NULL DEFAULT 1,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Establishments_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id),
    CONSTRAINT FK_Establishments_Cities FOREIGN KEY (CityId) REFERENCES Cities(Id),
    CONSTRAINT UQ_Establishments_Name UNIQUE (Name),
    CONSTRAINT CK_Establishments_Phone CHECK (PhoneLandline IS NOT NULL OR PhoneMobile IS NOT NULL)
);

-- 2. Tabla EstablishmentPhotos
CREATE TABLE EstablishmentPhotos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    ImageData LONGTEXT NOT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_EstablishmentPhotos_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE
);

-- 3. Tabla EstablishmentSchedules
CREATE TABLE EstablishmentSchedules (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    DayOfWeek TINYINT NOT NULL,
    OpenTime TIME NOT NULL,
    CloseTime TIME NOT NULL,
    BlockNumber TINYINT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_EstablishmentSchedules_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE,
    CONSTRAINT CK_EstablishmentSchedules_DayOfWeek CHECK (DayOfWeek BETWEEN 1 AND 7)
);

-- 4. Tabla Courts
CREATE TABLE Courts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    CourtType TINYINT NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Courts_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Courts_Name_Establishment UNIQUE (Name, EstablishmentId),
    CONSTRAINT CK_Courts_Type CHECK (CourtType IN (1, 2))
);

-- 5. Tabla CourtPhotos
CREATE TABLE CourtPhotos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourtId INT NOT NULL,
    ImageData LONGTEXT NOT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_CourtPhotos_Courts FOREIGN KEY (CourtId) 
        REFERENCES Courts(Id) ON DELETE CASCADE
);

-- 6. Permisos
INSERT INTO Permissions (Code, Name, Description, Module) VALUES
('establishments.view', 'Ver establecimientos', 'Permite ver la lista de establecimientos', 'Establishments'),
('establishments.create', 'Crear establecimientos', 'Permite crear nuevos establecimientos', 'Establishments'),
('establishments.edit', 'Editar establecimientos', 'Permite editar establecimientos y canchas', 'Establishments'),
('establishments.delete', 'Eliminar establecimientos', 'Permite desactivar establecimientos', 'Establishments');

-- 7. Asignar permisos a PlatformAdmin
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions WHERE Code LIKE 'establishments.%';
```

---

## Resumen de Archivos

### Por Capa

| Capa | Nuevos | Modificados | Total |
|------|--------|-------------|-------|
| Domain | 7 | 0 | 7 |
| Persistence | 7 | 2 | 9 |
| Application | 6 | 1 | 7 |
| API | 2 | 0 | 2 |
| Frontend | 24 | 2 | 26 |
| Migraciones | 1 | 0 | 1 |
| **TOTAL** | **47** | **5** | **52** |

### Lista Completa Ordenada

```
[NEW] src/Core/Core.Domain/Entities/Establishment.cs
[NEW] src/Core/Core.Domain/Entities/EstablishmentPhoto.cs
[NEW] src/Core/Core.Domain/Entities/EstablishmentSchedule.cs
[NEW] src/Core/Core.Domain/Entities/Court.cs
[NEW] src/Core/Core.Domain/Entities/CourtPhoto.cs
[NEW] src/Core/Core.Domain/Interfaces/Repositories/IEstablishmentRepository.cs
[NEW] src/Core/Core.Domain/Interfaces/Repositories/ICourtRepository.cs

[NEW] src/Infrastructure/Infrastructure.Persistence/Configurations/EstablishmentConfiguration.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Configurations/EstablishmentPhotoConfiguration.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Configurations/EstablishmentScheduleConfiguration.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Configurations/CourtConfiguration.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Configurations/CourtPhotoConfiguration.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Repositories/EstablishmentRepository.cs
[NEW] src/Infrastructure/Infrastructure.Persistence/Repositories/CourtRepository.cs
[MODIFY] src/Infrastructure/Infrastructure.Persistence/Context/ApplicationDbContext.cs
[MODIFY] src/Infrastructure/Infrastructure.Persistence/DependencyInjection.cs

[NEW] src/Core/Core.Application/DTOs/Establishments/EstablishmentDtos.cs
[NEW] src/Core/Core.Application/DTOs/Establishments/CourtDtos.cs
[NEW] src/Core/Core.Application/DTOs/Establishments/ScheduleDtos.cs
[NEW] src/Core/Core.Application/Validators/Establishments/EstablishmentValidators.cs
[NEW] src/Core/Core.Application/Validators/Establishments/CourtValidators.cs
[NEW] src/Core/Core.Application/Features/Establishments/EstablishmentFeatures.cs
[NEW] src/Core/Core.Application/Features/Establishments/CourtFeatures.cs
[MODIFY] src/Core/Core.Application/DependencyInjection.cs

[NEW] src/Presentation/API/Controllers/EstablishmentsController.cs
[NEW] src/Presentation/API/Controllers/CourtsController.cs

[NEW] src/UI/WebApp/src/app/features/admin/establishments/models/establishment.model.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/models/court.model.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/services/establishment.service.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-list/establishment-list.page.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-list/establishment-list.page.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-list/establishment-list.page.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-form/establishment-form.page.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-form/establishment-form.page.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-form/establishment-form.page.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-detail/establishment-detail.page.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-detail/establishment-detail.page.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/pages/establishment-detail/establishment-detail.page.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/court-form/court-form.component.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/court-form/court-form.component.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/court-form/court-form.component.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/schedule-form/schedule-form.component.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/schedule-form/schedule-form.component.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/schedule-form/schedule-form.component.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/photo-uploader/photo-uploader.component.ts
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/photo-uploader/photo-uploader.component.html
[NEW] src/UI/WebApp/src/app/features/admin/establishments/components/photo-uploader/photo-uploader.component.css
[NEW] src/UI/WebApp/src/app/features/admin/establishments/establishments.routes.ts
[MODIFY] src/UI/WebApp/src/app/features/admin/admin.routes.ts
[MODIFY] src/UI/WebApp/src/app/layouts/main-layout/main-layout.component.ts

[NEW] migraciones/migracion03.sql
```

---

## Plan de Verificación

### Tests Manuales

| # | Escenario | Pasos | Resultado Esperado |
|---|-----------|-------|-------------------|
| 1 | Crear establecimiento | Login → Admin → Establecimientos → Nuevo → Llenar datos → Guardar | Establecimiento creado |
| 2 | Validación teléfono | Intentar crear sin teléfono fijo ni celular | Error: se requiere al menos un teléfono |
| 3 | Nombre único | Intentar crear establecimiento con nombre existente | Error: nombre ya existe |
| 4 | Agregar cancha | Detalle establecimiento → Agregar cancha → Nombre + Tipo | Cancha agregada |
| 5 | Validar cancha requerida | Intentar guardar establecimiento sin canchas | Error o advertencia |
| 6 | Subir logo | Editar establecimiento → Seleccionar imagen → Guardar | Logo visible |
| 7 | Configurar horario continuo | Editar → Horario continuo → 08:00-22:00 | Horario guardado |
| 8 | Configurar horario bloques | Editar → Horario por bloques → Varios rangos | Bloques guardados |
| 9 | Soft delete | Eliminar establecimiento → Confirmar | Establecimiento inactivo pero no destruido |

### Verificación API

```bash
# Listar establecimientos
curl -X GET http://localhost:5242/api/establishments -H "Authorization: Bearer {token}"

# Crear establecimiento
curl -X POST http://localhost:5242/api/establishments \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Club test","countryId":5,"cityId":1,"address":"Calle 123","phoneMobile":"3001234567"}'

# Agregar cancha
curl -X POST http://localhost:5242/api/establishments/1/courts \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Cancha 1","courtType":1}'
```

---

## Orden de Implementación

1. **Migraciones SQL** - Crear tablas y permisos
2. **Domain Layer** - Entidades e interfaces
3. **Persistence Layer** - Configuraciones y repositorios
4. **Application Layer** - DTOs, Validators, Features
5. **API Layer** - Controllers
6. **Frontend** - Models, Services, Pages, Components
7. **Testing manual**

---

## Estimación

| Capa | Tiempo Estimado |
|------|-----------------|
| Migraciones SQL | 30 min |
| Domain + Persistence | 2 horas |
| Application Layer | 3 horas |
| API Layer | 1 hora |
| Frontend | 4-5 horas |
| Testing | 1 hora |
| **Total** | **11-13 horas** |

---

## Notas de Arquitectura

### Patrones Seguidos

1. **Estructura de carpetas:** Igual a Countries/Cities/Categories
2. **DTOs separados:** Record types para inmutabilidad
3. **Features como Use Cases:** Una clase por operación
4. **Validators con FluentValidation:** En subcarpeta por módulo
5. **EF Configurations:** Fluent API separado del DbContext
6. **Frontend standalone components:** Sin NgModules

### Decisiones de Diseño

| Decisión | Justificación |
|----------|---------------|
| Logo como base64 en BD | Simplicidad, sin necesidad de file server |
| Horarios en tabla separada | Flexibilidad para múltiples bloques |
| Soft delete | Mantener historial, evitar pérdida de datos |
| Canchas como entidad separada | Reutilización futura en asignación de partidos |
