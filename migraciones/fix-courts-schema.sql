-- =============================================================================
-- SCRIPT DE CORRECCIÓN: Agregar columnas faltantes a tablas existentes
-- Ejecutar SOLO si las tablas fueron creadas sin estas columnas
-- =============================================================================

-- Agregar UpdatedAt a Courts si no existe
ALTER TABLE Courts ADD COLUMN IF NOT EXISTS UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP;

-- Alternativamente, si "IF NOT EXISTS" no funciona en tu versión de MySQL:
-- SET @sql = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
--     WHERE TABLE_NAME = 'Courts' AND COLUMN_NAME = 'UpdatedAt') = 0,
--     'ALTER TABLE Courts ADD COLUMN UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP',
--     'SELECT 1');
-- PREPARE stmt FROM @sql;
-- EXECUTE stmt;
-- DEALLOCATE PREPARE stmt;

-- =============================================================================
-- Si necesitas recrear las tablas desde cero, ejecuta DROP y luego migracion03.sql:
-- DROP TABLE IF EXISTS CourtPhotos;
-- DROP TABLE IF EXISTS Courts;
-- DROP TABLE IF EXISTS EstablishmentSchedules;
-- DROP TABLE IF EXISTS EstablishmentPhotos;
-- DROP TABLE IF EXISTS Establishments;
-- Luego ejecuta: migracion03.sql
-- =============================================================================
