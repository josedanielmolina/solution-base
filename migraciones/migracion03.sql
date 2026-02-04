-- =============================================================================
-- MIGRACIÓN 03: Establecimientos y Canchas
-- Fase 3: Establecimientos - App Torneos de Pádel
-- Fecha: 2026-02-03
-- Base de Datos: MySQL 8.x
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. CREAR TABLA ESTABLISHMENTS (Establecimientos)
-- -----------------------------------------------------------------------------

CREATE TABLE Establishments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    CountryId INT NOT NULL,
    CityId INT NOT NULL,
    Address VARCHAR(500) NOT NULL,
    GoogleMapsUrl VARCHAR(500) NULL,
    PhoneLandline VARCHAR(20) NULL,
    PhoneMobile VARCHAR(20) NULL,
    Logo LONGTEXT NULL COMMENT 'Base64 encoded image',
    ScheduleType TINYINT NOT NULL DEFAULT 1 COMMENT '1=Continuous, 2=Blocks',
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Establishments_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Establishments_Cities FOREIGN KEY (CityId) REFERENCES Cities(Id) ON DELETE RESTRICT,
    CONSTRAINT UQ_Establishments_Name UNIQUE (Name),
    CONSTRAINT CK_Establishments_ScheduleType CHECK (ScheduleType IN (1, 2))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Establishments_CountryId ON Establishments(CountryId);
CREATE INDEX IX_Establishments_CityId ON Establishments(CityId);
CREATE INDEX IX_Establishments_IsActive ON Establishments(IsActive);

-- -----------------------------------------------------------------------------
-- 2. CREAR TABLA ESTABLISHMENT_PHOTOS (Fotos de Establecimientos)
-- -----------------------------------------------------------------------------

CREATE TABLE EstablishmentPhotos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    ImageData LONGTEXT NOT NULL COMMENT 'Base64 encoded image',
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_EstablishmentPhotos_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_EstablishmentPhotos_EstablishmentId ON EstablishmentPhotos(EstablishmentId);

-- -----------------------------------------------------------------------------
-- 3. CREAR TABLA ESTABLISHMENT_SCHEDULES (Horarios de Establecimientos)
-- -----------------------------------------------------------------------------

CREATE TABLE EstablishmentSchedules (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    DayOfWeek TINYINT NOT NULL COMMENT '1=Monday, 7=Sunday',
    OpenTime TIME NOT NULL,
    CloseTime TIME NOT NULL,
    BlockNumber TINYINT NOT NULL DEFAULT 1 COMMENT 'For multiple blocks per day',
    
    CONSTRAINT FK_EstablishmentSchedules_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE,
    CONSTRAINT CK_EstablishmentSchedules_DayOfWeek CHECK (DayOfWeek BETWEEN 1 AND 7)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_EstablishmentSchedules_EstablishmentId ON EstablishmentSchedules(EstablishmentId);
CREATE INDEX IX_EstablishmentSchedules_DayOfWeek ON EstablishmentSchedules(DayOfWeek);

-- -----------------------------------------------------------------------------
-- 4. CREAR TABLA COURTS (Canchas)
-- -----------------------------------------------------------------------------

CREATE TABLE Courts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    CourtType TINYINT NOT NULL COMMENT '1=Indoor, 2=Outdoor',
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Courts_Establishments FOREIGN KEY (EstablishmentId) 
        REFERENCES Establishments(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Courts_Name_Establishment UNIQUE (Name, EstablishmentId),
    CONSTRAINT CK_Courts_Type CHECK (CourtType IN (1, 2))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Courts_EstablishmentId ON Courts(EstablishmentId);
CREATE INDEX IX_Courts_IsActive ON Courts(IsActive);

-- -----------------------------------------------------------------------------
-- 5. CREAR TABLA COURT_PHOTOS (Fotos de Canchas)
-- -----------------------------------------------------------------------------

CREATE TABLE CourtPhotos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourtId INT NOT NULL,
    ImageData LONGTEXT NOT NULL COMMENT 'Base64 encoded image',
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_CourtPhotos_Courts FOREIGN KEY (CourtId) 
        REFERENCES Courts(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_CourtPhotos_CourtId ON CourtPhotos(CourtId);

-- -----------------------------------------------------------------------------
-- 6. PERMISOS PARA ESTABLECIMIENTOS
-- -----------------------------------------------------------------------------

INSERT INTO Permissions (Code, Name, Description, Module) VALUES
('establishments.view', 'Ver establecimientos', 'Permite ver la lista de establecimientos y sus detalles', 'Establishments'),
('establishments.create', 'Crear establecimientos', 'Permite crear nuevos establecimientos', 'Establishments'),
('establishments.edit', 'Editar establecimientos', 'Permite editar establecimientos, canchas y fotos', 'Establishments'),
('establishments.delete', 'Eliminar establecimientos', 'Permite desactivar establecimientos y canchas', 'Establishments');

-- -----------------------------------------------------------------------------
-- 7. ASIGNAR PERMISOS A PLATFORMADMIN (Role ID = 1)
-- -----------------------------------------------------------------------------

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions 
WHERE Code IN (
    'establishments.view', 
    'establishments.create', 
    'establishments.edit', 
    'establishments.delete'
);

-- =============================================================================
-- FIN DE MIGRACIÓN 03
-- =============================================================================
