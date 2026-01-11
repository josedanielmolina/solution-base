# Feature: Users Management

## Descripción
El feature de gestión de usuarios proporciona un CRUD completo para administrar usuarios en el sistema, incluyendo creación, lectura, actualización y eliminación de cuentas de usuario.

---

## 📋 Índice
- [Arquitectura](#arquitectura)
- [Endpoints](#endpoints)
- [Modelos de Datos](#modelos-de-datos)
- [Casos de Uso](#casos-de-uso)
- [Validaciones](#validaciones)
- [Reglas de Negocio](#reglas-de-negocio)
- [Ejemplos de Uso](#ejemplos-de-uso)

---

## 🏗️ Arquitectura

### Componentes Principales

```
Users Feature
├── Presentation Layer
│   └── API/Controllers/UsersController.cs
├── Application Layer
│   ├── Facades/UserFacade.cs
│   ├── Operations/Users/
│   │   ├── CreateUserOperation.cs
│   │   ├── GetUserOperation.cs
│   │   ├── UpdateUserOperation.cs
│   │   └── DeleteUserOperation.cs
│   ├── DTOs/Users/
│   │   ├── UserDto.cs
│   │   ├── CreateUserDto.cs
│   │   └── UpdateUserDto.cs
│   ├── Validators/
│   │   ├── CreateUserValidator.cs
│   │   └── UpdateUserValidator.cs
│   └── Mappings/UserMappingExtensions.cs
├── Domain Layer
│   ├── Entities/User.cs
│   └── Interfaces/Repositories/IUserRepository.cs
└── Infrastructure Layer
    ├── Repositories/UserRepository.cs
    └── Configurations/UserConfiguration.cs
```

### Patrón de Diseño
- **Facade Pattern**: `UserFacade` orquesta todas las operaciones de usuarios
- **CQRS Pattern**: Operaciones separadas por responsabilidad (Commands/Queries)
- **Repository Pattern**: Abstracción de acceso a datos
- **Unit of Work Pattern**: Gestión de transacciones
- **Result Pattern**: Manejo consistente de errores

---

## 🔌 Endpoints

### 1. GET `/api/users`
Obtiene la lista de todos los usuarios del sistema.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "isActive": true,
    "isEmailVerified": true,
    "lastLoginAt": "2026-01-10T14:30:00Z",
    "createdAt": "2026-01-01T10:00:00Z",
    "updatedAt": "2026-01-10T14:30:00Z"
  },
  {
    "id": 2,
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@example.com",
    "isActive": true,
    "isEmailVerified": false,
    "lastLoginAt": null,
    "createdAt": "2026-01-05T09:15:00Z",
    "updatedAt": null
  }
]
```

---

### 2. GET `/api/users/{id}`
Obtiene un usuario específico por su ID.

**Path Parameters:**
- `id` (int): ID del usuario

**Response (200 OK):**
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "isActive": true,
  "isEmailVerified": true,
  "lastLoginAt": "2026-01-10T14:30:00Z",
  "createdAt": "2026-01-01T10:00:00Z",
  "updatedAt": "2026-01-10T14:30:00Z"
}
```

**Response (404 Not Found):**
```json
{
  "error": "User.NotFound",
  "message": "User not found"
}
```

---

### 3. POST `/api/users`
Crea un nuevo usuario en el sistema.

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123!",
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
  "lastLoginAt": null,
  "createdAt": "2026-01-11T10:00:00Z",
  "updatedAt": null
}
```

**Response (400 Bad Request) - Validación:**
```json
{
  "error": "Validation.Failed",
  "message": "One or more validation errors occurred.",
  "details": [
    "First name is required",
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

---

### 4. PUT `/api/users/{id}`
Actualiza un usuario existente.

**Path Parameters:**
- `id` (int): ID del usuario a actualizar

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe Updated",
  "email": "john.doe.updated@example.com",
  "phoneNumber": "+1234567890"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe Updated",
  "email": "john.doe.updated@example.com",
  "isActive": true,
  "isEmailVerified": true,
  "lastLoginAt": "2026-01-10T14:30:00Z",
  "createdAt": "2026-01-01T10:00:00Z",
  "updatedAt": "2026-01-11T11:00:00Z"
}
```

**Response (404 Not Found):**
```json
{
  "error": "User.NotFound",
  "message": "User not found"
}
```

**Response (409 Conflict) - Email Duplicado:**
```json
{
  "error": "User.EmailAlreadyExists",
  "message": "A user with this email already exists"
}
```

---

### 5. DELETE `/api/users/{id}`
Elimina un usuario del sistema.

**Path Parameters:**
- `id` (int): ID del usuario a eliminar

**Response (204 No Content)**

**Response (404 Not Found):**
```json
{
  "error": "User.NotFound",
  "message": "User not found"
}
```

---

## 📊 Modelos de Datos

### User (Entity)
```csharp
public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
}
```

**Propiedades Heredadas de BaseEntity:**
- `Id` (int): Identificador único
- `CreatedAt` (DateTime): Fecha de creación
- `UpdatedAt` (DateTime?): Fecha de última actualización

**Propiedades:**
| Campo | Tipo | Default | Descripción |
|-------|------|---------|-------------|
| FirstName | string | "" | Nombre del usuario |
| LastName | string | "" | Apellido del usuario |
| Email | string | "" | Email único (usado para login) |
| PasswordHash | string | "" | Hash de la contraseña (BCrypt/Argon2) |
| IsActive | bool | true | Usuario activo/inactivo |
| IsEmailVerified | bool | false | Email verificado |
| LastLoginAt | DateTime? | null | Fecha del último login |

---

### UserDto
```csharp
public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool IsEmailVerified,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

**Uso:** Representa un usuario en respuestas de API (no incluye contraseña).

---

### CreateUserDto
```csharp
public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? PhoneNumber
);
```

**Propiedades:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| FirstName | string | ✅ Sí | Nombre (máx. 100 caracteres) |
| LastName | string | ✅ Sí | Apellido (máx. 100 caracteres) |
| Email | string | ✅ Sí | Email único (formato válido) |
| Password | string | ✅ Sí | Contraseña (8-100 caracteres) |
| PhoneNumber | string? | ❌ No | Teléfono (formato internacional) |

---

### UpdateUserDto
```csharp
public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
);
```

**Propiedades:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| FirstName | string | ✅ Sí | Nombre actualizado |
| LastName | string | ✅ Sí | Apellido actualizado |
| Email | string | ✅ Sí | Email actualizado (debe ser único) |
| PhoneNumber | string? | ❌ No | Teléfono actualizado |

**Nota:** La contraseña NO se actualiza mediante este endpoint (usar endpoint específico de cambio de contraseña).

---

## 💼 Casos de Uso

### 1. Crear Usuario
**Flujo:**
1. Cliente envía datos del nuevo usuario
2. Sistema valida datos con `CreateUserValidator`
3. Sistema verifica que el email no exista
4. Sistema hashea la contraseña
5. Sistema crea el usuario con `IsActive = true`, `IsEmailVerified = false`
6. Sistema persiste en base de datos
7. Sistema retorna el usuario creado

**Código de Operación:**
```csharp
// CreateUserOperation.cs
public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
{
    // 1. Verificar email único
    var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
    if (existingUser is not null)
        return Result<UserDto>.Failure("User.EmailAlreadyExists");

    // 2. Hashear contraseña
    var passwordHash = _passwordHasher.Hash(dto.Password);

    // 3. Crear entidad
    var user = new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PasswordHash = passwordHash,
        IsActive = true,
        IsEmailVerified = false
    };

    // 4. Persistir
    await _userRepository.AddAsync(user);
    await _unitOfWork.CommitAsync();

    // 5. Retornar DTO
    return Result<UserDto>.Success(user.ToDto());
}
```

---

### 2. Obtener Usuario por ID
**Flujo:**
1. Cliente solicita usuario por ID
2. Sistema busca en repositorio
3. Si existe: retorna UserDto
4. Si no existe: retorna error 404

**Código de Operación:**
```csharp
// GetUserOperation.cs
public async Task<Result<UserDto>> ExecuteAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    
    if (user is null)
        return Result<UserDto>.Failure("User.NotFound");

    return Result<UserDto>.Success(user.ToDto());
}
```

---

### 3. Actualizar Usuario
**Flujo:**
1. Cliente envía datos actualizados con ID
2. Sistema valida datos con `UpdateUserValidator`
3. Sistema busca usuario existente
4. Si no existe: retorna error 404
5. Sistema verifica que el nuevo email no esté en uso por otro usuario
6. Sistema actualiza propiedades
7. Sistema establece `UpdatedAt`
8. Sistema persiste cambios
9. Sistema retorna usuario actualizado

**Código de Operación:**
```csharp
// UpdateUserOperation.cs
public async Task<Result<UserDto>> ExecuteAsync(int id, UpdateUserDto dto)
{
    // 1. Buscar usuario
    var user = await _userRepository.GetByIdAsync(id);
    if (user is null)
        return Result<UserDto>.Failure("User.NotFound");

    // 2. Verificar email único (si cambió)
    if (user.Email != dto.Email)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser is not null)
            return Result<UserDto>.Failure("User.EmailAlreadyExists");
    }

    // 3. Actualizar propiedades
    user.FirstName = dto.FirstName;
    user.LastName = dto.LastName;
    user.Email = dto.Email;
    user.UpdatedAt = DateTime.UtcNow;

    // 4. Persistir
    await _unitOfWork.CommitAsync();

    // 5. Retornar DTO
    return Result<UserDto>.Success(user.ToDto());
}
```

---

### 4. Eliminar Usuario
**Flujo:**
1. Cliente solicita eliminar usuario por ID
2. Sistema busca usuario
3. Si no existe: retorna error 404
4. Sistema elimina del repositorio
5. Sistema persiste cambios
6. Sistema retorna éxito (204 No Content)

**Código de Operación:**
```csharp
// DeleteUserOperation.cs
public async Task<Result> ExecuteAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    
    if (user is null)
        return Result.Failure("User.NotFound");

    _userRepository.Delete(user);
    await _unitOfWork.CommitAsync();

    return Result.Success();
}
```

---

## ✅ Validaciones

### CreateUserValidator
```csharp
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in valid international format");
    }
}
```

**Reglas de Validación (Create):**

| Campo | Regla | Mensaje |
|-------|-------|---------|
| FirstName | NotEmpty | "First name is required" |
| FirstName | MaxLength(100) | "First name must not exceed 100 characters" |
| LastName | NotEmpty | "Last name is required" |
| LastName | MaxLength(100) | "Last name must not exceed 100 characters" |
| Email | NotEmpty | "Email is required" |
| Email | EmailAddress | "Email must be a valid email address" |
| Email | MaxLength(255) | "Email must not exceed 255 characters" |
| Password | NotEmpty | "Password is required" |
| Password | MinLength(8) | "Password must be at least 8 characters long" |
| Password | MaxLength(100) | "Password must not exceed 100 characters" |
| PhoneNumber | Matches(regex) | "Phone number must be in valid international format" |

---

### UpdateUserValidator
```csharp
public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in valid international format");
    }
}
```

**Nota:** El validador de actualización NO incluye la contraseña.

---

## 📏 Reglas de Negocio

### 1. Email Único
- ✅ Cada usuario debe tener un email único en el sistema
- ✅ Validación en creación y actualización
- ✅ Case-insensitive (john@example.com = JOHN@example.com)

### 2. Contraseñas Seguras
- ✅ Mínimo 8 caracteres
- ✅ Máximo 100 caracteres
- ✅ Hasheadas con algoritmo seguro (BCrypt/Argon2)
- ✅ NUNCA almacenadas en texto plano
- ✅ NUNCA retornadas en DTOs

### 3. Estado del Usuario
- ✅ Nuevos usuarios: `IsActive = true`, `IsEmailVerified = false`
- ✅ Usuario inactivo no puede hacer login
- ✅ `LastLoginAt` se actualiza automáticamente en cada login exitoso

### 4. Auditoría
- ✅ `CreatedAt` se establece automáticamente al crear
- ✅ `UpdatedAt` se establece automáticamente al actualizar
- ✅ Timestamps en UTC

### 5. Eliminación
- ⚠️ **HARD DELETE**: El usuario se elimina permanentemente de la base de datos
- 💡 **Recomendación futura**: Implementar SOFT DELETE (marcar como inactivo)

---

## 📝 Ejemplos de Uso

### cURL Examples

#### Crear Usuario
```bash
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "password": "SecurePass123!",
    "phoneNumber": "+1234567890"
  }'
```

#### Obtener Todos los Usuarios
```bash
curl -X GET http://localhost:5000/api/users
```

#### Obtener Usuario por ID
```bash
curl -X GET http://localhost:5000/api/users/1
```

#### Actualizar Usuario
```bash
curl -X PUT http://localhost:5000/api/users/1 \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe Updated",
    "email": "john.updated@example.com",
    "phoneNumber": "+1987654321"
  }'
```

#### Eliminar Usuario
```bash
curl -X DELETE http://localhost:5000/api/users/1
```

---

### C# HttpClient Example
```csharp
public class UserApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5000/api/users";

    public UserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Crear usuario
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    // Obtener todos los usuarios
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserDto>>(BaseUrl);
    }

    // Obtener usuario por ID
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<UserDto>($"{BaseUrl}/{id}");
    }

    // Actualizar usuario
    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    // Eliminar usuario
    public async Task DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
```

---

### Angular 20 Service Example
```typescript
// user.service.ts
import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface CreateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
}

export interface UpdateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
}

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  isEmailVerified: boolean;
  lastLoginAt: string | null;
  createdAt: string;
  updatedAt: string | null;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/users';
  
  users = signal<UserDto[]>([]);
  loading = signal<boolean>(false);
  error = signal<string>('');

  getAll(): Observable<UserDto[]> {
    this.loading.set(true);
    return this.http.get<UserDto[]>(this.apiUrl).pipe(
      tap(users => {
        this.users.set(users);
        this.loading.set(false);
      })
    );
  }

  getById(id: number): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateUserDto): Observable<UserDto> {
    return this.http.post<UserDto>(this.apiUrl, dto).pipe(
      tap(user => {
        this.users.update(users => [...users, user]);
      })
    );
  }

  update(id: number, dto: UpdateUserDto): Observable<UserDto> {
    return this.http.put<UserDto>(`${this.apiUrl}/${id}`, dto).pipe(
      tap(updatedUser => {
        this.users.update(users =>
          users.map(u => u.id === id ? updatedUser : u)
        );
      })
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => {
        this.users.update(users => users.filter(u => u.id !== id));
      })
    );
  }
}
```

---

## 🧪 Testing

### Unit Tests Examples

#### Crear Usuario - Caso Exitoso
```csharp
[Fact]
public async Task CreateUser_WithValidData_ReturnsUserDto()
{
    // Arrange
    var dto = new CreateUserDto(
        "John",
        "Doe",
        "john@example.com",
        "SecurePass123!",
        "+1234567890"
    );

    // Act
    var result = await _userFacade.CreateUserAsync(dto);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.Equal("John", result.Value.FirstName);
    Assert.Equal("john@example.com", result.Value.Email);
}
```

#### Crear Usuario - Email Duplicado
```csharp
[Fact]
public async Task CreateUser_WithDuplicateEmail_ReturnsFailure()
{
    // Arrange
    var dto = new CreateUserDto(
        "Jane",
        "Smith",
        "existing@example.com", // Email ya existe
        "SecurePass123!",
        null
    );

    // Act
    var result = await _userFacade.CreateUserAsync(dto);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Equal("User.EmailAlreadyExists", result.Error);
}
```

#### Actualizar Usuario - No Existe
```csharp
[Fact]
public async Task UpdateUser_WithNonExistentId_ReturnsFailure()
{
    // Arrange
    var dto = new UpdateUserDto("John", "Doe", "new@example.com", null);

    // Act
    var result = await _userFacade.UpdateUserAsync(999, dto);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Equal("User.NotFound", result.Error);
}
```

---

## 🔄 Diagrama de Flujo - Crear Usuario

```
┌─────────────┐
│   Cliente   │
└──────┬──────┘
       │ POST /api/users
       │ CreateUserDto
       ▼
┌──────────────────┐
│ UsersController  │
└────────┬─────────┘
         │ Validar DTO
         ▼
┌──────────────────────┐
│ CreateUserValidator  │◄──── FluentValidation
└────────┬─────────────┘
         │ ✅ Válido
         ▼
┌──────────────────┐
│   UserFacade     │
└────────┬─────────┘
         │ ExecuteAsync
         ▼
┌──────────────────────┐
│ CreateUserOperation  │
└────────┬─────────────┘
         │
         ├──► 1. Verificar Email Único
         │         (UserRepository)
         │         │
         │         ├──✅ No existe
         │         └──❌ Existe → Error 409
         │
         ├──► 2. Hashear Password
         │         (IPasswordHasher)
         │
         ├──► 3. Crear Entidad User
         │         - IsActive = true
         │         - IsEmailVerified = false
         │
         ├──► 4. Persistir (Repository)
         │
         └──► 5. Commit (UnitOfWork)
                │
                ▼
         ┌──────────────┐
         │   UserDto    │
         └──────┬───────┘
                │ Return 201
                ▼
         ┌──────────────┐
         │   Cliente    │
         └──────────────┘
```

---

## 🔐 Seguridad

### Protección de Datos Sensibles
- ✅ Las contraseñas **NUNCA** se retornan en UserDto
- ✅ PasswordHash no se expone en API
- ✅ Validación de formato de email

### Recomendaciones
- 🔒 Agregar autorización (`[Authorize]`) en endpoints sensibles
- 🔒 Implementar roles (Admin puede ver todos, User solo su perfil)
- 🔒 Rate limiting en endpoints de creación
- 🔒 Logging de operaciones críticas (creación, actualización, eliminación)
- 🔒 Soft delete en lugar de hard delete

---

## 📚 Base de Datos

### Configuración EF Core
```csharp
// UserConfiguration.cs
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.IsEmailVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedAt)
            .IsRequired();
    }
}
```

### Índices
- ✅ **Unique Index** en `Email` (constraint de email único)
- ✅ **Index** en `IsActive` (para queries de usuarios activos)

---

## 📈 Mejoras Futuras

### Funcionalidades Pendientes
- [ ] Paginación en GET /api/users (evitar cargar miles de usuarios)
- [ ] Filtros y búsqueda (por nombre, email, estado)
- [ ] Ordenamiento (por fecha, nombre, etc.)
- [ ] Soft delete (marcar como eliminado sin borrar)
- [ ] Cambio de contraseña (endpoint separado)
- [ ] Subir avatar/foto de perfil
- [ ] Verificación de email con token
- [ ] Reseteo de contraseña
- [ ] Historial de cambios (auditoría)
- [ ] Roles y permisos
- [ ] Activar/Desactivar usuario (toggle IsActive)

### Optimizaciones
- [ ] Caché de usuarios frecuentes
- [ ] Proyección de queries (seleccionar solo campos necesarios)
- [ ] Archivado de usuarios antiguos

---

**Última actualización:** 11 de enero de 2026  
**Versión:** 1.0.0
