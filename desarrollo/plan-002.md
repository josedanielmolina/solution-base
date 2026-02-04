# Plan de Implementación - Fase 2: Mantenimientos

## Objetivo

Panel de administración con catálogos globales del sistema: Países, Ciudades, Categorías y Jugadores.

## Dependencias

- ✅ FASE 1: Autenticación (para acceso al panel admin con permisos)

---

## Entregables

| # | Entregable | Descripción |
|---|------------|-------------|
| 1 | CRUD Países | Listado + Crear + Editar + Eliminar países |
| 2 | CRUD Ciudades | Listado + Crear + Editar + Eliminar ciudades (por país) |
| 3 | CRUD Categorías | Gestión de categorías jerárquicas (País > Género > Categoría) |
| 4 | CRUD Jugadores | Gestión global de jugadores (documento único) |
| 5 | Asociación Usuario-Jugador | Vincular/desvincular jugadores con usuarios |
| 6 | Soft Delete | Implementación de eliminación lógica |
| 7 | Validaciones | Documento único, nombres únicos por contexto |

---

## Modelo de Datos

### Entidades Nuevas

```
┌─────────────┐     ┌─────────────┐     ┌─────────────────┐
│   Country   │     │    City     │     │    Category     │
├─────────────┤     ├─────────────┤     ├─────────────────┤
│ Id          │1───*│ Id          │     │ Id              │
│ Name        │     │ CountryId   │     │ Name            │
│ Code (ISO)  │     │ Name        │     │ Gender          │
│ IsActive    │     │ IsActive    │     │ CountryId       │
│ CreatedAt   │     │ CreatedAt   │     │ IsActive        │
│ UpdatedAt   │     │ UpdatedAt   │     │ CreatedAt       │
└─────────────┘     └─────────────┘     └─────────────────┘
                           │
                           │ *
                    ┌──────┴──────┐
                    │   Player    │
                    ├─────────────┤
                    │ Id          │
                    │ FirstName   │
                    │ LastName    │
                    │ Document    │ (único globalmente)
                    │ Phone       │
                    │ Email       │
                    │ BirthDate   │
                    │ CityId      │
                    │ UserId      │ (nullable - vinculación)
                    │ IsActive    │
                    │ CreatedAt   │
                    │ UpdatedAt   │
                    └─────────────┘
```

---

## Implementación Por Capas

### 1. Domain Layer

#### Nuevas Entidades

| Archivo | Descripción |
|---------|-------------|
| `[NEW] Country.cs` | País con código ISO |
| `[NEW] City.cs` | Ciudad con FK a Country |
| `[NEW] Category.cs` | Categoría con País + Género |
| `[MODIFY] Player.cs` | Ya existe, agregar CityId y propiedades faltantes |

#### Interfaces de Repositorio

| Archivo | Descripción |
|---------|-------------|
| `[NEW] ICountryRepository.cs` | `GetByCodeAsync`, `GetAllActiveAsync` |
| `[NEW] ICityRepository.cs` | `GetByCountryAsync`, `ExistsByNameInCountryAsync` |
| `[NEW] ICategoryRepository.cs` | `GetByCountryAsync`, `GetByGenderAsync` |
| `[MODIFY] IPlayerRepository.cs` | Ya existe, agregar métodos de búsqueda |

---

### 2. Persistence Layer

#### Configurations

| Archivo | Descripción |
|---------|-------------|
| `[NEW] CountryConfiguration.cs` | Index único en Name y Code |
| `[NEW] CityConfiguration.cs` | Index único Name + CountryId |
| `[NEW] CategoryConfiguration.cs` | Index único Name + CountryId + Gender |
| `[MODIFY] PlayerConfiguration.cs` | Agregar relación City |

#### Repositories

| Archivo | Descripción |
|---------|-------------|
| `[NEW] CountryRepository.cs` | |
| `[NEW] CityRepository.cs` | |
| `[NEW] CategoryRepository.cs` | |
| `[MODIFY] PlayerRepository.cs` | Métodos de búsqueda |

---

### 3. Application Layer

#### DTOs

| Módulo | DTOs |
|--------|------|
| Countries | `CountryDto`, `CreateCountryDto`, `UpdateCountryDto` |
| Cities | `CityDto`, `CreateCityDto`, `UpdateCityDto` |
| Categories | `CategoryDto`, `CreateCategoryDto`, `UpdateCategoryDto` |
| Players | `PlayerDto`, `CreatePlayerDto`, `UpdatePlayerDto`, `LinkPlayerToUserDto` |

#### Validators

| Archivo | Validaciones |
|---------|--------------|
| `CreateCountryValidator` | Name required, Code 2-3 chars |
| `CreateCityValidator` | Name required, CountryId exists |
| `CreateCategoryValidator` | Name required, Gender enum, CountryId exists |
| `CreatePlayerValidator` | Document required y único, nombres required |

#### Features

| Módulo | Features |
|--------|----------|
| **Countries** | `GetCountries`, `GetCountryById`, `CreateCountry`, `UpdateCountry`, `DeleteCountry` |
| **Cities** | `GetCities`, `GetCitiesByCountry`, `CreateCity`, `UpdateCity`, `DeleteCity` |
| **Categories** | `GetCategories`, `GetCategoriesByCountry`, `CreateCategory`, `UpdateCategory`, `DeleteCategory` |
| **Players** | `GetPlayers`, `GetPlayerById`, `CreatePlayer`, `UpdatePlayer`, `DeletePlayer`, `SearchPlayers`, `LinkPlayerToUser`, `UnlinkPlayerFromUser` |

---

### 4. API Layer

