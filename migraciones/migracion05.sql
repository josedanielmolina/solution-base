-- =============================================================================
-- MIGRACIÓN 05: Agregar permiso users.assign-roles
-- Fecha: 2026-02-04
-- =============================================================================

-- Agregar el permiso users.assign-roles al módulo Users
INSERT INTO Permissions (Code, Name, Description, Module) VALUES
('users.assign-roles', 'Asignar roles a usuarios', 'Permite asignar o remover roles a usuarios existentes', 'Users');

-- Asignar este permiso a PlatformAdmin (Role ID = 1)
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions WHERE Code = 'users.assign-roles';

-- =============================================================================
-- FIN DE MIGRACIÓN 05
-- =============================================================================
