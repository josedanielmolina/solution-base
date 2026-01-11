# Feature: Authentication (Auth)

## Descripción
El feature de autenticación proporciona funcionalidades para gestionar el acceso de usuarios al sistema mediante credenciales de email y contraseña.

---

## 📋 Índice
- [Arquitectura](#arquitectura)
- [Endpoints](#endpoints)
- [Modelos de Datos](#modelos-de-datos)
- [Casos de Uso](#casos-de-uso)
- [Validaciones](#validaciones)
- [Seguridad](#seguridad)
- [Ejemplos de Uso](#ejemplos-de-uso)

---

## 🏗️ Arquitectura

### Componentes Principales

```
Auth Feature
├── Presentation Layer
│   └── API/Controllers/AuthController.cs
├── Application Layer
│   ├── Facades/AuthFacade.cs
│   ├── Operations/Auth/LoginOperation.cs
│   ├── DTOs/Auth/
│   │   ├── LoginDto.cs
│   │   └── AuthResponseDto.cs
│   └── Validators/LoginValidator.cs
└── Domain Layer
    └── Entities/User.cs (relacionado)
```

### Patrón de Diseño
- **Facade Pattern**: `AuthFacade` orquesta las operaciones de autenticación
- **CQRS Pattern**: Operaciones separadas por responsabilidad
- **Result Pattern**: Manejo de errores mediante `Result<T>`

---

## 🔌 Endpoints

### POST `/api/auth/login`
Autentica un usuario con email y contraseña.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-01-11T15:30:00Z",
  "user": {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "user@example.com",
    "isActive": true,
    "isEmailVerified": true
  }
}
```

**Response (400 Bad Request) - Validación:**
```json
{
  "error": "Validation.Failed",
  "message": "One or more validation errors occurred.",
  "details": [
    "Email is required",
    "Password must be at least 8 characters long"
  ]
}
```

**Response (401 Unauthorized):**
```json
{
  "error": "Auth.InvalidCredentials",
  "message": "Invalid email or password"
}
```

**Response (403 Forbidden) - Usuario Inactivo:**
```json
{
  "error": "Auth.UserInactive",
  "message": "Your account has been deactivated"
}
```

---

## 📊 Modelos de Datos

### LoginDto
```csharp
public record LoginDto(
    string Email,
    string Password
);
```

**Propiedades:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| Email | string | ✅ Sí | Email del usuario (formato válido) |
| Password | string | ✅ Sí | Contraseña (mínimo 8 caracteres) |

### AuthResponseDto
```csharp
public record AuthResponseDto(
    string Token,
    DateTime ExpiresAt,
    UserDto User
);
```

**Propiedades:**
| Campo | Tipo | Descripción |
|-------|------|-------------|
| Token | string | JWT token de autenticación |
| ExpiresAt | DateTime | Fecha/hora de expiración del token |
| User | UserDto | Información del usuario autenticado |

---

## 💼 Casos de Uso

### 1. Login Exitoso
**Flujo:**
1. Usuario envía credenciales válidas
2. Sistema valida formato de email y contraseña
3. Sistema busca usuario por email
4. Sistema verifica hash de contraseña
5. Sistema verifica que usuario esté activo
6. Sistema genera JWT token
7. Sistema actualiza `LastLoginAt`
8. Sistema retorna token y datos del usuario

**Código de Operación:**
```csharp
// LoginOperation.cs
public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginDto dto)
{
    // 1. Buscar usuario por email
    var user = await _userRepository.GetByEmailAsync(dto.Email);
    if (user is null)
        return Result<AuthResponseDto>.Failure("Auth.InvalidCredentials");

    // 2. Verificar contraseña
    if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
        return Result<AuthResponseDto>.Failure("Auth.InvalidCredentials");

    // 3. Verificar usuario activo
    if (!user.IsActive)
        return Result<AuthResponseDto>.Failure("Auth.UserInactive");

    // 4. Generar token
    var token = _jwtTokenGenerator.GenerateToken(user);

    // 5. Actualizar last login
    user.LastLoginAt = DateTime.UtcNow;
    await _unitOfWork.CommitAsync();

    // 6. Retornar respuesta
    return Result<AuthResponseDto>.Success(new AuthResponseDto(
        token,
        DateTime.UtcNow.AddHours(24),
        UserDto.FromEntity(user)
    ));
}
```

### 2. Credenciales Inválidas
**Flujo:**
1. Usuario envía credenciales
2. Sistema no encuentra usuario o contraseña no coincide
3. Sistema retorna error genérico (seguridad)

**Nota de Seguridad:** 
Por razones de seguridad, no se especifica si el email existe o si la contraseña es incorrecta. Siempre se retorna el mismo mensaje: "Invalid email or password".

### 3. Usuario Inactivo
**Flujo:**
1. Usuario envía credenciales válidas
2. Sistema valida credenciales
3. Sistema detecta `IsActive = false`
4. Sistema retorna error específico

---

## ✅ Validaciones

### LoginValidator
```csharp
public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}
```

**Reglas de Validación:**

| Campo | Regla | Mensaje |
|-------|-------|---------|
| Email | NotEmpty | "Email is required" |
| Email | EmailAddress | "Invalid email format" |
| Password | NotEmpty | "Password is required" |
| Password | MinimumLength(8) | "Password must be at least 8 characters long" |

---

## 🔒 Seguridad

### Hash de Contraseñas
- Las contraseñas **NUNCA** se almacenan en texto plano
- Se utiliza un servicio de hashing seguro (`IPasswordHasher`)
- Algoritmo recomendado: **BCrypt** o **Argon2**

### JWT Tokens
- **Expiración**: 24 horas por defecto
- **Claims incluidos**:
  - `sub`: User ID
  - `email`: User email
  - `name`: Full name
  - `exp`: Expiration time
  - `iat`: Issued at time

### Mejores Prácticas Implementadas
✅ Mensajes de error genéricos para credenciales inválidas
✅ Rate limiting en endpoint de login (recomendado)
✅ Logging de intentos de login fallidos
✅ Actualización de `LastLoginAt` para auditoría
✅ Verificación de cuenta activa antes de permitir login

---

## 📝 Ejemplos de Uso

### cURL
```bash
# Login exitoso
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePass123!"
  }'
```

### C# HttpClient
```csharp
var client = new HttpClient();
var loginDto = new LoginDto(
    "john.doe@example.com",
    "SecurePass123!"
);

var response = await client.PostAsJsonAsync(
    "http://localhost:5000/api/auth/login",
    loginDto
);

if (response.IsSuccessStatusCode)
{
    var authResponse = await response.Content
        .ReadFromJsonAsync<AuthResponseDto>();
    
    Console.WriteLine($"Token: {authResponse.Token}");
    Console.WriteLine($"Welcome {authResponse.User.FirstName}!");
}
```

### JavaScript/TypeScript
```typescript
const loginResponse = await fetch('http://localhost:5000/api/auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    email: 'john.doe@example.com',
    password: 'SecurePass123!'
  })
});

