# Skill: Feature Documentation

## Descripción
Este skill define los estándares y patrones para documentar features en el proyecto. **La documentación es un documento vivo** que debe actualizarse cada vez que se modifique funcionalidad existente o se agregue nueva. TODO desarrollador debe seguir estos patrones al completar su trabajo.

---

## 🎯 Principios Fundamentales

### 1. Documentación como Código
- La documentación es parte integral del desarrollo, NO opcional
- Se actualiza en el mismo commit que el código
- Se revisa en el mismo pull request
- Está versionada junto con el código

### 2. Documento Vivo
- **SIEMPRE** actualizar documentación al modificar funcionalidad
- Agregar nuevos endpoints cuando se crean
- Actualizar ejemplos cuando cambia el formato
- Marcar funcionalidades deprecated antes de eliminarlas

### 3. Claridad sobre Perfección
- Documentación clara e incompleta > documentación perfecta y desactualizada
- Priorizar ejemplos prácticos sobre descripciones teóricas
- Mantener formato consistente para facilitar lectura

---

## 📁 Estructura de Documentación

```
docs/
├── features/
│   ├── AUTH.md              # Feature de autenticación
│   ├── USERS.md             # Feature de gestión de usuarios
│   └── [FEATURE-NAME].md    # Cada feature tiene su documento
├── api/
│   └── endpoints.md         # Índice rápido de todos los endpoints
└── architecture/
    └── patterns.md          # Patrones de arquitectura del proyecto
```

---

## 📋 Template para Documentar Features

### Estructura Obligatoria

```markdown
# Feature: [Nombre del Feature]

## Descripción
[Descripción breve del feature, su propósito y responsabilidades]

---

## 📋 Índice
- [Arquitectura](#arquitectura)
- [Endpoints](#endpoints)
- [Modelos de Datos](#modelos-de-datos)
- [Casos de Uso](#casos-de-uso)
- [Validaciones](#validaciones)
- [Reglas de Negocio](#reglas-de-negocio)
- [Ejemplos de Uso](#ejemplos-de-uso)
- [Testing](#testing)

---

## 🏗️ Arquitectura

### Componentes Principales

[Árbol de estructura de archivos del feature]

### Patrón de Diseño
[Patrones aplicados: Facade, CQRS, Repository, etc.]

---

## 🔌 Endpoints

### [MÉTODO] `/api/[ruta]`
[Descripción del endpoint]

**Request Body:**
```json
{
  "campo": "valor"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "campo": "valor"
}
```

**Response (4xx/5xx) - Errores:**
```json
{
  "error": "ErrorCode",
  "message": "Descripción del error"
}
```

---

## 📊 Modelos de Datos

### [NombreDto]
```csharp
public record NombreDto(
    int Id,
    string Campo
);
```

**Propiedades:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| Id | int | ✅ Sí | Identificador único |

---

## 💼 Casos de Uso

### 1. [Nombre del Caso de Uso]
**Flujo:**
1. Paso 1
2. Paso 2
3. Paso 3

**Código de Operación:**
```csharp
// Código de ejemplo
```

---

## ✅ Validaciones

### [NombreValidator]
```csharp
public class NombreValidator : AbstractValidator<Dto>
{
    // Reglas de validación
}
```

**Reglas:**
| Campo | Regla | Mensaje |
|-------|-------|---------|
| Campo | NotEmpty | "Mensaje" |

---

## 📏 Reglas de Negocio

### 1. [Regla de Negocio]
- ✅ Descripción de la regla
- ✅ Casos que aplica

---

## 📝 Ejemplos de Uso

### cURL
```bash
curl -X POST http://localhost:5000/api/endpoint
```

### C# HttpClient
```csharp
// Ejemplo en C#
```

### Angular 20 Service
```typescript
// Ejemplo en Angular
```

---

## 🧪 Testing

### Casos de Prueba
```csharp
[Fact]
public async Task Test_Description()
{
    // Test
}
```

---

**Última actualización:** [Fecha]  
**Versión:** [X.Y.Z]
```

---

## 🔧 Secciones Detalladas

### 1. Endpoints (OBLIGATORIO)

Para **CADA** endpoint documentar:

✅ **Método HTTP y Ruta**
```markdown
### POST `/api/users`
```

✅ **Descripción breve**
```markdown
Crea un nuevo usuario en el sistema.
```