#### Controllers

| Controller | Endpoints |
|------------|-----------|
| `CountriesController` | `GET /api/countries`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |
| `CitiesController` | `GET /api/cities`, `GET /by-country/{countryId}`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |
| `CategoriesController` | `GET /api/categories`, `GET /by-country/{countryId}`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |
| `PlayersController` | `GET /api/players`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`, `GET /search`, `POST /{id}/link-user`, `DELETE /{id}/unlink-user` |

#### Permisos Requeridos

| Permiso | Descripción |
|---------|-------------|
| `countries.view` | Ver países |
| `countries.create` | Crear países |
| `countries.edit` | Editar países |
| `countries.delete` | Eliminar países |
| `cities.view` / `create` / `edit` / `delete` | CRUD ciudades |
| `categories.view` / `create` / `edit` / `delete` | CRUD categorías |
| `players.view` / `create` / `edit` / `delete` | CRUD jugadores |
| `players.link-user` | Vincular jugador a usuario |

---

### 5. Frontend

#### Estructura de Features

```
src/app/features/
├── admin/
│   ├── countries/
│   │   ├── pages/
│   │   │   ├── country-list/
│   │   │   └── country-form/
│   │   ├── models/country.model.ts
│   │   ├── services/country.service.ts
│   │   └── countries.routes.ts
│   ├── cities/
│   │   ├── pages/
│   │   │   ├── city-list/
│   │   │   └── city-form/
│   │   ├── models/city.model.ts
│   │   ├── services/city.service.ts
│   │   └── cities.routes.ts
│   ├── categories/
│   │   ├── pages/
│   │   │   ├── category-list/
│   │   │   └── category-form/
│   │   ├── models/category.model.ts
│   │   ├── services/category.service.ts
│   │   └── categories.routes.ts
│   └── players/
│       ├── pages/
│       │   ├── player-list/
│       │   └── player-form/
│       ├── models/player.model.ts
│       ├── services/player.service.ts
│       └── players.routes.ts
```

#### Componentes Comunes

| Componente | Descripción |
|------------|-------------|
| `confirmation-dialog` | Diálogo de confirmación para eliminar |
| `country-select` | Selector de país reutilizable |
| `city-select` | Selector de ciudad por país |
| `gender-select` | Selector de género (Masculino/Femenino/Mixto) |

---

## Migraciones SQL

### Script de Migración

```sql
-- Tabla Countries
CREATE TABLE Countries (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Code VARCHAR(3) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    UNIQUE INDEX IX_Countries_Name (Name),
    UNIQUE INDEX IX_Countries_Code (Code)
);

-- Tabla Cities
CREATE TABLE Cities (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    CountryId INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (CountryId) REFERENCES Countries(Id),
    UNIQUE INDEX IX_Cities_Name_Country (Name, CountryId)
);

-- Tabla Categories
CREATE TABLE Categories (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Gender ENUM('Male', 'Female', 'Mixed') NOT NULL,
    CountryId INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (CountryId) REFERENCES Countries(Id),
    UNIQUE INDEX IX_Categories_Name_Country_Gender (Name, CountryId, Gender)
);

-- Modificar Players (agregar CityId si no existe)
ALTER TABLE Players
    ADD COLUMN CityId INT NULL,
    ADD FOREIGN KEY (CityId) REFERENCES Cities(Id);

-- Seed data
INSERT INTO Countries (Name, Code) VALUES ('Colombia', 'CO');
```

---

## Plan de Verificación

### Tests Automatizados

1. **Unit Tests (Backend)**
   ```bash
   cd tests/Core.Application.Tests
   dotnet test --filter "FullyQualifiedName~Countries"
   dotnet test --filter "FullyQualifiedName~Cities"
   dotnet test --filter "FullyQualifiedName~Categories"
   dotnet test --filter "FullyQualifiedName~Players"
   ```

2. **Frontend Build**
   ```bash
   cd src/UI/WebApp
   npm run build
   ```

### Tests Manuales

| # | Escenario | Pasos | Resultado Esperado |
|---|-----------|-------|-------------------|
| 1 | Crear país | Login admin → Admin → Países → Nuevo → "Argentina" + "AR" → Guardar | País creado, aparece en lista |
| 2 | Crear país duplicado | Intentar crear "Colombia" de nuevo | Error: nombre ya existe |
| 3 | Crear ciudad | Países → Colombia → Ciudades → Nueva → "Bogotá" → Guardar | Ciudad creada bajo Colombia |
| 4 | Crear categoría | Categorías → Nueva → "Primera" + "Masculino" + "Colombia" | Categoría jerárquica creada |
| 5 | Crear jugador | Jugadores → Nuevo → Documento único → Guardar | Jugador creado |
| 6 | Buscar jugador | Buscador → Escribir documento → Seleccionar | Jugador encontrado y seleccionable |
| 7 | Soft delete | Eliminar país → Confirmar | País marcado como inactivo, no destruido |

---

## Orden de Implementación

1. **Backend Domain + Persistence** (Entidades, Configs, Repos)
2. **Backend Application** (DTOs, Validators, Features)
3. **Backend API** (Controllers con permisos)
4. **Migraciones SQL** + Seed data
5. **Frontend Services + Models**
6. **Frontend Pages**
7. **Tests y verificación**

---

## Estimación

| Capa | Tiempo Estimado |
|------|-----------------|
| Domain + Persistence | 1-2 horas |
| Application Layer | 2-3 horas |
| API Layer | 1 hora |
| Frontend complete | 3-4 horas |
| Testing | 1 hora |
| **Total** | **8-11 horas** |
