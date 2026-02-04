-- =============================================================================
-- MIGRACIÓN 04: FASE 4 - EVENTOS
-- =============================================================================
-- Tablas: Events, EventEstablishments, EventAdmins, EventInvitations
-- Permisos: events.*
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. TABLA EVENTS
-- -----------------------------------------------------------------------------
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

-- Índices para búsquedas eficientes
CREATE INDEX IX_Events_PublicId ON Events(PublicId);
CREATE INDEX IX_Events_OrganizerId ON Events(OrganizerId);
CREATE INDEX IX_Events_IsActive ON Events(IsActive);
CREATE INDEX IX_Events_StartDate ON Events(StartDate);

-- -----------------------------------------------------------------------------
-- 2. TABLA EVENT_ESTABLISHMENTS (Relación M:N con Establishments)
-- -----------------------------------------------------------------------------
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

CREATE INDEX IX_EventEstablishments_EventId ON EventEstablishments(EventId);
CREATE INDEX IX_EventEstablishments_EstablishmentId ON EventEstablishments(EstablishmentId);

-- -----------------------------------------------------------------------------
-- 3. TABLA EVENT_ADMINS (Administradores del evento)
-- -----------------------------------------------------------------------------
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

CREATE INDEX IX_EventAdmins_EventId ON EventAdmins(EventId);
CREATE INDEX IX_EventAdmins_UserId ON EventAdmins(UserId);

-- -----------------------------------------------------------------------------
-- 4. TABLA EVENT_INVITATIONS (Invitaciones pendientes)
-- -----------------------------------------------------------------------------
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
CREATE INDEX IX_EventInvitations_EventId ON EventInvitations(EventId);

-- -----------------------------------------------------------------------------
-- 5. PERMISOS DEL MÓDULO EVENTS
-- -----------------------------------------------------------------------------
INSERT INTO Permissions (Code, Name, Description, Module) VALUES
('events.view', 'Ver eventos', 'Permite ver eventos donde participa', 'Events'),
('events.create', 'Crear eventos', 'Permite crear nuevos eventos (Admin Plataforma)', 'Events'),
('events.edit', 'Editar eventos', 'Permite editar información del evento', 'Events'),
('events.delete', 'Eliminar eventos', 'Permite eliminar/desactivar eventos', 'Events'),
('events.manage-admins', 'Gestionar admins', 'Permite invitar y remover administradores', 'Events'),
('events.manage-establishments', 'Gestionar establecimientos', 'Permite asociar establecimientos al evento', 'Events');

-- -----------------------------------------------------------------------------
-- 6. ASIGNAR PERMISOS A ROLES
-- -----------------------------------------------------------------------------

-- PlatformAdmin (RoleId = 1): Todos los permisos
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions WHERE Code LIKE 'events.%';

-- Organizer (RoleId = 2): Permisos parciales (no puede crear eventos, eso lo hace PlatformAdmin)
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 2, Id FROM Permissions 
WHERE Code IN ('events.view', 'events.edit', 'events.manage-admins', 'events.manage-establishments');

-- EventAdmin (RoleId = 3): Solo ver y editar según configuración
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 3, Id FROM Permissions 
WHERE Code IN ('events.view', 'events.edit');

-- =============================================================================
-- FIN DE MIGRACIÓN 04
-- =============================================================================