✅ **Path Parameters (si aplica)**
```markdown
**Path Parameters:**
- `id` (int): ID del usuario
```

✅ **Query Parameters (si aplica)**
```markdown
**Query Parameters:**
- `page` (int, opcional): Número de página (default: 1)
- `pageSize` (int, opcional): Tamaño de página (default: 10)
```

✅ **Request Body con ejemplo JSON**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com"
}
```

✅ **Response exitosa con código de estado**
```markdown
**Response (200 OK):**
**Response (201 Created):**
**Response (204 No Content)**
```

✅ **Respuestas de error comunes**
```markdown
**Response (400 Bad Request):**
**Response (401 Unauthorized):**
**Response (404 Not Found):**
**Response (409 Conflict):**
```

✅ **Headers especiales (si aplica)**
```markdown
**Headers:**
- `Authorization`: Bearer {token}
```

---

### 2. Modelos de Datos (OBLIGATORIO)

Para **CADA** DTO/Model documentar:

✅ **Definición en código**
```csharp
public record UserDto(
    int Id,
    string FirstName,
    string Email
);
```

✅ **Tabla de propiedades**
| Campo | Tipo | Requerido | Default | Descripción |
|-------|------|-----------|---------|-------------|
| Id | int | ✅ Sí | - | Identificador único |
| FirstName | string | ✅ Sí | "" | Nombre del usuario |
| Email | string | ✅ Sí | - | Email único |
| IsActive | bool | ❌ No | true | Usuario activo |

---

### 3. Casos de Uso (OBLIGATORIO)

Para **CADA** operación principal:

✅ **Nombre descriptivo del caso de uso**

✅ **Flujo paso a paso**
```markdown
1. Sistema recibe request
2. Sistema valida datos
3. Sistema procesa
4. Sistema retorna respuesta
```

✅ **Código de la operación** (pseudocódigo o código real simplificado)
```csharp
public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
{
    // 1. Validar
    // 2. Procesar
    // 3. Persistir
    // 4. Retornar
}
```

---

### 4. Validaciones (OBLIGATORIO si usa FluentValidation)

✅ **Código del validador**
```csharp
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
```

✅ **Tabla de reglas**
| Campo | Regla | Parámetros | Mensaje de Error |
|-------|-------|------------|------------------|
| Email | NotEmpty | - | "Email is required" |
| Email | EmailAddress | - | "Invalid email format" |
| Password | MinLength | 8 | "Min 8 characters" |

---

### 5. Ejemplos de Uso (RECOMENDADO)

Incluir ejemplos en al menos 2 lenguajes:

✅ **cURL** (para pruebas rápidas)
✅ **C# HttpClient** (para consumo desde .NET)
✅ **JavaScript/TypeScript** (para frontend)
✅ **Angular 20 Service** (si aplica al proyecto)

---

### 6. Testing (RECOMENDADO)

✅ Al menos 3 casos de prueba:
- ✅ Caso exitoso (happy path)
- ✅ Caso de error de validación
- ✅ Caso de error de negocio

```csharp
[Fact]
public async Task Create_WithValidData_ReturnsSuccess()
{
    // Arrange
    var dto = new CreateUserDto(...);

    // Act
    var result = await _service.CreateAsync(dto);

    // Assert
    Assert.True(result.IsSuccess);
}
```

---

## 🔄 Proceso de Actualización

### Cuándo Actualizar la Documentación

**SIEMPRE actualizar cuando:**

1. ✅ **Agregas un nuevo endpoint**
   - Documentar en sección de Endpoints
   - Agregar ejemplos de request/response
   - Incluir casos de error

2. ✅ **Modificas un endpoint existente**
   - Actualizar request/response si cambia
   - Marcar como `[DEPRECATED]` si se va a eliminar
   - Actualizar ejemplos de uso

3. ✅ **Cambias un modelo de datos**
   - Actualizar tabla de propiedades
   - Actualizar ejemplos JSON
   - Actualizar ejemplos de código

4. ✅ **Agregas/modificas validaciones**
   - Actualizar tabla de reglas
   - Actualizar mensajes de error en ejemplos

5. ✅ **Cambias reglas de negocio**
   - Actualizar sección de Reglas de Negocio
   - Actualizar flujos de casos de uso
   - Agregar notas si cambia comportamiento

6. ✅ **Corriges un bug que cambia comportamiento**
   - Documentar el comportamiento correcto
   - Agregar nota sobre el cambio si es breaking

---

### Workflow de Documentación

```
┌─────────────────┐
│ Desarrollar     │
│ Feature/Fix     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Código Completo │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Actualizar      │◄──── ANTES de commit
│ Documentación   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Commit con      │
│ Código + Docs   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Pull Request    │
│ (incluye docs)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Code Review     │
│ + Docs Review   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Merge           │
└─────────────────┘
```

---

## 📝 Checklist de Documentación

Antes de hacer commit/PR, verificar:

### Para Nuevo Feature
- [ ] Crear archivo `docs/features/[FEATURE-NAME].md`
- [ ] Incluir todas las secciones obligatorias
- [ ] Documentar todos los endpoints
- [ ] Documentar todos los DTOs
- [ ] Incluir al menos 2 ejemplos de uso
- [ ] Incluir casos de uso principales
- [ ] Documentar validaciones
- [ ] Agregar fecha de última actualización
- [ ] Establecer versión inicial (1.0.0)

### Para Modificación de Feature
- [ ] Actualizar sección de Endpoints (si aplica)
- [ ] Actualizar modelos de datos (si aplica)
- [ ] Actualizar validaciones (si aplica)
- [ ] Actualizar ejemplos de uso (si aplica)
- [ ] Actualizar casos de uso (si aplica)
- [ ] Actualizar reglas de negocio (si aplica)
- [ ] Actualizar fecha de última actualización
- [ ] Incrementar versión (semver)

### Para Breaking Changes
- [ ] Marcar cambios con `⚠️ BREAKING CHANGE`
- [ ] Documentar migración/upgrade path
- [ ] Actualizar todos los ejemplos afectados
- [ ] Incrementar versión MAJOR

---

## 🚫 Antipatrones - NO Hacer

❌ **NO documentar después del merge**
- La documentación debe ir en el mismo PR

❌ **NO copiar/pegar documentación de otros features sin adaptar**
- Cada feature tiene su particularidad

❌ **NO dejar secciones vacías o con TODOs**
- Si no aplica, eliminar la sección o poner "N/A"

❌ **NO documentar código interno/implementación**
- Documentar la API/interfaz pública, no los detalles internos

❌ **NO usar screenshots para mostrar JSON**
- Usar bloques de código copiables

❌ **NO incluir información sensible**
- No passwords reales, tokens, datos de producción

❌ **NO documentar en comentarios del código únicamente**
- Comentarios en código ≠ Documentación de feature

---

## 📐 Convenciones de Formato

### Nombres de Archivos
```
docs/features/[FEATURE-NAME].md

