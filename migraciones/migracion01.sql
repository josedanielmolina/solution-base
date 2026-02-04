-- =============================================================================
-- MIGRACIÓN 01: Sistema de Autenticación y Permisos (COMPLETA)
-- Fase 1: Autenticación - App Torneos de Pádel
-- Fecha: 2026-02-01
-- Base de Datos: MySQL 8.x
-- NOTA: Migración desde cero - crea todas las tablas necesarias
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. CREAR TABLA USERS
-- -----------------------------------------------------------------------------

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Document VARCHAR(50) NULL,
    Phone VARCHAR(20) NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    RequiresPasswordChange BOOLEAN NOT NULL DEFAULT FALSE,
    LastLoginAt DATETIME NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT UQ_Users_Document UNIQUE (Document)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);

-- -----------------------------------------------------------------------------
-- 2. CREAR TABLA ROLES
-- -----------------------------------------------------------------------------

CREATE TABLE Roles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL,
    IsSystemRole BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT UQ_Roles_Name UNIQUE (Name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------------------------------
-- 3. CREAR TABLA PERMISSIONS
-- -----------------------------------------------------------------------------

CREATE TABLE Permissions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Code VARCHAR(100) NOT NULL,
    Name VARCHAR(200) NOT NULL,
    Description VARCHAR(500) NULL,
    Module VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT UQ_Permissions_Code UNIQUE (Code)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Permissions_Module ON Permissions(Module);

-- -----------------------------------------------------------------------------
-- 4. CREAR TABLA UserRoles (Intermedia)
-- -----------------------------------------------------------------------------

CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    AssignedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------------------------------
-- 5. CREAR TABLA RolePermissions (Intermedia)
-- -----------------------------------------------------------------------------

CREATE TABLE RolePermissions (
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    GrantedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (RoleId, PermissionId),
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------------------------------
-- 6. CREAR TABLA PLAYERS (Jugadores)
-- -----------------------------------------------------------------------------

CREATE TABLE Players (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Document VARCHAR(50) NOT NULL,
    Email VARCHAR(255) NULL,
    Phone VARCHAR(20) NULL,
    Photo LONGBLOB NULL,
    UserId INT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_Players_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE SET NULL,
    CONSTRAINT UQ_Players_Document UNIQUE (Document)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Players_UserId ON Players(UserId);
CREATE INDEX IX_Players_Document ON Players(Document);
CREATE INDEX IX_Players_IsDeleted ON Players(IsDeleted);

-- -----------------------------------------------------------------------------
-- 7. CREAR TABLA PasswordResetTokens
-- -----------------------------------------------------------------------------

CREATE TABLE PasswordResetTokens (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Code VARCHAR(10) NOT NULL,
    ExpiresAt DATETIME NOT NULL,
    IsUsed BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT FK_PasswordResetTokens_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_PasswordResetTokens_UserId ON PasswordResetTokens(UserId);
CREATE INDEX IX_PasswordResetTokens_Code ON PasswordResetTokens(Code);
CREATE INDEX IX_PasswordResetTokens_ExpiresAt ON PasswordResetTokens(ExpiresAt);

-- =============================================================================
-- SEED DATA: Roles del Sistema
-- =============================================================================

INSERT INTO Roles (Name, Description, IsSystemRole) VALUES
('PlatformAdmin', 'Administrador de la plataforma con acceso total', TRUE),
('Organizer', 'Organizador de eventos', TRUE),
('EventAdmin', 'Administrador de evento con permisos delegados', TRUE);

-- =============================================================================
-- SEED DATA: Permisos del Sistema
-- =============================================================================

INSERT INTO Permissions (Code, Name, Description, Module) VALUES
-- Users Module
('users.view', 'Ver usuarios', 'Permite ver la lista de usuarios y sus detalles', 'Users'),
('users.create', 'Crear usuarios', 'Permite crear nuevos usuarios en el sistema', 'Users'),
('users.edit', 'Editar usuarios', 'Permite editar información de usuarios existentes', 'Users'),
('users.delete', 'Eliminar usuarios', 'Permite eliminar usuarios del sistema', 'Users'),

-- Roles Module
('roles.view', 'Ver roles', 'Permite ver roles y sus permisos', 'Roles'),
('roles.configure', 'Configurar permisos', 'Permite modificar los permisos asignados a roles', 'Roles'),

-- Events Module
('events.create', 'Crear eventos', 'Permite crear nuevos eventos', 'Events'),
('events.edit', 'Editar eventos', 'Permite editar información de eventos', 'Events'),
('events.delete', 'Eliminar eventos', 'Permite eliminar eventos', 'Events'),
('events.view_all', 'Ver todos los eventos', 'Permite ver todos los eventos del sistema', 'Events'),

-- Tournaments Module
('tournaments.create', 'Crear torneos', 'Permite crear torneos dentro de un evento', 'Tournaments'),
('tournaments.edit', 'Editar torneos', 'Permite editar información de torneos', 'Tournaments'),
('tournaments.delete', 'Eliminar torneos', 'Permite eliminar torneos', 'Tournaments'),

-- Participants Module
('participants.manage', 'Gestionar participantes', 'Permite agregar, editar y eliminar participantes', 'Participants'),

-- Groups Module
('groups.configure', 'Configurar grupos', 'Permite generar y configurar grupos de torneo', 'Groups'),

-- Establishments Module
('establishments.manage', 'Gestionar establecimientos', 'Permite asociar y gestionar establecimientos', 'Establishments'),

-- Event Admins Module
('event_admins.invite', 'Invitar admins de evento', 'Permite invitar administradores a un evento', 'EventAdmins'),
('event_admins.remove', 'Remover admins de evento', 'Permite remover administradores de un evento', 'EventAdmins'),

-- Categories Module
('categories.manage', 'Gestionar categorías', 'Permite gestionar categorías globales del sistema', 'Categories');

-- =============================================================================
-- SEED DATA: Permisos por Rol (RolePermissions)
-- =============================================================================

-- PlatformAdmin (Role ID = 1): Todos los permisos
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions;

-- Organizer (Role ID = 2): Permisos de gestión de eventos
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 2, Id FROM Permissions 
WHERE Code IN (
    'events.edit',
    'tournaments.create', 'tournaments.edit', 'tournaments.delete',
    'participants.manage',
    'groups.configure',
    'establishments.manage',
    'event_admins.invite', 'event_admins.remove'
);

-- EventAdmin (Role ID = 3): Permisos básicos de torneo
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 3, Id FROM Permissions 
WHERE Code IN (
    'tournaments.create', 'tournaments.edit', 'tournaments.delete',
    'participants.manage',
    'groups.configure'
);

-- =============================================================================
-- SEED DATA: Usuario Administrador Inicial
-- =============================================================================

-- Password: Admin123! (hash BCrypt)
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, IsActive, RequiresPasswordChange)
VALUES ('Admin', 'Sistema', 'admin@torneos.com', '$2a$11$rXEh2sPk1E0vYJFpG4H.YeSM3YvJfZ7vL5I6JrK5xI.K0L.L5N5Aa', TRUE, TRUE);

-- Asignar rol PlatformAdmin al usuario admin
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1);

-- =============================================================================
-- FIN DE MIGRACIÓN 01
-- =============================================================================