const authData = await loginResponse.json();

if (loginResponse.ok) {
  // Guardar token
  localStorage.setItem('auth_token', authData.token);
  console.log('Login successful!', authData.user);
} else {
  console.error('Login failed:', authData.message);
}
```

### Angular 20 Service Example
```typescript
// auth.service.ts
import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

interface LoginDto {
  email: string;
  password: string;
}

interface AuthResponse {
  token: string;
  expiresAt: string;
  user: UserDto;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/auth';
  
  currentUser = signal<UserDto | null>(null);
  isAuthenticated = signal<boolean>(false);

  login(credentials: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          localStorage.setItem('auth_token', response.token);
          this.currentUser.set(response.user);
          this.isAuthenticated.set(true);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
  }

  isLoggedIn(): boolean {
    return this.isAuthenticated();
  }
}
```

---

## 🧪 Testing

### Casos de Prueba

#### 1. Login con Credenciales Válidas
```csharp
[Fact]
public async Task Login_WithValidCredentials_ReturnsAuthResponse()
{
    // Arrange
    var dto = new LoginDto("user@example.com", "ValidPassword123");

    // Act
    var result = await _authFacade.LoginAsync(dto);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.NotEmpty(result.Value.Token);
}
```

#### 2. Login con Credenciales Inválidas
```csharp
[Fact]
public async Task Login_WithInvalidPassword_ReturnsFailure()
{
    // Arrange
    var dto = new LoginDto("user@example.com", "WrongPassword");

    // Act
    var result = await _authFacade.LoginAsync(dto);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Equal("Auth.InvalidCredentials", result.Error);
}
```

#### 3. Login con Usuario Inactivo
```csharp
[Fact]
public async Task Login_WithInactiveUser_ReturnsFailure()
{
    // Arrange
    var dto = new LoginDto("inactive@example.com", "ValidPassword123");

    // Act
    var result = await _authFacade.LoginAsync(dto);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Equal("Auth.UserInactive", result.Error);
}
```

---

## 🔄 Diagrama de Flujo

```
┌─────────────┐
│   Cliente   │
└──────┬──────┘
       │ POST /api/auth/login
       │ { email, password }
       ▼
┌─────────────────┐
│ AuthController  │
└────────┬────────┘
         │ Validar DTO
         ▼
┌─────────────────┐
│  LoginValidator │◄──── FluentValidation
└────────┬────────┘
         │ ✅ Válido
         ▼
┌─────────────────┐
│   AuthFacade    │
└────────┬────────┘
         │ ExecuteAsync
         ▼
┌─────────────────┐
│ LoginOperation  │
└────────┬────────┘
         │
         ├──► 1. Buscar Usuario (UserRepository)
         │
         ├──► 2. Verificar Password (IPasswordHasher)
         │
         ├──► 3. Verificar IsActive
         │
         ├──► 4. Generar Token (IJwtTokenGenerator)
         │
         ├──► 5. Actualizar LastLoginAt
         │
         └──► 6. Commit (UnitOfWork)
                │
                ▼
         ┌──────────────┐
         │ AuthResponse │
         └──────┬───────┘
                │ Return
                ▼
         ┌──────────────┐
         │   Cliente    │
         └──────────────┘
```

---

## 📚 Referencias

- [JWT Best Practices](https://datatracker.ietf.org/doc/html/rfc8725)
- [OWASP Authentication Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)

---

## 📝 Notas Adicionales

### Futuras Mejoras
- [ ] Implementar refresh tokens
- [ ] Agregar autenticación de dos factores (2FA)
- [ ] Implementar "Remember Me" con tokens de larga duración
- [ ] Agregar OAuth2 / Social Login (Google, Microsoft, etc.)
- [ ] Implementar recuperación de contraseña
- [ ] Agregar verificación de email
- [ ] Rate limiting por IP
- [ ] Captcha en intentos fallidos

### Limitaciones Actuales
- No hay soporte para múltiples sesiones activas
- No hay revocación de tokens (logout real)
- Tokens no son renovables automáticamente

---

**Última actualización:** 11 de enero de 2026  
**Versión:** 1.0.0