Ejemplos:
- docs/features/AUTH.md
- docs/features/USERS.md
- docs/features/ORDERS.md
- docs/features/PRODUCTS.md
```

**Reglas:**
- MAYÚSCULAS para features principales
- Sin espacios (usar guiones si es necesario)
- Extensión `.md`

### Títulos y Secciones
```markdown
# Feature: [Nombre]           # H1 - Solo para título principal
## 📋 Sección Principal        # H2 - Secciones principales
### 1. Subsección              # H3 - Subsecciones
#### Detalle                   # H4 - Detalles específicos
```

### Emojis para Secciones (Consistencia)
- 📋 Índice
- 🏗️ Arquitectura
- 🔌 Endpoints
- 📊 Modelos de Datos
- 💼 Casos de Uso
- ✅ Validaciones
- 📏 Reglas de Negocio
- 📝 Ejemplos de Uso
- 🧪 Testing
- 🔒 Seguridad
- 🔄 Diagrama de Flujo
- 📚 Referencias
- 📈 Mejoras Futuras

### Bloques de Código
````markdown
```json
{
  "ejemplo": "JSON"
}
```

```csharp
// Código C#
```

```typescript
// Código TypeScript
```

```bash
# Comandos shell
```
````

### Tablas
```markdown
| Columna 1 | Columna 2 | Columna 3 |
|-----------|-----------|-----------|
| Valor 1   | Valor 2   | Valor 3   |
```

### Checkboxes y Listas
```markdown
- ✅ Item completado
- ❌ Item NO permitido
- ⚠️ Advertencia importante
- 💡 Tip o recomendación
- 🔒 Relacionado con seguridad

