# SolutionBase - Guía de Configuración

## Configuración del Backend

### 1. Configurar Base de Datos MySQL

Edita el archivo `src/Presentation/API/appsettings.json` y actualiza la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SolutionBaseDb;User=tu_usuario;Password=tu_contraseña;"
  }
}
```

### 2. Crear Migraciones de Base de Datos

Abre una terminal en la raíz del proyecto y ejecuta:

```bash
dotnet ef migrations add InitialCreate -p src/Infrastructure/Infrastructure.Persistence -s src/Presentation/API
```

### 3. Aplicar Migraciones

```bash
dotnet ef database update -p src/Infrastructure/Infrastructure.Persistence -s src/Presentation/API
```

### 4. Ejecutar el Backend

```bash
cd src/Presentation/API
dotnet run
```

El API estará disponible en: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

---

## Configuración del Frontend

### 1. Instalar Dependencias

```bash
cd src/UI/WebApp
npm install
```

### 2. Configurar URL del API

Edita el archivo `src/UI/WebApp/src/app/environments/environment.development.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',  // Ajusta el puerto si es diferente
  apiTimeout: 30000,
  tokenKey: 'auth_token',
  enableDebug: true
};
```

### 3. Ejecutar el Frontend

```bash
npm start
```

La aplicación estará disponible en: `http://localhost:4200`

---

## Estructura de la Solución

```
SolutionBase/
├── src/
│   ├── Core/
│   │   ├── Core.Domain/          # Entidades, Interfaces, Excepciones
│   │   └── Core.Application/     # DTOs, Operaciones, Fachadas, Validadores
│   ├── Infrastructure/
│   │   ├── Infrastructure.Persistence/   # DbContext, Repositorios
│   │   └── Infrastructure.Services/      # Servicios (Email, Storage, etc.)
│   ├── Presentation/
│   │   └── API/                  # Controllers, Middleware, Configuración
│   └── UI/
│       └── WebApp/               # Angular 20 Application
│           ├── src/app/
│           │   ├── core/         # Servicios, Guards, Interceptors
│           │   ├── features/     # Módulos de características (users, auth)
│           │   ├── shared/       # Componentes, Pipes, Directivas compartidas
│           │   └── layouts/      # Layouts de la aplicación
```

---

## Características Implementadas

### Backend (.NET 8)
- ✅ Clean Architecture (Domain, Application, Infrastructure, API)
- ✅ Entity Framework Core con MySQL
- ✅ Repository Pattern + Unit of Work
- ✅ Result Pattern para manejo de errores
- ✅ Facade Pattern para simplificar operaciones
- ✅ FluentValidation para validaciones
- ✅ JWT Authentication
- ✅ Password Hashing con BCrypt
- ✅ Swagger/OpenAPI documentation
- ✅ Global Exception Middleware
- ✅ Logging Middleware
- ✅ CORS configurado

### Frontend (Angular 20)
- ✅ Standalone Components
- ✅ Signal-based State Management
- ✅ Zoneless Change Detection
- ✅ CRUD completo de usuarios
- ✅ Autenticación con JWT
- ✅ Guards y Interceptors
- ✅ Servicio de notificaciones
- ✅ Componentes reutilizables (Loading, Confirmation, etc.)
- ✅ Pipes personalizados
- ✅ Directivas personalizadas
- ✅ Reactive Forms
- ✅ Lazy Loading de rutas

---

## API Endpoints

### Usuarios

- `GET /api/users` - Obtener todos los usuarios
- `GET /api/users/{id}` - Obtener usuario por ID
- `POST /api/users` - Crear nuevo usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

---

## Próximos Pasos

1. **Implementar endpoint de autenticación**: Crear `AuthController` con endpoint `/api/auth/login`
2. **Completar EmailService**: Implementar envío real de emails (SMTP, SendGrid, etc.)
3. **Completar StorageService**: Implementar almacenamiento de archivos (Azure Blob, AWS S3, etc.)
4. **Agregar Tests**: Implementar tests unitarios y de integración
5. **Configurar CI/CD**: Agregar pipelines para deploy automático

---

## Comandos Útiles

### Backend

```bash
# Restaurar paquetes
dotnet restore

# Compilar solución
dotnet build

# Ejecutar tests
dotnet test

# Crear nueva migración
dotnet ef migrations add <NombreMigracion> -p src/Infrastructure/Infrastructure.Persistence -s src/Presentation/API

# Revertir migración
dotnet ef migrations remove -p src/Infrastructure/Infrastructure.Persistence -s src/Presentation/API

# Ver SQL de migraciones
dotnet ef migrations script -p src/Infrastructure/Infrastructure.Persistence -s src/Presentation/API
```

### Frontend

```bash
# Instalar dependencias
npm install

# Ejecutar en desarrollo
npm start

# Compilar para producción
npm run build

# Ejecutar linter
npm run lint

# Ejecutar tests
npm test
```

---

## Tecnologías Utilizadas

### Backend
- .NET 8
- Entity Framework Core 8.0.11
- MySQL (Pomelo.EntityFrameworkCore.MySql 8.0.2)
- FluentValidation 12.1.1
- BCrypt.Net 4.0.3
- JWT Bearer Authentication 8.0.11
- Swashbuckle.AspNetCore 6.6.2

### Frontend
- Angular 20
- TypeScript
- RxJS
- Standalone Components
- Signals API

---

## Contacto y Soporte

Para dudas o problemas, consulta la documentación de Swagger en `/swagger` cuando el API esté ejecutándose.
