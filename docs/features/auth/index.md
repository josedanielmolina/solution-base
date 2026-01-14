# Autenticación

## 1. Descripción del Feature

Sistema de autenticación basado en JWT que permite a los usuarios iniciar sesión en la aplicación. Gestiona la validación de credenciales, generación de tokens y mantenimiento del estado de sesión tanto en el backend como en el frontend.

---

## 2. Historias de Usuario

### HU-001: Inicio de Sesión
**Como** usuario registrado  
**Quiero** poder iniciar sesión con mi email y contraseña  
**Para** acceder a las funcionalidades de la aplicación

**Criterios de Aceptación:**
- El sistema valida que el email tenga formato válido
- El sistema valida que la contraseña no esté vacía
- Si las credenciales son incorrectas, muestra mensaje de error genérico
- Si el usuario está inactivo, no permite el acceso
- Al iniciar sesión exitosamente, redirige al dashboard
- El token se almacena en localStorage para persistencia

---

## 3. Documentación Técnica

### 3.1 Consideraciones

- **Seguridad**: Se utilizan tokens JWT con expiración configurable
- **Hashing**: Las contraseñas se almacenan hasheadas usando BCrypt
- **Error genérico**: Los errores de credenciales inválidas no revelan si el email existe o no
- **Arquitectura**: Patrón Facade para orquestar la lógica de autenticación

### 3.2 Relaciones de Base de Datos y Modelos

```
┌─────────────────────────────────────────┐
│                 User                    │
├─────────────────────────────────────────┤
│ Id: int (PK)                            │
│ FirstName: string                       │
│ LastName: string                        │
│ Email: string (unique)                  │
│ PasswordHash: string                    │
│ IsActive: bool                          │
│ IsEmailVerified: bool                   │
│ LastLoginAt: DateTime?                  │
│ CreatedAt: DateTime                     │
│ UpdatedAt: DateTime                     │
└─────────────────────────────────────────┘
```

### 3.3 Entidades DTO

| DTO | Tipo | Propiedades | Uso |
|-----|------|-------------|-----|
| `LoginDto` | Request | `Email`, `Password` | Credenciales de entrada |
| `AuthResponseDto` | Response | `Token`, `User` | Respuesta de login exitoso |
| `AuthUserDto` | Response | `Id`, `FirstName`, `LastName`, `Email` | Datos del usuario autenticado |

### 3.4 Validaciones

| Campo | Regla | Mensaje |
|-------|-------|---------|
| `Email` | Requerido | "Email is required" |
| `Email` | Formato válido | "Invalid email format" |
| `Password` | Requerido | "Password is required" |

**Implementación:** `LoginValidator.cs` usando FluentValidation.

### 3.5 Errores de Dominio

| Código | Tipo HTTP | Mensaje |
|--------|-----------|---------|
| `Auth.InvalidCredentials` | 401 | Invalid email or password |
| `Auth.UserNotActive` | 401 | User account is not active |
| `Auth.EmailNotVerified` | 401 | Email address is not verified |

### 3.6 Mapa de Endpoints

| Método | Ruta | DTO Request | DTO Response | Servicio |
|--------|------|-------------|--------------|----------|
| POST | `/api/auth/login` | `LoginDto` | `AuthResponseDto` | `IAuthFacade.LoginAsync` |

**Flujo de Login:**
1. Controller recibe `LoginDto`
2. Validación con `LoginValidator`
3. Facade invoca `Login.ExecuteAsync`
4. Se busca usuario por email
5. Se verifica password con `IPasswordHasher`
6. Se valida estado activo del usuario
7. Se actualiza `LastLoginAt`
8. Se genera token JWT con `IJwtTokenGenerator`
9. Se retorna `AuthResponseDto`

### 3.7 Estructura de Archivos

```
📁 Backend
├── Presentation/API/Controllers/
│   └── AuthController.cs
├── Core/Core.Application/
│   ├── DTOs/Auth/
│   │   ├── LoginDto.cs
│   │   └── AuthResponseDto.cs
│   ├── Facades/
│   │   ├── IAuthFacade.cs
│   │   └── AuthFacade.cs
│   ├── Features/Auth/
│   │   └── Login.cs
│   ├── Validators/
│   │   └── LoginValidator.cs
│   └── Common/Errors/
│       └── AuthErrors.cs
└── Core/Core.Domain/Entities/
    └── User.cs

📁 Frontend
├── src/app/core/
│   ├── services/
│   │   └── auth.service.ts
│   ├── guards/
│   │   └── auth.guard.ts
│   └── interceptors/
│       └── auth.interceptor.ts
└── src/app/features/auth/
    ├── auth.routes.ts
    └── pages/login/
        ├── login.page.ts
        ├── login.page.html
        └── login.page.css
```

### 3.8 Dependencias

**Backend:**
- `FluentValidation` - Validación de DTOs
- `Microsoft.AspNetCore.Authentication.JwtBearer` - Autenticación JWT

**Frontend:**
- `@angular/common/http` - Cliente HTTP
- `@angular/router` - Navegación y guards

### 3.9 Tests

> ⚠️ **Pendiente:** No se han implementado tests para este feature.

### 3.10 Deuda Técnica

- [ ] Implementar refresh token
- [ ] Agregar funcionalidad de "Recordarme"
- [ ] Implementar logout en el backend (invalidar token)
- [ ] Agregar rate limiting para prevenir ataques de fuerza bruta
- [ ] Implementar verificación de email
- [ ] Agregar recuperación de contraseña
- [ ] Crear tests unitarios y de integración
- [ ] Considerar implementar 2FA

---

## Relaciones con Otros Features

| Feature | Relación |
|---------|----------|
| **Users** | El auth utiliza la entidad `User` para validar credenciales |
| **Dashboard Admin** | Las rutas protegidas dependen del estado de autenticación |