- [ ] Checkbox sin marcar
- [x] Checkbox marcado
```

---

## 🎯 Ejemplos de Buena Documentación

### ✅ Ejemplo CORRECTO - Endpoint Bien Documentado

```markdown
### POST `/api/users`
Crea un nuevo usuario en el sistema. El email debe ser único y la contraseña será hasheada antes de almacenar.

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePass123!",
  "phoneNumber": "+1234567890"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-01-11T10:00:00Z"
}
```

**Response (400 Bad Request) - Validación:**
```json
{
  "error": "Validation.Failed",
  "message": "One or more validation errors occurred.",
  "details": [
    "Email must be a valid email address",
    "Password must be at least 8 characters long"
  ]
}
```

**Response (409 Conflict) - Email Duplicado:**
```json
{
  "error": "User.EmailAlreadyExists",
  "message": "A user with this email already exists"
}
```
```

---

### ❌ Ejemplo INCORRECTO - Documentación Insuficiente

```markdown
### POST `/api/users`
Crea usuario.

Request: UserDto
Response: User
```

**Problemas:**
- Sin descripción detallada
- Sin ejemplos de JSON
- Sin casos de error
- Sin tipos específicos

---

## 🔗 Referencias entre Documentos

Cuando un feature depende de otro:

```markdown
## Dependencias

Este feature depende de:
- [Authentication](./AUTH.md) - Para autenticación de usuarios
- [Users](./USERS.md) - Para gestión de usuarios
```

Cuando se referencia un endpoint de otro feature:

```markdown
Para autenticar, usar el endpoint `POST /api/auth/login` 
documentado en [AUTH.md](./AUTH.md#post-apiauthlogin).
```

---

## 🔢 Versionado Semántico

Seguir [Semantic Versioning](https://semver.org/) para la documentación:

### MAJOR.MINOR.PATCH

**MAJOR** (1.0.0 → 2.0.0)
- Breaking changes en API
- Endpoints eliminados
- Cambios incompatibles en request/response

**MINOR** (1.0.0 → 1.1.0)
- Nuevos endpoints agregados
- Nuevas propiedades opcionales
- Funcionalidad nueva compatible

**PATCH** (1.0.0 → 1.0.1)
- Correcciones de bugs
- Aclaraciones en documentación
- Fixes que no cambian API

### Registro de Cambios

Al final de cada documento:

```markdown
---

## 📝 Historial de Cambios

### v1.2.0 - 2026-01-15
- Agregado endpoint GET /api/users/search
- Nueva propiedad opcional `avatar` en UserDto

### v1.1.0 - 2026-01-10
- Agregado endpoint DELETE /api/users/{id}
- Actualizada validación de email

### v1.0.0 - 2026-01-01
- Release inicial
```

---

## 📚 Recursos Adicionales

### Templates
- Ver [AUTH.md](../../docs/features/AUTH.md) como ejemplo completo
- Ver [USERS.md](../../docs/features/USERS.md) como ejemplo completo

### Herramientas Recomendadas
- **Editor**: VS Code con extensión Markdown All in One
- **Preview**: VS Code Markdown Preview
- **Linter**: markdownlint
- **Diagramas**: Mermaid (opcional)

### Lecturas Recomendadas
- [Microsoft API Documentation Guidelines](https://docs.microsoft.com/en-us/style-guide/developer-content/)
- [Write the Docs](https://www.writethedocs.org/)
- [REST API Documentation Best Practices](https://swagger.io/blog/api-documentation/best-practices-in-api-documentation/)

---

## ⚡ Quick Reference

### Estructura Mínima Viable

Para un feature nuevo, como MÍNIMO documentar:

1. ✅ **Descripción** (1 párrafo)
2. ✅ **Endpoints** (todos, con ejemplos JSON)
3. ✅ **Modelos de Datos** (todos los DTOs)
4. ✅ **Ejemplos de Uso** (al menos cURL)

### Tiempo Estimado

- **Feature nuevo completo**: 30-60 minutos
- **Actualización de endpoint**: 5-10 minutos
- **Actualización de modelo**: 5 minutos
- **Agregar ejemplo**: 5 minutos

---

**IMPORTANTE**: La documentación NO es opcional. Es parte integral del desarrollo y será revisada en los PRs junto con el código.

---

**Última actualización:** 11 de enero de 2026  
**Versión:** 1.0.0
