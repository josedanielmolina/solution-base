-- =============================================================================
-- MIGRACIÓN 02: Mantenimientos (Países, Ciudades, Categorías)
-- Fase 2: Mantenimientos - App Torneos de Pádel
-- Fecha: 2026-02-01
-- Base de Datos: MySQL 8.x
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. CREAR TABLA COUNTRIES (Países)
-- -----------------------------------------------------------------------------

CREATE TABLE Countries (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Code VARCHAR(3) NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT UQ_Countries_Name UNIQUE (Name),
    CONSTRAINT UQ_Countries_Code UNIQUE (Code)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Countries_IsActive ON Countries(IsActive);

-- -----------------------------------------------------------------------------
-- 2. CREAR TABLA CITIES (Ciudades)
-- -----------------------------------------------------------------------------

CREATE TABLE Cities (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    CountryId INT NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Cities_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Cities_Name_Country UNIQUE (Name, CountryId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Cities_CountryId ON Cities(CountryId);
CREATE INDEX IX_Cities_IsActive ON Cities(IsActive);

-- -----------------------------------------------------------------------------
-- 3. CREAR TABLA CATEGORIES (Categorías de Torneo)
-- -----------------------------------------------------------------------------

CREATE TABLE Categories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Gender TINYINT NOT NULL COMMENT '1=Male, 2=Female, 3=Mixed',
    CountryId INT NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Categories_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Categories_Name_Country_Gender UNIQUE (Name, CountryId, Gender),
    CONSTRAINT CK_Categories_Gender CHECK (Gender IN (1, 2, 3))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Categories_CountryId ON Categories(CountryId);
CREATE INDEX IX_Categories_Gender ON Categories(Gender);
CREATE INDEX IX_Categories_IsActive ON Categories(IsActive);

-- -----------------------------------------------------------------------------
-- 4. MODIFICAR TABLA PLAYERS (Agregar relación con Ciudad)
-- -----------------------------------------------------------------------------

ALTER TABLE Players ADD COLUMN CityId INT NULL;
ALTER TABLE Players ADD COLUMN BirthDate DATE NULL;
ALTER TABLE Players ADD COLUMN IsActive BOOLEAN NOT NULL DEFAULT TRUE;

-- Crear FK a Cities
ALTER TABLE Players ADD CONSTRAINT FK_Players_Cities 
    FOREIGN KEY (CityId) REFERENCES Cities(Id) ON DELETE SET NULL;

CREATE INDEX IX_Players_CityId ON Players(CityId);
CREATE INDEX IX_Players_IsActive ON Players(IsActive);

-- -----------------------------------------------------------------------------
-- 5. SEED DATA: Países de Latinoamérica
-- -----------------------------------------------------------------------------

INSERT INTO Countries (Name, Code, IsActive) VALUES
('Argentina', 'AR', TRUE),
('Bolivia', 'BO', TRUE),
('Brasil', 'BR', TRUE),
('Chile', 'CL', TRUE),
('Colombia', 'CO', TRUE),
('Costa Rica', 'CR', TRUE),
('Cuba', 'CU', TRUE),
('Ecuador', 'EC', TRUE),
('El Salvador', 'SV', TRUE),
('Guatemala', 'GT', TRUE),
('Honduras', 'HN', TRUE),
('México', 'MX', TRUE),
('Nicaragua', 'NI', TRUE),
('Panamá', 'PA', TRUE),
('Paraguay', 'PY', TRUE),
('Perú', 'PE', TRUE),
('Puerto Rico', 'PR', TRUE),
('República Dominicana', 'DO', TRUE),
('Uruguay', 'UY', TRUE),
('Venezuela', 'VE', TRUE),
('España', 'ES', TRUE);

-- -----------------------------------------------------------------------------
-- 6. SEED DATA: Permisos para Mantenimientos
-- -----------------------------------------------------------------------------

INSERT INTO Permissions (Code, Name, Description, Module) VALUES
-- Countries Module
('countries.view', 'Ver países', 'Permite ver la lista de países', 'Countries'),
('countries.create', 'Crear países', 'Permite crear nuevos países', 'Countries'),
('countries.edit', 'Editar países', 'Permite editar países existentes', 'Countries'),
('countries.delete', 'Eliminar países', 'Permite desactivar países', 'Countries'),

-- Cities Module
('cities.view', 'Ver ciudades', 'Permite ver la lista de ciudades', 'Cities'),
('cities.create', 'Crear ciudades', 'Permite crear nuevas ciudades', 'Cities'),
('cities.edit', 'Editar ciudades', 'Permite editar ciudades existentes', 'Cities'),
('cities.delete', 'Eliminar ciudades', 'Permite desactivar ciudades', 'Cities'),

-- Categories Module (nuevos permisos específicos)
('categories.view', 'Ver categorías', 'Permite ver la lista de categorías', 'Categories'),
('categories.create', 'Crear categorías', 'Permite crear nuevas categorías', 'Categories'),
('categories.edit', 'Editar categorías', 'Permite editar categorías existentes', 'Categories'),
('categories.delete', 'Eliminar categorías', 'Permite desactivar categorías', 'Categories');

-- -----------------------------------------------------------------------------
-- 7. Asignar nuevos permisos al PlatformAdmin (Role ID = 1)
-- -----------------------------------------------------------------------------

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions 
WHERE Code IN (
    'countries.view', 'countries.create', 'countries.edit', 'countries.delete',
    'cities.view', 'cities.create', 'cities.edit', 'cities.delete',
    'categories.view', 'categories.create', 'categories.edit', 'categories.delete'
);

-- =============================================================================
-- FIN DE MIGRACIÓN 02
-- =============================================================================
